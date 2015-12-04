namespace IROM.Util
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using System.IO;
	
	/// <summary>
	/// Helper class that handles automatic configuration management.
	/// </summary>
	public static class AutoConfig
	{	
		/// <summary>
		/// Tries to convert the specified string representation of a Type to its object equivalent. A return value indicates whether the conversion succeeded or failed.
		/// </summary>
		/// <param name="value">A string containing the value to convert.</param>
		/// <param name="result">When this method returns, if the conversion succeeded, contains the parsed value. 
		/// If the conversion failed, undefined. The conversion fails if value is null or is not a valid string representation for the object type.</param>
		/// <returns>true if value was converted successfully; otherwise, false.</returns>
		public delegate bool Parser<T>(string value, out T result);
		/// <summary>
		/// Converts the given object to a string.
		/// </summary>
		/// <param name="value">The object.</param>
		/// <returns>The serialized version.</returns>
		public delegate string Serializer<T>(T value);
		
		//binding flags for all members
		private const BindingFlags EVERYTHING = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
		/// <summary>
		/// Group of configurable values for each type.
		/// </summary>
		private static Dictionary<Type, Dictionary<string, IConfig>> fieldLookup = new Dictionary<Type, Dictionary<string, IConfig>>();
		//parsers and serializers for each type
		private static Dictionary<Type, Parser<object>> parsers = new Dictionary<Type, Parser<object>>();
		private static Dictionary<Type, Serializer<object>> serializers = new Dictionary<Type, Serializer<object>>();
		
		/// <summary>
		/// Sets a parser for configuration.
		/// </summary>
		/// <param name="parser">The parsing method.</param>
		public static void SetParser<T>(Parser<T> parser)
		{
			parsers[typeof(T)] = new Parser<object>(parser);
		}
		
		/// <summary>
		/// Sets a serializer for configuration. If one is missing, uses ToString().
		/// </summary>
		/// <param name="serializer">The serializing method.</param>
		public static void SetSerialize<T>(Serializer<T> serializer)
		{
			serializers[typeof(T)] = new Serializer<object>(serializer);
		}
		
		static AutoConfig()
		{
			//set default parsers
			SetParser(bool.TryParse);
			SetParser(byte.TryParse);
			SetParser(sbyte.TryParse);
			SetParser(ushort.TryParse);
			SetParser(short.TryParse);
			SetParser(uint.TryParse);
			SetParser(int.TryParse);
			SetParser(ulong.TryParse);
			SetParser(long.TryParse);
			SetParser(float.TryParse);
			SetParser(double.TryParse);
			SetParser(decimal.TryParse);
			SetParser(DateTime.TryParse);
			SetParser(char.TryParse);
			SetParser((string v, out string r) => 
	        {
	          	r = v;
	          	return true;
	        });
		}
		
		/// <summary>
		/// Discovers all configurable fields and properties in the given <see cref="Type"/>.
		/// </summary>
		/// <param name="type">The type.</param>
		private static void Discover(Type type)
		{
			if(!fieldLookup.ContainsKey(type))
			{
				Dictionary<string, IConfig> fields = new Dictionary<string, IConfig>();
				foreach(FieldInfo info in type.GetFields(EVERYTHING))
				{
					ConfigAttribute attr = info.GetCustomAttribute(typeof(ConfigAttribute)) as ConfigAttribute;
					if(attr != null)
					{
						if(fields.ContainsKey(attr.Tag))
						{
							throw new Exception("Duplicate Config tag " + attr.Tag + " on members " + type.Name + "." + info.Name + " and " + type.Name + "." + fields[attr.Tag].GetName());
						}
						fields[attr.Tag] = new FieldConfig(info, attr.FileID);
					}
				}
				foreach(PropertyInfo info in type.GetProperties(EVERYTHING))
				{
					ConfigAttribute attr = info.GetCustomAttribute(typeof(ConfigAttribute)) as ConfigAttribute;
					if(attr != null)
					{
						if(fields.ContainsKey(attr.Tag))
						{
							throw new Exception("Duplicate Config tag " + attr.Tag + " on members " + type.Name + "." + info.Name + " and " + type.Name + "." + fields[attr.Tag].GetName());
						}
						fields[attr.Tag] = new PropertyConfig(info, attr.FileID);
					}
				}
			}
		}
		
		public static void Load(Type type, object instance, string path, uint fileID, bool isReadonly)
		{
			//init
			Discover(type);
			if(File.Exists(path))
			{
				Dictionary<string, IConfig> fields = fieldLookup[type];
				using(StreamReader input = new StreamReader(path))
				{
					//for every line
					string line;
					while((line = input.ReadLine()) != null)
					{
						string[] parts = line.Split(new []{':'}, 2);
						//trim parts
						for(int i = 0; i < parts.Length; i++) parts[i] = parts[i].Trim();
						
						IConfig config;
						//if exists and right scope (static or instance) and right file
						if(fields.TryGetValue(parts[0], out config) && ((instance == null) == config.IsStatic) && (fileID == config.GetFileID()))
						{
							Parser<object> parser;
							if(parsers.TryGetValue(config.GetFieldType(), out parser))
							{
								//parse string
								object result;
								parser(parts[1], out result);
								//set value
								config.Set(result, instance);
							}else
							{
								throw new Exception("Type " + config.GetFieldType() + " missing a configuration parser. Please see AutoConfig.SetParser.");
							}
						}
					}
				}
			}
			if(!isReadonly) Save(type, instance, path);
		}
		
		public static void Save(Type type, object instance, string path, uint fileID)
		{
			Discover(type);
			Dictionary<string, IConfig> fields = fieldLookup[type];
			using(StreamWriter output = new StreamWriter(path))
			{
				foreach(var field in fields)
				{
					//if right scope (static or instance) and right file
					if((instance == null) == field.Value.IsStatic && (fileID == field.Value.GetFileID()))
					{
						Serializer<object> serializer;
						serializers.TryGetValue(field.Value.GetFieldType(), out serializer);
						if(serializer == null)
						{
							serializer = (o => o.ToString());
						}
						string result = serializer(field.Value.Get(instance));
						output.WriteLine(field.Key + ": " + result);
					}
				}
			}
		}
		
		/// <summary>
		/// Loads configuration for the given obj from the given path. For static configs, obj should be the <see cref="Type"/>
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <param name="path">The path.</param>
		/// <param name="fileID">The id for this configuration.</param>
		/// <param name="isReadonly">True if the file should not be created or updated.</param>
		public static void LoadConfig(this object obj, string path, uint fileID = 0, bool isReadonly = false)
		{
			if(obj is Type)
			{
				Load((Type)obj, null, path, fileID, isReadonly);
			}else
			{
				Load(obj.GetType(), obj, path, fileID, isReadonly);
			}
		}
		
		/// <summary>
		/// Saves configuration for the given obj from the given path. For static configs, obj should be the <see cref="Type"/>
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <param name="path">The path.</param>
		/// <param name="fileID">The id for this configuration.</param>
		public static void SaveConfig(this object obj, string path, uint fileID = 0)
		{
			if(obj is Type)
			{
				Save((Type)obj, null, path, fileID);
			}else
			{
				Save(obj.GetType(), obj, path, fileID);
			}
		}
		
		private interface IConfig
		{
			bool IsStatic();
			void Set(object obj, object instance = null);
			object Get(object instance = null);
			Type GetFieldType();
			string GetName();
			uint GetFileID();
		}
		
		private class FieldConfig : IConfig
		{
			public FieldInfo Info;
			public uint fileID;
			public FieldConfig(FieldInfo info, uint id){Info = info; fileID = id;}
			public bool IsStatic(){return Info.IsStatic;}
			public void Set(object obj, object instance = null){Info.SetValue(instance, obj);}
			public object Get(object instance = null){return Info.GetValue(instance);}
			public Type GetFieldType(){return Info.FieldType;}
			public string GetName(){return Info.Name;}
			public uint GetFileID(){return fileID;}
		}
		
		private class PropertyConfig : IConfig
		{
			public PropertyInfo Info;
			public uint fileID;
			public PropertyConfig(PropertyInfo info, uint id){Info = info; fileID = id;}
			public bool IsStatic(){return Info.GetGetMethod(true).IsStatic;}
			public void Set(object obj, object instance = null){Info.SetValue(instance, obj);}
			public object Get(object instance = null){return Info.GetValue(instance);}
			public Type GetFieldType(){return Info.PropertyType;}
			public string GetName(){return Info.Name;}
			public uint GetFileID(){return fileID;}
		}
	}
}
