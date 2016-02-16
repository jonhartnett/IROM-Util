namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Simple rounded rectangle struct.
	/// </summary>
	public struct RoundedRectangle
	{
		/// <summary>
		/// The <see cref="RoundedRectangle"/> encompassing rectangle.
		/// </summary>
		public Rectangle Rect;
		
		/// <summary>
		/// The <see cref="Ellipse"/> radii.
		/// </summary>
		public Vec2D Radii;
		
		 public override string ToString()
		{
			return string.Format("RoundedRectangle (({0}, {1}), ({2}, {3})), ({4}, {5})", Rect.Min.X, Rect.Min.Y, Rect.Max.X, Rect.Max.Y, Radii.X, Radii.Y);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is RoundedRectangle)
        	{
        		RoundedRectangle rect = (RoundedRectangle)obj;
        		return this == rect;
        	}else
        	{
        		return false;
        	}
        }
        
        public override int GetHashCode()
        {
        	// disable NonReadonlyReferencedInGetHashCode
        	return (int)Hash.PerformStaticHash((uint)Rect.GetHashCode(), (uint)Radii.GetHashCode());
        }

		public void Scan(Scanner scanner, Rectangle clip)
		{
			scanner.yMin = Rect.Min.Y;
			scanner.yMax = Rect.Max.Y;
			//clip to clip, but with leeway
			if(scanner.yMin < clip.Min.Y - 1) scanner.yMin = clip.Min.Y - 1;
			if(scanner.yMax > clip.Max.Y + 1) scanner.yMax = clip.Max.Y + 1;
			for(int y = scanner.yMin; y <= scanner.yMax; y++)
			{
				if(y < Rect.Min.Y + Radii.Y)
				{
					double dy = (Rect.Min.Y + Radii.Y) - y;
					double dx = Math.Sqrt(1 - (dy * dy) / (Radii.Y * Radii.Y)) * Radii.X;
					dx -= Radii.X;
					scanner[y] = new Scanner.Scan{min = Rect.Min.X - (float)dx, max = Rect.Max.X + (float)dx};
				}else
				if(y > Rect.Max.Y - Radii.Y)
				{
					double dy = y - (Rect.Max.Y - Radii.Y);
					double dx = Math.Sqrt(1 - (dy * dy) / (Radii.Y * Radii.Y)) * Radii.X;
					dx -= Radii.X;
					scanner[y] = new Scanner.Scan{min = Rect.Min.X - (float)dx, max = Rect.Max.X + (float)dx};
				}else
				{
					scanner[y] = new Scanner.Scan{min = Rect.Min.X, max = Rect.Max.X};
				}
			}
			//true clip
			if(scanner.yMin < clip.Min.Y)
			{
				scanner.yMin = clip.Min.Y;
				scanner.isYMinClipped = true;
			}
			if(scanner.yMax > clip.Max.Y)
			{
				scanner.yMax = clip.Max.Y;
				scanner.isYMaxClipped = true;
			}
		}
		
        /// <summary>
        /// Returns true if this <see cref="Viewport"/> is a valid, space-filling ellipse.
        /// </summary>
        /// <returns>True if valid.</returns>
        public bool IsValid()
        {
        	return Radii.X >= 0 && Radii.Y >= 0 && Rect.IsValid();
        }
        
        /// <summary>
        /// True if the rects are equal.
        /// </summary>
        /// <param name="rect">The first rect.</param>
        /// <param name="rect2">The second rect.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(RoundedRectangle rect, RoundedRectangle rect2)
        {
        	return (rect.Rect == rect2.Rect) && (rect.Radii == rect2.Radii);
        }
        
        /// <summary>
        /// True if the rects are not equal.
        /// </summary>
        /// <param name="rect">The first rect.</param>
        /// <param name="rect2">The second rect.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(RoundedRectangle rect, RoundedRectangle rect2)
        {
        	return !(rect == rect2);
        }
	}
}
