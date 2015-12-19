namespace IROM.Util
{
	using System;
	using IROM.Util;
	using System.Reflection;
	
	/// <summary>
	/// Stores methods for unsafe data dest operations. 
	/// These type of operations significantly improve 
	/// the speed of <see cref="DataMapExtensions2D"/> methods 
	/// if implemented correctly.
	/// </summary>
	public static class UnsafeDataMapOperations2D<T> where T : struct
	{
		/// <summary>
		/// A delegate that performs unsafe blitting between two <see cref="DataMap2D{T}">DataMap2D</see>.
		/// </summary>
		/// <param name="clip">The rectangle to copy.</param>
		/// <param name="dest">The dest dest.</param>
		/// <param name="src">The src dest.</param>
		/// <param name="position">The position to blit to.</param>
		public delegate void UnsafeBlitDelegate(Rectangle clip, DataMap2D<T> dest, DataMap2D<T> src, Point2D position);
		
		/// <summary>
		/// A delegate that performs unsafe rectangle filling for a <see cref="DataMap2D{T}">DataMap2D</see>.
		/// </summary>
		/// <param name="clip">The rectangle to fill.</param>
		/// <param name="dest">The dest dest.</param>
		/// <param name="value">The fill value.</param>
		public delegate void UnsafeFillDelegate(Rectangle clip, DataMap2D<T> dest, T value);
		
		/// <summary>
		/// A delegate that performs unsafe scan filling for a <see cref="DataMap2D{T}">DataMap2D</see>.
		/// </summary>
		/// <param name="clip">The clip.</param>
		/// <param name="dest">The dest dest.</param>
		/// <param name="xmin">The min x value.</param>
		/// <param name="xmax">The max x value.</param>
		/// <param name="y">The y value.</param>
		/// <param name="value">The fill value.</param>
		public delegate void UnsafeXScanDelegate(Rectangle clip, DataMap2D<T> dest, int xmin, int xmax, int y, T value);
		
		/// <summary>
		/// A delegate that performs unsafe scan filling for a <see cref="DataMap2D{T}">DataMap2D</see>.
		/// </summary>
		/// <param name="clip">The clip.</param>
		/// <param name="dest">The dest dest.</param>
		/// <param name="x">The x value.</param>
		/// <param name="ymin">The min y value.</param>
		/// <param name="ymax">The max y value.</param>
		/// <param name="value">The fill value.</param>
		public delegate void UnsafeYScanDelegate(Rectangle clip, DataMap2D<T> dest, int x, int ymin, int ymax, T value);
		
		/// <summary>
		/// A delegate that performs unsafe ellipse operations for a <see cref="DataMap2D{T}">DataMap2D</see>.
		/// </summary>
		/// <param name="clip">The clip.</param>
		/// <param name="dest">The dest dest.</param>
		/// <param name="c">The center of the circle.</param>
		/// <param name="rx">The x radius of the circle.</param>
		/// <param name="ry">The x radius of the circle.</param>
		/// <param name="value">The fill value.</param>
		public delegate void UnsafeEllipseDelegate(Rectangle clip, DataMap2D<T> dest, Point2D c, int rx, int ry, T value);
		
		/// <summary>
		/// A delegate that performs unsafe rounded rectangle operations for a <see cref="DataMap2D{T}">DataMap2D</see>.
		/// </summary>
		/// <param name="clip">The clip.</param>
		/// <param name="dest">The dest dest.</param>
		/// <param name="rect">The outer bounds of the rectangle.</param>
		/// <param name="rx">The x radius of the curvature.</param>
		/// <param name="ry">The x radius of the curvature.</param>
		/// <param name="value">The fill value.</param>
		public delegate void UnsafeRoundedRectangleDelegate(Rectangle clip, DataMap2D<T> dest, Rectangle rect, int rx, int ry, T value);
		
		/// <summary>
		/// A delegate that performs unsafe pre-scanned polygon filling for a <see cref="DataMap2D{T}">DataMap2D</see>.
		/// </summary>
		/// <param name="clip">The clip.</param>
		/// <param name="dest">The dest dest.</param>
		/// <param name="scans">The pre-scanned polygon scans.</param>
		/// <param name = "ymin">The min y scan.</param>
		/// <param name = "ymax">The max y scan.</param>
		/// <param name="value">The fill value.</param>
		public delegate void UnsafeScannedPolygonDelegate(Rectangle clip, DataMap2D<T> dest, int[,] scans, int ymin, int ymax, T value);
		
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
		private static UnsafeXScanDelegate FillXScanDelegate;
		
		/// <summary>
		/// Delegate for unsafe y scan filling.
		/// </summary>
		private static UnsafeYScanDelegate FillYScanDelegate;
		
		/// <summary>
		/// Delegate for unsafe ellipse outline drawing.
		/// </summary>
		private static UnsafeEllipseDelegate DrawEllipseDelegate;
		
		/// <summary>
		/// Delegate for unsafe ellipse filling.
		/// </summary>
		private static UnsafeEllipseDelegate FillEllipseDelegate;
		
		/// <summary>
		/// Delegate for unsafe rounded rectangle outline drawing.
		/// </summary>
		private static UnsafeRoundedRectangleDelegate DrawRoundedRectangleDelegate;
		
		/// <summary>
		/// Delegate for unsafe rounded rectangle filling.
		/// </summary>
		private static UnsafeRoundedRectangleDelegate FillRoundedRectangleDelegate;
		
		/// <summary>
		/// Delegate for unsafe pre-scanned polygon filling.
		/// </summary>
		private static UnsafeScannedPolygonDelegate FillScannedPolygonDelegate;
		
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
			FillXScanDelegate = (UnsafeXScanDelegate)Delegate.CreateDelegate(typeof(UnsafeXScanDelegate), clazz.GetMethod("FillXScan"));
			FillYScanDelegate = (UnsafeYScanDelegate)Delegate.CreateDelegate(typeof(UnsafeYScanDelegate), clazz.GetMethod("FillYScan"));
			DrawEllipseDelegate = (UnsafeEllipseDelegate)Delegate.CreateDelegate(typeof(UnsafeEllipseDelegate), clazz.GetMethod("DrawEllipse"));
			FillEllipseDelegate = (UnsafeEllipseDelegate)Delegate.CreateDelegate(typeof(UnsafeEllipseDelegate), clazz.GetMethod("FillEllipse"));
			DrawRoundedRectangleDelegate = (UnsafeRoundedRectangleDelegate)Delegate.CreateDelegate(typeof(UnsafeRoundedRectangleDelegate), clazz.GetMethod("DrawRoundedRectangle"));
			FillRoundedRectangleDelegate = (UnsafeRoundedRectangleDelegate)Delegate.CreateDelegate(typeof(UnsafeRoundedRectangleDelegate), clazz.GetMethod("FillRoundedRectangle"));
			FillScannedPolygonDelegate = (UnsafeScannedPolygonDelegate)Delegate.CreateDelegate(typeof(UnsafeScannedPolygonDelegate), clazz.GetMethod("FillScannedPolygon"));
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
		/// Registers the given <see cref="UnsafeXScanDelegate"/>.
		/// </summary>
		/// <param name="del">The delegate to register.</param>
		public static void RegisterFillXScanDelegate(UnsafeXScanDelegate del)
		{
			FillXScanDelegate = del;
		}
		
		/// <summary>
		/// Registers the given <see cref="UnsafeYScanDelegate"/>.
		/// </summary>
		/// <param name="del">The delegate to register.</param>
		public static void RegisterFillYScanDelegate(UnsafeYScanDelegate del)
		{
			FillYScanDelegate = del;
		}
		
		/// <summary>
		/// Registers the given <see cref="UnsafeEllipseDelegate"/>.
		/// </summary>
		/// <param name="del">The delegate to register.</param>
		public static void RegisterDrawEllipseDelegate(UnsafeEllipseDelegate del)
		{
			DrawEllipseDelegate = del;
		}
		
		/// <summary>
		/// Registers the given <see cref="UnsafeEllipseDelegate"/>.
		/// </summary>
		/// <param name="del">The delegate to register.</param>
		public static void RegisterFillEllipseDelegate(UnsafeEllipseDelegate del)
		{
			FillEllipseDelegate = del;
		}
		
		/// <summary>
		/// Registers the given <see cref="UnsafeRoundedRectangleDelegate"/>.
		/// </summary>
		/// <param name="del">The delegate to register.</param>
		public static void RegisterDrawRoundedRectangleDelegate(UnsafeRoundedRectangleDelegate del)
		{
			DrawRoundedRectangleDelegate = del;
		}
		
		/// <summary>
		/// Registers the given <see cref="UnsafeRoundedRectangleDelegate"/>.
		/// </summary>
		/// <param name="del">The delegate to register.</param>
		public static void RegisterFillRoundedRectangleDelegate(UnsafeRoundedRectangleDelegate del)
		{
			FillRoundedRectangleDelegate = del;
		}
		
		/// <summary>
		/// Registers the given <see cref="UnsafeScannedPolygonDelegate"/>.
		/// </summary>
		/// <param name="del">The delegate to register.</param>
		public static void RegisterFillScannedPolygonDelegate(UnsafeScannedPolygonDelegate del)
		{
			FillScannedPolygonDelegate = del;
		}
		
		/// <summary>
		/// Performs an unsafe blit.
		/// </summary>
		/// <param name="clip">The clip.</param>
		/// <param name="dest">The dest dest.</param>
		/// <param name="src">The src dest.</param>
		/// <param name="position">The position to blit to.</param>
		internal static void Blit(Rectangle clip, DataMap2D<T> dest, DataMap2D<T> src, Point2D position)
		{
			BlitDelegate(clip, dest, src, position);
		}
		
		/// <summary>
		/// Performs an unsafe fill.
		/// </summary>
		/// <param name="clip">The clip.</param>
		/// <param name="dest">The dest dest.</param>
		/// <param name="value">The fill value.</param>
		internal static void Fill(Rectangle clip, DataMap2D<T> dest, T value)
		{
			FillDelegate(clip, dest, value);
		}
		
		/// <summary>
		/// Performs an unsafe x scan fill.
		/// </summary>
		/// <param name="clip">The clip.</param>
		/// <param name="dest">The dest dest.</param>
		/// <param name="xmin">The min x value.</param>
		/// <param name="xmax">The max x value.</param>
		/// <param name="y">The y value.</param>
		/// <param name="value">The fill value.</param>
		internal static void FillXScan(Rectangle clip, DataMap2D<T> dest, int xmin, int xmax, int y, T value)
		{
			FillXScanDelegate(clip, dest, xmin, xmax, y, value);
		}
		
		/// <summary>
		/// Performs an unsafe y scan fill.
		/// </summary>
		/// <param name="clip">The clip.</param>
		/// <param name="dest">The dest dest.</param>
		/// <param name="x">The x value.</param>
		/// <param name="ymin">The min y value.</param>
		/// <param name="ymax">The max y value.</param>
		/// <param name="value">The fill value.</param>
		internal static void FillYScan(Rectangle clip, DataMap2D<T> dest, int x, int ymin, int ymax, T value)
		{
			FillYScanDelegate(clip, dest, x, ymin, ymax, value);
		}
		
		/// <summary>
		/// Performs an unsafe ellipse outline draw.
		/// </summary>
		/// <param name="clip">The clip.</param>
		/// <param name="dest">The dest dest.</param>
		/// <param name="c">The center of the circle.</param>
		/// <param name="rx">The x radius of the circle.</param>
		/// <param name="ry">The x radius of the circle.</param>
		/// <param name="value">The fill value.</param>
		internal static void DrawEllipse(Rectangle clip, DataMap2D<T> dest, Point2D c, int rx, int ry, T value)
		{
			DrawEllipseDelegate(clip, dest, c, rx, ry, value);
		}
		
		/// <summary>
		/// Performs an unsafe ellipse fill.
		/// </summary>
		/// <param name="clip">The clip.</param>
		/// <param name="dest">The dest dest.</param>
		/// <param name="c">The center of the circle.</param>
		/// <param name="rx">The x radius of the circle.</param>
		/// <param name="ry">The x radius of the circle.</param>
		/// <param name="value">The fill value.</param>
		internal static void FillEllipse(Rectangle clip, DataMap2D<T> dest, Point2D c, int rx, int ry, T value)
		{
			FillEllipseDelegate(clip, dest, c, rx, ry, value);
		}
		
		/// <summary>
		/// Performs an unsafe rounded rectangle outline draw.
		/// </summary>
		/// <param name="clip">The clip.</param>
		/// <param name="dest">The dest dest.</param>
		/// <param name="rect">The outer bounds of the rectangle.</param>
		/// <param name="rx">The x radius of the curvature.</param>
		/// <param name="ry">The x radius of the curvature.</param>
		/// <param name="value">The fill value.</param>
		internal static void DrawRoundedRectangle(Rectangle clip, DataMap2D<T> dest, Rectangle rect, int rx, int ry, T value)
		{
			DrawRoundedRectangleDelegate(clip, dest, rect, rx, ry, value);
		}
		
		/// <summary>
		/// Performs an unsafe rounded rectangle fill.
		/// </summary>
		/// <param name="clip">The clip.</param>
		/// <param name="dest">The dest dest.</param>
		/// <param name="rect">The outer bounds of the rectangle.</param>
		/// <param name="rx">The x radius of the curvature.</param>
		/// <param name="ry">The x radius of the curvature.</param>
		/// <param name="value">The fill value.</param>
		internal static void FillRoundedRectangle(Rectangle clip, DataMap2D<T> dest, Rectangle rect, int rx, int ry, T value)
		{
			FillRoundedRectangleDelegate(clip, dest, rect, rx, ry, value);
		}
		
		/// <summary>
		/// Performs an unsafe pre-scanned polygon fill.
		/// </summary>
		/// <param name="clip">The clip.</param>
		/// <param name="dest">The dest dest.</param>
		/// <param name="scans">The pre-scanned polygon scans.</param>
		/// <param name = "ymin">The min y scan.</param>
		/// <param name = "ymax">The max y scan.</param>
		/// <param name="value">The fill value.</param>
		internal static void FillScannedPolygon(Rectangle clip, DataMap2D<T> dest, int[,] scans, int ymin, int ymax, T value)
		{
			FillScannedPolygonDelegate(clip, dest, scans, ymin, ymax, value);
		}
		
		private static readonly string DefaultCode = 
		@"namespace IROM.Util
		{
			using System;
			using IROM.Util;
			#NAMESPACE#
			public class Unsafe#NAME#Class
			{
				public unsafe static void Blit(Rectangle clip, DataMap2D<#NAME#> dest, DataMap2D<#NAME#> src, Point2D position)
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
				
				public unsafe static void Fill(Rectangle clip, DataMap2D<#NAME#> dest, #NAME# value)
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
				
				public unsafe static void FillXScan(Rectangle clip, DataMap2D<#NAME#> dest, int xmin, int xmax, int y, #NAME# value)
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
				
				public unsafe static void FillYScan(Rectangle clip, DataMap2D<#NAME#> dest, int x, int ymin, int ymax, #NAME# value)
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
				
				public unsafe static void DrawEllipse(Rectangle clip, DataMap2D<#NAME#> dest, Point2D c, int rx, int ry, #NAME# value)
				{
					#NAME#* destData = (#NAME#*)dest.BeginUnsafeOperation();
					destData += dest.GetRawDataOffset();
					int destStride = dest.GetRawDataStride();
					int destWidth = dest.Width;
					
					#NAME#* destIndex;
					#NAME#* endIndex;
					
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
					
					int scanLeft;
					int scanRight;
					for(int dy = 0; dy < ry; dy++, prevLeft = left, prevRight = right)
					{
						//find edges
						dx = (int)(Math.Sqrt(1 - (((dy + 1) * (dy + 1)) / rySq)) * rx);
						
						left = c.X - dx;
						right = c.X + dx;
						
						//fill scan
						y = c.Y - dy;
						if(y >= clip.Min.Y && y <= clip.Max.Y)
						{
							scanLeft = Math.Max(prevLeft, clip.Min.X);
							scanRight = Math.Min(left, clip.Max.X);
							destIndex = destData + ((scanLeft + (y * destWidth)) * destStride);
							endIndex = destIndex + (destStride * (scanRight - scanLeft + 1));
							while(destIndex != endIndex)
							{
								*destIndex = value;
								destIndex += destStride;
							}
							
							scanLeft = Math.Max(right, clip.Min.X);
							scanRight = Math.Min(prevRight, clip.Max.X);
							destIndex = destData + ((scanLeft + (y * destWidth)) * destStride);
							endIndex = destIndex + (destStride * (scanRight - scanLeft + 1));
							while(destIndex != endIndex)
							{
								*destIndex = value;
								destIndex += destStride;
							}
						}
						
						y = c.Y + dy;
						if(y >= clip.Min.Y && y <= clip.Max.Y)
						{
							scanLeft = Math.Max(prevLeft, clip.Min.X);
							scanRight = Math.Min(left, clip.Max.X);
							destIndex = destData + ((scanLeft + (y * destWidth)) * destStride);
							endIndex = destIndex + (destStride * (scanRight - scanLeft + 1));
							while(destIndex != endIndex)
							{
								*destIndex = value;
								destIndex += destStride;
							}
							
							scanLeft = Math.Max(right, clip.Min.X);
							scanRight = Math.Min(prevRight, clip.Max.X);
							destIndex = destData + ((scanLeft + (y * destWidth)) * destStride);
							endIndex = destIndex + (destStride * (scanRight - scanLeft + 1));
							while(destIndex != endIndex)
							{
								*destIndex = value;
								destIndex += destStride;
							}
						}
					}
					
					dest.EndUnsafeOperation();
				}
				
				public unsafe static void FillEllipse(Rectangle clip, DataMap2D<#NAME#> dest, Point2D c, int rx, int ry, #NAME# value)
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
				
				public unsafe static void DrawRoundedRectangle(Rectangle clip, DataMap2D<#NAME#> dest, Rectangle rect, int rx, int ry, #NAME# value)
				{
					#NAME#* destData = (#NAME#*)dest.BeginUnsafeOperation();
					destData += dest.GetRawDataOffset();
					int destStride = dest.GetRawDataStride();
					int destWidth = dest.Width;
					
					#NAME#* destIndex;
					#NAME#* endIndex;
					
					double rySq = ry * ry;
					int dx = (int)(Math.Sqrt(1 - (1 / rySq)) * rx);
					int y;
					int left;
					int right;
					int prevLeft = rect.Min.X + (rx - dx);
					int prevRight = rect.Max.X - (rx - dx);
					
					//draw top and bottom
					left = Math.Max(rect.Min.X + rx, clip.Min.X);
					right = Math.Min(rect.Max.X - rx + 1, clip.Max.X);
					if(left <= right)
					{
						if(rect.Min.Y >= clip.Min.Y)
						{
							destIndex = destData + ((left + (rect.Min.Y * destWidth)) * destStride);
							endIndex = destIndex + (destStride * (right - left));
						
							while(destIndex != endIndex)
							{
								*destIndex = value;
								destIndex += destStride;
							}
						}
						if(rect.Max.Y <= clip.Max.Y)
						{
							destIndex = destData + ((left + (rect.Max.Y * destWidth)) * destStride);
							endIndex = destIndex + (destStride * (right - left));
						
							while(destIndex != endIndex)
							{
								*destIndex = value;
								destIndex += destStride;
							}
						}
					}
					//draw left and right
					int bottom = Math.Max(rect.Min.Y + ry + 1, clip.Min.Y);
					int top = Math.Min(rect.Max.Y - ry, clip.Max.Y);
					if(bottom <= top)
					{
						if(rect.Min.X >= clip.Min.X)
						{
							destIndex = destData + ((rect.Min.X + (bottom * destWidth)) * destStride);
							endIndex = destIndex + (destStride * destWidth * (top - bottom));
						
							while(destIndex != endIndex)
							{
								*destIndex = value;
								destIndex += destStride * destWidth;
							}
						}
						if(rect.Max.X <= clip.Max.X)
						{
							destIndex = destData + ((rect.Max.X + (bottom * destWidth)) * destStride);
							endIndex = destIndex + (destStride * destWidth * (top - bottom));
						
							while(destIndex != endIndex)
							{
								*destIndex = value;
								destIndex += destStride * destWidth;
							}
						}
					}
					
					int scanLeft;
					int scanRight;
					for(int dy = 0; dy <= ry; dy++, prevLeft = left, prevRight = right)
					{
						//find edges
						dx = (int)(Math.Sqrt(1 - ((dy * dy) / rySq)) * rx);
						left = rect.Min.X + (rx - dx);
						right = rect.Max.X - (rx - dx);
						
						//fill scans
						y = rect.Min.Y + (ry - dy) + 1;
						if(y >= clip.Min.Y && y <= clip.Max.Y)
						{
							scanLeft = Math.Max(prevLeft, clip.Min.X);
							scanRight = Math.Min(left, clip.Max.X);
							if(scanLeft <= scanRight)
							{
								destIndex = destData + ((scanLeft + (y * destWidth)) * destStride);
								endIndex = destIndex + (destStride * (scanRight - scanLeft + 1));
								
								while(destIndex != endIndex)
								{
									*destIndex = value;
									destIndex += destStride;
								}
							}
							
							scanLeft = Math.Max(right, clip.Min.X);
							scanRight = Math.Min(prevRight, clip.Max.X);
							if(scanLeft <= scanRight)
							{
								destIndex = destData + ((scanLeft + (y * destWidth)) * destStride);
								endIndex = destIndex + (destStride * (scanRight - scanLeft + 1));
								
								while(destIndex != endIndex)
								{
									*destIndex = value;
									destIndex += destStride;
								}
							}
						}
						y = rect.Max.Y - (ry - dy) - 1;
						if(y >= clip.Min.Y && y <= clip.Max.Y)
						{
							scanLeft = Math.Max(prevLeft, clip.Min.X);
							scanRight = Math.Min(left, clip.Max.X);
							if(scanLeft <= scanRight)
							{
								destIndex = destData + ((scanLeft + (y * destWidth)) * destStride);
								endIndex = destIndex + (destStride * (scanRight - scanLeft + 1));
								
								while(destIndex != endIndex)
								{
									*destIndex = value;
									destIndex += destStride;
								}
							}
							
							scanLeft = Math.Max(right, clip.Min.X);
							scanRight = Math.Min(prevRight, clip.Max.X);
							if(scanLeft <= scanRight)
							{
								destIndex = destData + ((scanLeft + (y * destWidth)) * destStride);
								endIndex = destIndex + (destStride * (scanRight - scanLeft + 1));
								
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
				
				public unsafe static void FillRoundedRectangle(Rectangle clip, DataMap2D<#NAME#> dest, Rectangle rect, int rx, int ry, #NAME# value)
				{
					#NAME#* destData = (#NAME#*)dest.BeginUnsafeOperation();
					destData += dest.GetRawDataOffset();
					int destStride = dest.GetRawDataStride();
					int destWidth = dest.Width;
					
					#NAME#* destIndex;
					#NAME#* endIndex;
					
					double rySq = ry * ry;
					int dx;
					int y;
					int left;
					int right;
					
					//fill center, left and right
					for(int j = Math.Max(rect.Min.Y + ry, clip.Min.Y); j <= Math.Min(rect.Max.Y - ry, clip.Max.Y); j++)
					{
						left = clip.Min.X;
						right = clip.Max.X;
						
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
					
					for(int dy = 1; dy <= ry; dy++)
					{
						//find edges
						dx = (int)(Math.Sqrt(1 - ((dy * dy) / rySq)) * rx);
						left = rect.Min.X + (rx - dx);
						right = rect.Max.X - (rx - dx);
						
						//fill scans
						y = rect.Min.Y + (ry - dy);
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
						y = rect.Max.Y - (ry - dy);
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
					
					dest.EndUnsafeOperation();
				}
				
				public unsafe static void FillScannedPolygon(Rectangle clip, DataMap2D<#NAME#> dest, int[,] scans, int ymin, int ymax, #NAME# value)
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
