namespace IROM.Util
{
	using System;
	using System.Collections.Generic;
	
	/// <summary>
	/// Stores extension methods for DataMaps.
	/// </summary>
	public static class DataMapExtensions2D
	{
		private const bool DisableUnsafe = false;
		
		/// <summary>
		/// The infinite clipping rectangle.
		/// </summary>
		private static readonly Rectangle NoClip = new Rectangle(new Point2D(int.MinValue, int.MinValue), new Point2D(int.MaxValue, int.MaxValue));
		
		/// <summary>
		/// The clipping rectangle instances.
		/// </summary>
		private static readonly Dictionary<object, Rectangle> ClippingInstances = new Dictionary<object, Rectangle>();
		
		/// <summary>
		/// Sets the clipping rectangle for data operations on this <see cref="DataMap2D{T}">DataMap2D</see>. Clear with <see cref="ClearClip"/>.
		/// </summary>
		/// <param name="map">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="clip">The clipping rectangle.</param>
		public static void SetClip<T>(this DataMap2D<T> map, Rectangle clip) where T : struct
		{
			ClippingInstances[map] = clip;
		}
		
		/// <summary>
		/// Gets the clipping rectangle for data operations on this <see cref="DataMap2D{T}">DataMap2D</see>. Clear with <see cref="ClearClip"/>.
		/// </summary>
		/// <param name="map">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <returns>The clip bounds.</returns>
		public static Rectangle GetClip<T>(this DataMap2D<T> map) where T : struct
		{
			Rectangle result;
			if(!ClippingInstances.TryGetValue(map, out result))
			{
				result = NoClip;
			}
			return result;
		}
		
		/// <summary>
		/// Clears the clipping rectangle for data operations on this <see cref="DataMap2D{T}">DataMap2D</see>.
		/// </summary>
		/// <param name="map">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		public static void ClearClip<T>(this DataMap2D<T> map) where T : struct
		{
			ClippingInstances.Remove(map);
		}
		
		/// <summary>
		/// Copys the source <see cref="DataMap2D{T}">DataMap2D</see> to this <see cref="DataMap2D{T}">DataMap2D</see>.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="src">The source <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		public static void Copy<T>(this DataMap2D<T> dest, DataMap2D<T> src) where T : struct
		{
			dest.Blit(src, 0);
		}
		
		/// <summary>
		/// Blits the source <see cref="DataMap2D{T}">DataMap2D</see> to this <see cref="DataMap2D{T}">DataMap2D</see> in the given position.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="src">The source <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="x">The x coord to blit to.</param>
		/// <param name="y">The y coord to blit to.</param>
		public static void Blit<T>(this DataMap2D<T> dest, DataMap2D<T> src, int x, int y) where T : struct
		{
			dest.Blit(src, new Point2D(x, y));
		}
		
		/// <summary>
		/// Blits the source <see cref="DataMap2D{T}">DataMap2D</see> to this <see cref="DataMap2D{T}">DataMap2D</see> in the given position.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="src">The source <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="position">The position to blit to.</param>
		public static void Blit<T>(this DataMap2D<T> dest, DataMap2D<T> src, Point2D position) where T : struct
		{
			Rectangle clip = VectorUtil.Overlap((Rectangle)dest.Size, ((Rectangle)src.Size) + position, dest.GetClip());
			if(clip.IsValid())
			{
				if(!DisableUnsafe && dest.UnsafeOperationsSupported() && src.UnsafeOperationsSupported())
				{
					UnsafeDataMapOperations2D<T>.Blit(clip, dest, src, position);
				}else
				{
					for(int i = clip.Min.X; i <= clip.Max.X; i++)
					{
						for(int j = clip.Min.Y; j <= clip.Max.Y; j++)
						{
							dest[i, j] = src[i - position.X, j - position.Y];
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Fills this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="value">The value.</param>
		public static void Fill<T>(this DataMap2D<T> dest, T value) where T : struct
		{
			Rectangle clip = VectorUtil.Overlap((Rectangle)dest.Size, dest.GetClip());
			if(clip.IsValid())
			{
				if(!DisableUnsafe && dest.UnsafeOperationsSupported())
				{
					UnsafeDataMapOperations2D<T>.Fill(clip, dest, value);
				}else
				{
					for(int i = clip.Min.X; i <= clip.Max.X; i++)
					{
						for(int j = clip.Min.Y; j <= clip.Max.Y; j++)
						{
							dest[i, j] = value;
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Fills a x scan in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="xmin">The min x value for the scan.</param>
		/// <param name="xmax">The max x value for the scan.</param>
		/// <param name="y">The y value for the scan.</param>
		/// <param name="value">The value.</param>
		public static void FillXScan<T>(this DataMap2D<T> dest, int xmin, int xmax, int y, T value) where T : struct
		{
			Rectangle clip = VectorUtil.Overlap((Rectangle)dest.Size, dest.GetClip());
			if(clip.IsValid()) dest.FillXScan(clip, xmin, xmax, y, value);
		}
		
		/// <summary>
		/// Fills a x scan in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value in the given clip.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="clip">The pre-calculated clip.</param>
		/// <param name="xmin">The min x value for the scan.</param>
		/// <param name="xmax">The max x value for the scan.</param>
		/// <param name="y">The y value for the scan.</param>
		/// <param name="value">The value.</param>
		internal static void FillXScan<T>(this DataMap2D<T> dest, Rectangle clip, int xmin, int xmax, int y, T value) where T : struct
		{
			if(!DisableUnsafe && dest.UnsafeOperationsSupported())
			{
				UnsafeDataMapOperations2D<T>.FillXScan(clip, dest, xmin, xmax, y, value);
			}else
			{
				if(y >= clip.Min.Y && y <= clip.Max.Y)
				{
					xmin = Math.Max(xmin, clip.Min.X);
					xmax = Math.Min(xmax, clip.Max.X);
					for(int i = xmin; i <= xmax; i++)
					{
						dest[i, y] = value;
					}
				}
			}
		}
		
		/// <summary>
		/// Fills a y scan in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="x">The x value for the scan.</param>
		/// <param name="ymin">The min y value for the scan.</param>
		/// <param name="ymax">The max y value for the scan.</param>
		/// <param name="value">The value.</param>
		public static void FillYScan<T>(this DataMap2D<T> dest, int x, int ymin, int ymax, T value) where T : struct
		{
			Rectangle clip = VectorUtil.Overlap((Rectangle)dest.Size, dest.GetClip());
			if(clip.IsValid()) dest.FillYScan(clip, x, ymin, ymax, value);
		}
		
		/// <summary>
		/// Fills a y scan in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value in the given clip.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="clip">The pre-calculated clip.</param>
		/// <param name="x">The x value for the scan.</param>
		/// <param name="ymin">The min y value for the scan.</param>
		/// <param name="ymax">The max y value for the scan.</param>
		/// <param name="value">The value.</param>
		internal static void FillYScan<T>(this DataMap2D<T> dest, Rectangle clip, int x, int ymin, int ymax, T value) where T : struct
		{
			if(!DisableUnsafe && dest.UnsafeOperationsSupported())
			{
				UnsafeDataMapOperations2D<T>.FillYScan(clip, dest, x, ymin, ymax, value);
			}else
			{
				if(x >= clip.Min.X && x <= clip.Max.X)
				{
					ymin = Math.Max(ymin, clip.Min.Y);
					ymax = Math.Min(ymax, clip.Max.Y);
					for(int j = ymin; j <= ymax; j++)
					{
						dest[x, j] = value;
					}
				}
			}
		}
		
		/// <summary>
		/// Draws the outline of a <see cref="Rectangle"/> in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="rect">The <see cref="Rectangle"/> to fill.</param>
		/// <param name="value">The value.</param>
		public static void DrawRectangle<T>(this DataMap2D<T> dest, Rectangle rect, T value) where T : struct
		{
			Rectangle clip = VectorUtil.Overlap((Rectangle)dest.Size, dest.GetClip(), rect);
			if(clip.IsValid())
			{
				dest.FillYScan(clip, rect.Min.X, rect.Min.Y, rect.Max.Y, value);
				dest.FillYScan(clip, rect.Max.X, rect.Min.Y, rect.Max.Y, value);
				dest.FillXScan(clip, rect.Min.X, rect.Max.X, rect.Min.Y, value);
				dest.FillXScan(clip, rect.Min.X, rect.Max.X, rect.Max.Y, value);
			}
		}
		
		/// <summary>
		/// Fills a <see cref="Rectangle"/> in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="rect">The <see cref="Rectangle"/> to fill.</param>
		/// <param name="value">The value.</param>
		public static void FillRectangle<T>(this DataMap2D<T> dest, Rectangle rect, T value) where T : struct
		{
			Rectangle clip = VectorUtil.Overlap((Rectangle)dest.Size, dest.GetClip(), rect);
			if(clip.IsValid())
			{
				if(!DisableUnsafe && dest.UnsafeOperationsSupported())
				{
					UnsafeDataMapOperations2D<T>.Fill(clip, dest, value);
				}else
				{
					for(int i = clip.Min.X; i <= clip.Max.X; i++)
					{
						for(int j = clip.Min.Y; j <= clip.Max.Y; j++)
						{
							dest[i, j] = value;
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Draws the outline of a circle in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="c">The center of the circle.</param>
		/// <param name="r">The radius of the circle.</param>
		/// <param name="value">The value.</param>
		public static void DrawCircle<T>(this DataMap2D<T> dest, Point2D c, int r, T value) where T : struct
		{
			dest.DrawEllipse(c, r, r, value);
		}
		
		/// <summary>
		/// Draws the outline of an ellipse in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="c">The center of the ellipse.</param>
		/// <param name="rx">The x radius of the ellipse.</param>
		/// <param name="ry">The y radius of the ellipse.</param>
		/// <param name="value">The value.</param>
		public static void DrawEllipse<T>(this DataMap2D<T> dest, Point2D c, int rx, int ry, T value) where T : struct
		{
			Rectangle clip = new Rectangle(new Point2D(c.X - rx, c.Y - ry), new Point2D(c.X + rx, c.Y + ry));
			clip = VectorUtil.Overlap((Rectangle)dest.Size, dest.GetClip(), clip);
			if(clip.IsValid())
			{
				if(!DisableUnsafe && dest.UnsafeOperationsSupported())
				{
					UnsafeDataMapOperations2D<T>.DrawEllipse(clip, dest, c, rx, ry, value);
				}else
				{
					double rySq = ry * ry;
					int dx;
					int y;
					int left;
					int right;
					int prevLeft = c.X - rx;
					int prevRight = c.X + rx;
					
					//fill edge dots
					if(c.Y >= clip.Min.Y && c.Y <= clip.Max.Y)
					{
						if(prevLeft >= clip.Min.X && prevLeft <= clip.Max.X)
						{
							dest[prevLeft, c.Y] = value;
						}
						if(prevRight >= clip.Min.X && prevRight <= clip.Max.X)
						{
							dest[prevRight, c.Y] = value;
						}
					}
					
					if(c.X >= clip.Min.X && c.X <= clip.Max.X)
					{
						y = c.Y - ry;
						if(y >= clip.Min.Y && y <= clip.Max.Y)
						{
							dest[c.X, y] = value;
						}
						y = c.Y + ry;
						if(y >= clip.Min.Y && y <= clip.Max.Y)
						{
							dest[c.X, y] = value;
						}
					}
					
					for(int dy = 0; dy < ry; dy++, prevLeft = left, prevRight = right)
					{
						//find edges
						dx = (int)(Math.Sqrt(1 - (((dy + 1) * (dy + 1)) / rySq)) * rx);
						
						left = c.X - dx;
						right = c.X + dx;
						
						//fill scans
						y = c.Y - dy;
						dest.FillXScan(clip, prevLeft, left, y, value);
						dest.FillXScan(clip, right, prevRight, y, value);
						y = c.Y + dy;
						dest.FillXScan(clip, prevLeft, left, y, value);
						dest.FillXScan(clip, right, prevRight, y, value);
					}
				}
			}
		}
		
		/// <summary>
		/// Fills a circle in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="c">The center of the circle.</param>
		/// <param name="r">The radius of the circle.</param>
		/// <param name="value">The value.</param>
		public static void FillCircle<T>(this DataMap2D<T> dest, Point2D c, int r, T value) where T : struct
		{
			dest.FillEllipse(c, r, r, value);
		}
		
		/// <summary>
		/// Fills an ellipse in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="c">The center of the ellipse.</param>
		/// <param name="rx">The x radius of the ellipse.</param>
		/// <param name="ry">The y radius of the ellipse.</param>
		/// <param name="value">The value.</param>
		public static void FillEllipse<T>(this DataMap2D<T> dest, Point2D c, int rx, int ry, T value) where T : struct
		{
			Rectangle clip = new Rectangle(new Point2D(c.X - rx, c.Y - ry), new Point2D(c.X + rx, c.Y + ry));
			clip = VectorUtil.Overlap((Rectangle)dest.Size, dest.GetClip(), clip);
			if(clip.IsValid())
			{
				if(!DisableUnsafe && dest.UnsafeOperationsSupported())
				{
					UnsafeDataMapOperations2D<T>.FillEllipse(clip, dest, c, rx, ry, value);
				}else
				{
					double rySq = ry * ry;
					for(int dy = 0; dy <= ry; dy++)
					{
						//find edges
						int dx = (int)(Math.Sqrt(1 - ((dy * dy) / rySq)) * rx);
						
						int left = c.X - dx;
						int right = c.X + dx;
							
						left = Math.Max(left, clip.Min.X);
						right = Math.Min(right, clip.Max.X);
						
						//fill scans
						int y = c.Y - dy;
						dest.FillXScan(clip, left, right, y, value);
						
						if(dy != 0) continue;
						
						y = c.Y + dy;
						dest.FillXScan(clip, left, right, y, value);
					}
				}
			}
		}
		
		/// <summary>
		/// Draws the outline of a rounded <see cref="Rectangle"/> in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="rect">The <see cref="Rectangle"/> to fill.</param>
		/// <param name = "r">The radius.</param>
		/// <param name="value">The value.</param>
		public static void DrawRoundedRectangle<T>(this DataMap2D<T> dest, Rectangle rect, int r, T value) where T : struct
		{
			dest.DrawRoundedRectangle(rect, r, r, value);
		}
		
		/// <summary>
		/// Draws the outline of a rounded <see cref="Rectangle"/> in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="rect">The <see cref="Rectangle"/> to fill.</param>
		/// <param name = "rx">The x radius.</param>
		/// <param name = "ry">The y radius.</param>
		/// <param name="value">The value.</param>
		public static void DrawRoundedRectangle<T>(this DataMap2D<T> dest, Rectangle rect, int rx, int ry, T value) where T : struct
		{
			Rectangle clip = VectorUtil.Overlap((Rectangle)dest.Size, dest.GetClip(), rect);
			if(clip.IsValid())
			{
				if(!DisableUnsafe && dest.UnsafeOperationsSupported())
				{
					UnsafeDataMapOperations2D<T>.DrawRoundedRectangle(clip, dest, rect, rx, ry, value);
				}else
				{
					//draw top and bottom
					dest.FillXScan(clip, rect.Min.X + rx, rect.Max.X - rx, rect.Min.Y, value);
					dest.FillXScan(clip, rect.Min.X + rx, rect.Max.X - rx, rect.Max.Y, value);
					//draw left and right
					dest.FillYScan(clip, rect.Min.X, rect.Min.Y + ry, rect.Max.Y - ry, value);
					dest.FillYScan(clip, rect.Max.X, rect.Min.Y + ry, rect.Max.Y - ry, value);
					
					double rySq = ry * ry;
					int dx = (int)(Math.Sqrt(1 - (1 / rySq)) * rx);
					int y;
					int left;
					int right;
					int prevLeft = rect.Min.X + (rx - dx);
					int prevRight = rect.Max.X - (rx - dx);
					
					for(int dy = 0; dy < ry; dy++, prevLeft = left, prevRight = right)
					{
						//find edges
						dx = (int)(Math.Sqrt(1 - (((dy + 1) * (dy + 1)) / rySq)) * rx);
						
						left = rect.Min.X + (rx - dx);
						right = rect.Max.X - (rx - dx);
						
						//fill scans
						y = rect.Min.Y + (ry - dy);
						dest.FillXScan(clip, prevLeft, left, y, value);
						dest.FillXScan(clip, right, prevRight, y, value);
						y = rect.Max.Y - (ry - dy);
						dest.FillXScan(clip, prevLeft, left, y, value);
						dest.FillXScan(clip, right, prevRight, y, value);
					}
				}
			}
		}
		
		/// <summary>
		/// Fills a rounded <see cref="Rectangle"/> in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="rect">The <see cref="Rectangle"/> to fill.</param>
		/// <param name = "r">The radius.</param>
		/// <param name="value">The value.</param>
		public static void FillRoundedRectangle<T>(this DataMap2D<T> dest, Rectangle rect, int r, T value) where T : struct
		{
			dest.FillRoundedRectangle(rect, r, r, value);
		}
		
		/// <summary>
		/// Fills a rounded <see cref="Rectangle"/> in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="rect">The <see cref="Rectangle"/> to fill.</param>
		/// <param name = "rx">The x radius.</param>
		/// <param name = "ry">The y radius.</param>
		/// <param name="value">The value.</param>
		public static void FillRoundedRectangle<T>(this DataMap2D<T> dest, Rectangle rect, int rx, int ry, T value) where T : struct
		{
			Rectangle clip = VectorUtil.Overlap((Rectangle)dest.Size, dest.GetClip(), rect);
			if(clip.IsValid())
			{
				if(!DisableUnsafe && dest.UnsafeOperationsSupported())
				{
					UnsafeDataMapOperations2D<T>.FillRoundedRectangle(clip, dest, rect, rx, ry, value);
				}else
				{
					//fill center, left and right
					for(int j = Math.Max(rect.Min.Y + ry, clip.Min.Y); j <= Math.Min(rect.Max.Y - ry, clip.Max.Y); j++)
					{
						for(int i = clip.Min.X; i <= clip.Max.X; i++)
						{
							dest[i, j] = value;
						}
					}
					
					double rySq = ry * ry;
					int dx;
					int y;
					int left;
					int right;
					
					for(int dy = 1; dy <= ry; dy++)
					{
						//find edges
						dx = (int)(Math.Sqrt(1 - ((dy * dy) / rySq)) * rx);
						left = rect.Min.X + (rx - dx);
						right = rect.Max.X - (rx - dx);
						
						//fill scans
						y = rect.Min.Y + (ry - dy);
						dest.FillXScan(clip, left, right, y, value);
						y = rect.Max.Y - (ry - dy);
						dest.FillXScan(clip, left, right, y, value);
					}
				}
			}
		}
		
		/// <summary>
		/// Fills a triangle in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name = "scans">The scan storage. Generally should by exclusive to one image.</param>
		/// <param name="v1">The first vertex.</param>
		/// <param name="v2">The second vertex.</param>
		/// <param name="v3">The third vertex.</param>
		/// <param name="value">The value.</param>
		public static void FillTriangle<T>(this DataMap2D<T> dest, ref double[,] scans, T value, Point2D v1, Point2D v2, Point2D v3) where T : struct
		{
			dest.FillPolygon(ref scans, value, v1, v2, v3);
		}
		
		/// <summary>
		/// Fills a polygon in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name = "scans">The scan storage. Generally should by exclusive to one image.</param>
		/// <param name="value">The value.</param>
		/// <param name="verts">The verticies of the polygon.</param>
		public static void FillPolygon<T>(this DataMap2D<T> dest, ref double[,] scans, T value, params Point2D[] verts) where T : struct
		{
			Rectangle clip = VectorUtil.Overlap((Rectangle)dest.Size, dest.GetClip());
			if(clip.IsValid())
			{
				int minY, maxY;
				dest.ScanPolygon(clip, ref scans, out minY, out maxY, verts);
				if(!DisableUnsafe && dest.UnsafeOperationsSupported())
				{
					UnsafeDataMapOperations2D<T>.FillScannedPolygon(clip, dest, scans, minY, maxY, value);
				}else
				{
					for(int y = Math.Max(minY, clip.Min.Y); y <= Math.Min(maxY, clip.Max.Y); y++)
					{
						for(int x = Math.Max((int)Math.Floor(scans[y, 0]), clip.Min.X); x <= Math.Min((int)Math.Ceiling(scans[y, 1]), clip.Max.X); x++)
						{
							dest[x, y] = value;
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Scans the given polygon into the given scan buffer in the given clip.
		/// Stores top and bottom scan indicies in minY and maxY.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{T}">DataMap2D</see> target.</param>
		/// <param name="clip">The clip.</param>
		/// <param name="scans">The scan buffer.</param>
		/// <param name="minY">The y coord of the min scan.</param>
		/// <param name="maxY">The y coord of the max scan.</param>
		/// <param name="verts">The polygon.</param>
		internal static void ScanPolygon<T>(this DataMap2D<T> dest, Rectangle clip, ref double[,] scans, out int minY, out int maxY, params Point2D[] verts) where T : struct
		{
			if(scans == null || scans.GetLength(0) != dest.Height)
			{
				scans = new double[dest.Height, 2];
			}
			for(int i = 0; i < scans.GetLength(0); i++)
			{
				scans[i, 0] = double.PositiveInfinity;
				scans[i, 1] = double.NegativeInfinity;
			}
			minY = int.MaxValue;
			maxY = int.MinValue;
			for(int i = 0; i < verts.Length; i++)
			{
				minY = Math.Min(minY, verts[i].Y);
				maxY = Math.Max(maxY, verts[i].Y);
				ScanLine(clip, scans, verts[i], verts[(i + 1) % verts.Length]);
			}
		}
		
		private static void ScanLine(Rectangle clip, double[,] scans, Point2D p1, Point2D p2)
		{
			if((p1.X < clip.Min.X && p2.X < clip.Min.X) ||
			   (p1.X > clip.Max.X && p2.X > clip.Max.X) ||
			   (p1.Y < clip.Min.Y && p2.Y < clip.Min.Y) ||
			   (p1.Y > clip.Max.Y && p2.Y > clip.Max.Y))
			{
				return;
			}
			if(p1.Y == p2.Y)
			{
				scans[p1.Y, 0] = Math.Min(scans[p1.Y, 0], Math.Min(p1.X, p2.X));
				scans[p1.Y, 1] = Math.Max(scans[p1.Y, 1], Math.Max(p1.X, p2.X));
				return;
			}
			
			//p1.Y always less than p2.Y
			if(p1.Y > p2.Y)
			{
				Util.Swap(ref p1, ref p2);
			}
			
			double x = p1.X;
			double dx = (p2.X - p1.X) / (double)(p2.Y - p1.Y);
			int y = p1.Y;
			//start at beginning
			if(y < clip.Min.Y)
			{
				x += dx * (clip.Min.Y - y);
				y = clip.Min.Y;
			}
			for(; y <= Math.Min(p2.Y, clip.Max.Y); y++, x += dx)
			{
				scans[y, 0] = Math.Min(scans[y, 0], x);
				scans[y, 1] = Math.Max(scans[y, 1], x);
			}
		}
	}
}
