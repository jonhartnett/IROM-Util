namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Simple 1D double vector struct.
	/// </summary>
	public struct Vec1D
	{
		/// <summary>
    	/// The zero value.
    	/// </summary>
    	public static readonly Vec1D Zero = 0;
    	
        /// <summary>
        /// The x value.
        /// </summary>
        public double X;
        
        /// <summary>
        /// Creates a new <see cref="Vec1D"/> with the given value.
        /// </summary>
        /// <param name="i">The x value.</param>
        public Vec1D(double i)
        {
            X = i;
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
        			default: throw new IndexOutOfRangeException(index + " out of Vec1D range.");
        		}
        	}
        	set
        	{
        		switch(index)
        		{
        			case 0: X = value; break;
        			default: throw new IndexOutOfRangeException(index + " out of Vec1D range.");
        		}
        	}
        }
        
        public override string ToString()
		{
			return string.Format("Vec1D({0})", X);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is Vec1D)
        	{
        		return this == (Vec1D)obj;
        	}else
        	{
        		return false;
        	}
        }
        
        public override int GetHashCode()
        {
        	// disable NonReadonlyReferencedInGetHashCode
        	return (int)Hash.PerformStaticHash((uint)X.GetHashCode());
        }
        
        /// <summary>
        /// Returns the component-wise rounded version of this vector.
        /// </summary>
        /// <returns>The rounded vec.</returns>
        public Point1D Round()
        {
        	return new Point1D((int)Math.Round(X));
        }
        
        /// <summary>
        /// Returns the component-wise floor version of this vector.
        /// </summary>
        /// <returns>The rounded vec.</returns>
        public Point1D Floor()
        {
        	return new Point1D((int)Math.Floor(X));
        }
        
        /// <summary>
        /// Returns the component-wise ceiling version of this vector.
        /// </summary>
        /// <returns>The rounded vec.</returns>
        public Point1D Ceiling()
        {
        	return new Point1D((int)Math.Ceiling(X));
        }

        /// <summary>
        /// Implicit cast to the data type.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The value.</returns>
        public static implicit operator double(Vec1D vec)
        {
            return vec.X;
        }

        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="val">The value to cast.</param>
        /// <returns>The vec.</returns>
        public static implicit operator Vec1D(double val)
        {
            return new Vec1D(val);
        }
        
        /// <summary>
        /// Explicit cast to <see cref="Point1D"/>.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting point.</returns>
        public static explicit operator Point1D(Vec1D vec)
        {
        	return new Point1D((int)vec.X);
        }

        /// <summary>
        /// Explicit cast to <see cref="Vec2D"/>. Fills missing data with 0's.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static implicit operator Vec2D(Vec1D vec)
        {
            return new Vec2D(vec.X, 0);
        }

		/// <summary>
        /// Negates this vec.
        /// </summary>
        /// <param name="vec">The vec to negate.</param>
        /// <returns>The negated vec.</returns>
        public static Vec1D operator -(Vec1D vec)
        {
            return new Vec1D(-vec.X);
        }

        /// <summary>
        /// Adds the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The sum vec.</returns>
        public static Vec1D operator +(Vec1D vec, Vec1D vec2)
        {
            return new Vec1D(vec.X + vec2.X);
        }

        /// <summary>
        /// Subtracts the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The difference vec.</returns>
        public static Vec1D operator -(Vec1D vec, Vec1D vec2)
        {
            return new Vec1D(vec.X - vec2.X);
        }

        /// <summary>
        /// Multiplies the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The product vec.</returns>
        public static Vec1D operator *(Vec1D vec, Vec1D vec2)
        {
            return new Vec1D(vec.X * vec2.X);
        }

        /// <summary>
        /// Divides the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The quotient vec.</returns>
        public static Vec1D operator /(Vec1D vec, Vec1D vec2)
        {
            return new Vec1D(vec.X / vec2.X);
        }
        
        /// <summary>
        /// Modulos the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The modulo vec.</returns>
        public static Vec1D operator %(Vec1D vec, Vec1D vec2)
        {
            return new Vec1D(vec.X % vec2.X);
        }
        
        /// <summary>
        /// True if the vecs are equal.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(Vec1D vec, Vec1D vec2)
        {
        	return vec.X == vec2.X;
        }
        
        /// <summary>
        /// True if the vecs are not equal.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(Vec1D vec, Vec1D vec2)
        {
        	return !(vec == vec2);
        }
	}
}
