namespace IROM.Util
{
	using System;
	using IROM.Util;
	using System.Reflection;
	
	/// <summary>
	/// Stores methods for unsafe data map operations. 
	/// These type of operations significantly improve 
	/// the speed of <see cref="DataMapExtensions2D"/> methods 
	/// if implemented correctly.
	/// </summary>
	public static class UnsafeDataMapOperations2D<T> where T : struct
	{
		/// <summary>
		/// A delegate that performs unsafe blitting between two <see cref="DataMap2D{T}">DataMap2D</see>.
		/// </summary>
		/// <param name="dest">The dest map.</param>
		/// <param name="src">The src map.</param>
		/// <param name="position">The position to blit to.</param>
		/// <param name="clip">The rectangle to copy.</param>
		public delegate void UnsafeBlitDelegate(DataMap2D<T> dest, DataMap2D<T> src, Point2D position, Rectangle clip);
		
		/// <summary>
		/// A delegate that performs unsafe rectangle filling for a <see cref="DataMap2D{T}">DataMap2D</see>.
		/// </summary>
		/// <param name="dest">The dest map.</param>
		/// <param name="value">The fill value.</param>
		/// <param name="clip">The rectangle to fill.</param>
		public delegate void UnsafeFillDelegate(DataMap2D<T> dest, T value, Rectangle clip);
		
		/// <summary>
		/// A delegate that performs unsafe scan filling for a <see cref="DataMap2D{T}">DataMap2D</see>.
		/// </summary>
		/// <param name="dest">The dest map.</param>
		/// <param name="xmin">The min x value.</param>
		/// <param name="xmax">The max x value.</param>
		/// <param name="y">The y value.</param>
		/// <param name="value">The fill value.</param>
		/// <param name="clip">The clip.</param>
		public delegate void UnsafeFillXScanDelegate(DataMap2D<T> dest, int xmin, int xmax, int y, T value, Rectangle clip);
		
		/// <summary>
		/// A delegate that performs unsafe scan filling for a <see cref="DataMap2D{T}">DataMap2D</see>.
		/// </summary>
		/// <param name="dest">The dest map.</param>
		/// <param name="x">The x value.</param>
		/// <param name="ymin">The min y value.</param>
		/// <param name="ymax">The max y value.</param>
		/// <param name="value">The fill value.</param>
		/// <param name="clip">The clip.</param>
		public delegate void UnsafeFillYScanDelegate(DataMap2D<T> dest, int x, int ymin, int ymax, T value, Rectangle clip);
		
		/// <summary>
		/// A delegate that performs unsafe ellipse filling for a <see cref="DataMap2D{T}">DataMap2D</see>.
		/// </summary>
		/// <param name="dest">The dest map.</param>
		/// <param name="c">The center of the circle.</param>
		/// <param name="rx">The x radius of the circle.</param>
		/// <param name="ry">The x radius of the circle.</param>
		/// <param name="value">The fill value.</param>
		/// <param name="clip">The clip.</param>
		public delegate void UnsafeFillEllipseDelegate(DataMap2D<T> dest, Point2D c, int rx, int ry, T value, Rectangle clip);
		
		/// <summary>
		/// A delegate that performs unsafe rounded rectangle filling for a <see cref="DataMap2D{T}">DataMap2D</see>.
		/// </summary>
		/// <param name="dest">The dest map.</param>
		/// <param name="rect">The outer bounds of the rectangle.</param>
		/// <param name="rx">The x radius of the curvature.</param>
		/// <param name="ry">The x radius of the curvature.</param>
		/// <param name="value">The fill value.</param>
		/// <param name="clip">The clip.</param>
		public delegate void UnsafeFillRoundedRectangleDelegate(DataMap2D<T> dest, Rectangle rect, int rx, int ry, T value, Rectangle clip);
		
		/// <summary>
		/// A delegate that performs unsafe pre-scanned polygon filling for a <see cref="DataMap2D{T}">DataMap2D</see>.
		/// </summary>
		/// <param name="dest">The dest map.</param>
		/// <param name="scans">The pre-scanned polygon scans.</param>
		/// <param name = "ymin">The min y scan.</param>
		/// <param name = "ymax">The max y scan.</param>
		/// <param name="value">The fill value.</param>
		/// <param name="clip">The clip.</param>
		public delegate void UnsafeFillScannedPolygonDelegate(DataMap2D<T> dest, int[,] scans, int ymin, int ymax, T value, Rectangle clip);
		
		/// <summary>
		/// Delegate for unsafe blitting.
		/// </summary>
		// disable StaticFieldInGenericType
		private static UnsafeBlitDelegate BlitDelegate;
		
		/// <summary>
		/// Delegate for unsafe filling.
		/// </summary>
		private static UnsafeFillDelegate FillDelegate;
		
		/// <summary>
		/// Delegate for unsafe x scan filling.
		/// </summary>
		private static UnsafeFillXScanDelegate FillXScanDelegate;
		
		/// <summary>
		/// Delegate for unsafe y scan filling.
		/// </summary>
		private static UnsafeFillYScanDelegate FillYScanDelegate;
		
		/// <summary>
		/// Delegate for unsafe ellipse filling.
		/// </summary>
		private static UnsafeFillEllipseDelegate FillEllipseDelegate;
		
		/// <summary>
		/// Delegate for unsafe rounded rectangle filling.
		/// </summary>
		private static UnsafeFillRoundedRectangleDelegate FillRoundedRectangleDelegate;
		
		/// <summary>
		/// Delegate for unsafe pre-scanned polygon filling.
		/// </summary>
		private static UnsafeFillScannedPolygonDelegate FillScannedPolygonDelegate;
		
		static UnsafeDataMapOperations2D()
		{
			try
			{
				typeof(T).MakePointerType();
			}catch(Exception)
			{
				return;
			}
			
			string space;
			if(typeof(T).Namespace != "System" && typeof(T).Namespace != "IROM.Util")
			{
				space = string.Format("	using {0};\n", typeof(T).Namespace);
			}else
			{
				space = "";
			}
			
			string specificCode = DefaultCode.Replace("#NAME#", typeof(T).Name).Replace("#NAMESPACE#", space);
			
			Assembly assembly = RuntimeCompiler.Compile(specificCode, new string[]{typeof(T).Assembly.GetName().Name + ".dll", typeof(UnsafeDataMapOperations2D<>).Assembly.GetName().Name + ".dll"});
			
			Type clazz = assembly.GetType(string.Format("IROM.Util.Unsafe{0}Class", typeof(T).Name));
			
			BlitDelegate = (UnsafeBlitDelegate)Delegate.CreateDelegate(typeof(UnsafeBlitDelegate), clazz.GetMethod("Blit"));
			FillDelegate = (UnsafeFillDelegate)Delegate.CreateDelegate(typeof(UnsafeFillDelegate), clazz.GetMethod("Fill"));
			FillXScanDelegate = (UnsafeFillXScanDelegate)Delegate.CreateDelegate(typeof(UnsafeFillXScanDelegate), clazz.GetMethod("FillXScan"));
			FillYScanDelegate = (UnsafeFillYScanDelegate)Delegate.CreateDelegate(typeof(UnsafeFillYScanDelegate), clazz.GetMethod("FillYScan"));
			FillEllipseDelegate = (UnsafeFillEllipseDelegate)Delegate.CreateDelegate(typeof(UnsafeFillEllipseDelegate), clazz.GetMethod("FillEllipse"));
			FillRoundedRectangleDelegate = (UnsafeFillRoundedRectangleDelegate)Delegate.CreateDelegate(typeof(UnsafeFillRoundedRectangleDelegate), clazz.GetMethod("FillRoundedRectangle"));
			FillScannedPolygonDelegate = (UnsafeFillScannedPolygonDelegate)Delegate.CreateDelegate(typeof(UnsafeFillScannedPolygonDelegate), clazz.GetMethod("FillScannedPolygon"));
		}
		
		/// <summary>
		/// Registers the given <see cref="UnsafeBlitDelegate"/>.
		/// </summary>
		/// <param name="del">The delegate to register.</param>
		public static void RegisterBlitDelegate(UnsafeBlitDelegate del)
		{
			BlitDelegate = del;
		}
		
		/// <summary>
		/// Registers the given <see cref="UnsafeFillDelegate"/>.
		/// </summary>
		/// <param name="del">The delegate to register.</param>
		public static void RegisterFillDelegate(UnsafeFillDelegate del)
		{
			FillDelegate = del;
		}
		
		/// <summary>
		/// Registers the given <see cref="UnsafeFillXScanDelegate"/>.
		/// </summary>
		/// <param name="del">The delegate to register.</param>
		public static void RegisterFillXScanDelegate(UnsafeFillXScanDelegate del)
		{
			FillXScanDelegate = del;
		}
		
		/// <summary>
		/// Registers the given <see cref="UnsafeFillYScanDelegate"/>.
		/// </summary>
		/// <param name="del">The delegate to register.</param>
		public static void RegisterFillYScanDelegate(UnsafeFillYScanDelegate del)
		{
			FillYScanDelegate = del;
		}
		
		/// <summary>
		/// Registers the given <see cref="UnsafeFillEllipseDelegate"/>.
		/// </summary>
		/// <param name="del">The delegate to register.</param>
		public static void RegisterFillEllipseDelegate(UnsafeFillEllipseDelegate del)
		{
			FillEllipseDelegate = del;
		}
		
		/// <summary>
		/// Registers the given <see cref="UnsafeFillRoundedRectangleDelegate"/>.
		/// </summary>
		/// <param name="del">The delegate to register.</param>
		public static void RegisterFillRoundedRectangleDelegate(UnsafeFillRoundedRectangleDelegate del)
		{
			FillRoundedRectangleDelegate = del;
		}
		
		/// <summary>
		/// Registers the given <see cref="UnsafeFillScannedPolygonDelegate"/>.
		/// </summary>
		/// <param name="del">The delegate to register.</param>
		public static void RegisterFillScannedPolygonDelegate(UnsafeFillScannedPolygonDelegate del)
		{
			FillScannedPolygonDelegate = del;
		}
		
		/// <summary>
		/// Performs an unsafe blit.
		/// </summary>
		/// <param name="dest">The dest map.</param>
		/// <param name="src">The src map.</param>
		/// <param name="position">The position to blit to.</param>
		/// <param name="clip">The clip.</param>
		internal static void Blit(DataMap2D<T> dest, DataMap2D<T> src, Point2D position, Rectangle clip)
		{
			BlitDelegate(dest, src, position, clip);
		}
		
		/// <summary>
		/// Performs an unsafe fill.
		/// </summary>
		/// <param name="dest">The dest map.</param>
		/// <param name="value">The fill value.</param>
		/// <param name="clip">The clip.</param>
		internal static void Fill(DataMap2D<T> dest, T value, Rectangle clip)
		{
			FillDelegate(dest, value, clip);
		}
		
		/// <summary>
		/// Performs an unsafe x scan fill.
		/// </summary>
		/// <param name="dest">The dest map.</param>
		/// <param name="xmin">The min x value.</param>
		/// <param name="xmax">The max x value.</param>
		/// <param name="y">The y value.</param>
		/// <param name="value">The fill value.</param>
		/// <param name="clip">The clip.</param>
		internal static void FillXScan(DataMap2D<T> dest, int xmin, int xmax, int y, T value, Rectangle clip)
		{
			FillXScanDelegate(dest, xmin, xmax, y, value, clip);
		}
		
		/// <summary>
		/// Performs an unsafe y scan fill.
		/// </summary>
		/// <param name="dest">The dest map.</param>
		/// <param name="x">The x value.</param>
		/// <param name="ymin">The min y value.</param>
		/// <param name="ymax">The max y value.</param>
		/// <param name="value">The fill value.</param>
		/// <param name="clip">The clip.</param>
		internal static void FillYScan(DataMap2D<T> dest, int x, int ymin, int ymax, T value, Rectangle clip)
		{
			FillYScanDelegate(dest, x, ymin, ymax, value, clip);
		}
		
		/// <summary>
		/// Performs an unsafe ellipse fill.
		/// </summary>
		/// <param name="dest">The dest map.</param>
		/// <param name="c">The center of the circle.</param>
		/// <param name="rx">The x radius of the circle.</param>
		/// <param name="ry">The x radius of the circle.</param>
		/// <param name="value">The fill value.</param>
		/// <param name="region">The region.</param>
		internal static void FillEllipse(DataMap2D<T> dest, Point2D c, int rx, int ry, T value, Rectangle region)
		{
			FillEllipseDelegate(dest, c, rx, ry, value, region);
		}
		
		/// <summary>
		/// Performs an unsafe rounded rectangle fill.
		/// </summary>
		/// <param name="dest">The dest map.</param>
		/// <param name="rect">The outer bounds of the rectangle.</param>
		/// <param name="rx">The x radius of the curvature.</param>
		/// <param name="ry">The x radius of the curvature.</param>
		/// <param name="value">The fill value.</param>
		/// <param name="region">The region.</param>
		internal static void FillRoundedRectangle(DataMap2D<T> dest, Rectangle rect, int rx, int ry, T value, Rectangle region)
		{
			FillRoundedRectangleDelegate(dest, rect, rx, ry, value, region);
		}
		
		/// <summary>
		/// Performs an unsafe pre-scanned polygon fill.
		/// </summary>
		/// <param name="dest">The dest map.</param>
		/// <param name="scans">The pre-scanned polygon scans.</param>
		/// <param name = "ymin">The min y scan.</param>
		/// <param name = "ymax">The max y scan.</param>
		/// <param name="value">The fill value.</param>
		/// <param name="region">The region.</param>
		internal static void FillScannedPolygon(DataMap2D<T> dest, int[,] scans, int ymin, int ymax, T value, Rectangle region)
		{
			FillScannedPolygonDelegate(dest, scans, ymin, ymax, value, region);
		}
		
		private static readonly string DefaultCode = 
		@"namespace IROM.Util
		{
			using System;
			using IROM.Util;
			#NAMESPACE#
			public class Unsafe#NAME#Class
			{
				public unsafe static void Blit(DataMap2D<#NAME#> dest, DataMap2D<#NAME#> src, Point2D position, Rectangle clip)
				{
					#NAME#* destData = (#NAME#*)dest.BeginUnsafeOperation();
					#NAME#* srcData = (#NAME#*)src.BeginUnsafeOperation();
					destData += dest.GetRawDataOffset();
					srcData += src.GetRawDataOffset();
					int destStride = dest.GetRawDataStride();
					int srcStride = src.GetRawDataStride();
					int destWidth = dest.Width;
					int srcWidth = src.Width;
					
					#NAME#* destIndex;
					#NAME#* srcIndex;
					#NAME#* endIndex;
					
					for(int j = clip.Min.Y; j <= clip.Max.Y; j++)
					{
						destIndex = destData + ((clip.Min.X + (j * destWidth)) * destStride);
						srcIndex = srcData + (((clip.Min.X - position.X) + ((j - position.Y) * srcWidth)) * srcStride);
						endIndex = destIndex + (destStride * clip.Width);
						while(destIndex != endIndex)
						{
							*destIndex = *srcIndex;
							destIndex += destStride;
							srcIndex += srcStride;
						}
					}
					dest.EndUnsafeOperation();
					src.EndUnsafeOperation();
				}
				
				public unsafe static void Fill(DataMap2D<#NAME#> dest, #NAME# value, Rectangle clip)
				{
					#NAME#* destData = (#NAME#*)dest.BeginUnsafeOperation();
					destData += dest.GetRawDataOffset();
					int destStride = dest.GetRawDataStride();
					int destWidth = dest.Width;
					
					#NAME#* destIndex;
					#NAME#* endIndex;
					
					for(int j = clip.Min.Y; j <= clip.Max.Y; j++)
					{
						destIndex = destData + ((clip.Min.X + (j * destWidth)) * destStride);
						endIndex = destIndex + (destStride * clip.Width);
						while(destIndex != endIndex)
						{
							*destIndex = value;
							destIndex += destStride;
						}
					}
					
					dest.EndUnsafeOperation();
				}
				
				public unsafe static void FillXScan(DataMap2D<#NAME#> dest, int xmin, int xmax, int y, #NAME# value, Rectangle clip)
				{
					#NAME#* destData = (#NAME#*)dest.BeginUnsafeOperation();
					destData += dest.GetRawDataOffset();
					int destStride = dest.GetRawDataStride();
					int destWidth = dest.Width;
					
					#NAME#* destIndex;
					#NAME#* endIndex;
					
					xmin = Math.Max(xmin, clip.Min.X);
					xmax = Math.Min(xmax, clip.Max.X);
					
					if(xmin <= xmax)
					{
						destIndex = destData + ((xmin + (y * destWidth)) * destStride);
						endIndex = destIndex + (destStride * (xmax - xmin));
					
						while(destIndex != endIndex)
						{
							*destIndex = value;
							destIndex += destStride;
						}
					}
						
					dest.EndUnsafeOperation();
				}
				
				public unsafe static void FillYScan(DataMap2D<#NAME#> dest, int x, int ymin, int ymax, #NAME# value, Rectangle clip)
				{
					#NAME#* destData = (#NAME#*)dest.BeginUnsafeOperation();
					destData += dest.GetRawDataOffset();
					int destStride = dest.GetRawDataStride();
					int destWidth = dest.Width;
					
					#NAME#* destIndex;
					#NAME#* endIndex;
					
					ymin = Math.Max(ymin, clip.Min.Y);
					ymax = Math.Min(ymax, clip.Max.Y);
					
					if(ymin <= ymax)
					{
						destIndex = destData + ((x + (ymin * destWidth)) * destStride);
						endIndex = destIndex + (destStride * destWidth * (ymax - ymin));
					
						while(destIndex != endIndex)
						{
							*destIndex = value;
							destIndex += destStride * destWidth;
						}
					}
						
					dest.EndUnsafeOperation();
				}
				
				public unsafe static void FillEllipse(DataMap2D<#NAME#> dest, Point2D c, int rx, int ry, #NAME# value, Rectangle clip)
				{
					#NAME#* destData = (#NAME#*)dest.BeginUnsafeOperation();
					destData += dest.GetRawDataOffset();
					int destStride = dest.GetRawDataStride();
					int destWidth = dest.Width;
					
					#NAME#* destIndex;
					#NAME#* endIndex;
					
					double rySq = ry * ry;
					for(int dy = 0; dy <= ry; dy++)
					{
						//find edges
						int dx = (int)(Math.Sqrt(1 - ((dy * dy) / rySq)) * rx);
						
						int left = c.X - dx;
						int right = c.X + dx;
							
						left = Math.Max(left, clip.Min.X);
						right = Math.Min(right, clip.Max.X);
						
						//fill scan
						if(left <= right)
						{
							int y = c.Y - dy;
							if(y >= clip.Min.Y && y <= clip.Max.Y)
							{
								destIndex = destData + ((left + (y * destWidth)) * destStride);
								endIndex = destIndex + (destStride * (right - left + 1));
								
								while(destIndex != endIndex)
								{
									*destIndex = value;
									destIndex += destStride;
								}
							}
							y = c.Y + dy;
							if(y >= clip.Min.Y && y <= clip.Max.Y)
							{
								destIndex = destData + ((left + (y * destWidth)) * destStride);
								endIndex = destIndex + (destStride * (right - left + 1));
								while(destIndex != endIndex)
								{
									*destIndex = value;
									destIndex += destStride;
								}
							}
						}
					}
					
					dest.EndUnsafeOperation();
				}
				
				public unsafe static void FillRoundedRectangle(DataMap2D<#NAME#> dest, Rectangle rect, int rx, int ry, #NAME# value, Rectangle clip)
				{
					#NAME#* destData = (#NAME#*)dest.BeginUnsafeOperation();
					destData += dest.GetRawDataOffset();
					int destStride = dest.GetRawDataStride();
					int destWidth = dest.Width;
					
					#NAME#* destIndex;
					#NAME#* endIndex;
					
					int left;
					int right;
					
					//fill center, top, and bottom
					for(int j = rect.Min.Y; j <= rect.Max.Y; j++)
					{
						if(j < clip.Min.Y) continue;
						if(j > clip.Max.Y) break;
						
						left = Math.Max(rect.Min.X + rx, clip.Min.X);
						right = Math.Min(rect.Max.X - rx, clip.Max.X);
						
						if(left < right)
						{
							destIndex = destData + ((left + (j * destWidth)) * destStride);
							endIndex = destIndex + (destStride * (right - left + 1));
						
							while(destIndex != endIndex)
							{
								*destIndex = value;
								destIndex += destStride;
							}
						}
					}
					
					//fill left and right
					for(int j = rect.Min.Y + ry; j <= rect.Max.Y - ry; j++)
					{
						if(j < clip.Min.Y) continue;
						if(j > clip.Max.Y) break;
						
						//fill left
						left = Math.Max(rect.Min.X, clip.Min.X);
						right = Math.Min(rect.Min.X + rx, clip.Max.X);
						
						if(left <= right)
						{
							destIndex = destData + ((left + (j * destWidth)) * destStride);
							endIndex = destIndex + (destStride * (right - left + 1));
						
							while(destIndex != endIndex)
							{
								*destIndex = value;
								destIndex += destStride;
							}
						}
						
						//fill right
						left = Math.Max(rect.Max.X - rx, clip.Min.X);
						right = Math.Min(rect.Max.X, clip.Max.X);
						
						if(left <= right)
						{
							destIndex = destData + ((left + (j * destWidth)) * destStride);
							endIndex = destIndex + (destStride * (right - left + 1));
							
							while(destIndex != endIndex)
							{
								*destIndex = value;
								destIndex += destStride;
							}
						}
					}
					
					double rySq = ry * ry;
					for(int dy = 1; dy <= ry; dy++)
					{
						//find edges
						int dx = (int)(Math.Sqrt(1 - ((dy * dy) / rySq)) * rx);
						
						//fill corners
						int y = rect.Min.Y + ry - dy;
						if(y >= clip.Min.Y && y <= clip.Max.Y)
						{
							right = rect.Min.X + rx;
							left = right - dx;
							right = Math.Min(right, clip.Max.X);
							left = Math.Max(left, clip.Min.X);
							
							if(left <= right)
							{
								destIndex = destData + ((left + (y * destWidth)) * destStride);
								endIndex = destIndex + (destStride * (right - left + 1));
								
								while(destIndex != endIndex)
								{
									*destIndex = value;
									destIndex += destStride;
								}
							}
							
							left = rect.Max.X - rx;
							right = left + dx;
							right = Math.Min(right, clip.Max.X);
							left = Math.Max(left, clip.Min.X);
							
							if(left <= right)
							{
								destIndex = destData + ((left + (y * destWidth)) * destStride);
								endIndex = destIndex + (destStride * (right - left + 1));
								
								while(destIndex != endIndex)
								{
									*destIndex = value;
									destIndex += destStride;
								}
							}
						}
						y = rect.Max.Y - ry + dy;
						if(y >= clip.Min.Y && y < clip.Max.Y)
						{
							right = rect.Min.X + rx;
							left = right - dx;
							right = Math.Min(right, clip.Max.X);
							left = Math.Max(left, clip.Min.X);
							
							if(left <= right)
							{
								destIndex = destData + ((left + (y * destWidth)) * destStride);
								endIndex = destIndex + (destStride * (right - left + 1));
							
								while(destIndex != endIndex)
								{
									*destIndex = value;
									destIndex += destStride;
								}
							}
							
							left = rect.Max.X - rx;
							right = left + dx;
							right = Math.Min(right, clip.Max.X);
							left = Math.Max(left, clip.Min.X);
							
							if(left <= right)
							{
								destIndex = destData + ((left + (y * destWidth)) * destStride);
								endIndex = destIndex + (destStride * (right - left + 1));
								
								while(destIndex != endIndex)
								{
									*destIndex = value;
									destIndex += destStride;
								}
							}
						}
					}
					
					dest.EndUnsafeOperation();
				}
				
				public unsafe static void FillScannedPolygon(DataMap2D<#NAME#> dest, int[,] scans, int ymin, int ymax, #NAME# value, Rectangle clip)
				{
					#NAME#* destData = (#NAME#*)dest.BeginUnsafeOperation();
					destData += dest.GetRawDataOffset();
					int destStride = dest.GetRawDataStride();
					int destWidth = dest.Width;
					
					#NAME#* destIndex;
					#NAME#* endIndex;
					
					int left;
					int right;
					
					ymin = Math.Max(ymin, clip.Min.Y);
					ymax = Math.Min(ymax, clip.Max.Y);
					for(int j = ymin; j <= ymax; j++)
					{
						left = Math.Max(scans[j, 0], clip.Min.X);
						right = Math.Min(scans[j, 1], clip.Max.X);
						
						if(left <= right)
						{
							destIndex = destData + ((left + (j * destWidth)) * destStride);
							endIndex = destIndex + (destStride * (right - left + 1));
						
							while(destIndex != endIndex)
							{
								*destIndex = value;
								destIndex += destStride;
							}
						}
					}
						
					dest.EndUnsafeOperation();
				}
			}
		}";
	}
}
