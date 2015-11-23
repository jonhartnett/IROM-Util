namespace IROM.Util
{
	using System;
	using System.Linq;
	
    /// <summary>
    /// Simple 2D int vector struct.
    /// </summary>
    public struct Point2D
    {
    	/// <summary>
    	/// The zero value.
    	/// </summary>
    	public static readonly Point2D Zero = 0;
    	
        /// <summary>
        /// The x value.
        /// </summary>
        public int X;
        
        /// <summary>
        /// The y value.
        /// </summary>
        public int Y;
        
        /// <summary>
        /// Creates a new <see cref="Point2D"/> with the given values.
        /// </summary>
        /// <param name="i">The x value.</param>
        /// <param name="j">The y value.</param>
        public Point2D(int i, int j)
        {
            X = i;
            Y = j;
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
        			default: throw new IndexOutOfRangeException(index + " out of Point2D range.");
        		}
        	}
        	set
        	{
        		switch(index)
        		{
        			case 0: X = value; break;
        			case 1: Y = value; break;
        			default: throw new IndexOutOfRangeException(index + " out of Point2D range.");
        		}
        	}
        }
        
        public override string ToString()
		{
			return string.Format("Point2D({0}, {1})", X, Y);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is Point2D)
        	{
        		return this == (Point2D)obj;
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
        /// Component-wise clips this <see cref="Point2D"/> to between min and max.
        /// </summary>
        /// <param name="min">The min point.</param>
        /// <param name="max">The max point.</param>
        /// <returns>The component-wise clipped point.</returns>
        public Point2D Clip(Point2D min, Point2D max)
        {
        	Point2D point;
        	
        	if(X <= min.X) point.X = min.X;
        	else if(X >= max.X) point.X = max.X;
        	else point.X = X;
        	
        	if(Y <= min.Y) point.Y = min.Y;
        	else if(Y >= max.Y) point.Y = max.Y;
        	else point.Y = Y;
        	
        	return point;
        }
        
        /// <summary>
        /// Component-wise wraps this <see cref="Point2D"/> to between min (inclusive) and max (exclusive).
        /// </summary>
        /// <param name="min">The min point.</param>
        /// <param name="max">The max point.</param>
        /// <returns>The component-wise wrapped point.</returns>
        public Point2D Wrap(Point2D min, Point2D max)
        {
        	Point2D point = this;
        	
        	point.X -= min.X;
			point.X %= max.X - min.X;
        	if(point.X < 0) point.X += max.X;
        	else point.X += min.X;
        	
        	point.Y -= min.Y;
			point.Y %= max.Y - min.Y;
        	if(point.Y < 0) point.Y += max.Y;
        	else point.Y += min.Y;
        	
        	return point;
        }
        
        /// <summary>
        /// Implicit cast to <see cref="Vec2D"/>.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The resulting point.</returns>
        public static implicit operator Vec2D(Point2D point)
        {
        	return new Vec2D(point.X, point.Y);
        }
        
        /// <summary>
        /// Implicit cast from int. Fills all fields with the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The resulting vec.</returns>
        public static implicit operator Point2D(int value)
        {
            return new Point2D(value, value);
        }

        /// <summary>
        /// Explicit cast to <see cref="Point1D"/>. Drops extra data.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The resulting point.</returns>
        public static explicit operator Point1D(Point2D point)
        {
            return new Point1D(point.X);
        }

        /// <summary>
        /// Explicit cast to <see cref="Vec3D"/>. Fills missing data with 0's.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The resulting point.</returns>
        public static implicit operator Point3D(Point2D point)
        {
            return new Point3D(point.X, point.Y, 0);
        }
        
		/// <summary>
        /// Negates this point.
        /// </summary>
        /// <param name="point">The point to negate.</param>
        /// <returns>The negated point.</returns>
        public static Point2D operator -(Point2D point)
        {
            return new Point2D(-point.X, -point.Y);
        }

        /// <summary>
        /// Adds the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The sum point.</returns>
        public static Point2D operator +(Point2D point, Point2D point2)
        {
            return new Point2D(point.X + point2.X, point.Y + point2.Y);
        }

        /// <summary>
        /// Subtracts the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The difference point.</returns>
        public static Point2D operator -(Point2D point, Point2D point2)
        {
            return new Point2D(point.X - point2.X, point.Y - point2.Y);
        }

        /// <summary>
        /// Multiplies the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The product point.</returns>
        public static Point2D operator *(Point2D point, Point2D point2)
        {
            return new Point2D(point.X * point2.X, point.Y * point2.Y);
        }
        
        /// <summary>
        /// Multiplies the given point by the given value.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="value">The value.</param>
        /// <returns>The product point.</returns>
        public static Point2D operator *(Vec2D value, Point2D point)
        {
        	return new Point2D((int)(point.X * value.X), (int)(point.Y * value.Y));
        }
        
        /// <summary>
        /// Multiplies the given point by the given value.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="value">The value.</param>
        /// <returns>The product point.</returns>
        public static Point2D operator *(Point2D point, Vec2D value)
        {
        	return new Point2D((int)(point.X * value.X), (int)(point.Y * value.Y));
        }

        /// <summary>
        /// Divides the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The quotient point.</returns>
        public static Point2D operator /(Point2D point, Point2D point2)
        {
            return new Point2D(point.X / point2.X, point.Y / point2.Y);
        }
        
        /// <summary>
        /// Divides the given point by the given value.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="value">The value.</param>
        /// <returns>The quotient point.</returns>
        public static Point2D operator /(Point2D point, Vec2D value)
        {
        	return new Point2D((int)(point.X / value.X), (int)(point.Y / value.Y));
        }
        
        /// <summary>
        /// Modulos the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The modulo point.</returns>
        public static Point2D operator %(Point2D point, Point2D point2)
        {
            return new Point2D(point.X % point2.X, point.Y % point2.Y);
        }
        
        /// <summary>
        /// True if the points are equal.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(Point2D point, Point2D point2)
        {
        	return point.X == point2.X && point.Y == point2.Y;
        }
        
        /// <summary>
        /// True if the points are not equal.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(Point2D point, Point2D point2)
        {
        	return !(point == point2);
        }
    }
}
