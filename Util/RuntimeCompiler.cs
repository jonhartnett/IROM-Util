namespace IROM.Util
{
	using System;
	using Microsoft.CSharp;
	using System.CodeDom.Compiler;
	using System.Reflection;
	
	/// <summary>
	/// Compiles code at runtime.
	/// </summary>
	public static class RuntimeCompiler
	{
		/// <summary>
		/// Compiles the given file into the current runtime. Classes can be extract via 
		/// </summary>
		/// <param name="code">The code file to compile.</param>
		/// <param name="assemblies">The references assemblies of the code.</param>
		/// <returns>The resulting assembly.</returns>
		public static Assembly Compile(string code, params string[] assemblies)
		{
			//compile code
			CSharpCodeProvider provider = new CSharpCodeProvider();
			CompilerParameters paras = new CompilerParameters(assemblies);
			paras.ReferencedAssemblies.Add("System.dll");
			paras.GenerateInMemory = true;
			paras.CompilerOptions = "/unsafe";
			CompilerResults results = provider.CompileAssemblyFromSource(paras, code);
			
			if (results.Errors.HasErrors)
			{
			    foreach (CompilerError error in results.Errors)
			    {
			    	throw new InvalidOperationException(String.Format("Runtime Compile Error ({0}): {1} at {2}", error.ErrorNumber, error.ErrorText, error.Line));
			    }
			}
			
			//return compiled assembly
			return results.CompiledAssembly;
		}
	}
}
