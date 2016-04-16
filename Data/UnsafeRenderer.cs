namespace IROM.Util
{
	using System;
	using System.IO;
	using System.Reflection;
	
	/// <summary>
	/// Storage for the loaded unsafe compile-at-runtime code.
	/// </summary>
	internal static class UnsafeSrcStorage
	{
		internal static readonly string src = ReadEmbedResource("Data.UnsafeDummyRenderer.cs");
		
		private static string ReadEmbedResource(string file)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			
			using (Stream stream = assembly.GetManifestResourceStream(file))
			if(stream != null)
			using (StreamReader reader = new StreamReader(stream))
			{
			    return reader.ReadToEnd();
			}
			throw new FileNotFoundException();
		}
	}
	
	/// <summary>
	/// Storage class for a set of unsafe (faster) rendering methods.
	/// </summary>
	public static class UnsafeRenderer<T> where T : struct
	{
		// disable once StaticFieldInGenericType
		public static readonly RenderContext<T> Instance;
		static UnsafeRenderer()
		{
			Instance = new RenderContext<T>();
			
			//ensure pointerable before continuing
			try
			{
				typeof(T).MakePointerType();
			}catch(Exception)
			{
				return;
			}
			
			string src = UnsafeSrcStorage.src;
			if(typeof(T).Namespace != "System" && typeof(T).Namespace != "IROM.Util")
			{
				src = src.Replace("//NAMESPACE", string.Format("	using {0};\n", typeof(T).Namespace));
			}
			src = src.Replace("Dummy", typeof(T).Name);
			
			string[] references = {typeof(UnsafeRenderer<>).Assembly.GetName().Name + ".dll"};
			if(typeof(T).Assembly != typeof(UnsafeRenderer<>).Assembly)
			{
				ArrayUtil.Add(ref references, typeof(T).Assembly.GetName().Name + ".dll");
			}
			
			Assembly assembly = RuntimeCompiler.Compile(src, references);
			Type clazz = assembly.GetType(string.Format("IROM.Util.Unsafe{0}Renderer", typeof(T).Name));
			
			Instance.SolidConst = (ConstRender<T>)Delegate.CreateDelegate(typeof(ConstRender<T>), clazz.GetMethod("SolidConstRender"));
			Instance.SolidCopy = (CopyRender<T>)Delegate.CreateDelegate(typeof(CopyRender<T>), clazz.GetMethod("SolidCopyRender"));
			Instance.OutlineConst = (ConstRender<T>)Delegate.CreateDelegate(typeof(ConstRender<T>), clazz.GetMethod("OutlineConstRender"));
			Instance.OutlineCopy = (CopyRender<T>)Delegate.CreateDelegate(typeof(CopyRender<T>), clazz.GetMethod("OutlineCopyRender"));
		}
	}
}
