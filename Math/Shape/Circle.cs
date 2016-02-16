namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Simple double circle struct.
	/// </summary>
	public struct Circle : IRenderableShape
	{
		/// <summary>
		/// The <see cref="Circle"/> center.
		/// </summary>
		public Vec2D Position;
		
		/// <summary>
		/// The <see cref="Circle"/> radius.
		/// </summary>
		public double Radius;
		
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
			return string.Format("Circle ({0}, {1}), ({2}, {3})", Position.X, Position.Y, Radius);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is Circle)
        	{
        		Circle cir = (Circle)obj;
        		return this == cir;
        	}else
        	{
        		return false;
        	}
        }
        
        public override int GetHashCode()
        {
        	// disable NonReadonlyReferencedInGetHashCode
        	return (int)Hash.PerformStaticHash((uint)Position.GetHashCode(), (uint)Radius.GetHashCode());
        }

		public void Scan(Scanner scanner, Rectangle clip)
		{
			((Ellipse)this).Scan(scanner, clip);
		}
		
        /// <summary>
        /// Returns true if this <see cref="Viewport"/> is a valid, space-filling circle.
        /// </summary>
        /// <returns>True if valid.</returns>
        public bool IsValid()
        {
        	return Radius > 0;
        }
        
        /// <summary>
        /// Returns true if the given <see cref="Vec2D"/> is within this <see cref="Viewport"/>'s bounds.
        /// </summary>
        /// <param name="vec">The vec.</param>
        /// <returns>True if within, else false.</returns>
        public bool Contains(Vec2D vec)
        {
        	return ((vec - Position) / Radius).LengthSq() <= 1;
        }
        
        /// <summary>
        /// Implicit cast to ellipse.
        /// </summary>
        /// <param name="circle">The circle.</param>
        /// <returns>The equivalent ellipse.</returns>
        public static implicit operator Ellipse(Circle circle)
        {
        	return new Ellipse{Position = circle.Position, Radii = circle.Radius};
        }

        /// <summary>
        /// Multiplies the given circle's size by the given value, keeping it centered.
        /// </summary>
        /// <param name="circle">The circle.</param>
        /// <param name="val">The value.</param>
        /// <returns>The resized circle.</returns>
        public static Circle operator *(Circle circle, double val)
        {
        	return new Circle{Position = circle.Position, Radius = circle.Radius * val};
        }

        /// <summary>
        /// Divides the given circle's size by the given value, keeping it centered.
        /// </summary>
        /// <param name="circle">The circle.</param>
        /// <param name="val">The value.</param>
        /// <returns>The resized circle.</returns>
        public static Circle operator /(Circle circle, double val)
        {
        	return new Circle{Position = circle.Position, Radius = circle.Radius / val};
        }
        
        /// <summary>
        /// True if the circles are equal.
        /// </summary>
        /// <param name="circle">The first circle.</param>
        /// <param name="circle2">The second circle.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(Circle circle, Circle circle2)
        {
        	return (circle.Position == circle2.Position) && (circle.Radius == circle2.Radius);
        }
        
        /// <summary>
        /// True if the circles are not equal.
        /// </summary>
        /// <param name="circle">The first circle.</param>
        /// <param name="circle2">The second circle.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(Circle circle, Circle circle2)
        {
        	return !(circle == circle2);
        }
	}
}