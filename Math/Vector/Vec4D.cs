namespace IROM.Util
{
	using System;
	using System.Linq;
	
    /// <summary>
    /// Simple 4D double vector struct.
    /// </summary>
    public struct Vec4D
    {
    	/// <summary>
    	/// The zero value.
    	/// </summary>
    	public static readonly Vec4D Zero = 0;
    	
        /// <summary>
        /// The x value.
        /// </summary>
        public double X;
        
        /// <summary>
        /// The y value.
        /// </summary>
        public double Y;
        
        /// <summary>
        /// The z value.
        /// </summary>
        public double Z;
        
        /// <summary>
        /// The w value.
        /// </summary>
        public double W;
        
        /// <summary>
        /// Creates a new <see cref="Vec4D"/> with the given values.
        /// </summary>
        /// <param name="i">The x value.</param>
        /// <param name="j">The y value.</param>
        /// <param name="k">The z value.</param>
        /// <param name="l">The w value.</param>
        public Vec4D(double i, double j, double k, double l)
        {
            X = i;
            Y = j;
            Z = k;
            W = l;
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
        			case 2: return Z;
        			case 3: return W;
        			default: throw new IndexOutOfRangeException(index + " out of Vec4D range.");
        		}
        	}
        	set
        	{
        		switch(index)
        		{
        			case 0: X = value; break;
        			case 1: Y = value; break;
        			case 2: Z = value; break;
        			case 3: W = value; break;
        			default: throw new IndexOutOfRangeException(index + " out of Vec4D range.");
        		}
        	}
        }
        
        public override string ToString()
		{
			return string.Format("Vec4D({0}, {1}, {2}, {3})", X, Y, Z, W);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is Vec4D)
        	{
        		return this == (Vec4D)obj;
        	}else
        	{
        		return false;
        	}
        }
        
        public override int GetHashCode()
        {
        	// disable NonReadonlyReferencedInGetHashCode
        	return (int)Hash.PerformStaticHash((uint)X.GetHashCode(), (uint)Y.GetHashCode(), (uint)Z.GetHashCode(), (uint)W.GetHashCode());
        }
        
        /// <summary>
        /// Returns the component-wise rounded version of this vector.
        /// </summary>
        /// <returns>The rounded vec.</returns>
        public Point4D Round()
        {
        	return new Point4D((int)Math.Round(X), (int)Math.Round(Y), (int)Math.Round(Z), (int)Math.Round(W));
        }
        
        /// <summary>
        /// Returns the component-wise floor version of this vector.
        /// </summary>
        /// <returns>The rounded vec.</returns>
        public Point4D Floor()
        {
        	return new Point4D((int)Math.Floor(X), (int)Math.Floor(Y), (int)Math.Floor(Z), (int)Math.Floor(W));
        }
        
        /// <summary>
        /// Returns the component-wise ceiling version of this vector.
        /// </summary>
        /// <returns>The rounded vec.</returns>
        public Point4D Ceiling()
        {
        	return new Point4D((int)Math.Ceiling(X), (int)Math.Ceiling(Y), (int)Math.Ceiling(Z), (int)Math.Ceiling(W));
        }
        
        /// <summary>
        /// Returns the squared length of this <see cref="Vec4D"/>.
        /// </summary>
        /// <returns>The squared length.</returns>
        public double LengthSq()
        {
        	return (X * X) + (Y * Y) + (Z * Z) + (W * W);
        }
        
        /// <summary>
        /// Returns the length of this <see cref="Vec4D"/>.
        /// </summary>
        /// <returns>The length.</returns>
        public double Length()
        {
        	return Math.Sqrt(LengthSq());
        }
        
        /// <summary>
        /// Returns the direction of this <see cref="Vec4D"/>.
        /// </summary>
        /// <returns>The direction <see cref="Vec4D"/></returns>
        public Vec4D Normalized()
        {
        	return this / Length();
        }
        
        /// <summary>
        /// Explicit cast to <see cref="Point4D"/>.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static explicit operator Point4D(Vec4D vec)
        {
        	return new Point4D((int)vec.X, (int)vec.Y, (int)vec.Z, (int)vec.W);
        }
        
        /// <summary>
        /// Implicit cast from double. Fills all fields with the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The resulting vec.</returns>
        public static implicit operator Vec4D(double value)
        {
            return new Vec4D(value, value, value, value);
        }

        /// <summary>
        /// Explicit cast to <see cref="Vec3D"/>. Drops extra data.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static explicit operator Vec3D(Vec4D vec)
        {
            return new Vec3D(vec.X, vec.Y, vec.Z);
        }

		/// <summary>
        /// Negates this vec.
        /// </summary>
        /// <param name="vec">The vec to negate.</param>
        /// <returns>The negated vec.</returns>
        public static Vec4D operator -(Vec4D vec)
        {
            return new Vec4D(-vec.X, -vec.Y, -vec.Z, -vec.W);
        }

        /// <summary>
        /// Adds the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The sum vec.</returns>
        public static Vec4D operator +(Vec4D vec, Vec4D vec2)
        {
            return new Vec4D(vec.X + vec2.X, vec.Y + vec2.Y, vec.Z + vec2.Z, vec.W + vec2.W);
        }

        /// <summary>
        /// Subtracts the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The difference vec.</returns>
        public static Vec4D operator -(Vec4D vec, Vec4D vec2)
        {
            return new Vec4D(vec.X - vec2.X, vec.Y - vec2.Y, vec.Z - vec2.Z, vec.W - vec2.W);
        }

        /// <summary>
        /// Multiplies the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The product vec.</returns>
        public static Vec4D operator *(Vec4D vec, Vec4D vec2)
        {
            return new Vec4D(vec.X * vec2.X, vec.Y * vec2.Y, vec.Z * vec2.Z, vec.W * vec2.W);
        }

        /// <summary>
        /// Divides the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The quotient vec.</returns>
        public static Vec4D operator /(Vec4D vec, Vec4D vec2)
        {
            return new Vec4D(vec.X / vec2.X, vec.Y / vec2.Y, vec.Z / vec2.Z, vec.W / vec2.W);
        }
        
        /// <summary>
        /// Modulos the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The modulo vec.</returns>
        public static Vec4D operator %(Vec4D vec, Vec4D vec2)
        {
            return new Vec4D(vec.X % vec2.X, vec.Y % vec2.Y, vec.Z % vec2.Z, vec.W % vec2.W);
        }
        
        /// <summary>
        /// True if the vecs are equal.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(Vec4D vec, Vec4D vec2)
        {
        	return vec.X == vec2.X && vec.Y == vec2.Y && vec.Z == vec2.Z && vec.W == vec2.W;
        }
        
        /// <summary>
        /// True if the vecs are not equal.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(Vec4D vec, Vec4D vec2)
        {
        	return !(vec == vec2);
        }
        
        /// <summary>
        /// Performs the dot product of two vectors.
        /// </summary>
        /// <param name="vec">The first vector.</param>
        /// <param name="vec2">The second vector.</param>
        /// <returns>The dot product.</returns>
        public static double Dot(Vec4D vec, Vec4D vec2)
        {
        	return vec.X * vec2.X + vec.Y * vec2.Y + vec.Z * vec2.Z + vec.W * vec2.W;
        }
    }
}
