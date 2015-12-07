namespace IROM.Util
{
	using System;
	using System.Linq;
	
    /// <summary>
    /// Simple 4D int vector struct.
    /// </summary>
    public struct Point4D
    {
    	/// <summary>
    	/// The zero value.
    	/// </summary>
    	public static readonly Point4D Zero = 0;
    	
        /// <summary>
        /// The x value.
        /// </summary>
        public int X;
        
        /// <summary>
        /// The y value.
        /// </summary>
        public int Y;
        
        /// <summary>
        /// The z value.
        /// </summary>
        public int Z;
        
        /// <summary>
        /// The w value.
        /// </summary>
        public int W;
        
        /// <summary>
        /// Creates a new <see cref="Point4D"/> with the given values.
        /// </summary>
        /// <param name="i">The x value.</param>
        /// <param name="j">The y value.</param>
        /// <param name="k">The z value.</param>
        /// <param name="l">The w value.</param>
        public Point4D(int i, int j, int k, int l)
        {
            X = i;
            Y = j;
            Z = k;
            W = l;
        }
        
        /// <summary>
        /// Accesses the given value of this point.
        /// </summary>
        public int this[int index]
        {
        	get
        	{
        		switch(index)
        		{
        			case 0: return X;
        			case 1: return Y;
        			case 2: return Z;
        			case 3: return W;
        			default: throw new IndexOutOfRangeException(index + " out of Point4D range.");
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
        			default: throw new IndexOutOfRangeException(index + " out of Point4D range.");
        		}
        	}
        }
        
        public override string ToString()
		{
			return string.Format("Point4D({0}, {1}, {2}, {3})", X, Y, Z, W);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is Point4D)
        	{
        		return this == (Point4D)obj;
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
        /// Implicit cast to <see cref="Vec4D"/>.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The resulting point.</returns>
        public static implicit operator Vec4D(Point4D point)
        {
        	return new Vec4D(point.X, point.Y, point.Z, point.W);
        }
        
        /// <summary>
        /// Implicit cast from int. Fills all fields with the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The resulting vec.</returns>
        public static implicit operator Point4D(int value)
        {
            return new Point4D(value, value, value, value);
        }

        /// <summary>
        /// Explicit cast to <see cref="Point3D"/>. Drops extra data.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The resulting point.</returns>
        public static explicit operator Point3D(Point4D point)
        {
            return new Point3D(point.X, point.Y, point.Z);
        }

		/// <summary>
        /// Negates this point.
        /// </summary>
        /// <param name="point">The point to negate.</param>
        /// <returns>The negated point.</returns>
        public static Point4D operator -(Point4D point)
        {
            return new Point4D(-point.X, -point.Y, -point.Z, -point.W);
        }

        /// <summary>
        /// Adds the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The sum point.</returns>
        public static Point4D operator +(Point4D point, Point4D point2)
        {
            return new Point4D(point.X + point2.X, point.Y + point2.Y, point.Z + point2.Z, point.W + point2.W);
        }

        /// <summary>
        /// Subtracts the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The difference point.</returns>
        public static Point4D operator -(Point4D point, Point4D point2)
        {
            return new Point4D(point.X - point2.X, point.Y - point2.Y, point.Z - point2.Z, point.W - point2.W);
        }

        /// <summary>
        /// Multiplies the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The product point.</returns>
        public static Point4D operator *(Point4D point, Point4D point2)
        {
            return new Point4D(point.X * point2.X, point.Y * point2.Y, point.Z * point2.Z, point.W * point2.W);
        }
        
        /// <summary>
        /// Multiplies the given point by the given value.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="value">The value.</param>
        /// <returns>The product point.</returns>
        public static Point4D operator *(Vec4D value, Point4D point)
        {
        	return new Point4D((int)(point.X * value.X), (int)(point.Y * value.Y), (int)(point.Z * value.Z), (int)(point.W * value.W));
        }
        
        /// <summary>
        /// Multiplies the given point by the given value.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="value">The value.</param>
        /// <returns>The product point.</returns>
        public static Point4D operator *(Point4D point, Vec4D value)
        {
        	return new Point4D((int)(point.X * value.X), (int)(point.Y * value.Y), (int)(point.Z * value.Z), (int)(point.W * value.W));
        }

        /// <summary>
        /// Divides the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The quotient point.</returns>
        public static Point4D operator /(Point4D point, Point4D point2)
        {
            return new Point4D(point.X / point2.X, point.Y / point2.Y, point.Z / point2.Z, point.W / point2.W);
        }
        
        /// <summary>
        /// Divides the given point by the given value.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="value">The value.</param>
        /// <returns>The quotient point.</returns>
        public static Point4D operator /(Point4D point, Vec4D value)
        {
        	return new Point4D((int)(point.X / value.X), (int)(point.Y / value.Y), (int)(point.Z / value.Z), (int)(point.W / value.W));
        }
        
        /// <summary>
        /// Modulos the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The modulo point.</returns>
        public static Point4D operator %(Point4D point, Point4D point2)
        {
            return new Point4D(point.X % point2.X, point.Y % point2.Y, point.Z % point2.Z, point.W % point2.W);
        }
        
        /// <summary>
        /// True if the points are equal.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(Point4D point, Point4D point2)
        {
        	return point.X == point2.X && point.Y == point2.Y && point.Z == point2.Z && point.W == point2.W;
        }
        
        /// <summary>
        /// True if the points are not equal.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(Point4D point, Point4D point2)
        {
        	return !(point == point2);
        }
    }
}