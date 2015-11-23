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
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise max vector.</returns>
        public static Vec1D<T> Max<T>(params Vec1D<T>[] vecs) where T : struct
        {
        	Vec1D<T> max = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(Operator<T>.GreaterThan(vecs[i].X, max.X))
        		{
        			max.X = vecs[i].X;
        		}
        	}
            return max;
        }
        
        /// <summary>
        /// Returns the component-wise max of the given vectors.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise max vector.</returns>
        public static Vec2D<T> Max<T>(params Vec2D<T>[] vecs) where T : struct
        {
        	Vec2D<T> max = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(Operator<T>.GreaterThan(vecs[i].X, max.X))
        		{
        			max.X = vecs[i].X;
        		}
        		if(Operator<T>.GreaterThan(vecs[i].Y, max.Y))
        		{
        			max.Y = vecs[i].Y;
        		}
        	}
            return max;
        }
        
        /// <summary>
        /// Returns the component-wise max of the given vectors.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise max vector.</returns>
        public static Vec3D<T> Max<T>(params Vec3D<T>[] vecs) where T : struct
        {
        	Vec3D<T> max = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(Operator<T>.GreaterThan(vecs[i].X, max.X))
        		{
        			max.X = vecs[i].X;
        		}
        		if(Operator<T>.GreaterThan(vecs[i].Y, max.Y))
        		{
        			max.Y = vecs[i].Y;
        		}
        		if(Operator<T>.GreaterThan(vecs[i].Z, max.Z))
        		{
        			max.Z = vecs[i].Z;
        		}
        	}
            return max;
        }
        
        /// <summary>
        /// Returns the component-wise max of the given vectors.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise max vector.</returns>
        public static Vec4D<T> Max<T>(params Vec4D<T>[] vecs) where T : struct
        {
        	Vec4D<T> max = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(Operator<T>.GreaterThan(vecs[i].X, max.X))
        		{
        			max.X = vecs[i].X;
        		}
        		if(Operator<T>.GreaterThan(vecs[i].Y, max.Y))
        		{
        			max.Y = vecs[i].Y;
        		}
        		if(Operator<T>.GreaterThan(vecs[i].Z, max.Z))
        		{
        			max.Z = vecs[i].Z;
        		}
        		if(Operator<T>.GreaterThan(vecs[i].W, max.W))
        		{
        			max.W = vecs[i].W;
        		}
        	}
            return max;
        }
        
        /// <summary>
        /// Returns the component-wise min of the given vectors.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise max vector.</returns>
        public static Vec1D<T> Min<T>(params Vec1D<T>[] vecs) where T : struct
        {
            Vec1D<T> min = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(Operator<T>.LessThan(vecs[i].X, min.X))
        		{
        			min.X = vecs[i].X;
        		}
        	}
            return min;
        }
        
        /// <summary>
        /// Returns the component-wise min of the given vectors.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise max vector.</returns>
        public static Vec2D<T> Min<T>(params Vec2D<T>[] vecs) where T : struct
        {
            Vec2D<T> min = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(Operator<T>.LessThan(vecs[i].X, min.X))
        		{
        			min.X = vecs[i].X;
        		}
        		if(Operator<T>.LessThan(vecs[i].Y, min.Y))
        		{
        			min.Y = vecs[i].Y;
        		}
        	}
            return min;
        }
        
        /// <summary>
        /// Returns the component-wise min of the given vectors.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise max vector.</returns>
        public static Vec3D<T> Min<T>(params Vec3D<T>[] vecs) where T : struct
        {
            Vec3D<T> min = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(Operator<T>.LessThan(vecs[i].X, min.X))
        		{
        			min.X = vecs[i].X;
        		}
        		if(Operator<T>.LessThan(vecs[i].Y, min.Y))
        		{
        			min.Y = vecs[i].Y;
        		}
        		if(Operator<T>.LessThan(vecs[i].Z, min.Z))
        		{
        			min.Z = vecs[i].Z;
        		}
        	}
            return min;
        }

        /// <summary>
        /// Returns the component-wise min of the given vectors.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="vecs">The vectors.</param>
        /// <returns>The component-wise max vector.</returns>
        public static Vec4D<T> Min<T>(params Vec4D<T>[] vecs) where T : struct
        {
            Vec4D<T> min = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(Operator<T>.LessThan(vecs[i].X, min.X))
        		{
        			min.X = vecs[i].X;
        		}
        		if(Operator<T>.LessThan(vecs[i].Y, min.Y))
        		{
        			min.Y = vecs[i].Y;
        		}
        		if(Operator<T>.LessThan(vecs[i].Z, min.Z))
        		{
        			min.Z = vecs[i].Z;
        		}
        		if(Operator<T>.LessThan(vecs[i].W, min.W))
        		{
        			min.W = vecs[i].W;
        		}
        	}
            return min;
        }
        
        /// <summary>
        /// Returns the overlap of the given <see cref="Rectangle{T}">Rectangle</see>s.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="rects">The rectangles.</param>
        /// <returns>The overlapping <see cref="Rectangle{T}">Rectangle</see>.</returns>
        public static Rectangle<T> Overlap<T>(params Rectangle<T>[] rects) where T : struct
        {
        	Vec2D<T> min = rects[0].Min;
        	Vec2D<T> max = rects[0].Max;
        	for(int i = 1; i < rects.Length; i++)
        	{
        		min = VectorUtil.Max(min, rects[i].Min);
        		max = VectorUtil.Min(max, rects[i].Max);
        	}
        	return new Rectangle<T>(min, max);
        }
        
        /// <summary>
        /// Returns the encompassing <see cref="Rectangle{T}">Rectangle</see> of the given <see cref="Rectangle{T}">Rectangle</see>s.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="rects">The rectangles.</param>
        /// <returns>The encompassing <see cref="Rectangle{T}">Rectangle</see>.</returns>
        public static Rectangle<T> Encompass<T>(params Rectangle<T>[] rects) where T : struct
        {
        	Vec2D<T> min = rects[0].Min;
        	Vec2D<T> max = rects[0].Max;
        	for(int i = 1; i < rects.Length; i++)
        	{
        		min = VectorUtil.Min(min, rects[i].Min);
        		max = VectorUtil.Max(max, rects[i].Max);
        	}
        	return new Rectangle<T>(min, max);
        }
	}
}
