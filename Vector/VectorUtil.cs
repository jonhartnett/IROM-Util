namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Defines a variety of utility methods for vectors.
	/// </summary>
	public static class VectorUtil
	{
        /// <summary>
        /// Returns the component-wise max of the given vectors.
        /// </summary>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise max vector.</returns>
        public static Vec1D Max(params Vec1D[] vecs)
        {
        	Vec1D max = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X > max.X)
        		{
        			max.X = vecs[i].X;
        		}
        	}
            return max;
        }
        
        /// <summary>
        /// Returns the component-wise max of the given vectors.
        /// </summary>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise max vector.</returns>
        public static Vec2D Max(params Vec2D[] vecs)
        {
        	Vec2D max = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X > max.X)
        		{
        			max.X = vecs[i].X;
        		}
        		if(vecs[i].Y > max.Y)
        		{
        			max.Y = vecs[i].Y;
        		}
        	}
            return max;
        }
        
        /// <summary>
        /// Returns the component-wise max of the given vectors.
        /// </summary>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise max vector.</returns>
        public static Vec3D Max(params Vec3D[] vecs)
        {
        	Vec3D max = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X > max.X)
        		{
        			max.X = vecs[i].X;
        		}
        		if(vecs[i].Y > max.Y)
        		{
        			max.Y = vecs[i].Y;
        		}
        		if(vecs[i].Z > max.Z)
        		{
        			max.Z = vecs[i].Z;
        		}
        	}
            return max;
        }
        
        /// <summary>
        /// Returns the component-wise max of the given vectors.
        /// </summary>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise max vector.</returns>
        public static Vec4D Max(params Vec4D[] vecs)
        {
        	Vec4D max = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X > max.X)
        		{
        			max.X = vecs[i].X;
        		}
        		if(vecs[i].Y > max.Y)
        		{
        			max.Y = vecs[i].Y;
        		}
        		if(vecs[i].Z > max.Z)
        		{
        			max.Z = vecs[i].Z;
        		}
        		if(vecs[i].W > max.W)
        		{
        			max.W = vecs[i].W;
        		}
        	}
            return max;
        }
        
        /// <summary>
        /// Returns the component-wise min of the given vectors.
        /// </summary>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise min vector.</returns>
        public static Vec1D Min(params Vec1D[] vecs)
        {
        	Vec1D min = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X < min.X)
        		{
        			min.X = vecs[i].X;
        		}
        	}
            return min;
        }
        
        /// <summary>
        /// Returns the component-wise min of the given vectors.
        /// </summary>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise min vector.</returns>
        public static Vec2D Min(params Vec2D[] vecs)
        {
        	Vec2D min = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X < min.X)
        		{
        			min.X = vecs[i].X;
        		}
        		if(vecs[i].Y < min.Y)
        		{
        			min.Y = vecs[i].Y;
        		}
        	}
            return min;
        }
        
        /// <summary>
        /// Returns the component-wise min of the given vectors.
        /// </summary>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise min vector.</returns>
        public static Vec3D Min(params Vec3D[] vecs)
        {
        	Vec3D min = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X < min.X)
        		{
        			min.X = vecs[i].X;
        		}
        		if(vecs[i].Y < min.Y)
        		{
        			min.Y = vecs[i].Y;
        		}
        		if(vecs[i].Z < min.Z)
        		{
        			min.Z = vecs[i].Z;
        		}
        	}
            return min;
        }
        
        /// <summary>
        /// Returns the component-wise min of the given vectors.
        /// </summary>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise min vector.</returns>
        public static Vec4D Min(params Vec4D[] vecs)
        {
        	Vec4D min = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X < min.X)
        		{
        			min.X = vecs[i].X;
        		}
        		if(vecs[i].Y < min.Y)
        		{
        			min.Y = vecs[i].Y;
        		}
        		if(vecs[i].Z < min.Z)
        		{
        			min.Z = vecs[i].Z;
        		}
        		if(vecs[i].W < min.W)
        		{
        			min.W = vecs[i].W;
        		}
        	}
            return min;
        }
        
        /// <summary>
        /// Returns the component-wise max of the given points.
        /// </summary>
        /// <param name="vecs">The points.</param>
        /// <returns>The component-wise max vector.</returns>
        public static Point1D Max(params Point1D[] vecs)
        {
        	Point1D max = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X > max.X)
        		{
        			max.X = vecs[i].X;
        		}
        	}
            return max;
        }
        
        /// <summary>
        /// Returns the component-wise max of the given points.
        /// </summary>
        /// <param name="vecs">The points.</param>
        /// <returns>The component-wise max vector.</returns>
        public static Point2D Max(params Point2D[] vecs)
        {
        	Point2D max = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X > max.X)
        		{
        			max.X = vecs[i].X;
        		}
        		if(vecs[i].Y > max.Y)
        		{
        			max.Y = vecs[i].Y;
        		}
        	}
            return max;
        }
        
        /// <summary>
        /// Returns the component-wise max of the given points.
        /// </summary>
        /// <param name="vecs">The points.</param>
        /// <returns>The component-wise max vector.</returns>
        public static Point3D Max(params Point3D[] vecs)
        {
        	Point3D max = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X > max.X)
        		{
        			max.X = vecs[i].X;
        		}
        		if(vecs[i].Y > max.Y)
        		{
        			max.Y = vecs[i].Y;
        		}
        		if(vecs[i].Z > max.Z)
        		{
        			max.Z = vecs[i].Z;
        		}
        	}
            return max;
        }
        
        /// <summary>
        /// Returns the component-wise max of the given points.
        /// </summary>
        /// <param name="vecs">The points.</param>
        /// <returns>The component-wise max vector.</returns>
        public static Point4D Max(params Point4D[] vecs)
        {
        	Point4D max = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X > max.X)
        		{
        			max.X = vecs[i].X;
        		}
        		if(vecs[i].Y > max.Y)
        		{
        			max.Y = vecs[i].Y;
        		}
        		if(vecs[i].Z > max.Z)
        		{
        			max.Z = vecs[i].Z;
        		}
        		if(vecs[i].W > max.W)
        		{
        			max.W = vecs[i].W;
        		}
        	}
            return max;
        }
        
        /// <summary>
        /// Returns the component-wise min of the given points.
        /// </summary>
        /// <param name="vecs">The points.</param>
        /// <returns>The component-wise min vector.</returns>
        public static Point1D Min(params Point1D[] vecs)
        {
        	Point1D min = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X < min.X)
        		{
        			min.X = vecs[i].X;
        		}
        	}
            return min;
        }
        
        /// <summary>
        /// Returns the component-wise min of the given points.
        /// </summary>
        /// <param name="vecs">The points.</param>
        /// <returns>The component-wise min vector.</returns>
        public static Point2D Min(params Point2D[] vecs)
        {
        	Point2D min = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X < min.X)
        		{
        			min.X = vecs[i].X;
        		}
        		if(vecs[i].Y < min.Y)
        		{
        			min.Y = vecs[i].Y;
        		}
        	}
            return min;
        }
        
        /// <summary>
        /// Returns the component-wise min of the given points.
        /// </summary>
        /// <param name="vecs">The points.</param>
        /// <returns>The component-wise min vector.</returns>
        public static Point3D Min(params Point3D[] vecs)
        {
        	Point3D min = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X < min.X)
        		{
        			min.X = vecs[i].X;
        		}
        		if(vecs[i].Y < min.Y)
        		{
        			min.Y = vecs[i].Y;
        		}
        		if(vecs[i].Z < min.Z)
        		{
        			min.Z = vecs[i].Z;
        		}
        	}
            return min;
        }
        
        /// <summary>
        /// Returns the component-wise min of the given points.
        /// </summary>
        /// <param name="vecs">The points.</param>
        /// <returns>The component-wise min vector.</returns>
        public static Point4D Min(params Point4D[] vecs)
        {
        	Point4D min = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].X < min.X)
        		{
        			min.X = vecs[i].X;
        		}
        		if(vecs[i].Y < min.Y)
        		{
        			min.Y = vecs[i].Y;
        		}
        		if(vecs[i].Z < min.Z)
        		{
        			min.Z = vecs[i].Z;
        		}
        		if(vecs[i].W < min.W)
        		{
        			min.W = vecs[i].W;
        		}
        	}
            return min;
        }
        
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
        		min = Max(min, rects[i].Min);
        		max = Min(max, rects[i].Max);
        	}
        	return new Rectangle(min, max);
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
        		min = Min(min, rects[i].Min);
        		max = Max(max, rects[i].Max);
        	}
        	return new Rectangle(min, max);
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
        		min = Max(min, views[i].Min);
        		max = Min(max, views[i].Max);
        	}
        	return new Viewport(min, max);
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
        		min = Min(min, views[i].Min);
        		max = Max(max, views[i].Max);
        	}
        	return new Viewport(min, max);
        }
	}
}
