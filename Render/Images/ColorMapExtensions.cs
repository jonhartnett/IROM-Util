namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Represents a method that render a set of scans in the given map with the given constant value.
	/// </summary>
	/// <param name="clip">The clipping rectangle.</param>
	/// <param name="scanner">The scans to render.</param>
	/// <param name="dest">The map to render to.</param>
	/// <param name="value">The constant value.</param>
	/// <param name="mode">The color blending mode to use.</param>
	/// <param name="isAA">True if anti-aliasing is enabled.</param>
	public delegate void ColorConstRender(Rectangle clip, Scanner scanner, DataMap<ARGB> dest, ARGB value, ColorMode mode, bool isAA);
	
	/// <summary>
	/// Represents a method that render a set of scans in the given map with the given src map.
	/// </summary>
	/// <param name="clip">The clipping rectangle.</param>
	/// <param name="scanner">The scans to render.</param>
	/// <param name="dest">The map to render to.</param>
	/// <param name="src">The source map.</param>
	/// <param name="offset">The offset of the source map.</param>
	/// <param name="mode">The color blending mode to use.</param>
	/// <param name="isAA">True if anti-aliasing is enabled.</param>
	public delegate void ColorCopyRender(Rectangle clip, Scanner scanner, DataMap<ARGB> dest, DataMap<ARGB> src, Point2D offset, ColorMode mode, bool isAA);
	
	/// <summary>
	/// Represents a set of methods for rendering to a color datamap.
	/// </summary>
	public class ColorRenderContext : RenderContext<ARGB>
	{
		private ColorConstRender colorSolidConst;
		private ColorCopyRender colorSolidCopy;
		private ColorConstRender colorOutlineConst;
		private ColorCopyRender colorOutlineCopy;
		
		public ColorConstRender ColorSolidConst
		{
			get{return colorSolidConst;}
			set
			{
				colorSolidConst = value;
				//override base to default rendering values
				if(value == null) SolidConst = null;
				else 			  SolidConst = (clip, scanner, dest, val) => colorSolidConst(clip, scanner, dest, val, ColorMode.NORMAL, false);
			}
		}
		public ColorCopyRender ColorSolidCopy
		{
			get{return colorSolidCopy;}
			set
			{
				colorSolidCopy = value;
				//override base to default rendering values
				if(value == null) SolidCopy = null;
				else 			  SolidCopy = (clip, scanner, dest, src, offset) => colorSolidCopy(clip, scanner, dest, src, offset, ColorMode.NORMAL, false);
			}
		}
		public ColorConstRender ColorOutlineConst
		{
			get{return colorOutlineConst;}
			set
			{
				colorOutlineConst = value;
				//override base to default rendering values
				if(value == null) OutlineConst = null;
				else 			  OutlineConst = (clip, scanner, dest, val) => colorOutlineConst(clip, scanner, dest, val, ColorMode.NORMAL, false);
			}
		}
		public ColorCopyRender ColorOutlineCopy
		{
			get{return colorOutlineCopy;}
			set
			{
				colorOutlineCopy = value;
				//override base to default rendering values
				if(value == null) OutlineCopy = null;
				else 			  OutlineCopy = (clip, scanner, dest, src, offset) => colorOutlineCopy(clip, scanner, dest, src, offset, ColorMode.NORMAL, false);
			}
		}
	}
	
	/// <summary>
	/// Stores extension methods for color DataMaps.
	/// </summary>
	public static class ColorMapExtensions
	{
		/// <summary>
		/// Fills the given <see cref="DataMap{ARGB}">DataMap</see> with the given value.
		/// </summary>
		/// <param name="map">The <see cref="DataMap{ARGB}">DataMap</see>.</param>
		/// <param name="value">The value to fill with.</param>
		/// <param name="mode">The color blending mode to use.</param>
		/// <param name="isAA">True if anti-aliasing is enabled.</param>
		public static void Fill(this DataMap<ARGB> map, ARGB value, ColorMode mode, bool isAA)
		{
			map.RenderSolid((Rectangle)map.Size, value, mode, isAA);
		}
		
		/// <summary>
		/// Fills the given <see cref="DataMap{T}">DataMap</see> from the given src.
		/// </summary>
		/// <param name="map">The <see cref="DataMap{T}">DataMap</see>.</param>
		/// <param name="src">The <see cref="DataMap{T}">DataMap</see> to render from.</param>
		/// <param name="offset">The offset for the src map.</param>
		/// <param name="mode">The color blending mode to use.</param>
		/// <param name="isAA">True if anti-aliasing is enabled.</param>
		public static void Blit(this DataMap<ARGB> map, DataMap<ARGB> src, Point2D offset, ColorMode mode, bool isAA)
		{
			map.RenderSolid(new Rectangle{Position = offset, Size = src.Size}, src, -offset, mode, isAA);
		}
		
		/// <summary>
		/// Renders the given solid shape to this <see cref="DataMap{T}">DataMap</see> with the given value.
		/// </summary>
		/// <param name="map">The <see cref="DataMap{T}">DataMap</see>.</param>
		/// <param name="shape">The shape to render.</param>
		/// <param name="value">The value to render with.</param>
		/// <param name="mode">The color blending mode to use.</param>
		/// <param name="isAA">True if anti-aliasing is enabled.</param>
		public static void RenderSolid(this DataMap<ARGB> map, IRenderableShape shape, ARGB value, ColorMode mode, bool isAA)
		{
			map.RenderHelper(shape, (con, clip, scanner) =>
            {
			    ColorRenderContext ccon = con as ColorRenderContext;
				if(ccon != null && ccon.ColorSolidConst != null)
				{
					ccon.ColorSolidConst(clip, scanner, map, value, mode, isAA);
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
		/// <param name="mode">The color blending mode to use.</param>
		/// <param name="isAA">True if anti-aliasing is enabled.</param>
		public static void RenderSolid(this DataMap<ARGB> map, IRenderableShape shape, DataMap<ARGB> src, Point2D offset, ColorMode mode, bool isAA)
		{
			map.RenderHelper(shape, (con, clip, scanner) =>
            {
			    ColorRenderContext ccon = con as ColorRenderContext;
				if(ccon != null && ccon.ColorSolidCopy != null)
				{
					ccon.ColorSolidCopy(clip, scanner, map, src, offset, mode, isAA);
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
		/// <param name="mode">The color blending mode to use.</param>
		/// <param name="isAA">True if anti-aliasing is enabled.</param>
		public static void RenderOutline(this DataMap<ARGB> map, IRenderableShape shape, ARGB value, ColorMode mode, bool isAA)
		{
			map.RenderHelper(shape, (con, clip, scanner) =>
            {
			    ColorRenderContext ccon = con as ColorRenderContext;
				if(ccon != null && ccon.ColorOutlineConst != null)
				{
					ccon.ColorOutlineConst(clip, scanner, map, value, mode, isAA);
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
		/// <param name="mode">The color blending mode to use.</param>
		/// <param name="isAA">True if anti-aliasing is enabled.</param>
		public static void RenderOutline(this DataMap<ARGB> map, IRenderableShape shape, DataMap<ARGB> src, Point2D offset, ColorMode mode, bool isAA)
		{
			map.RenderHelper(shape, (con, clip, scanner) =>
            {
			    ColorRenderContext ccon = con as ColorRenderContext;
				if(ccon != null && ccon.ColorOutlineCopy != null)
				{
					ccon.ColorOutlineCopy(clip, scanner, map, src, offset, mode, isAA);
					return true;
				}else
				{
					return false;
				}
            });
		}
	}
}
