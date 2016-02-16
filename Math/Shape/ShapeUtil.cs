namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Defines a variety of utility methods for shapes.
	/// </summary>
	public static class ShapeUtil
	{
        
        /// <summary>
        /// Returns the overlap of the given <see cref="Rectangle"/>s.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="rects">The rectangles.</param>
        /// <returns>The overlapping <see cref="Rectangle"/>.</returns>
        public static Rectangle Overlap(params Rectangle[] rects)
        {
        	Point2D min = rects[0].Min;
        	Point2D max = rects[0].Max;
        	for(int i = 1; i < rects.Length; i++)
        	{
        		min = VectorUtil.Max(min, rects[i].Min);
        		max = VectorUtil.Min(max, rects[i].Max);
        	}
        	return new Rectangle{Min = min, Max = max};
        }
        
        /// <summary>
        /// Returns the encompassing <see cref="Rectangle"/> of the given <see cref="Rectangle"/>s.
        /// </summary>
        /// <param name="rects">The rectangles.</param>
        /// <returns>The encompassing <see cref="Rectangle"/>.</returns>
        public static Rectangle Encompass(params Rectangle[] rects)
        {
        	Point2D min = rects[0].Min;
        	Point2D max = rects[0].Max;
        	for(int i = 1; i < rects.Length; i++)
        	{
        		min = VectorUtil.Min(min, rects[i].Min);
        		max = VectorUtil.Max(max, rects[i].Max);
        	}
        	return new Rectangle{Min = min, Max = max};
        }
        
        /// <summary>
        /// Returns the overlap of the given <see cref="Viewport"/>s.
        /// </summary>
        /// <param name="views">The viewports.</param>
        /// <returns>The overlapping <see cref="Viewport"/>.</returns>
        public static Viewport Overlap(params Viewport[] views)
        {
        	Vec2D min = views[0].Min;
        	Vec2D max = views[0].Max;
        	for(int i = 1; i < views.Length; i++)
        	{
        		min = VectorUtil.Max(min, views[i].Min);
        		max = VectorUtil.Min(max, views[i].Max);
        	}
        	return new Viewport{Min = min, Max = max};
        }
        
        /// <summary>
        /// Returns the encompassing <see cref="Viewport"/> of the given <see cref="Viewport"/>s.
        /// </summary>
        /// <param name="views">The viewports.</param>
        /// <returns>The encompassing <see cref="Viewport"/>.</returns>
        public static Viewport Encompass(params Viewport[] views)
        {
        	Vec2D min = views[0].Min;
        	Vec2D max = views[0].Max;
        	for(int i = 1; i < views.Length; i++)
        	{
        		min = VectorUtil.Min(min, views[i].Min);
        		max = VectorUtil.Max(max, views[i].Max);
        	}
        	return new Viewport{Min = min, Max = max};
        }
	}
}