namespace IROM.Util
{
	using System;
	using System.Collections.Generic;
	
	/// <summary>
	/// Represents a method that render a set of scans in the given map with the given constant value.
	/// </summary>
	/// <param name="clip">The clipping rectangle.</param>
	/// <param name="scanner">The scans to render.</param>
	/// <param name="dest">The map to render to.</param>
	/// <param name="value">The constant value.</param>
	public delegate void ConstRender<T>(Rectangle clip, Scanner scanner, DataMap<T> dest, T value) where T : struct;
	
	/// <summary>
	/// Represents a method that render a set of scans in the given map with the given src map.
	/// </summary>
	/// <param name="clip">The clipping rectangle.</param>
	/// <param name="scanner">The scans to render.</param>
	/// <param name="dest">The map to render to.</param>
	/// <param name="src">The source map.</param>
	/// <param name="offset">The offset of the source map.</param>
	public delegate void CopyRender<T>(Rectangle clip, Scanner scanner, DataMap<T> dest, DataMap<T> src, Point2D offset) where T : struct;
	
	/// <summary>
	/// Represents a set of methods for rendering to a datamap.
	/// </summary>
	public class RenderContext<T> where T : struct
	{
		public ConstRender<T> SolidConst;
		public CopyRender<T> SolidCopy;
		public ConstRender<T> OutlineConst;
		public CopyRender<T> OutlineCopy;
	}
	
	/// <summary>
	/// Stores extension methods for DataMaps.
	/// </summary>
	public static class DataMapExtensions
	{
		/// <summary>
		/// The infinite clipping rectangle.
		/// </summary>
		private static readonly Rectangle NoClip = new Rectangle{Min = int.MinValue, Max = int.MaxValue};
		
		/// <summary>
		/// The clipping rectangle instances.
		/// </summary>
		private static readonly Dictionary<object, Stack<Rectangle>> ClippingStacks = new Dictionary<object, Stack<Rectangle>>();
		
		/// <summary>
		/// Sets the clipping rectangle for data operations on this <see cref="DataMap{T}">DataMap</see>. Clear with <see cref="PopClip"/>.
		/// </summary>
		/// <param name="map">The <see cref="DataMap{T}">DataMap</see>.</param>
		/// <param name="clip">The clipping rectangle.</param>
		public static void PushClip<T>(this DataMap<T> map, Rectangle clip) where T : struct
		{
			Stack<Rectangle> stack;
			if(ClippingStacks.TryGetValue(map, out stack))
			{
				clip = ShapeUtil.Overlap(stack.Peek(), clip);
			}else
			{
				stack = new Stack<Rectangle>();
				ClippingStacks.Add(map, stack);
			}
			stack.Push(clip);
		}
		
		/// <summary>
		/// Gets the clipping rectangle for data operations on this <see cref="DataMap{T}">DataMap</see>. Clear with <see cref="PopClip"/>.
		/// </summary>
		/// <param name="map">The <see cref="DataMap{T}">DataMap</see>.</param>
		/// <returns>The clip bounds.</returns>
		public static Rectangle GetClip<T>(this DataMap<T> map) where T : struct
		{
			Stack<Rectangle> stack;
			if(ClippingStacks.TryGetValue(map, out stack))
			{
				return stack.Peek();
			}else
			{
				return NoClip;
			}
		}
		
		/// <summary>
		/// Clears the clipping rectangle for data operations on this <see cref="DataMap{T}">DataMap</see>.
		/// </summary>
		/// <param name="map">The <see cref="DataMap{T}">DataMap</see>.</param>
		public static void PopClip<T>(this DataMap<T> map) where T : struct
		{
			Stack<Rectangle> stack;
			if(ClippingStacks.TryGetValue(map, out stack))
			{
				stack.Pop();
				if(stack.Count == 0) 
					ClippingStacks.Remove(map);
			}else
			{
				throw new Exception("There are no clips on the stack of " + map.GetType().Name + " " + map);
			}
		}
		
		/// <summary>
		/// Fills the given <see cref="DataMap{T}">DataMap</see> with the given value.
		/// </summary>
		/// <param name="map">The <see cref="DataMap{T}">DataMap</see>.</param>
		/// <param name="value">The value to fill with.</param>
		public static void Fill<T>(this DataMap<T> map, T value) where T : struct
		{
			map.RenderSolid((Rectangle)map.Size, value);
		}
		
		/// <summary>
		/// Fills the given <see cref="DataMap{T}">DataMap</see> from the given src.
		/// </summary>
		/// <param name="map">The <see cref="DataMap{T}">DataMap</see>.</param>
		/// <param name="src">The <see cref="DataMap{T}">DataMap</see> to render from.</param>
		/// <param name="offset">The offset for the src map.</param>
		public static void Blit<T>(this DataMap<T> map, DataMap<T> src, Point2D offset = default(Point2D)) where T : struct
		{
			map.RenderSolid(new Rectangle{Position = offset, Size = src.Size}, src, -offset);
		}
		
		/// <summary>
		/// Renders the given solid shape to this <see cref="DataMap{T}">DataMap</see> with the given value.
		/// </summary>
		/// <param name="map">The <see cref="DataMap{T}">DataMap</see>.</param>
		/// <param name="shape">The shape to render.</param>
		/// <param name="value">The value to render with.</param>
		public static void RenderSolid<T>(this DataMap<T> map, IRenderableShape shape, T value) where T : struct
		{
			map.RenderHelper(shape, (con, clip, scanner) =>
            {
				if(con.SolidConst != null)
				{
					con.SolidConst(clip, scanner, map, value);
					return true;
				}else
				{
					return false;
				}
            });
		}
		
		/// <summary>
		/// Renders the given solid shape to this <see cref="DataMap{T}">DataMap</see> from the given src.
		/// </summary>
		/// <param name="map">The <see cref="DataMap{T}">DataMap</see>.</param>
		/// <param name="shape">The shape to render.</param>
		/// <param name="src">The <see cref="DataMap{T}">DataMap</see> to render from.</param>
		/// <param name="offset">The offset for the src map.</param>
		public static void RenderSolid<T>(this DataMap<T> map, IRenderableShape shape, DataMap<T> src, Point2D offset = default(Point2D)) where T : struct
		{
			map.RenderHelper(shape, (con, clip, scanner) =>
            {
				if(con.SolidCopy != null)
				{
					con.SolidCopy(clip, scanner, map, src, offset);
					return true;
				}else
				{
					return false;
				}
            });
		}
		
		/// <summary>
		/// Renders the given outline shape to this <see cref="DataMap{T}">DataMap</see> with the given value.
		/// </summary>
		/// <param name="map">The <see cref="DataMap{T}">DataMap</see>.</param>
		/// <param name="shape">The shape to render.</param>
		/// <param name="value">The value to render with.</param>
		public static void RenderOutline<T>(this DataMap<T> map, IRenderableShape shape, T value) where T : struct
		{
			map.RenderHelper(shape, (con, clip, scanner) =>
            {
				if(con.OutlineConst != null)
				{
					con.OutlineConst(clip, scanner, map, value);
					return true;
				}else
				{
					return false;
				}
            });
		}
		
		/// <summary>
		/// Renders the given solid shape to this <see cref="DataMap{T}">DataMap</see> from the given src.
		/// </summary>
		/// <param name="map">The <see cref="DataMap{T}">DataMap</see>.</param>
		/// <param name="shape">The shape to render.</param>
		/// <param name="src">The <see cref="DataMap{T}">DataMap</see> to render from.</param>
		/// <param name="offset">The offset for the src map.</param>
		public static void RenderOutline<T>(this DataMap<T> map, IRenderableShape shape, DataMap<T> src, Point2D offset = default(Point2D)) where T : struct
		{
			map.RenderHelper(shape, (con, clip, scanner) =>
            {
				if(con.OutlineCopy != null)
				{
					con.OutlineCopy(clip, scanner, map, src, offset);
					return true;
				}else
				{
					return false;
				}
            });
		}
		
		/// <summary>
		/// Renders the given shape to this <see cref="DataMap{T}">DataMap</see> with the given function.
		/// </summary>
		/// <param name="map">The <see cref="DataMap{T}">DataMap</see>.</param>
		/// <param name="shape">The shape to render.</param>
		/// <param name="func">A function that attempts to render with the given context, returning true if successful.</param>
		private static void RenderHelper<T>(this DataMap<T> map, IRenderableShape shape, Func<RenderContext<T>, Rectangle, Scanner, bool> func) where T : struct
		{
			Scanner scanner = Scanner.Get(map.Height);
			Rectangle clip = ShapeUtil.Overlap(map.GetClip(), (Rectangle)map.Size);
			shape.Scan(scanner, clip);
			foreach(var context in map.GetContexts())
			{
				if(func(context, clip, scanner)) return;
			}
			throw new InvalidOperationException("[DataMapExtensions] Operation not supported for " + map.GetType().Name + " " + map + "!");
		}
	}
}
