namespace IROM.Util
{
	using System;
	using System.ComponentModel;
	using System.Runtime.InteropServices;
	using System.Reflection;
	
	/// <summary>
	/// Contains utilities for dealing with the WinAPI.
	/// </summary>
	public static class WinAPIUtils
	{
		private static readonly Action<int> SetErrorMethod = (Action<int>)typeof(Marshal).GetMethod("SetLastWin32Error", BindingFlags.NonPublic | BindingFlags.Static).CreateDelegate(typeof(Action<int>));
		
		/// <summary>
		/// Throws a <see cref="Win32Exception"/> if the given condition is not true.
		/// </summary>
		/// <param name="test">The condition.</param>
		public static void Assert(bool test)
		{
			if(!test)
			{
				int error = Marshal.GetLastWin32Error();
				if(error != 0/*Success*/)
					throw new Win32Exception(error);
				//reset error for next time
				SetErrorMethod(0);
			}
		}
	}
}
