namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Stores a number of extension methods for specialty rendering methods for Images.
	/// </summary>
	public static class Render
	{
		/// <summary>
		/// Fills a circle in this <see cref="Image"/> with the given value.
		/// </summary>
		/// <param name="image">The <see cref="Image"/>.</param>
		/// <param name="c">The center of the circle.</param>
		/// <param name="r">The radius of the circle.</param>
		/// <param name="value">The value.</param>
		/// <param name="aa">True if the circle is anti-aliased.</param>
		public static void FillCircle(this Image image, Point2D c, double r, RGB value, bool aa)
		{
			image.FillEllipse(c, r, r, value, aa);
		}
		
		/// <summary>
		/// Fills an ellipse in this <see cref="Image"/> with the given value.
		/// </summary>
		/// <param name="image">The <see cref="Image"/>.</param>
		/// <param name="c">The center of the ellipse.</param>
		/// <param name="rx">The x radius of the ellipse.</param>
		/// <param name="ry">The y radius of the ellipse.</param>
		/// <param name="value">The value.</param>
		/// <param name="aa">True if the ellipse is anti-aliased.</param>
		public static void FillEllipse(this Image image, Point2D c, double rx, double ry, RGB value, bool aa)
		{
			if(!aa)
			{
				image.FillEllipse(c, (int)rx, (int)ry, value);
				return;
			}
			Point2D half = new Point2D((int)Math.Ceiling(rx), (int)Math.Ceiling(ry));
			Rectangle view = new Rectangle(c - half, c + half);
			view = VectorUtil.Overlap((Rectangle)image.Size, image.GetClip(), view);
			if(view.IsValid())
			{
				Point2D inner = new Point2D((int)(rx / Math.Sqrt(2)), (int)(ry / Math.Sqrt(2)));
				
				image.FillRectangle(new Rectangle(c - inner + 1, c + inner - 1), value);
				
				if(c.Y >= view.Min.Y && c.Y <= view.Max.Y)
				{
					image.FillRightXScan(Math.Max(c.X + inner.X, view.Min.X), Math.Min(c.X + rx, view.Max.X), c.Y, value);
					image.FillLeftXScan(Math.Max(c.X - rx, view.Min.X), Math.Min(c.X - inner.X, view.Max.X), c.Y, value);
				}
				if(c.X >= view.Min.X && c.X < view.Max.X)
				{
					image.FillUpYScan(c.X, Math.Max(c.Y + inner.Y, view.Min.Y), Math.Min(c.Y + ry, view.Max.Y), value);
					image.FillDownYScan(c.X, Math.Max(c.Y - ry, view.Min.Y), Math.Min(c.Y - inner.Y, view.Max.Y), value);
				}
				
				Vec2D rSq = new Vec2D(rx, ry);
				rSq *= rSq;
				
				for(int dy = inner.Y; dy > 0; dy--)
				{
					double dx = Math.Sqrt(1 - ((dy * dy) / rSq.Y)) * rx;
					
					int y = c.Y - dy;
					if(y >= view.Min.Y && y <= view.Max.Y)
					{
						image.FillRightXScan(Math.Max(c.X + inner.X, view.Min.X), Math.Min(c.X + dx, view.Max.X), y, value);
						image.FillLeftXScan(Math.Max(c.X - dx, view.Min.X), Math.Min(c.X - inner.X, view.Max.X), y, value);
					}
					
					y = c.Y + dy;
					if(y >= view.Min.Y && y <= view.Max.Y)
					{
						image.FillRightXScan(Math.Max(c.X + inner.X, view.Min.X), Math.Min(c.X + dx, view.Max.X), y, value);
						image.FillLeftXScan(Math.Max(c.X - dx, view.Min.X), Math.Min(c.X - inner.X, view.Max.X), y, value);
					}
				}
				
				for(int dx = inner.X; dx > 0; dx--)
				{
					double dy = Math.Sqrt(1 - ((dx * dx) / rSq.X)) * ry;
					
					int x = c.X - dx;
					if(x >= view.Min.X && x <= view.Max.X)
					{
						image.FillUpYScan(x, Math.Max(c.Y + inner.Y, view.Min.Y), Math.Min(c.Y + dy, view.Max.Y), value);
						image.FillDownYScan(x, Math.Max(c.Y - dy, view.Min.Y), Math.Min(c.Y - inner.Y, view.Max.Y), value);
					}
					
					x = c.X + dx;
					if(x >= view.Min.X && x <= view.Max.X)
					{
						image.FillUpYScan(x, Math.Max(c.Y + inner.Y, view.Min.Y), Math.Min(c.Y + dy, view.Max.Y), value);
						image.FillDownYScan(x, Math.Max(c.Y - dy, view.Min.Y), Math.Min(c.Y - inner.Y, view.Max.Y), value);
					}
				}
			}
		}
		
		/// <summary>
		/// Fills a rounded <see cref="Rectangle"/> in this <see cref="Image"/> with the given value.
		/// </summary>
		/// <param name="image">The <see cref="Image"/>.</param>
		/// <param name="rect">The <see cref="Rectangle"/> to fill.</param>
		/// <param name = "r">The radius.</param>
		/// <param name="value">The value.</param>
		/// <param name="aa">True if the rect is anti-aliased.</param>
		public static void FillRoundedRectangle(this Image image, Rectangle rect, double r, RGB value, bool aa)
		{
			image.FillRoundedRectangle(rect, r, r, value, aa);
		}
		
		/// <summary>
		/// Fills a rounded <see cref="Rectangle"/> in this <see cref="Image"/> with the given value.
		/// </summary>
		/// <param name="image">The <see cref="Image"/>.</param>
		/// <param name="rect">The <see cref="Rectangle"/> to fill.</param>
		/// <param name = "rx">The x radius.</param>
		/// <param name = "ry">The y radius.</param>
		/// <param name="value">The value.</param>
		/// <param name="aa">True if the rect is anti-aliased.</param>
		public static void FillRoundedRectangle(this Image image, Rectangle rect, double rx, double ry, RGB value, bool aa)
		{
			if(!aa)
			{
				image.FillRoundedRectangle(rect, (int)rx, (int)ry, value);
				return;
			}
			Rectangle view = VectorUtil.Overlap((Rectangle)image.Size, image.GetClip(), rect);
			if(view.IsValid())
			{
				Rectangle small = new Rectangle(rect.Min + new Point2D((int)rx, (int)ry), rect.Max - new Point2D((int)rx, (int)ry));
				Point2D inner = new Point2D((int)(rx / Math.Sqrt(2)), (int)(ry / Math.Sqrt(2)));
				
				image.FillRectangle(new Rectangle(small.Min - inner + 1, small.Max + inner - 1), value);
				
				image.FillRectangle(new Rectangle(new Point2D(small.Max.X, small.Min.Y), new Point2D(rect.Max.X, small.Max.Y)), value);
				image.FillRectangle(new Rectangle(new Point2D(rect.Min.X, small.Min.Y), new Point2D(small.Min.X, small.Max.Y)), value);
				
				image.FillRectangle(new Rectangle(new Point2D(small.Min.X, small.Max.Y), new Point2D(small.Max.X, rect.Max.Y)), value);
				image.FillRectangle(new Rectangle(new Point2D(small.Min.X, rect.Min.Y), new Point2D(small.Max.X, small.Min.Y)), value);
				
				Vec2D rSq = new Vec2D(rx, ry);
				rSq *= rSq;
				
				for(int dy = inner.Y; dy > 0; dy--)
				{
					double dx = Math.Sqrt(1 - ((dy * dy) / rSq.Y)) * rx;
					
					int y = small.Min.Y - dy;
					if(y >= view.Min.Y && y <= view.Max.Y)
					{
						image.FillRightXScan(Math.Max(small.Max.X + inner.X, view.Min.X), Math.Min(small.Max.X + dx, view.Max.X), y, value);
						image.FillLeftXScan(Math.Max(small.Min.X - dx, view.Min.X), Math.Min(small.Min.X - inner.X, view.Max.X), y, value);
					}
					
					y = small.Max.Y + dy;
					if(y >= view.Min.Y && y <= view.Max.Y)
					{
						image.FillRightXScan(Math.Max(small.Max.X + inner.X, view.Min.X), Math.Min(small.Max.X + dx, view.Max.X), y, value);
						image.FillLeftXScan(Math.Max(small.Min.X - dx, view.Min.X), Math.Min(small.Min.X - inner.X, view.Max.X), y, value);
					}
				}
				
				for(int dx = inner.X; dx > 0; dx--)
				{
					double dy = Math.Sqrt(1 - ((dx * dx) / rSq.X)) * ry;
					
					int x = small.Min.X - dx;
					if(x >= view.Min.X && x <= view.Max.X)
					{
						image.FillUpYScan(x, Math.Max(small.Max.Y + inner.Y, view.Min.Y), Math.Min(small.Max.Y + dy, view.Max.Y), value);
						image.FillDownYScan(x, Math.Max(small.Min.Y - dy, view.Min.Y), Math.Min(small.Min.Y - inner.Y, view.Max.Y), value);
					}
					
					x = small.Max.X + dx;
					if(x >= view.Min.X && x <= view.Max.X)
					{
						image.FillUpYScan(x, Math.Max(small.Max.Y + inner.Y, view.Min.Y), Math.Min(small.Max.Y + dy, view.Max.Y), value);
						image.FillDownYScan(x, Math.Max(small.Min.Y - dy, view.Min.Y), Math.Min(small.Min.Y - inner.Y, view.Max.Y), value);
					}
				}
			}
		}
		
		private static void FillRightXScan(this Image image, int x1, double x2, int y, RGB value)
		{
			if(x1 <= x2)
			{
				int ix2 = (int)Math.Ceiling(x2);
				for(int x = x1; x < ix2; x++)
				{
					image[x, y] = value;
				}
				image[ix2, y] &= new ARGB((byte)((1 - (ix2 - x2)) * 255), value);
			}
		}
		
		private static void FillLeftXScan(this Image image, double x1, int x2, int y, RGB value)
		{
			if(x1 <= x2)
			{
				int ix1 = (int)x1;
				for(int x = x2; x > ix1; x--)
				{
					image[x, y] = value;
				}
				image[ix1, y] &= new ARGB((byte)((1 - (x1 - ix1)) * 255), value);
			}
		}
		
		private static void FillUpYScan(this Image image, int x, int y1, double y2, RGB value)
		{
			if(y1 <= y2)
			{
				int iy2 = (int)Math.Ceiling(y2);
				for(int y = y1; y < iy2; y++)
				{
					image[x, y] = value;
				}
				image[x, iy2] &= new ARGB((byte)((1 - (iy2 - y2)) * 255), value);
			}
		}
		
		private static void FillDownYScan(this Image image, int x, double y1, int y2, RGB value)
		{
			if(y1 <= y2)
			{
				int iy1 = (int)y1;
				for(int y = y2; y > iy1; y--)
				{
					image[x, y] = value;
				}
				image[x, iy1] &= new ARGB((byte)((1 - (y1 - iy1)) * 255), value);
			}
		}
	}
}
