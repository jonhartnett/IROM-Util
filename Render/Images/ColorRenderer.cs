namespace IROM.Util
{
	using System;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// Enum for color combination modes.
	/// </summary>
	public enum ColorMode
	{
		NORMAL, BLEND, MASK
	}
	
	/// <summary>
	/// Storage class for a set of advanced rendering methods for color datamaps.
	/// </summary>
	public unsafe static class ColorRenderer
	{
		// disable once StaticFieldInGenericType
		public static readonly ColorRenderContext Instance;
		static ColorRenderer()
		{
			Instance = new ColorRenderContext();
			Instance.ColorSolidConst = SolidConstRender;
			Instance.ColorSolidCopy = SolidCopyRender;
			Instance.ColorOutlineConst = null;//OutlineConstRender;
			Instance.ColorOutlineCopy = null;//OutlineCopyRender;
		}
		
		//custom implementations of basic math functions to remove unnecessary fluff like isNAN checks.
		private static double Max(double x, double y)
		{
			return x >= y ? x : y;
		}
		private static double Min(double x, double y)
		{
			return x <= y ? x : y;
		}
		private static int Max(int x, int y)
		{
			return x >= y ? x : y;
		}
		private static int Min(int x, int y)
		{
			return x <= y ? x : y;
		}
		
		/// <summary>
		/// Basic implementation of a constant src solid filling renderer.
		/// </summary>
		/// <param name="clip">The clipping rectangle.</param>
		/// <param name="scanner">The scans to render.</param>
		/// <param name="dest">The map to render to.</param>
		/// <param name="value">The constant value.</param>
		/// <param name="mode">The color blending mode to use.</param>
		/// <param name="isAA">True if anti-aliasing is enabled.</param>
		public static void SolidConstRender(Rectangle clip, Scanner scanner, DataMap<ARGB> dest, ARGB value, ColorMode mode = ColorMode.NORMAL, bool isAA = true)
		{
			//for constants, we can early return on blend and mask when alpha is 0
			if((mode == ColorMode.BLEND || mode == ColorMode.MASK) && value.A == 0) return;
			//mask at non-zero is equivalent to normal
			if(mode == ColorMode.MASK) mode = ColorMode.NORMAL;
			//blend at opaque is equivalent to normal
			if(mode == ColorMode.BLEND && value.A == 255) mode = ColorMode.NORMAL;
			
			IUnsafeMap uDest = dest as IUnsafeMap;
			if(uDest == null)
			{
				throw new InvalidOperationException("Destination image does not support advanced color rendering!");
			}
			byte* addr = uDest.BeginUnsafeOperation();
			int width = dest.Width;
			int stride = uDest.GetStride();
			
			for(int y = scanner.yMin; y <= scanner.yMax; y++)
			{
				double x1 = Max(scanner[y].min, clip.Min.X);
				double x2 = Min(scanner[y].max, clip.Max.X);
				//Anti-Aliasing variables
				//x1 vars are x min side
				//x2 vars are x max side
				//xb vars are y min side
				//xa vars are y max side
				double x1b = 0, x1a = 0;
				double x2b = 0, x2a = 0;
				int left;
				int right;
				if(isAA)
				{
					bool hasMin = false, hasMax = false;
					//if not at ymin or ymin is a clipped min (aka we have another "ghost" scan)
					if(y > scanner.yMin || scanner.isYMinClipped)
					{
						x1b = Max(scanner[y - 1].min, clip.Min.X);
						x2b = Min(scanner[y - 1].max, clip.Max.X);
						x1b = (x1b + x1) / 2;
						x2b = (x2b + x2) / 2;
						hasMin = true;
					}
					//if not at ymax or ymax is a clipped max (aka we have another "ghost" scan)
					if(y < scanner.yMax || scanner.isYMaxClipped)
					{
						x1a = Max(scanner[y + 1].min, clip.Min.X);
						x2a = Min(scanner[y + 1].max, clip.Max.X);
						x1a = (x1a + x1) / 2;
						x2a = (x2a + x2) / 2;
						hasMax = true;
					}
					
					if(!hasMin)
					{
						//if single line shape, just make everything literal
						if(!hasMax)
						{
							x1b = x1a = x1;
							x2b = x2a = x2;
						}else//if at ymin, extrapolate max->current into current->min
						{
							x1b = x1 - (x1a - x1);
							x2b = x2 - (x2a - x2);
						}
					}else//if at ymax, extrapolate min->current into current->max
					if(!hasMax)
					{
						x1a = x1 + (x1 - x1b);
						x2a = x2 + (x2 - x2b);
					}
	
					
					//if aa, round toward center
					left = (int)Math.Ceiling(Max(x1b, x1a));
					right = (int)Math.Floor(Min(x2b, x2a));
				}else
				{
					//if not aa, round away from center to match default renderer
					left = (int)Math.Floor(x1);
					right = (int)Math.Ceiling(x2);
				}
				//if valid center
				if(left <= right)
				{
					//solid fill with blend modes
					byte* ptr = addr + (left + (y * width)) * stride;
					byte* endPtr = ptr + (right - left + 1) * stride;
					switch(mode)
					{
						case ColorMode.BLEND:
						{
							//precalc value
							ARGB preValue = new ARGB{A = (byte)(255 - value.A),
					        						 R = (byte)((value.R * value.A) >> 8),
					        	                	 G = (byte)((value.G * value.A) >> 8),
					        	                	 B = (byte)((value.B * value.A) >> 8)};
							while(ptr != endPtr)
							{
								//*((ARGB*)ptr) &= value;
								//manually inline blending
								ARGB color = *(ARGB*)ptr;
								color = new ARGB{A = (byte)(value.A + color.A),
					        					 R = (byte)(preValue.R + ((color.R * preValue.A) >> 8)),
					        	                 G = (byte)(preValue.G + ((color.G * preValue.A) >> 8)),
					        	                 B = (byte)(preValue.B + ((color.B * preValue.A) >> 8))};
								*(ARGB*)ptr = color;
								ptr += stride;
							}
							break;
						}
						case ColorMode.NORMAL:
						{
							while(ptr != endPtr)
							{
								*((ARGB*)ptr) = value;
								ptr += stride;
							}
							break;
						}
					}
				}
				//if aa, add smoothing to edges
				if(isAA)
				{
					int center = (left + right) / 2;
					if(Min(x1b, x1a) != left)
						AAEdgeConst(addr, width, stride, x1b, x1a, y, value, true, clipMax: Min(center, clip.Max.X));
					if(Max(x2b, x2a) != right)
						AAEdgeConst(addr, width, stride, x2b + 1, x2a + 1, y, value, false, clipMin: Max(center + 1, clip.Min.X));
				}
			}
			
			uDest.EndUnsafeOperation();
		}
		
		private static void AAEdgeConst(byte* addr, int width, int stride, double x1, double x2, int y, ARGB value, bool left, int clipMin = int.MinValue, int clipMax = int.MaxValue)
		{
			if(x1 > x2) Util.Swap(ref x1, ref x2);
			
			addr += (y * width * stride);
			double xmin;
			double xmax = x1;
			double ymin;
			double ymax = 1;
			double dydx = -1 / (x2 - x1);
			if(x2 == x1) dydx = 0;
			double alpha;
			do
			{
				xmin = xmax;
				xmax = Min((int)xmin + 1, x2);
				ymin = ymax;
				ymax = ymin + (xmax - xmin) * dydx;
				if(xmin < clipMin) continue;
				if(xmin > clipMax) break;
				alpha = (xmin - (int)xmin) * ymin;
				alpha += (ymax + ymin) * (xmax - xmin) / 2;
				*(ARGB*)(addr + ((int)xmin * stride)) &= new ARGB((byte)(value.A * (left ? (1 - alpha) : alpha)), value.RGB);
			}while(xmax != x2);
		}
		
		/// <summary>
		/// Basic implementation of a map src solid filling renderer.
		/// </summary>
		/// <param name="clip">The clipping rectangle.</param>
		/// <param name="scanner">The scans to render.</param>
		/// <param name="dest">The map to render to.</param>
		/// <param name="src">The source map.</param>
		/// <param name="offset">The offset of the source map.</param>
		/// <param name="mode">The color blending mode to use.</param>
		/// <param name="isAA">True if anti-aliasing is enabled.</param>
		public static void SolidCopyRender(Rectangle clip, Scanner scanner, DataMap<ARGB> dest, DataMap<ARGB> src, Point2D offset, ColorMode mode = ColorMode.NORMAL, bool isAA = true)
		{
			IUnsafeMap uDest = dest as IUnsafeMap;
			IUnsafeMap uSrc = src as IUnsafeMap;
			if(uDest == null)
			{
				throw new InvalidOperationException("Destination image does not support advanced color rendering!");
			}
			if(uSrc == null)
			{
				throw new InvalidOperationException("Source image does not support advanced color rendering!");
			}
			byte* addr = uDest.BeginUnsafeOperation();
			int width = dest.Width;
			int stride = uDest.GetStride();
			byte* srcAddr = uSrc.BeginUnsafeOperation();
			int srcWidth = src.Width;
			int srcStride = uSrc.GetStride();
			
			for(int y = scanner.yMin; y <= scanner.yMax; y++)
			{
				double x1 = Max(scanner[y].min, clip.Min.X);
				double x2 = Min(scanner[y].max, clip.Max.X);
				//Anti-Aliasing variables
				//x1 vars are x min side
				//x2 vars are x max side
				//xb vars are y min side
				//xa vars are y max side
				double x1b = 0, x1a = 0;
				double x2b = 0, x2a = 0;
				int left;
				int right;
				if(isAA)
				{
					bool hasMin = false, hasMax = false;
					//if not at ymin or ymin is a clipped min (aka we have another "ghost" scan)
					if(y > scanner.yMin || scanner.isYMinClipped)
					{
						x1b = Max(scanner[y - 1].min, clip.Min.X);
						x2b = Min(scanner[y - 1].max, clip.Max.X);
						x1b = (x1b + x1) / 2;
						x2b = (x2b + x2) / 2;
						hasMin = true;
					}
					//if not at ymax or ymax is a clipped max (aka we have another "ghost" scan)
					if(y < scanner.yMax || scanner.isYMaxClipped)
					{
						x1a = Max(scanner[y + 1].min, clip.Min.X);
						x2a = Min(scanner[y + 1].max, clip.Max.X);
						x1a = (x1a + x1) / 2;
						x2a = (x2a + x2) / 2;
						hasMax = true;
					}
					
					if(!hasMin)
					{
						//if single line shape, just make everything literal
						if(!hasMax)
						{
							x1b = x1a = x1;
							x2b = x2a = x2;
						}else//if at ymin, extrapolate max->current into current->min
						{
							x1b = x1 - (x1a - x1);
							x2b = x2 - (x2a - x2);
						}
					}else//if at ymax, extrapolate min->current into current->max
					if(!hasMax)
					{
						x1a = x1 + (x1 - x1b);
						x2a = x2 + (x2 - x2b);
					}
	
					
					//if aa, round toward center, otherwise round away to match default renderer
					left = (int)Math.Ceiling(Max(x1b, x1a));
					right = (int)Math.Floor(Min(x2b, x2a));
				}else
				{
					//if not aa, round away to match default renderer
					left = (int)Math.Floor(x1);
					right = (int)Math.Ceiling(x2);
				}
				
				//if valid center
				if(left <= right)
				{
					//solid fill with blend modes
					byte* ptr = addr + (left + (y * width)) * stride;
					byte* srcPtr = srcAddr + ((left + offset.X) + ((y + offset.Y) * srcWidth)) * srcStride;
					byte* endPtr = ptr + (right - left + 1) * stride;
					switch(mode)
					{
						case ColorMode.BLEND:
						{
							while(ptr != endPtr)
							{
								//*((ARGB*)ptr) &= *((ARGB*)srcPtr);
								//manually inline blending
								ARGB srcColor = *(ARGB*)srcPtr;
								ARGB color = *(ARGB*)ptr;
								int aComp = 255 - srcColor.A;
								color = new ARGB{A = (byte)(srcColor.A + color.A),
												 R = (byte)(((srcColor.R * srcColor.A) + (color.R * aComp)) >> 8),
					        	                 G = (byte)(((srcColor.G * srcColor.A) + (color.G * aComp)) >> 8),
					        	                 B = (byte)(((srcColor.B * srcColor.A) + (color.B * aComp)) >> 8)};
								*(ARGB*)ptr = color;
								ptr += stride;
								srcPtr += srcStride;
							}
							break;
						}
						case ColorMode.NORMAL:
						{
							while(ptr != endPtr)
							{
								*((ARGB*)ptr) = *((ARGB*)srcPtr);
								ptr += stride;
								srcPtr += srcStride;
							}
							break;
						}
						case ColorMode.MASK:
						{
							while(ptr != endPtr)
							{
								if((*((ARGB*)srcPtr)).A != 0)
									*((ARGB*)ptr) = *((ARGB*)srcPtr);
								ptr += stride;
								srcPtr += srcStride;
							}
							break;
						}
					}
				}
				//if aa, add smoothing to edges
				if(isAA)
				{
					int center = (left + right) / 2;
					if(Min(x1b, x1a) != left)
						AAEdgeCopy(addr, width, stride, x1b, x1a, y, srcAddr, srcWidth, srcStride, offset, true, clipMax: Min(center, clip.Max.X));
					if(Max(x2b, x2a) != right)
						AAEdgeCopy(addr, width, stride, x2b + 1, x2a + 1, y, srcAddr, srcWidth, srcStride, offset, false, clipMin: Max(center + 1, clip.Min.X));
				}
			}
			
			uDest.EndUnsafeOperation();
			uSrc.EndUnsafeOperation();
		}
		
		private static void AAEdgeCopy(byte* addr, int width, int stride, double x1, double x2, int y, byte* srcAddr, int srcWidth, int srcStride, Point2D offset, bool left, int clipMin = int.MinValue, int clipMax = int.MaxValue)
		{
			if(x1 > x2) Util.Swap(ref x1, ref x2);
			
			addr += (y * width * stride);
			srcAddr += ((y + offset.Y) * srcWidth * stride);
			double xmin;
			double xmax = x1;
			double ymin;
			double ymax = 1;
			double dydx = -1 / (x2 - x1);
			if(x2 == x1) dydx = 0;
			double alpha;
			do
			{
				xmin = xmax;
				xmax = Min((int)xmin + 1, x2);
				ymin = ymax;
				ymax = ymin + (xmax - xmin) * dydx;
				if(xmin < clipMin) continue;
				if(xmin > clipMax) break;
				alpha = (xmin - (int)xmin) * ymin;
				alpha += (ymax + ymin) * (xmax - xmin) / 2;
				ARGB color = *(ARGB*)(srcAddr + (((int)xmin + offset.X) * srcStride));
				*(ARGB*)(addr + ((int)xmin * stride)) &= new ARGB((byte)(color.A * (left ? (1 - alpha) : alpha)), color.RGB);
			}while(xmax != x2);
		}
	}
}
