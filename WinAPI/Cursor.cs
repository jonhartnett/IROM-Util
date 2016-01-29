namespace IROM.Util
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.InteropServices;

	/// <summary>
	/// Standard cursor options
	/// </summary>
	public enum Cursor : int
	{
		/// <summary>
		/// An unspecified cursor.
		/// </summary>
		UNSPECIFIED = 0,
		/// <summary>
		/// The standard arrow.
		/// </summary>
		ARROW = 32512,
		/// <summary>
		/// The hand.
		/// </summary>
		HAND = 32649,
		/// <summary>
		/// The 'I' cursor.
		/// </summary>
		I_BEAM = 32513,
		/// <summary>
		/// The hourglass.
		/// </summary>
		HOURGLASS = 32514,
		/// <summary>
		/// The four pointed resize.
		/// </summary>
		RESIZE_ALL = 32646,
		/// <summary>
		/// The vertical resize.
		/// </summary>
		RESIZE_VERTICAL = 32645,
		/// <summary>
		/// The horizontal resize.
		/// </summary>
		RESIZE_HORIZONTAL = 32644,
		/// <summary>
		/// The north-east to south-west resize.
		/// </summary>
		RESIZE_NESW = 32643,
		/// <summary>
		/// The north-west to south-east resize.
		/// </summary>
		RESIZE_NWSE = 32642,
		/// <summary>
		/// The arrow with an hourglass.
		/// </summary>
		ARROW_LOADING = 32650,
		/// <summary>
		/// The arrow with a question mark.
		/// </summary>
		ARROW_HELP = 32651,
		/// <summary>
		/// The crosshair.
		/// </summary>
		CROSSHAIR = 32515,
		/// <summary>
		/// The slashed out circle.
		/// </summary>
		SLASH_CIRCLE = 32648,
		/// <summary>
		/// The vertical pointed arrow.
		/// </summary>
		ARROW_UP = 32516
	}
	
	/// <summary>
	/// Extension methods for <see cref="Cursor"/>
	/// </summary>
	public static class CursorExtensions
	{
		private static readonly Dictionary<Cursor, IntPtr> pointerMap = new Dictionary<Cursor, IntPtr>();
		
		/// <summary>
		/// Returns the pointer for this cursor.
		/// </summary>
		/// <param name="cursor">The cursor to find.</param>
		/// <returns>The pointer for the cursor.</returns>
		public static IntPtr GetPointer(this Cursor cursor)
		{
			IntPtr ptr;
			if(!pointerMap.TryGetValue(cursor, out ptr))
			{
				ptr = LoadCursor(IntPtr.Zero, cursor);
				pointerMap[cursor] = ptr;
			}
			return ptr;
		}
		
		//winapi methods
		[DllImport("user32.dll")]
		private static extern IntPtr LoadCursor(IntPtr file, Cursor name);
	}
}
