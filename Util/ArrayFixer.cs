namespace IROM.Util
{
	using System;
	using System.Runtime.InteropServices;
	
	/// <summary>
	/// Performs array fixing in a more flexible way than the fixed statement.
	/// </summary>
	public static class ArrayFixer
	{
		/// <summary>
		/// Fixes this array. It is very important to call <see cref="Free"/> once the array no longer needs to be fixed.
		/// </summary>
		/// <param name="array">The array to fix.</param>
		/// <param name="handle">The <see cref="GCHandle"/> variable. Required for calling <see cref="Free"/></param>
		/// <returns>The array element pointer.</returns>
		public unsafe static void* Fix(this Array array, ref GCHandle handle)
		{
			handle = GCHandle.Alloc(array, GCHandleType.Pinned);
			return (void*)handle.AddrOfPinnedObject();
		}
		
		/// <summary>
		/// Frees this array.
		/// </summary>
		/// <param name="array">The array to free.</param>
		/// <param name="handle">The <see cref="GCHandle"/> given by <see cref="Fix"/>.</param>
		public unsafe static void Free(this Array array, ref GCHandle handle)
		{
			handle.Free();
		}
	}
}
