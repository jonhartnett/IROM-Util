namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Simple double ellipse struct.
	/// </summary>
	public struct Ellipse : IRenderableShape
	{
		/// <summary>
		/// The <see cref="Ellipse"/> center.
		/// </summary>
		public Vec2D Position;
		
		/// <summary>
		/// The <see cref="Ellipse"/> principal radii.
		/// </summary>
		public Vec2D Radii;
		
		/// <summary>
		/// The <see cref="Viewport"/> x.
		/// </summary>
		public double X
		{
			get{return Position.X;}
			set{Position.X = value;}
		}
		
		/// <summary>
		/// The <see cref="Viewport"/> y.
		/// </summary>
		public double Y
		{
			get{return Position.Y;}
			set{Position.Y = value;}
		}
        
        public override string ToString()
		{
			return string.Format("Ellipse ({0}, {1}), ({2}, {3})", Position.X, Position.Y, Radii.X, Radii.Y);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is Ellipse)
        	{
        		Ellipse ell = (Ellipse)obj;
        		return this == ell;
        	}else
        	{
        		return false;
        	}
        }
        
        public override int GetHashCode()
        {
        	// disable NonReadonlyReferencedInGetHashCode
        	return (int)Hash.PerformStaticHash((uint)Position.GetHashCode(), (uint)Radii.GetHashCode());
        }

		public void Scan(Scanner scanner, Rectangle clip)
		{
			Point2D center = (Point2D)Position.Round();
			scanner.yMin = center.Y - (int)Radii.Y;
			scanner.yMax = center.Y + (int)Radii.Y;
			//clip to clip, but with leeway
			if(scanner.yMin < clip.Min.Y - 1) scanner.yMin = clip.Min.Y - 1;
			if(scanner.yMax > clip.Max.Y + 1) scanner.yMax = clip.Max.Y + 1;
			for(int y = scanner.yMin; y <= scanner.yMax; y++)
			{
				double dy = center.Y - y;
				double dx = Math.Sqrt(1 - (dy * dy) / (Radii.Y * Radii.Y)) * Radii.X;
				scanner[y] = new Scanner.Scan{min = center.X - (float)dx, max = center.X + (float)dx};
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
        	return Radii.X > 0 && Radii.Y > 0;
        }
        
        /// <summary>
        /// Returns true if the given <see cref="Vec2D"/> is within this <see cref="Viewport"/>'s bounds.
        /// </summary>
        /// <param name="vec">The vec.</param>
        /// <returns>True if within, else false.</returns>
        public bool Contains(Vec2D vec)
        {
        	return ((vec - Position) / Radii).LengthSq() <= 1;
        }

        /// <summary>
        /// Multiplies the given ellipse's size by the given value, keeping it centered.
        /// </summary>
        /// <param name="ellipse">The ellipse.</param>
        /// <param name="val">The value.</param>
        /// <returns>The resized ellipse.</returns>
        public static Ellipse operator *(Ellipse ellipse, double val)
        {
        	return new Ellipse{Position = ellipse.Position, Radii = ellipse.Radii * val};
        }

        /// <summary>
        /// Divides the given ellipse's size by the given value, keeping it centered.
        /// </summary>
        /// <param name="ellipse">The ellipse.</param>
        /// <param name="val">The value.</param>
        /// <returns>The resized ellipse.</returns>
        public static Ellipse operator /(Ellipse ellipse, double val)
        {
        	return new Ellipse{Position = ellipse.Position, Radii = ellipse.Radii / val};
        }
        
        /// <summary>
        /// True if the ellipses are equal.
        /// </summary>
        /// <param name="ellipse">The first ellipse.</param>
        /// <param name="ellipse2">The second ellipse.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(Ellipse ellipse, Ellipse ellipse2)
        {
        	return (ellipse.Position == ellipse2.Position) && (ellipse.Radii == ellipse2.Radii);
        }
        
        /// <summary>
        /// True if the ellipses are not equal.
        /// </summary>
        /// <param name="ellipse">The first ellipse.</param>
        /// <param name="ellipse2">The second ellipse.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(Ellipse ellipse, Ellipse ellipse2)
        {
        	return !(ellipse == ellipse2);
        }
	}
}