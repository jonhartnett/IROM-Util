namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Modes for image rendering.
	/// </summary>
	public enum RenderMode
	{
		/// <summary>
		/// Replaces dest with src.
		/// </summary>
		NORMAL, 
		/// <summary>
		/// Blends src with dest.
		/// </summary>
		BLEND, 
		/// <summary>
		/// Masks dest with src.
		/// </summary>
		MASK
	}
	
	/// <summary>
	/// Stores a number of extension methods for specialty rendering methods for DataMap2D<ARGB>s.
	/// </summary>
	public static class Render
	{
		/// <summary>
		/// Copies the given <see cref="DataMap2D{ARGB}"/> onto this <see cref="DataMap2D{ARGB}"/> with the given render mode.
		/// </summary>
		/// <param name="dest">The src <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name="src">The dest <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name="mode">The render mode.</param>
		public static void Copy(this DataMap2D<ARGB> dest, DataMap2D<ARGB> src, RenderMode mode)
		{
			dest.Blit(src, 0, 0, mode);
		}
		
		/// <summary>
		/// Blits the source <see cref="DataMap2D{ARGB}"/> to this <see cref="DataMap2D{ARGB}"/> in the given position with the given render mode.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name="src">The source <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name="x">The x coord to blit to.</param>
		/// <param name="y">The y coord to blit to.</param>
		/// <param name="mode">The render mode.</param>
		public static void Blit(this DataMap2D<ARGB> dest, DataMap2D<ARGB> src, int x, int y, RenderMode mode)
		{
			dest.Blit(src, new Point2D(x, y), mode);
		}
		
		/// <summary>
		/// Blits the source <see cref="DataMap2D{ARGB}"/> to this <see cref="DataMap2D{ARGB}"/> in the given position with the given render mode.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name="src">The source <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name="position">The position to blit to.</param>
		/// <param name="mode">The render mode.</param>
		public unsafe static void Blit(this DataMap2D<ARGB> dest, DataMap2D<ARGB> src, Point2D position, RenderMode mode)
		{
			if(mode == RenderMode.NORMAL)
			{
				dest.Blit(src, position);
				return;
			}
			
			Rectangle clip = VectorUtil.Overlap((Rectangle)dest.Size, ((Rectangle)src.Size) + position, dest.GetClip());
			if(clip.IsValid())
			{
				ARGB* destData = (ARGB*)dest.BeginUnsafeOperation();
				ARGB* srcData = (ARGB*)src.BeginUnsafeOperation();
				destData += dest.GetRawDataOffset();
				srcData += src.GetRawDataOffset();
				int destStride = dest.GetRawDataStride();
				int srcStride = src.GetRawDataStride();
				int destWidth = dest.Width;
				int srcWidth = src.Width;
				
				ARGB* destIndex;
				ARGB* srcIndex;
				ARGB* endIndex;
				
				for(int j = clip.Min.Y; j <= clip.Max.Y; j++)
				{
					destIndex = destData + ((clip.Min.X + (j * destWidth)) * destStride);
					srcIndex = srcData + (((clip.Min.X - position.X) + ((j - position.Y) * srcWidth)) * srcStride);
					endIndex = destIndex + (destStride * clip.Width);
					
					if(mode == RenderMode.BLEND)
					{
						while(destIndex != endIndex)
						{
							*destIndex &= *srcIndex;
							destIndex += destStride;
							srcIndex += srcStride;
						}
					}else
					if(mode == RenderMode.MASK)
					{
						while(destIndex != endIndex)
						{
							if((*srcIndex).A != 0)
								*destIndex = *srcIndex;
							destIndex += destStride;
							srcIndex += srcStride;
						}
					}
				}
				dest.EndUnsafeOperation();
				src.EndUnsafeOperation();
			}
		}
		
		/// <summary>
		/// Fills this <see cref="DataMap2D{ARGB}"/> with the given value with the given render mode.
		/// </summary>
		/// <param name="image">The <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name="value">The value.</param>
		/// <param name="mode">The render mode.</param>
		public static void Fill(this DataMap2D<ARGB> image, ARGB value, RenderMode mode)
		{
			image.FillRectangle(new Rectangle{Size = image.Size}, value, mode);
		}
		
		/// <summary>
		/// Fills a <see cref="Rectangle"/> in this <see cref="DataMap2D{ARGB}"/> with the given value with the given render mode.
		/// </summary>
		/// <param name="image">The <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name="rect">The <see cref="Rectangle"/> to fill.</param>
		/// <param name="value">The value.</param>
		/// <param name="mode">The render mode.</param>
		public static void FillRectangle(this DataMap2D<ARGB> image, Rectangle rect, ARGB value, RenderMode mode)
		{
			if(mode != RenderMode.NORMAL && value.A == 0) return;
			switch(mode)
			{
				case RenderMode.NORMAL:
				case RenderMode.MASK:
					image.FillRectangle(rect, value);
					break;
				case RenderMode.BLEND:
					image.FillBlendRectangle(rect, value);
					break;
			}
		}
		
		/// <summary>
		/// Fills a blended <see cref="Rectangle"/> in this <see cref="DataMap2D{ARGB}"/> with the given value.
		/// </summary>
		/// <param name="image">The <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name="rect">The <see cref="Rectangle"/> to fill.</param>
		/// <param name="value">The value.</param>
		private unsafe static void FillBlendRectangle(this DataMap2D<ARGB> image, Rectangle rect, ARGB value)
		{
			Rectangle clip = VectorUtil.Overlap((Rectangle)image.Size, image.GetClip(), rect);
			if(clip.IsValid())
			{
				ARGB* destData = (ARGB*)image.BeginUnsafeOperation();
				destData += image.GetRawDataOffset();
				int destStride = image.GetRawDataStride();
				int destWidth = image.Width;
				
				ARGB* destIndex;
				ARGB* endIndex;
				
				for(int j = clip.Min.Y; j <= clip.Max.Y; j++)
				{
					destIndex = destData + ((clip.Min.X + (j * destWidth)) * destStride);
					endIndex = destIndex + (destStride * clip.Width);
					while(destIndex != endIndex)
					{
						*destIndex &= value;
						destIndex += destStride;
					}
				}
				
				image.EndUnsafeOperation();
			}
		}
		
		/// <summary>
		/// Draws the outline of a circle in this <see cref="DataMap2D{ARGB}"/> with the given value with the given render mode.
		/// </summary>
		/// <param name="image">The <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name="c">The center of the circle.</param>
		/// <param name="r">The radius of the circle.</param>
		/// <param name="value">The value.</param>
		/// <param name="mode">The render mode.</param>
		public static void DrawCircle(this DataMap2D<ARGB> image, Point2D c, int r, ARGB value, RenderMode mode)
		{
			image.DrawEllipse(c, (Point2D)r, value, mode);
		}
		
		/// <summary>
		/// Draws the outline of an ellipse in this <see cref="DataMap2D{ARGB}"/> with the given value with the given render mode.
		/// </summary>
		/// <param name="image">The <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name="c">The center of the ellipse.</param>
		/// <param name="r">The radii of the ellipse.</param>
		/// <param name="value">The value.</param>
		/// <param name="mode">The render mode.</param>
		public static void DrawEllipse(this DataMap2D<ARGB> image, Point2D c, Point2D r, ARGB value, RenderMode mode)
		{
			if(mode != RenderMode.NORMAL && value.A == 0) return;
			if(mode == RenderMode.NORMAL || mode == RenderMode.MASK)
			{
				image.DrawEllipse(c, r, value);
			}
			if(mode == RenderMode.BLEND)
			{
				image.DrawBlendEllipse(c, r, value);
			}
		}
		
		/// <summary>
		/// Draws the outline of a blended ellipse in this <see cref="DataMap2D{ARGB}"/> with the given value.
		/// </summary>
		/// <param name="image">The <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name="c">The center of the ellipse.</param>
		/// <param name="r">The radii of the ellipse.</param>
		/// <param name="value">The value.</param>
		private unsafe static void DrawBlendEllipse(this DataMap2D<ARGB> image, Point2D c, Point2D r, ARGB value)
		{
			Rectangle clip = new Rectangle{Min = c - r, Max = c + r};
			clip = VectorUtil.Overlap((Rectangle)image.Size, image.GetClip(), clip);
			if(clip.IsValid())
			{
				ARGB* destData = (ARGB*)image.BeginUnsafeOperation();
				destData += image.GetRawDataOffset();
				int destStride = image.GetRawDataStride();
				int destWidth = image.Width;
				
				ARGB* destIndex;
				ARGB* endIndex;
				
				double rySq = r.Y * r.Y;
				int dx;
				int y;
				int left;
				int right;
				int prevLeft = c.X - r.X;
				int prevRight = c.X + r.X;
				
				//fill edge dots
				if(c.X >= clip.Min.X && c.X <= clip.Max.X)
				{
					y = c.Y - r.Y;
					if(y >= clip.Min.Y && y <= clip.Max.Y)
					{
						image[c.X, y] &= value;
					}
					y = c.Y + r.Y;
					if(y >= clip.Min.Y && y <= clip.Max.Y)
					{
						image[c.X, y] &= value;
					}
				}
				
				int scanLeft;
				int scanRight;
				int offset;
				for(int dy = 0; dy < r.Y; dy++, prevLeft = left, prevRight = right)
				{
					//find edges
					dx = (int)(Math.Sqrt(1 - (((dy + 1) * (dy + 1)) / rySq)) * r.X);
					
					left = c.X - dx;
					right = c.X + dx;
					
					
					offset = (dy == r.Y - 1) ? 1 : 0;
					
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
							*destIndex &= value;
							destIndex += destStride;
						}
						
						scanLeft = Math.Max(right + offset, clip.Min.X);
						scanRight = Math.Min(prevRight, clip.Max.X);
						destIndex = destData + ((scanLeft + (y * destWidth)) * destStride);
						endIndex = destIndex + (destStride * (scanRight - scanLeft + 1));
						while(destIndex != endIndex)
						{
							*destIndex &= value;
							destIndex += destStride;
						}
					}
					
					if(dy == 0) continue;
						
					y = c.Y + dy;
					if(y >= clip.Min.Y && y <= clip.Max.Y)
					{
						scanLeft = Math.Max(prevLeft, clip.Min.X);
						scanRight = Math.Min(left, clip.Max.X);
						destIndex = destData + ((scanLeft + (y * destWidth)) * destStride);
						endIndex = destIndex + (destStride * (scanRight - scanLeft + 1));
						while(destIndex != endIndex)
						{
							*destIndex &= value;
							destIndex += destStride;
						}
						
						scanLeft = Math.Max(right + offset, clip.Min.X);
						scanRight = Math.Min(prevRight, clip.Max.X);
						destIndex = destData + ((scanLeft + (y * destWidth)) * destStride);
						endIndex = destIndex + (destStride * (scanRight - scanLeft + 1));
						while(destIndex != endIndex)
						{
							*destIndex &= value;
							destIndex += destStride;
						}
					}
				}
				
				image.EndUnsafeOperation();
			}
		}
		
		/// <summary>
		/// Fills a circle in this <see cref="DataMap2D{ARGB}"/> with the given value with the given render mode.
		/// </summary>
		/// <param name="image">The <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name="c">The center of the circle.</param>
		/// <param name="r">The radius of the circle.</param>
		/// <param name="value">The value.</param>
		/// <param name="mode">The render mode.</param>
		/// <param name="aa">True if the circle is anti-aliased.</param>
		public static void FillCircle(this DataMap2D<ARGB> image, Point2D c, double r, ARGB value, RenderMode mode, bool aa = false)
		{
			image.FillEllipse(c, (Vec2D)r, value, mode, aa);
		}
		
		/// <summary>
		/// Fills an ellipse in this <see cref="DataMap2D{ARGB}"/> with the given value with the given render mode.
		/// </summary>
		/// <param name="image">The <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name="c">The center of the ellipse.</param>
		/// <param name="r">The radii of the ellipse.</param>
		/// <param name="value">The value.</param>
		/// <param name="mode">The render mode.</param>
		/// <param name="aa">True if the ellipse is anti-aliased.</param>
		public static void FillEllipse(this DataMap2D<ARGB> image, Point2D c, Vec2D r, ARGB value, RenderMode mode, bool aa = false)
		{
			if(mode != RenderMode.NORMAL && value.A == 0) return;
			if(!aa)
			{
				if(mode == RenderMode.NORMAL || mode == RenderMode.MASK)
				{
					image.FillEllipse(c, (Point2D)r, value);
				}
				if(mode == RenderMode.BLEND)
				{
					image.FillBlendEllipse(c, (Point2D)r, value);
				}
			}else
			{
				image.FillAAEllipse(c, r, value, mode);
			}
		}
		
		/// <summary>
		/// Fills an blended ellipse in this <see cref="DataMap2D{ARGB}"/> with the given value.
		/// </summary>
		/// <param name="image">The <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name="c">The center of the ellipse.</param>
		/// <param name="r">The radii of the ellipse.</param>
		/// <param name="value">The value.</param>
		private unsafe static void FillBlendEllipse(this DataMap2D<ARGB> image, Point2D c, Point2D r, ARGB value)
		{
			Rectangle clip = new Rectangle{Min = c - r, Max = c + r};
			clip = VectorUtil.Overlap((Rectangle)image.Size, image.GetClip(), clip);
			if(clip.IsValid())
			{
				ARGB* destData = (ARGB*)image.BeginUnsafeOperation();
				destData += image.GetRawDataOffset();
				int destStride = image.GetRawDataStride();
				int destWidth = image.Width;
				
				ARGB* destIndex;
				ARGB* endIndex;
				
				double rySq = r.Y * r.Y;
				for(int dy = 0; dy <= r.Y; dy++)
				{
					//find edges
					int dx = (int)(Math.Sqrt(1 - ((dy * dy) / rySq)) * r.X);
					
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
								*destIndex &= value;
								destIndex += destStride;
							}
						}
						
						if(dy != 0) continue;
						
						y = c.Y + dy;
						if(y >= clip.Min.Y && y <= clip.Max.Y)
						{
							destIndex = destData + ((left + (y * destWidth)) * destStride);
							endIndex = destIndex + (destStride * (right - left + 1));
							while(destIndex != endIndex)
							{
								*destIndex &= value;
								destIndex += destStride;
							}
						}
					}
				}
				
				image.EndUnsafeOperation();
			}
		}
		
		/// <summary>
		/// Fills an anti-aliased ellipse in this <see cref="DataMap2D{ARGB}"/> with the given value with the given render mode.
		/// </summary>
		/// <param name="image">The <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name="c">The center of the ellipse.</param>
		/// <param name="r">The radii of the ellipse.</param>
		/// <param name="value">The value.</param>
		/// <param name="mode">The render mode.</param>
		private static void FillAAEllipse(this DataMap2D<ARGB> image, Point2D c, Vec2D r, ARGB value, RenderMode mode)
		{
			if(mode != RenderMode.NORMAL && value.A == 0) return;
			
			Point2D half = new Point2D((int)Math.Ceiling(r.X), (int)Math.Ceiling(r.Y));
			Rectangle clip = new Rectangle{Min = c - half, Max = c + half};
			clip = VectorUtil.Overlap((Rectangle)image.Size, image.GetClip(), clip);
			if(clip.IsValid())
			{
				Point2D inner = (Point2D)(r / Math.Sqrt(2));
				
				image.FillRectangle(new Rectangle{Min = c - (inner - 1), Max = c + (inner - 1)}, value, mode);
				
				if(c.Y >= clip.Min.Y && c.Y <= clip.Max.Y)
				{
					image.FillRightXScan(Math.Max(c.X + inner.X, clip.Min.X), Math.Min(c.X + r.X, clip.Max.X), c.Y, value, mode);
					image.FillLeftXScan(Math.Max(c.X - r.X, clip.Min.X), Math.Min(c.X - inner.X, clip.Max.X), c.Y, value, mode);
				}
				if(c.X >= clip.Min.X && c.X < clip.Max.X)
				{
					image.FillUpYScan(c.X, Math.Max(c.Y + inner.Y, clip.Min.Y), Math.Min(c.Y + r.Y, clip.Max.Y), value, mode);
					image.FillDownYScan(c.X, Math.Max(c.Y - r.Y, clip.Min.Y), Math.Min(c.Y - inner.Y, clip.Max.Y), value, mode);
				}
				
				Vec2D rSq = r * r;
				
				for(int dy = inner.Y; dy > 0; dy--)
				{
					double dx = Math.Sqrt(1 - ((dy * dy) / rSq.Y)) * r.X;
					
					int y = c.Y - dy;
					if(y >= clip.Min.Y && y <= clip.Max.Y)
					{
						image.FillRightXScan(Math.Max(c.X + inner.X, clip.Min.X), Math.Min(c.X + dx, clip.Max.X), y, value, mode);
						image.FillLeftXScan(Math.Max(c.X - dx, clip.Min.X), Math.Min(c.X - inner.X, clip.Max.X), y, value, mode);
					}
					
					y = c.Y + dy;
					if(y >= clip.Min.Y && y <= clip.Max.Y)
					{
						image.FillRightXScan(Math.Max(c.X + inner.X, clip.Min.X), Math.Min(c.X + dx, clip.Max.X), y, value, mode);
						image.FillLeftXScan(Math.Max(c.X - dx, clip.Min.X), Math.Min(c.X - inner.X, clip.Max.X), y, value, mode);
					}
				}
				
				int offset;
				for(int dx = inner.X; dx > 0; dx--)
				{
					offset = dx == inner.X ? 1 : 0;
					double dy = Math.Sqrt(1 - ((dx * dx) / rSq.X)) * r.Y;
					
					int x = c.X - dx;
					if(x >= clip.Min.X && x <= clip.Max.X)
					{
						image.FillUpYScan(x, Math.Max(c.Y + inner.Y + offset, clip.Min.Y), Math.Min(c.Y + dy, clip.Max.Y), value, mode);
						image.FillDownYScan(x, Math.Max(c.Y - dy, clip.Min.Y), Math.Min(c.Y - inner.Y - offset, clip.Max.Y), value, mode);
					}
					
					x = c.X + dx;
					if(x >= clip.Min.X && x <= clip.Max.X)
					{
						image.FillUpYScan(x, Math.Max(c.Y + inner.Y + offset, clip.Min.Y), Math.Min(c.Y + dy, clip.Max.Y), value, mode);
						image.FillDownYScan(x, Math.Max(c.Y - dy, clip.Min.Y), Math.Min(c.Y - inner.Y - offset, clip.Max.Y), value, mode);
					}
				}
			}
		}
		
		/// <summary>
		/// Draws the outline of a rounded <see cref="Rectangle"/> in this <see cref="DataMap2D{ARGB}"/> with the given value with the given render mode.
		/// </summary>
		/// <param name="image">The <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name="rect">The <see cref="Rectangle"/> to fill.</param>
		/// <param name = "r">The radii.</param>
		/// <param name="value">The value.</param>
		/// <param name="mode">The render mode.</param>
		public static void DrawRoundedRectangle(this DataMap2D<ARGB> image, Rectangle rect, Point2D r, ARGB value, RenderMode mode)
		{
			if(mode != RenderMode.NORMAL && value.A == 0) return;
			if(mode == RenderMode.NORMAL || mode == RenderMode.MASK)
			{
				image.DrawRoundedRectangle(rect, r, value);
			}
			if(mode == RenderMode.BLEND)
			{
				image.DrawBlendRoundedRectangle(rect, r, value);
			}
		}
		
		/// <summary>
		/// Draws the outline of a rounded <see cref="Rectangle"/> in this <see cref="DataMap2D{ARGB}"/> with the given value.
		/// </summary>
		/// <param name="image">The <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name="rect">The <see cref="Rectangle"/> to fill.</param>
		/// <param name = "r">The radii.</param>
		/// <param name="value">The value.</param>
		private unsafe static void DrawBlendRoundedRectangle(this DataMap2D<ARGB> image, Rectangle rect, Point2D r, ARGB value)
		{
			Rectangle clip = VectorUtil.Overlap((Rectangle)image.Size, image.GetClip(), rect);
			if(clip.IsValid())
			{
				ARGB* destData = (ARGB*)image.BeginUnsafeOperation();
				destData += image.GetRawDataOffset();
				int destStride = image.GetRawDataStride();
				int destWidth = image.Width;
				
				ARGB* destIndex;
				ARGB* endIndex;
				
				double rySq = r.Y * r.Y;
				int dx = (int)(Math.Sqrt(1 - (1 / rySq)) * r.X);
				int y;
				int left;
				int right;
				int prevLeft = rect.Min.X + (r.X - dx);
				int prevRight = rect.Max.X - (r.X - dx);
				
				//draw top and bottom
				left = Math.Max(rect.Min.X + r.X, clip.Min.X);
				right = Math.Min(rect.Max.X - r.X + 1, clip.Max.X);
				if(left <= right)
				{
					if(rect.Min.Y >= clip.Min.Y)
					{
						destIndex = destData + ((left + (rect.Min.Y * destWidth)) * destStride);
						endIndex = destIndex + (destStride * (right - left));
					
						while(destIndex != endIndex)
						{
							*destIndex &= value;
							destIndex += destStride;
						}
					}
					if(rect.Max.Y <= clip.Max.Y)
					{
						destIndex = destData + ((left + (rect.Max.Y * destWidth)) * destStride);
						endIndex = destIndex + (destStride * (right - left));
					
						while(destIndex != endIndex)
						{
							*destIndex &= value;
							destIndex += destStride;
						}
					}
				}
				//draw left and right
				int bottom = Math.Max(rect.Min.Y + r.Y + 1, clip.Min.Y);
				int top = Math.Min(rect.Max.Y - r.Y, clip.Max.Y);
				if(bottom <= top)
				{
					if(rect.Min.X >= clip.Min.X)
					{
						destIndex = destData + ((rect.Min.X + (bottom * destWidth)) * destStride);
						endIndex = destIndex + (destStride * destWidth * (top - bottom));
					
						while(destIndex != endIndex)
						{
							*destIndex &= value;
							destIndex += destStride * destWidth;
						}
					}
					if(rect.Max.X <= clip.Max.X)
					{
						destIndex = destData + ((rect.Max.X + (bottom * destWidth)) * destStride);
						endIndex = destIndex + (destStride * destWidth * (top - bottom));
					
						while(destIndex != endIndex)
						{
							*destIndex &= value;
							destIndex += destStride * destWidth;
						}
					}
				}
				
				int scanLeft;
				int scanRight;
				for(int dy = 0; dy <= r.Y; dy++, prevLeft = left, prevRight = right)
				{
					//find edges
					dx = (int)(Math.Sqrt(1 - ((dy * dy) / rySq)) * r.X);
					left = rect.Min.X + (r.X - dx);
					right = rect.Max.X - (r.X - dx);
					
					//fill scans
					y = rect.Min.Y + (r.Y - dy) + 1;
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
								*destIndex &= value;
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
								*destIndex &= value;
								destIndex += destStride;
							}
						}
					}
					y = rect.Max.Y - (r.Y - dy) - 1;
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
								*destIndex &= value;
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
								*destIndex &= value;
								destIndex += destStride;
							}
						}
					}
				}
				
				image.EndUnsafeOperation();
			}
		}
		
		/// <summary>
		/// Fills a rounded <see cref="Rectangle"/> in this <see cref="DataMap2D{ARGB}"/> with the given value with the given render mode.
		/// </summary>
		/// <param name="image">The <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name="rect">The <see cref="Rectangle"/> to fill.</param>
		/// <param name = "r">The radii.</param>
		/// <param name="value">The value.</param>
		/// <param name="mode">The render mode.</param>
		/// <param name="aa">True if the rect is anti-aliased.</param>
		public static void FillRoundedRectangle(this DataMap2D<ARGB> image, Rectangle rect, Vec2D r, ARGB value, RenderMode mode, bool aa = false)
		{
			if(mode != RenderMode.NORMAL && value.A == 0) return;
			if(!aa)
			{
				if(mode == RenderMode.NORMAL || mode == RenderMode.MASK)
				{
					image.FillRoundedRectangle(rect, (Point2D)r, value);
				}
				if(mode == RenderMode.BLEND)
				{
					image.FillBlendRoundedRectangle(rect, (Point2D)r, value);
				}
			}else
			{
				image.FillAARoundedRectangle(rect, r, value, mode);
			}
		}
		
		/// <summary>
		/// Fills a blended rounded <see cref="Rectangle"/> in this <see cref="DataMap2D{ARGB}"/> with the given value.
		/// </summary>
		/// <param name="image">The <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name="rect">The <see cref="Rectangle"/> to fill.</param>
		/// <param name = "r">The radii.</param>
		/// <param name="value">The value.</param>
		private unsafe static void FillBlendRoundedRectangle(this DataMap2D<ARGB> image, Rectangle rect, Point2D r, ARGB value)
		{
			Rectangle clip = VectorUtil.Overlap((Rectangle)image.Size, image.GetClip(), rect);
			if(clip.IsValid())
			{
				ARGB* destData = (ARGB*)image.BeginUnsafeOperation();
				destData += image.GetRawDataOffset();
				int destStride = image.GetRawDataStride();
				int destWidth = image.Width;
				
				ARGB* destIndex;
				ARGB* endIndex;
				
				double rySq = r.Y * r.Y;
				int dx;
				int y;
				int left;
				int right;
				
				//fill center, left and right
				for(int j = Math.Max(rect.Min.Y + r.Y, clip.Min.Y); j <= Math.Min(rect.Max.Y - r.Y, clip.Max.Y); j++)
				{
					left = clip.Min.X;
					right = clip.Max.X;
					
					if(left <= right)
					{
						destIndex = destData + ((left + (j * destWidth)) * destStride);
						endIndex = destIndex + (destStride * (right - left + 1));
					
						while(destIndex != endIndex)
						{
							*destIndex &= value;
							destIndex += destStride;
						}
					}
				}
				
				for(int dy = 1; dy <= r.Y; dy++)
				{
					//find edges
					dx = (int)(Math.Sqrt(1 - ((dy * dy) / rySq)) * r.X);
					left = rect.Min.X + (r.X - dx);
					right = rect.Max.X - (r.X - dx);
					
					//fill scans
					y = rect.Min.Y + (r.Y - dy);
					if(left <= right)
					{
						destIndex = destData + ((left + (y * destWidth)) * destStride);
						endIndex = destIndex + (destStride * (right - left + 1));
						
						while(destIndex != endIndex)
						{
							*destIndex &= value;
							destIndex += destStride;
						}
					}
					y = rect.Max.Y - (r.Y - dy);
					if(left <= right)
					{
						destIndex = destData + ((left + (y * destWidth)) * destStride);
						endIndex = destIndex + (destStride * (right - left + 1));
						
						while(destIndex != endIndex)
						{
							*destIndex &= value;
							destIndex += destStride;
						}
					}
				}
				
				image.EndUnsafeOperation();
			}
		}
		
		/// <summary>
		/// Fills an anti-aliased rounded <see cref="Rectangle"/> in this <see cref="DataMap2D{ARGB}"/> with the given value with the given render mode.
		/// </summary>
		/// <param name="image">The <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name="rect">The <see cref="Rectangle"/> to fill.</param>
		/// <param name = "r">The radii.</param>
		/// <param name="value">The value.</param>
		/// <param name="mode">The render mode.</param>
		private static void FillAARoundedRectangle(this DataMap2D<ARGB> image, Rectangle rect, Vec2D r, ARGB value, RenderMode mode)
		{
			Rectangle clip = VectorUtil.Overlap((Rectangle)image.Size, image.GetClip(), rect);
			if(clip.IsValid())
			{
				Rectangle small = new Rectangle{Min = rect.Min + (Point2D)r, Max = rect.Max - (Point2D)r};
				Point2D inner = (Point2D)(r / Math.Sqrt(2));
				
				image.FillRectangle(new Rectangle{Min = small.Min - (inner - 1), Max = small.Max + (inner - 1)}, value, mode);
				
				image.FillRectangle(new Rectangle{Min = new Point2D(small.Max.X + inner.X, small.Min.Y), Max = new Point2D(rect.Max.X, small.Max.Y)}, value, mode);
				image.FillRectangle(new Rectangle{Min = new Point2D(rect.Min.X, small.Min.Y), Max = new Point2D(small.Min.X - inner.X, small.Max.Y)}, value, mode);
				
				image.FillRectangle(new Rectangle{Min = new Point2D(small.Min.X, small.Max.Y + inner.Y), Max = new Point2D(small.Max.X, rect.Max.Y)}, value, mode);
				image.FillRectangle(new Rectangle{Min = new Point2D(small.Min.X, rect.Min.Y), Max = new Point2D(small.Max.X, small.Min.Y - inner.Y)}, value, mode);
				
				Vec2D rSq = r * r;
				
				for(int dy = inner.Y; dy > 0; dy--)
				{
					double dx = Math.Sqrt(1 - ((dy * dy) / rSq.Y)) * r.X;
					
					int y = small.Min.Y - dy;
					if(y >= clip.Min.Y && y <= clip.Max.Y)
					{
						image.FillRightXScan(Math.Max(small.Max.X + inner.X, clip.Min.X), Math.Min(small.Max.X + dx, clip.Max.X), y, value, mode);
						image.FillLeftXScan(Math.Max(small.Min.X - dx, clip.Min.X), Math.Min(small.Min.X - inner.X, clip.Max.X), y, value, mode);
					}
					
					y = small.Max.Y + dy;
					if(y >= clip.Min.Y && y <= clip.Max.Y)
					{
						image.FillRightXScan(Math.Max(small.Max.X + inner.X, clip.Min.X), Math.Min(small.Max.X + dx, clip.Max.X), y, value, mode);
						image.FillLeftXScan(Math.Max(small.Min.X - dx, clip.Min.X), Math.Min(small.Min.X - inner.X, clip.Max.X), y, value, mode);
					}
				}
				
				int offset;
				for(int dx = inner.X; dx > 0; dx--)
				{
					offset = dx == inner.X ? 1 : 0;
					double dy = Math.Sqrt(1 - ((dx * dx) / rSq.X)) * r.Y;
					
					int x = small.Min.X - dx;
					if(x >= clip.Min.X && x <= clip.Max.X)
					{
						image.FillUpYScan(x, Math.Max(small.Max.Y + inner.Y + offset, clip.Min.Y), Math.Min(small.Max.Y + dy, clip.Max.Y), value, mode);
						image.FillDownYScan(x, Math.Max(small.Min.Y - dy, clip.Min.Y), Math.Min(small.Min.Y - inner.Y - offset, clip.Max.Y), value, mode);
					}
					
					x = small.Max.X + dx;
					if(x >= clip.Min.X && x <= clip.Max.X)
					{
						image.FillUpYScan(x, Math.Max(small.Max.Y + inner.Y + offset, clip.Min.Y), Math.Min(small.Max.Y + dy, clip.Max.Y), value, mode);
						image.FillDownYScan(x, Math.Max(small.Min.Y - dy, clip.Min.Y), Math.Min(small.Min.Y - inner.Y - offset, clip.Max.Y), value, mode);
					}
				}
			}
		}
		
		private static void FillRightXScan(this DataMap2D<ARGB> image, int x1, double x2, int y, ARGB value, RenderMode mode)
		{
			if(x1 <= x2)
			{
				int ix2 = (int)Math.Ceiling(x2);
				switch(mode)
				{
					case RenderMode.NORMAL:
					case RenderMode.MASK:
						for(int x = x1; x < ix2; x++)
						{
							image[x, y] = value;
						}
						break;
					case RenderMode.BLEND:
						for(int x = x1; x < ix2; x++)
						{
							image[x, y] &= value;
						}
						break;
				}
				value.A = (byte)(value.A * (1 - (ix2 - x2)));
				image[ix2, y] &= value;
			}
		}
		
		private static void FillLeftXScan(this DataMap2D<ARGB> image, double x1, int x2, int y, ARGB value, RenderMode mode)
		{
			if(x1 <= x2)
			{
				int ix1 = (int)x1;
				switch(mode)
				{
					case RenderMode.NORMAL:
					case RenderMode.MASK:
						for(int x = x2; x > ix1; x--)
						{
							image[x, y] = value;
						}
						break;
					case RenderMode.BLEND:
						for(int x = x2; x > ix1; x--)
						{
							image[x, y] &= value;
						}
						break;
				}
				value.A = (byte)(value.A * (1 - (x1 - ix1)));
				image[ix1, y] &= value;
			}
		}
		
		private static void FillUpYScan(this DataMap2D<ARGB> image, int x, int y1, double y2, ARGB value, RenderMode mode)
		{
			if(y1 <= y2)
			{
				int iy2 = (int)Math.Ceiling(y2);
				switch(mode)
				{
					case RenderMode.NORMAL:
					case RenderMode.MASK:
						for(int y = y1; y < iy2; y++)
						{
							image[x, y] = value;
						}
						break;
					case RenderMode.BLEND:
						for(int y = y1; y < iy2; y++)
						{
							image[x, y] &= value;
						}
						break;
				}
				value.A = (byte)(value.A * (1 - (iy2 - y2)));
				image[x, iy2] &= value;
			}
		}
		
		private static void FillDownYScan(this DataMap2D<ARGB> image, int x, double y1, int y2, ARGB value, RenderMode mode)
		{
			if(y1 <= y2)
			{
				int iy1 = (int)y1;
				switch(mode)
				{
					case RenderMode.NORMAL:
					case RenderMode.MASK:
						for(int y = y2; y > iy1; y--)
						{
							image[x, y] = value;
						}
						break;
					case RenderMode.BLEND:
						for(int y = y2; y > iy1; y--)
						{
							image[x, y] &= value;
						}
						break;
				}
				value.A = (byte)(value.A * (1 - (y1 - iy1)));
				image[x, iy1] &= value;
			}
		}
		
		/// <summary>
		/// Fills a triangle in this <see cref="DataMap2D{ARGB}"/> with the given value with the given render mode.
		/// </summary>
		/// <param name="image">The <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name = "scans">The scan storage. Generally should by exclusive to one image.</param>
		/// <param name="v1">The first vertex.</param>
		/// <param name="v2">The second vertex.</param>
		/// <param name="v3">The third vertex.</param>
		/// <param name="value">The value.</param>
		/// <param name="mode">The render mode.</param>
		/// <param name="aa">True if the triangle is anti-aliased.</param>
		public static void FillTriangle(this DataMap2D<ARGB> image, ref double[,] scans, ARGB value, RenderMode mode, bool aa, Point2D v1, Point2D v2, Point2D v3)
		{
			image.FillPolygon(ref scans, value, mode, aa, v1, v2, v3);
		}
		
		/// <summary>
		/// Fills a polygon in this <see cref="DataMap2D{ARGB}"/> with the given value with the given render mode.
		/// </summary>
		/// <param name="image">The <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name = "scans">The scan storage. Generally should by exclusive to one image.</param>
		/// <param name="verts">The verticies of the polygon.</param>
		/// <param name="value">The value.</param>
		/// <param name="mode">The render mode.</param>
		/// <param name="aa">True if the polygon is anti-aliased.</param>
		public static void FillPolygon(this DataMap2D<ARGB> image, ref double[,] scans, ARGB value, RenderMode mode, bool aa, params Point2D[] verts)
		{
			if(mode != RenderMode.NORMAL && value.A == 0) return;
			if(!aa)
			{
				if(mode == RenderMode.NORMAL || mode == RenderMode.MASK)
				{
					image.FillPolygon(ref scans, value, verts);
				}
				if(mode == RenderMode.BLEND)
				{
					image.FillBlendPolygon(ref scans, value, verts);
				}
			}else
			{
				image.FillAAPolygon(ref scans, value, mode, verts);
			}
		}
		
		/// <summary>
		/// Fills a blended polygon in this <see cref="DataMap2D{ARGB}"/> with the given value.
		/// </summary>
		/// <param name="dest">The <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name = "scans">The scan storage. Generally should by exclusive to one image.</param>
		/// <param name="value">The value.</param>
		/// <param name="verts">The verticies of the polygon.</param>
		private unsafe static void FillBlendPolygon(this DataMap2D<ARGB> dest, ref double[,] scans, ARGB value, params Point2D[] verts)
		{
			Rectangle clip = VectorUtil.Overlap((Rectangle)dest.Size, dest.GetClip());
			if(clip.IsValid())
			{
				int minY, maxY;
				dest.ScanPolygon(clip, ref scans, out minY, out maxY, verts);
				
				ARGB* destData = (ARGB*)dest.BeginUnsafeOperation();
				destData += dest.GetRawDataOffset();
				int destStride = dest.GetRawDataStride();
				int destWidth = dest.Width;
				
				ARGB* destIndex;
				ARGB* endIndex;
				
				int left;
				int right;
				
				minY = Math.Max(minY, clip.Min.Y);
				maxY = Math.Min(maxY, clip.Max.Y);
				for(int j = minY; j <= maxY; j++)
				{
					left = Math.Max((int)Math.Floor(scans[j , 0]), clip.Min.X);
					right = Math.Min((int)Math.Ceiling(scans[j, 1]), clip.Max.X);
					
					if(left <= right)
					{
						destIndex = destData + ((left + (j * destWidth)) * destStride);
						endIndex = destIndex + (destStride * (right - left + 1));
					
						while(destIndex != endIndex)
						{
							*destIndex &= value;
							destIndex += destStride;
						}
					}
				}
					
				dest.EndUnsafeOperation();
			}
		}
		
		/// <summary>
		/// Fills an anti-aliased polygon in this <see cref="DataMap2D{ARGB}"/> with the given value with the given render mode.
		/// </summary>
		/// <param name="image">The <see cref="DataMap2D{ARGB}"/>.</param>
		/// <param name = "scans">The scan storage. Generally should by exclusive to one image.</param>
		/// <param name="value">The value.</param>
		/// <param name="verts">The verticies of the polygon.</param>
		/// <param name="mode">The render mode.</param>
		private unsafe static void FillAAPolygon(this DataMap2D<ARGB> image, ref double[,] scans, ARGB value, RenderMode mode, params Point2D[] verts)
		{
			Rectangle clip = VectorUtil.Overlap((Rectangle)image.Size, image.GetClip());
			if(clip.IsValid())
			{
				int minY, maxY;
				image.ScanPolygon(clip, ref scans, out minY, out maxY, verts);
				
				ARGB* destData = (ARGB*)image.BeginUnsafeOperation();
				destData += image.GetRawDataOffset();
				int destStride = image.GetRawDataStride();
				int destWidth = image.Width;
				
				ARGB* destIndex;
				ARGB* endIndex;
				int left;
				int right;
			
				minY = Math.Max(minY, clip.Min.Y);
				maxY = Math.Min(maxY, clip.Max.Y);
				for(int y = minY; y <= maxY; y++)
				{
					//x1 vars are x bottom side
					//x2 vars are x top side
					//xb vars are y bottom side
					//xa vars are y top side
					double x1 = Math.Max(scans[y, 0], clip.Min.X);
					double x2 = Math.Min(scans[y, 1], clip.Max.X);
					double x1b;
					double x2b;
					if(y > minY)
					{
						x1b = Math.Max(scans[y - 1, 0], clip.Min.X);
						x2b = Math.Min(scans[y - 1, 1], clip.Max.X);
						x1b = (x1b + x1) / 2;
						x2b = (x2b + x2) / 2;
					}else
					{
						x1b = x1;
						x2b = x2;
					}
					double x1a;
					double x2a;
					if(y < maxY)
					{
						x1a = Math.Max(scans[y + 1, 0], clip.Min.X);
						x2a = Math.Min(scans[y + 1, 1], clip.Max.X);
						x1a = (x1a + x1) / 2;
						x2a = (x2a + x2) / 2;
					}else
					{
						x1a = x1;
						x2a = x2;
					}
					//fix local extreme values
					if(Math.Sign(x1 - x1b) != Math.Sign(x1a - x1))
					{
						x1b = x1a = x1;
					}
					if(Math.Sign(x2 - x2b) != Math.Sign(x2a - x2))
					{
						x2b = x2a = x2;
					}
					//fill scan
					left = (int)Math.Ceiling(Math.Max(x1b, x1a));
					right = (int)Math.Floor(Math.Min(x2b, x2a));
					if(left <= right)
					{
						destIndex = destData + ((left + (y * destWidth)) * destStride);
						endIndex = destIndex + (destStride * (right - left + 1));
					
						switch(mode)
						{
							case RenderMode.NORMAL:
							case RenderMode.MASK:
								while(destIndex != endIndex)
								{
									*destIndex = value;
									destIndex += destStride;
								}
								break;
							case RenderMode.BLEND:
								while(destIndex != endIndex)
								{
									*destIndex &= value;
									destIndex += destStride;
								}
								break;
						}
					}
					
					if(Math.Min(x1b, x1a) != left)
						FillLeftAAEdge(image, x1b, x1a, y, value);
					if(Math.Max(x2b, x2a) != right)
						FillRightAAEdge(image, x2b + 1, x2a + 1, y, value);
				}
			}
		}
		
		private static void FillLeftAAEdge(DataMap2D<ARGB> dest, double x1, double x2, int y, ARGB value)
		{
			if(x1 > x2) Util.Swap(ref x1, ref x2);
			
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
				xmax = Math.Min((int)xmin + 1, x2);
				ymin = ymax;
				ymax = ymin + (xmax - xmin) * dydx;
				alpha = (xmin - (int)xmin) * ymin;
				alpha += (ymax + ymin) * (xmax - xmin) / 2;
				dest[(int)xmin, y] &= new ARGB((byte)(value.A * (1 - alpha)), value.RGB);
			}while(xmax != x2);
		}
		
		private static void FillRightAAEdge(DataMap2D<ARGB> dest, double x1, double x2, int y, ARGB value)
		{
			if(x1 > x2) Util.Swap(ref x1, ref x2);
			
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
				xmax = Math.Min((int)xmin + 1, x2);
				ymin = ymax;
				ymax = ymin + (xmax - xmin) * dydx;
				alpha = (xmin - (int)xmin) * ymin;
				alpha += (ymax + ymin) * (xmax - xmin) / 2;
				dest[(int)xmin, y] &= new ARGB((byte)(value.A * alpha), value.RGB);
			}while(xmax != x2);
		}
	}
}
