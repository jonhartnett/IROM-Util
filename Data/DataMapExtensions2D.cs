namespace IROM.Util
{
	using System;
	using System.Collections.Generic;
	
	/// <summary>
	/// Stores extension methods for DataMaps.
	/// </summary>
	public static class DataMapExtensions2D
	{
		public enum FillMode
		{
			REPLACE, ADD, SUBTRACT
		}
		
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
			if(ClippingInstances.ContainsKey(map))
			{
				return ClippingInstances[map];
			}else
			{
				return NoClip;
			}
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
		/// <param name="map">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="src">The source <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		public static void Copy<T>(this DataMap2D<T> map, DataMap2D<T> src) where T : struct
		{
			map.Blit(src, 0, 0);
		}
		
		/// <summary>
		/// Blits the source <see cref="DataMap2D{T}">DataMap2D</see> to this <see cref="DataMap2D{T}">DataMap2D</see> in the given position.
		/// </summary>
		/// <param name="map">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="src">The source <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="x">The x coord to blit to.</param>
		/// <param name="y">The y coord to blit to.</param>
		public static void Blit<T>(this DataMap2D<T> map, DataMap2D<T> src, int x, int y) where T : struct
		{
			map.Blit(src, new Point2D(x, y));
		}
		
		/// <summary>
		/// Blits the source <see cref="DataMap2D{T}">DataMap2D</see> to this <see cref="DataMap2D{T}">DataMap2D</see> in the given position.
		/// </summary>
		/// <param name="map">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="src">The source <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="position">The position to blit to.</param>
		public static void Blit<T>(this DataMap2D<T> map, DataMap2D<T> src, Point2D position) where T : struct
		{
			Rectangle view = VectorUtil.Overlap((Rectangle)map.Size, ((Rectangle)src.Size) + position, map.GetClip());
			if(view.IsValid())
			{
				if(map.UnsafeOperationsSupported() && src.UnsafeOperationsSupported())
				{
					UnsafeDataMapOperations2D<T>.Blit(map, src, position, view);
				}else
				{
					for(int i = view.Min.X; i <= view.Max.X; i++)
					{
						for(int j = view.Min.Y; j <= view.Max.Y; j++)
						{
							map[i, j] = src[i - position.X, j - position.Y];
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Fills this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="map">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="value">The value.</param>
		public static void Fill<T>(this DataMap2D<T> map, T value) where T : struct
		{
			Rectangle view = VectorUtil.Overlap((Rectangle)map.Size, map.GetClip());
			if(view.IsValid())
			{
				if(map.UnsafeOperationsSupported())
				{
					UnsafeDataMapOperations2D<T>.Fill(map, value, view);
				}else
				{
					for(int i = view.Min.X; i <= view.Max.X; i++)
					{
						for(int j = view.Min.Y; j <= view.Max.Y; j++)
						{
							map[i, j] = value;
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Fills a x scan in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="map">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="xmin">The min x value for the scan.</param>
		/// <param name="xmax">The max x value for the scan.</param>
		/// <param name="y">The y value for the scan.</param>
		/// <param name="value">The value.</param>
		public static void FillXScan<T>(this DataMap2D<T> map, int xmin, int xmax, int y, T value) where T : struct
		{
			Rectangle view = VectorUtil.Overlap((Rectangle)map.Size, map.GetClip());
			if(view.IsValid())
			{
				if(map.UnsafeOperationsSupported())
				{
					UnsafeDataMapOperations2D<T>.FillXScan(map, xmin, xmax, y, value, view);
				}else
				{
					if(y >= view.Min.Y && y <= view.Max.Y)
					{
						xmin = Math.Max(xmin, view.Min.X);
						xmax = Math.Min(xmax, view.Max.X);
						for(int i = xmin; i <= xmax; i++)
						{
							map[i, y] = value;
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Fills a y scan in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="map">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="x">The x value for the scan.</param>
		/// <param name="ymin">The min y value for the scan.</param>
		/// <param name="ymax">The max y value for the scan.</param>
		/// <param name="value">The value.</param>
		public static void FillYScan<T>(this DataMap2D<T> map, int x, int ymin, int ymax, T value) where T : struct
		{
			Rectangle view = VectorUtil.Overlap((Rectangle)map.Size, map.GetClip());
			if(view.IsValid())
			{
				if(map.UnsafeOperationsSupported())
				{
					UnsafeDataMapOperations2D<T>.FillYScan(map, x, ymin, ymax, value, view);
				}else
				{
					if(x >= view.Min.X && x <= view.Max.X)
					{
						ymin = Math.Max(ymin, view.Min.Y);
						ymax = Math.Min(ymax, view.Max.Y);
						for(int j = ymin; j <= ymax; j++)
						{
							map[x, j] = value;
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Fills a <see cref="Rectangle"/> in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="map">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="rect">The <see cref="Rectangle"/> to fill.</param>
		/// <param name="value">The value.</param>
		public static void FillRectangle<T>(this DataMap2D<T> map, Rectangle rect, T value) where T : struct
		{
			Rectangle view = VectorUtil.Overlap((Rectangle)map.Size, map.GetClip(), rect);
			if(view.IsValid())
			{
				if(map.UnsafeOperationsSupported())
				{
					UnsafeDataMapOperations2D<T>.Fill(map, value, view);
				}else
				{
					for(int i = view.Min.X; i <= view.Max.X; i++)
					{
						for(int j = view.Min.Y; j <= view.Max.Y; j++)
						{
							map[i, j] = value;
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Fills a circle in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="map">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="c">The center of the circle.</param>
		/// <param name="r">The radius of the circle.</param>
		/// <param name="value">The value.</param>
		public static void FillCircle<T>(this DataMap2D<T> map, Point2D c, int r, T value) where T : struct
		{
			map.FillEllipse(c, r, r, value);
		}
		
		/// <summary>
		/// Fills an ellipse in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="map">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="c">The center of the ellipse.</param>
		/// <param name="rx">The x radius of the ellipse.</param>
		/// <param name="ry">The y radius of the ellipse.</param>
		/// <param name="value">The value.</param>
		public static void FillEllipse<T>(this DataMap2D<T> map, Point2D c, int rx, int ry, T value) where T : struct
		{
			Rectangle view = new Rectangle(new Point2D(c.X - rx, c.Y - ry), new Point2D(c.X + rx, c.Y + ry));
			view = VectorUtil.Overlap((Rectangle)map.Size, map.GetClip(), view);
			if(view.IsValid())
			{
				if(map.UnsafeOperationsSupported())
				{
					UnsafeDataMapOperations2D<T>.FillEllipse(map, c, rx, ry, value, view);
				}else
				{
					double rySq = ry * ry;
					for(int dy = 0; dy <= ry; dy++)
					{
						//find edges
						int dx = (int)(Math.Sqrt(1 - ((dy * dy) / rySq)) * rx);
						
						int left = c.X - dx;
						int right = c.X + dx;
							
						left = Math.Max(left, view.Min.X);
						right = Math.Min(right, view.Max.X);
						
						//fill scan
						int y = c.Y - dy;
						if(y >= view.Min.Y && y <= view.Max.Y)
						{
							for(int i = left; i <= right; i++)
							{
								map[i, y] = value;
							}
						}
						y = c.Y + dy;
						if(y >= view.Min.Y && y <= view.Max.Y)
						{
							for(int i = left; i <= right; i++)
							{
								map[i, y] = value;
							}
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Fills a rounded <see cref="Rectangle"/> in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="map">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="rect">The <see cref="Rectangle"/> to fill.</param>
		/// <param name = "r">The radius.</param>
		/// <param name="value">The value.</param>
		public static void FillRoundedRectangle<T>(this DataMap2D<T> map, Rectangle rect, int r, T value) where T : struct
		{
			map.FillRoundedRectangle(rect, r, r, value);
		}
		
		/// <summary>
		/// Fills a rounded <see cref="Rectangle"/> in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="map">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name="rect">The <see cref="Rectangle"/> to fill.</param>
		/// <param name = "rx">The x radius.</param>
		/// <param name = "ry">The y radius.</param>
		/// <param name="value">The value.</param>
		public static void FillRoundedRectangle<T>(this DataMap2D<T> map, Rectangle rect, int rx, int ry, T value) where T : struct
		{
			Rectangle view = VectorUtil.Overlap((Rectangle)map.Size, map.GetClip(), rect);
			if(view.IsValid())
			{
				if(map.UnsafeOperationsSupported())
				{
					UnsafeDataMapOperations2D<T>.FillRoundedRectangle(map, rect, rx, ry, value, view);
				}else
				{
					//fill center, top, and bottom
					for(int i = Math.Max(rect.Min.X + rx, view.Min.X); i <= Math.Min(rect.Max.X - rx, view.Max.X); i++)
					{
						for(int j = Math.Max(rect.Min.Y, view.Min.Y); j <= Math.Min(rect.Max.Y, view.Max.Y); j++)
						{
							map[i, j] = value;
						}
					}
					
					//fill left and right
					for(int j = Math.Max(rect.Min.Y + ry, view.Min.Y); j <= Math.Min(rect.Max.Y - ry, view.Max.Y); j++)
					{
						//fill left
						for(int i = Math.Max(rect.Min.X, view.Min.X); i <= Math.Min(rect.Min.X + rx, view.Max.X); i++)
						{
							map[i, j] = value;
						}
						//fill right
						for(int i = Math.Max(rect.Max.X - rx, view.Min.X); i <= Math.Min(rect.Max.X, view.Max.X); i++)
						{
							map[i, j] = value;
						}
					}
					
					double rySq = ry * ry;
					for(int dy = 1; dy <= ry; dy++)
					{
						//find edges
						int dx = (int)(Math.Sqrt(1 - ((dy * dy) / rySq)) * rx);
						
						//fill scan
						int left;
						int right;
						int y = rect.Min.Y + ry - dy;
						if(y >= view.Min.Y && y <= view.Max.Y)
						{
							right = rect.Min.X + rx;
							left = right - dx;
							right = Math.Min(right, view.Max.X);
							left = Math.Max(left, view.Min.X);
							for(int i = left; i <= right; i++)
							{
								map[i, y] = value;
							}
							left = rect.Max.X - rx;
							right = left + dx;
							right = Math.Min(right, view.Max.X);
							left = Math.Max(left, view.Min.X);
							for(int i = left; i <= right; i++)
							{
								map[i, y] = value;
							}
						}
						y = rect.Max.Y - ry + dy;
						if(y >= view.Min.Y && y < view.Max.Y)
						{
							right = rect.Min.X + rx;
							left = right - dx;
							right = Math.Min(right, view.Max.X);
							left = Math.Max(left, view.Min.X);
							for(int i = left; i <= right; i++)
							{
								map[i, y] = value;
							}
							left = rect.Max.X - rx;
							right = left + dx;
							right = Math.Min(right, view.Max.X);
							left = Math.Max(left, view.Min.X);
							for(int i = left; i <= right; i++)
							{
								map[i, y] = value;
							}
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Fills a triangle in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="map">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name = "scans">The scan storage. Generally should by exclusive to one image.</param>
		/// <param name="v1">The first vertex.</param>
		/// <param name="v2">The second vertex.</param>
		/// <param name="v3">The third vertex.</param>
		/// <param name="value">The value.</param>
		public static void FillTriangle<T>(this DataMap2D<T> map, ref int[,] scans, T value, Point2D v1, Point2D v2, Point2D v3) where T : struct
		{
			map.FillPolygon(ref scans, value, v1, v2, v3);
		}
		
		/// <summary>
		/// Fills a polygon in this <see cref="DataMap2D{T}">DataMap2D</see> with the given value.
		/// </summary>
		/// <param name="map">The <see cref="DataMap2D{T}">DataMap2D</see>.</param>
		/// <param name = "scans">The scan storage. Generally should by exclusive to one image.</param>
		/// <param name="value">The value.</param>
		/// <param name="verts">The verticies of the polygon.</param>
		public static void FillPolygon<T>(this DataMap2D<T> map, ref int[,] scans, T value, params Point2D[] verts) where T : struct
		{
			if(scans == null || scans.GetLength(0) != map.Height)
			{
				scans = new int[map.Height, 2];
			}
			for(int i = 0; i < scans.GetLength(0); i++)
			{
				scans[i, 0] = int.MaxValue;
				scans[i, 1] = int.MinValue;
			}
			int minY = int.MaxValue;
			int maxY = int.MinValue;
			for(int i = 0; i < verts.Length; i++)
			{
				minY = Math.Min(minY, verts[i].Y);
				maxY = Math.Max(maxY, verts[i].Y);
				ScanLine<T>(map, scans, verts[i], verts[(i + 1) % verts.Length]);
			}
			
			Rectangle view = VectorUtil.Overlap((Rectangle)map.Size, map.GetClip());
			if(view.IsValid())
			{
				if(map.UnsafeOperationsSupported())
				{
					UnsafeDataMapOperations2D<T>.FillScannedPolygon(map, scans, minY, maxY, value, view);
				}else
				{
					for(int y = Math.Max(minY, view.Min.Y); y <= Math.Min(maxY, view.Max.Y); y++)
					{
						for(int x = Math.Max(scans[y, 0], view.Min.X); x <= Math.Min(scans[y, 1], view.Max.X); x++)
						{
							map[x, y] = value;
						}
					}
				}
			}
		}
		
		private static void ScanLine<T>(DataMap2D<T> map, int[,] scans, Point2D p1, Point2D p2) where T : struct
		{
			if(p1.Y == p2.Y)
			{
				if(p1.Y >= 0 && p1.Y < map.Height)
				{
					scans[p1.Y, 0] = Math.Min(scans[p1.Y, 0], Math.Min(p1.X, p2.X));
					scans[p1.Y, 1] = Math.Max(scans[p1.Y, 1], Math.Max(p1.X, p2.X));
				}
				return;
			}
			if(p2.Y < p1.Y)
			{
				Util.Swap(ref p1, ref p2);
			}
			double x = p1.X;
			double dx = (p2.X - p1.X + 1) / (double)(p2.Y - p1.Y + 1);
			int y = p1.Y;
			if(y < 0)
			{
				x += dx * -y;
				y = 0;
			}
			for(; y <= Math.Min(p2.Y, map.Height - 1); y++, x += dx)
			{
				scans[y, 0] = Math.Min(scans[y, 0], (int)x);
				scans[y, 1] = Math.Max(scans[y, 1], (int)x);
			}
		}
	}
}
