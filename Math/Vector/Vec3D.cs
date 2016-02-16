namespace IROM.Util
{
	using System;
	using System.Linq;
	
    /// <summary>
    /// Simple 3D double vector struct.
    /// </summary>
    public struct Vec3D
    {
    	/// <summary>
    	/// The zero value.
    	/// </summary>
    	public static readonly Vec3D Zero = 0;
    	
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
        /// Creates a new <see cref="Vec3D"/> with the given values.
        /// </summary>
        /// <param name="i">The x value.</param>
        /// <param name="j">The y value.</param>
        /// <param name="k">The z value.</param>
        public Vec3D(double i, double j, double k)
        {
            X = i;
            Y = j;
            Z = k;
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
        			default: throw new IndexOutOfRangeException(index + " out of Vec3D range.");
        		}
        	}
        	set
        	{
        		switch(index)
        		{
        			case 0: X = value; break;
        			case 1: Y = value; break;
        			case 2: Z = value; break;
        			default: throw new IndexOutOfRangeException(index + " out of Vec3D range.");
        		}
        	}
        }
        
        public override string ToString()
		{
			return string.Format("Vec3D({0}, {1}, {2})", X, Y, Z);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is Vec3D)
        	{
        		return this == (Vec3D)obj;
        	}else
        	{
        		return false;
        	}
        }
        
        public override int GetHashCode()
        {
        	// disable NonReadonlyReferencedInGetHashCode
        	return (int)Hash.PerformStaticHash((uint)X.GetHashCode(), (uint)Y.GetHashCode(), (uint)Z.GetHashCode());
        }
        
        /// <summary>
        /// Returns the component-wise rounded version of this vector.
        /// </summary>
        /// <returns>The rounded vec.</returns>
        public Point3D Round()
        {
        	return new Point3D((int)Math.Round(X), (int)Math.Round(Y), (int)Math.Round(Z));
        }
        
        /// <summary>
        /// Returns the component-wise floor version of this vector.
        /// </summary>
        /// <returns>The rounded vec.</returns>
        public Point3D Floor()
        {
        	return new Point3D((int)Math.Floor(X), (int)Math.Floor(Y), (int)Math.Floor(Z));
        }
        
        /// <summary>
        /// Returns the component-wise ceiling version of this vector.
        /// </summary>
        /// <returns>The rounded vec.</returns>
        public Point3D Ceiling()
        {
        	return new Point3D((int)Math.Ceiling(X), (int)Math.Ceiling(Y), (int)Math.Ceiling(Z));
        }
        
        /// <summary>
        /// Returns the squared length of this <see cref="Vec3D"/>.
        /// </summary>
        /// <returns>The squared length.</returns>
        public double LengthSq()
        {
        	return (X * X) + (Y * Y) + (Z * Z);
        }
        
        /// <summary>
        /// Returns the length of this <see cref="Vec3D"/>.
        /// </summary>
        /// <returns>The length.</returns>
        public double Length()
        {
        	return Math.Sqrt(LengthSq());
        }
        
        /// <summary>
        /// Returns the direction of this <see cref="Vec3D"/>.
        /// </summary>
        /// <returns>The direction <see cref="Vec3D"/></returns>
        public Vec3D Normalized()
        {
        	return this / Length();
        }
        
        /// <summary>
        /// Explicit cast to <see cref="Point3D"/>.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static explicit operator Point3D(Vec3D vec)
        {
        	return new Point3D((int)vec.X, (int)vec.Y, (int)vec.Z);
        }
        
        /// <summary>
        /// Implicit cast from double. Fills all fields with the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The resulting vec.</returns>
        public static implicit operator Vec3D(double value)
        {
            return new Vec3D(value, value, value);
        }

        /// <summary>
        /// Explicit cast to <see cref="Vec2D"/>. Drops extra data.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static explicit operator Vec2D(Vec3D vec)
        {
            return new Vec2D(vec.X, vec.Y);
        }

        /// <summary>
        /// Explicit cast to <see cref="Vec4D"/>. Fills missing data with 0's.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static implicit operator Vec4D(Vec3D vec)
        {
            return new Vec4D(vec.X, vec.Y, vec.Z, 0);
        }

		/// <summary>
        /// Negates this vec.
        /// </summary>
        /// <param name="vec">The vec to negate.</param>
        /// <returns>The negated vec.</returns>
        public static Vec3D operator -(Vec3D vec)
        {
            return new Vec3D(-vec.X, -vec.Y, -vec.Z);
        }

        /// <summary>
        /// Adds the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The sum vec.</returns>
        public static Vec3D operator +(Vec3D vec, Vec3D vec2)
        {
            return new Vec3D(vec.X + vec2.X, vec.Y + vec2.Y, vec.Z + vec2.Z);
        }

        /// <summary>
        /// Subtracts the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The difference vec.</returns>
        public static Vec3D operator -(Vec3D vec, Vec3D vec2)
        {
            return new Vec3D(vec.X - vec2.X, vec.Y - vec2.Y, vec.Z - vec2.Z);
        }

        /// <summary>
        /// Multiplies the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The product vec.</returns>
        public static Vec3D operator *(Vec3D vec, Vec3D vec2)
        {
            return new Vec3D(vec.X * vec2.X, vec.Y * vec2.Y, vec.Z * vec2.Z);
        }

        /// <summary>
        /// Divides the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The quotient vec.</returns>
        public static Vec3D operator /(Vec3D vec, Vec3D vec2)
        {
            return new Vec3D(vec.X / vec2.X, vec.Y / vec2.Y, vec.Z / vec2.Z);
        }
        
        /// <summary>
        /// Modulos the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The modulo vec.</returns>
        public static Vec3D operator %(Vec3D vec, Vec3D vec2)
        {
            return new Vec3D(vec.X % vec2.X, vec.Y % vec2.Y, vec.Z % vec2.Z);
        }
        
        /// <summary>
        /// True if the vecs are equal.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(Vec3D vec, Vec3D vec2)
        {
        	return vec.X == vec2.X && vec.Y == vec2.Y && vec.Z == vec2.Z;
        }
        
        /// <summary>
        /// True if the vecs are not equal.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(Vec3D vec, Vec3D vec2)
        {
        	return !(vec == vec2);
        }
        
        /// <summary>
        /// Performs the dot product of two vectors.
        /// </summary>
        /// <param name="vec">The first vector.</param>
        /// <param name="vec2">The second vector.</param>
        /// <returns>The dot product.</returns>
        public static double Dot(Vec3D vec, Vec3D vec2)
        {
        	return vec.X * vec2.X + vec.Y * vec2.Y + vec.Z * vec2.Z;
        }
        
        /// <summary>
        /// Performs the cross product of two vectors.
        /// </summary>
        /// <param name="vec">The first vector.</param>
        /// <param name="vec2">The second vector.</param>
        /// <returns>The cross product.</returns>
        public static Vec3D Cross(Vec3D vec, Vec3D vec2)
        {
        	return new Vec3D((vec.Y * vec2.Z) - (vec.Z * vec2.Y), (vec.Z * vec2.X) - (vec.X * vec2.Z), (vec.X * vec2.Y) - (vec.Y * vec2.X));
        }
    }
}
