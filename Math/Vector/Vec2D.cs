namespace IROM.Util
{
	using System;
	using System.Linq;
	
    /// <summary>
    /// Simple 2D double vector struct.
    /// </summary>
    public struct Vec2D
    {
    	/// <summary>
    	/// The zero value.
    	/// </summary>
    	public static readonly Vec2D Zero = 0;
    	
        /// <summary>
        /// The x value.
        /// </summary>
        public double X;
        
        /// <summary>
        /// The y value.
        /// </summary>
        public double Y;
        
        /// <summary>
        /// Creates a new <see cref="Vec2D"/> with the given values.
        /// </summary>
        /// <param name="i">The x value.</param>
        /// <param name="j">The y value.</param>
        public Vec2D(double i, double j)
        {
            X = i;
            Y = j;
        }
        
        /// <summary>
        /// Accesses the given value of this vector.
        /// </summary>
        public double this[int index]
        {
        	get
        	{
        		switch(index)
        		{
        			case 0: return X;
        			case 1: return Y;
        			default: throw new IndexOutOfRangeException(index + " out of Vec2D range.");
        		}
        	}
        	set
        	{
        		switch(index)
        		{
        			case 0: X = value; break;
        			case 1: Y = value; break;
        			default: throw new IndexOutOfRangeException(index + " out of Vec2D range.");
        		}
        	}
        }
        
        public override string ToString()
		{
			return string.Format("Vec2D({0}, {1})", X, Y);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is Vec2D)
        	{
        		return this == (Vec2D)obj;
        	}else
        	{
        		return false;
        	}
        }
        
        public override int GetHashCode()
        {
        	// disable NonReadonlyReferencedInGetHashCode
        	return (int)Hash.PerformStaticHash((uint)X.GetHashCode(), (uint)Y.GetHashCode());
        }
        
        /// <summary>
        /// Returns the component-wise rounded version of this vector.
        /// </summary>
        /// <returns>The rounded vec.</returns>
        public Point2D Round()
        {
        	return new Point2D((int)Math.Round(X), (int)Math.Round(Y));
        }
        
        /// <summary>
        /// Returns the component-wise floor version of this vector.
        /// </summary>
        /// <returns>The rounded vec.</returns>
        public Point2D Floor()
        {
        	return new Point2D((int)Math.Floor(X), (int)Math.Floor(Y));
        }
        
        /// <summary>
        /// Returns the component-wise ceiling version of this vector.
        /// </summary>
        /// <returns>The rounded vec.</returns>
        public Point2D Ceiling()
        {
        	return new Point2D((int)Math.Ceiling(X), (int)Math.Ceiling(Y));
        }
        
        /// <summary>
        /// Returns the squared length of this <see cref="Vec2D"/>.
        /// </summary>
        /// <returns>The squared length.</returns>
        public double LengthSq()
        {
        	return (X * X) + (Y * Y);
        }
        
        /// <summary>
        /// Returns the length of this <see cref="Vec2D"/>.
        /// </summary>
        /// <returns>The length.</returns>
        public double Length()
        {
        	return Math.Sqrt(LengthSq());
        }
        
        /// <summary>
        /// Returns the direction of this <see cref="Vec2D"/>.
        /// </summary>
        /// <returns>The direction <see cref="Vec2D"/></returns>
        public Vec2D Normalized()
        {
        	return this / Length();
        }
        
        /// <summary>
        /// Rotates this <see cref="Vec2D"/> the given number of radians.
        /// </summary>
        /// <param name="theta">The angle in radians.</param>
        /// <returns>The component-wise wrapped vec.</returns>
        public Vec2D Rotate(double theta)
        {
        	double cos = Math.Cos(theta);
        	double sin = Math.Sin(theta);
        	return new Vec2D((X * cos) - (Y * sin), (X * sin) + (Y * cos));
        }
        
        /// <summary>
        /// Explicit cast to <see cref="Point2D"/>.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static explicit operator Point2D(Vec2D vec)
        {
        	return new Point2D((int)vec.X, (int)vec.Y);
        }
        
        /// <summary>
        /// Implicit cast from double. Fills all fields with the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The resulting vec.</returns>
        public static implicit operator Vec2D(double value)
        {
            return new Vec2D(value, value);
        }

        /// <summary>
        /// Explicit cast to <see cref="Vec1D"/>. Drops extra data.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static explicit operator Vec1D(Vec2D vec)
        {
            return new Vec1D(vec.X);
        }

        /// <summary>
        /// Explicit cast to <see cref="Vec3D"/>. Fills missing data with 0's.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static implicit operator Vec3D(Vec2D vec)
        {
            return new Vec3D(vec.X, vec.Y, 0);
        }
        
		/// <summary>
        /// Negates this vec.
        /// </summary>
        /// <param name="vec">The vec to negate.</param>
        /// <returns>The negated vec.</returns>
        public static Vec2D operator -(Vec2D vec)
        {
            return new Vec2D(-vec.X, -vec.Y);
        }

        /// <summary>
        /// Adds the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The sum vec.</returns>
        public static Vec2D operator +(Vec2D vec, Vec2D vec2)
        {
            return new Vec2D(vec.X + vec2.X, vec.Y + vec2.Y);
        }

        /// <summary>
        /// Subtracts the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The difference vec.</returns>
        public static Vec2D operator -(Vec2D vec, Vec2D vec2)
        {
            return new Vec2D(vec.X - vec2.X, vec.Y - vec2.Y);
        }

        /// <summary>
        /// Multiplies the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The product vec.</returns>
        public static Vec2D operator *(Vec2D vec, Vec2D vec2)
        {
            return new Vec2D(vec.X * vec2.X, vec.Y * vec2.Y);
        }

        /// <summary>
        /// Divides the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The quotient vec.</returns>
        public static Vec2D operator /(Vec2D vec, Vec2D vec2)
        {
            return new Vec2D(vec.X / vec2.X, vec.Y / vec2.Y);
        }
        
        /// <summary>
        /// Modulos the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The modulo vec.</returns>
        public static Vec2D operator %(Vec2D vec, Vec2D vec2)
        {
            return new Vec2D(vec.X % vec2.X, vec.Y % vec2.Y);
        }
        
        /// <summary>
        /// True if the vecs are equal.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(Vec2D vec, Vec2D vec2)
        {
        	return vec.X == vec2.X && vec.Y == vec2.Y;
        }
        
        /// <summary>
        /// True if the vecs are not equal.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(Vec2D vec, Vec2D vec2)
        {
        	return !(vec == vec2);
        }
        
        /// <summary>
        /// Performs the dot product of two vectors.
        /// </summary>
        /// <param name="vec">The first vector.</param>
        /// <param name="vec2">The second vector.</param>
        /// <returns>The dot product.</returns>
        public static double Dot(Vec2D vec, Vec2D vec2)
        {
        	return vec.X * vec2.X + vec.Y * vec2.Y;
        }
    }
}
