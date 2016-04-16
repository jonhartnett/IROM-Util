namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Storage class for a set of basic rendering methods.
	/// </summary>
	public static class BasicRenderer<T> where T : struct
	{
		// disable once StaticFieldInGenericType
		public static readonly RenderContext<T> Instance;
		static BasicRenderer()
		{
			Instance = new RenderContext<T>();
			Instance.SolidConst = SolidConstRender;
			Instance.SolidCopy = SolidCopyRender;
			Instance.OutlineConst = OutlineConstRender;
			Instance.OutlineCopy = OutlineCopyRender;
		}
		
		/// <summary>
		/// Basic implementation of a constant src solid filling renderer.
		/// </summary>
		/// <param name="clip">The clipping rectangle.</param>
		/// <param name="scanner">The scans to render.</param>
		/// <param name="dest">The map to render to.</param>
		/// <param name="value">The constant value.</param>
		public static void SolidConstRender(Rectangle clip, Scanner scanner, DataMap<T> dest, T value)
		{
			Scanner.Scan scan;
			for(int y = scanner.yMin; y <= scanner.yMax; y++)
			{
				scan = scanner[y];
				scan.min = Math.Max(scan.min, clip.Min.X);
				scan.max = Math.Min(scan.max, clip.Max.X);
				for(int x = (int)Math.Floor(scan.min); x <= (int)Math.Ceiling(scan.max); x++)
				{
					dest[x, y] = value;
				}
			}
		}
		
		/// <summary>
		/// Basic implementation of a map src solid filling renderer.
		/// </summary>
		/// <param name="clip">The clipping rectangle.</param>
		/// <param name="scanner">The scans to render.</param>
		/// <param name="dest">The map to render to.</param>
		/// <param name="src">The source map.</param>
		/// <param name="offset">The offset of the source map.</param>
		public static void SolidCopyRender(Rectangle clip, Scanner scanner, DataMap<T> dest, DataMap<T> src, Point2D offset)
		{
			Scanner.Scan scan;
			for(int y = scanner.yMin; y <= scanner.yMax; y++)
			{
				scan = scanner[y];
				scan.min = Math.Max(scan.min, clip.Min.X);
				scan.max = Math.Min(scan.max, clip.Max.X);
				for(int x = (int)scan.min; x <= (int)scan.max; x++)
				{
					dest[x, y] = src[(x + offset.X) % src.Width, (y + offset.Y) % src.Height];
				}
			}
		}
		
		/// <summary>
		/// Basic implementation of a constant src outlining renderer.
		/// </summary>
		/// <param name="clip">The clipping rectangle.</param>
		/// <param name="scanner">The scans to render.</param>
		/// <param name="dest">The map to render to.</param>
		/// <param name="value">The constant value.</param>
		public static void OutlineConstRender(Rectangle clip, Scanner scanner, DataMap<T> dest, T value)
		{
			double prevLeft = 0;
			double left = 0;
			double prevRight = 0;
			double right = 0;
			double min;
			double max;
			bool isEndScan = false;
			
			for(int y = scanner.yMin; y <= scanner.yMax; y++)
			{
				if(y == scanner.yMin)
				{
					if(scanner.isYMinClipped)
					{
						prevLeft = (scanner[y].min + scanner[y-1].min) / 2;
						prevRight = (scanner[y].max + scanner[y-1].max) / 2;
					}else
					{
						isEndScan = true;
					}
				}else
				{
					prevLeft = left;
					prevRight = right;
				}
				if(y == scanner.yMax && !scanner.isYMaxClipped)
				{
					isEndScan = true;
					left = prevLeft;
					right = prevRight;
				}else
				{
					left = (scanner[y].min + scanner[y+1].min) / 2;
					right = (scanner[y].max + scanner[y+1].max) / 2;
				}
				
				if(isEndScan)
				{
					min = left;
					max = right;
					min = Math.Max(min, clip.Min.X);
					max = Math.Min(max, clip.Max.X);
					for(int x = (int)min; x <= (int)max; x++)
					{
						dest[x, y] = value;
					}
				}else
				{
					min = Math.Min(prevLeft, left);
					max = Math.Max(prevLeft, left);
					min = Math.Max(min, clip.Min.X);
					max = Math.Min(max, clip.Max.X);
					for(int x = (int)min; x <= (int)max; x++)
					{
						dest[x, y] = value;
					}
					
					min = Math.Min(prevRight, right);
					max = Math.Max(prevRight, right);
					min = Math.Max(min, clip.Min.X);
					max = Math.Min(max, clip.Max.X);
					for(int x = (int)min; x <= (int)max; x++)
					{
						dest[x, y] = value;
					}
				}
				isEndScan = false;
			}
		}
		
		/// <summary>
		/// Basic implementation of a map src outlining renderer.
		/// </summary>
		/// <param name="clip">The clipping rectangle.</param>
		/// <param name="scanner">The scans to render.</param>
		/// <param name="dest">The map to render to.</param>
		/// <param name="src">The source map.</param>
		/// <param name="offset">The offset of the source map.</param>
		public static void OutlineCopyRender(Rectangle clip, Scanner scanner, DataMap<T> dest, DataMap<T> src, Point2D offset)
		{
			double prevLeft = 0;
			double left = 0;
			double prevRight = 0;
			double right = 0;
			double min;
			double max;
			bool isEndScan = false;
			
			for(int y = scanner.yMin; y <= scanner.yMax; y++)
			{
				if(y == scanner.yMin)
				{
					if(scanner.isYMinClipped)
					{
						prevLeft = (scanner[y].min + scanner[y-1].min) / 2;
						prevRight = (scanner[y].max + scanner[y-1].max) / 2;
					}else
					{
						isEndScan = true;
					}
				}else
				{
					prevLeft = left;
					prevRight = right;
				}
				if(y == scanner.yMax && !scanner.isYMaxClipped)
				{
					isEndScan = true;
					left = prevLeft;
					right = prevRight;
				}else
				{
					left = (scanner[y].min + scanner[y+1].min) / 2;
					right = (scanner[y].max + scanner[y+1].max) / 2;
				}
				
				if(isEndScan)
				{
					min = left;
					max = right;
					min = Math.Max(min, clip.Min.X);
					max = Math.Min(max, clip.Max.X);
					for(int x = (int)min; x <= (int)max; x++)
					{
						dest[x, y] = src[(x + offset.X) % src.Width, (y + offset.Y) % src.Height];
					}
				}else
				{
					min = Math.Min(prevLeft, left);
					max = Math.Max(prevLeft, left);
					min = Math.Max(min, clip.Min.X);
					max = Math.Min(max, clip.Max.X);
					for(int x = (int)min; x <= (int)max; x++)
					{
						dest[x, y] = src[(x + offset.X) % src.Width, (y + offset.Y) % src.Height];
					}
					
					min = Math.Min(prevRight, right);
					max = Math.Max(prevRight, right);
					min = Math.Max(min, clip.Min.X);
					max = Math.Min(max, clip.Max.X);
					for(int x = (int)min; x <= (int)max; x++)
					{
						dest[x, y] = src[(x + offset.X) % src.Width, (y + offset.Y) % src.Height];
					}
				}
				isEndScan = false;
			}
		}
	}
}
