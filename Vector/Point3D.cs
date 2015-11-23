namespace IROM.Util
{
	using System;
	using System.Linq;
	
    /// <summary>
    /// Simple 3D int vector struct.
    /// </summary>
    public struct Point3D
    {
    	/// <summary>
    	/// The zero value.
    	/// </summary>
    	public static readonly Point3D Zero = 0;
    	
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
        /// Creates a new <see cref="Point3D"/> with the given values.
        /// </summary>
        /// <param name="i">The x value.</param>
        /// <param name="j">The y value.</param>
        /// <param name="k">The z value.</param>
        public Point3D(int i, int j, int k)
        {
            X = i;
            Y = j;
            Z = k;
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
        			default: throw new IndexOutOfRangeException(index + " out of Point3D range.");
        		}
        	}
        	set
        	{
        		switch(index)
        		{
        			case 0: X = value; break;
        			case 1: Y = value; break;
        			case 2: Z = value; break;
        			default: throw new IndexOutOfRangeException(index + " out of Point3D range.");
        		}
        	}
        }
        
        public override string ToString()
		{
			return string.Format("Point3D({0}, {1}, {2})", X, Y, Z);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is Point3D)
        	{
        		return this == (Point3D)obj;
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
        /// Component-wise clips this <see cref="Point3D"/> to between min and max.
        /// </summary>
        /// <param name="min">The min point.</param>
        /// <param name="max">The max point.</param>
        /// <returns>The component-wise clipped point.</returns>
        public Point3D Clip(Point3D min, Point3D max)
        {
        	Point3D point;
        	
        	if(X <= min.X) point.X = min.X;
        	else if(X >= max.X) point.X = max.X;
        	else point.X = X;
        	
        	if(Y <= min.Y) point.Y = min.Y;
        	else if(Y >= max.Y) point.Y = max.Y;
        	else point.Y = Y;
        	
        	if(Z <= min.Z) point.Z = min.Z;
        	else if(Z >= max.Z) point.Z = max.Z;
        	else point.Z = Z;
        	
        	return point;
        }
        
        /// <summary>
        /// Component-wise wraps this <see cref="Point3D"/> to between min (inclusive) and max (exclusive).
        /// </summary>
        /// <param name="min">The min point.</param>
        /// <param name="max">The max point.</param>
        /// <returns>The component-wise wrapped point.</returns>
        public Point3D Wrap(Point3D min, Point3D max)
        {
        	Point3D point = this;
        	
        	point.X -= min.X;
			point.X %= max.X - min.X;
        	if(point.X < 0) point.X += max.X - min.X;
        	point.X += min.X;
        	
        	point.Y -= min.Y;
			point.Y %= max.Y - min.Y;
        	if(point.Y < 0) point.Y += max.Y - min.Y;
        	point.Y += min.Y;
        	
        	point.Z -= min.Z;
			point.Z %= max.Z - min.Z;
        	if(point.Z < 0) point.Z += max.Z - min.Z;
        	point.Z += min.Z;
        	
        	return point;
        }
        
        /// <summary>
        /// Implicit cast to <see cref="Vec3D"/>.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The resulting point.</returns>
        public static implicit operator Vec3D(Point3D point)
        {
        	return new Vec3D(point.X, point.Y, point.Z);
        }
        
        /// <summary>
        /// Implicit cast from int. Fills all fields with the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The resulting vec.</returns>
        public static implicit operator Point3D(int value)
        {
            return new Point3D(value, value, value);
        }

        /// <summary>
        /// Explicit cast to <see cref="Point2D"/>. Drops extra data.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The resulting point.</returns>
        public static explicit operator Point2D(Point3D point)
        {
            return new Point2D(point.X, point.Y);
        }

        /// <summary>
        /// Explicit cast to <see cref="Point4D"/>. Fills missing data with 0's.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The resulting point.</returns>
        public static implicit operator Point4D(Point3D point)
        {
            return new Point4D(point.X, point.Y, point.Z, 0);
        }

		/// <summary>
        /// Negates this point.
        /// </summary>
        /// <param name="point">The point to negate.</param>
        /// <returns>The negated point.</returns>
        public static Point3D operator -(Point3D point)
        {
            return new Point3D(-point.X, -point.Y, -point.Z);
        }

        /// <summary>
        /// Adds the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The sum point.</returns>
        public static Point3D operator +(Point3D point, Point3D point2)
        {
            return new Point3D(point.X + point2.X, point.Y + point2.Y, point.Z + point2.Z);
        }

        /// <summary>
        /// Subtracts the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The difference point.</returns>
        public static Point3D operator -(Point3D point, Point3D point2)
        {
            return new Point3D(point.X - point2.X, point.Y - point2.Y, point.Z - point2.Z);
        }

        /// <summary>
        /// Multiplies the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The product point.</returns>
        public static Point3D operator *(Point3D point, Point3D point2)
        {
            return new Point3D(point.X * point2.X, point.Y * point2.Y, point.Z * point2.Z);
        }
        
        /// <summary>
        /// Multiplies the given point by the given value.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="value">The value.</param>
        /// <returns>The product point.</returns>
        public static Point3D operator *(Vec3D value, Point3D point)
        {
        	return new Point3D((int)(point.X * value.X), (int)(point.Y * value.Y), (int)(point.Z * value.Z));
        }
        
        /// <summary>
        /// Multiplies the given point by the given value.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="value">The value.</param>
        /// <returns>The product point.</returns>
        public static Point3D operator *(Point3D point, Vec3D value)
        {
        	return new Point3D((int)(point.X * value.Y), (int)(point.Y * value.Y), (int)(point.Z * value.Z));
        }
        
        /// <summary>
        /// Divides the given point by the given value.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="value">The value.</param>
        /// <returns>The quotient point.</returns>
        public static Point3D operator /(Point3D point, Vec3D value)
        {
        	return new Point3D((int)(point.X / value.X), (int)(point.Y / value.Y), (int)(point.Z / value.Z));
        }
        
        /// <summary>
        /// Divides the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The quotient point.</returns>
        public static Point3D operator /(Point3D point, Point3D point2)
        {
            return new Point3D(point.X / point2.X, point.Y / point2.Y, point.Z / point2.Z);
        }
        
        /// <summary>
        /// Modulos the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The modulo point.</returns>
        public static Point3D operator %(Point3D point, Point3D point2)
        {
            return new Point3D(point.X % point2.X, point.Y % point2.Y, point.Z % point2.Z);
        }
        
        /// <summary>
        /// True if the points are equal.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(Point3D point, Point3D point2)
        {
        	return point.X == point2.X && point.Y == point2.Y && point.Z == point2.Z;
        }
        
        /// <summary>
        /// True if the points are not equal.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(Point3D point, Point3D point2)
        {
        	return !(point == point2);
        }
    }
}
