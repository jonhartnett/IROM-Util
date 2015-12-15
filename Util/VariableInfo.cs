namespace IROM.Util
{
	using System;
	using System.Reflection;
	
	/// <summary>
	/// Simple interface for handling fields and properties together.
	/// </summary>
	public abstract class DataInfo
	{
		/// <summary>
		/// Returns the name.
		/// </summary>
		/// <returns>The name.</returns>
		public abstract string GetName();
		
		/// <summary>
		/// Returns the data type.
		/// </summary>
		/// <returns></returns>
		public abstract Type GetDataType();
		
		/// <summary>
		/// Returns true if the data storage is static.
		/// </summary>
		/// <returns></returns>
		public abstract bool IsStatic();
		
		/// <summary>
		/// Returns the current value for the given instance.
		/// </summary>
		/// <param name="obj">The instance, null if static.</param>
		/// <returns>The current value.</returns>
		public abstract object GetValue(object obj);
		
		/// <summary>
		/// Sets the current value for the given instance.
		/// </summary>
		/// <param name="obj">The instance, null if static.</param>
		/// <param name="value">The value to set to.</param>
		public abstract void SetValue(object obj, object value);
		
		public static implicit operator DataInfo(FieldInfo info)
		{
			return new FieldDataInfo(info);
		}
		
		public static implicit operator DataInfo(PropertyInfo info)
		{
			return new PropertyDataInfo(info);
		}
	}
	
	public class FieldDataInfo : DataInfo
	{
		public FieldInfo Info;
		
		public FieldDataInfo(FieldInfo info)
		{
			Info = info; 
		}
		
		public string GetName()
		{
			return Info.Name;
		}
		
		public Type GetDataType()
		{
			return Info.FieldType;
		}
		
		public bool IsStatic()
		{
			return Info.IsStatic;
		}
		
		public object GetValue(object obj)
		{
			return Info.GetValue(obj);
		}
		
		public void SetValue(object obj, object value)
		{
			Info.SetValue(obj, value);
		}
	}
	
	public class PropertyDataInfo : DataInfo
	{
		public PropertyInfo Info;
		
		public PropertyDataInfo(PropertyInfo info)
		{
			Info = info; 
		}
		
		public string GetName()
		{
			return Info.Name;
		}
		
		public Type GetDataType()
		{
			return Info.PropertyType;
		}
		
		public bool IsStatic()
		{
			return Info.GetGetMethod(true).IsStatic;
		}
		
		public object GetValue(object obj)
		{
			return Info.GetValue(obj);
		}
		
		public void SetValue(object obj, object value)
		{
			Info.SetValue(obj, value);
		}
	}
}
