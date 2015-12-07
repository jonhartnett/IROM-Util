namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Simple 1D int vector struct.
	/// </summary>
	public struct Point1D
	{
		/// <summary>
    	/// The zero value.
    	/// </summary>
    	public static readonly Point1D Zero = 0;
    	
        /// <summary>
        /// The x value.
        /// </summary>
        public int X;
        
        /// <summary>
        /// Creates a new <see cref="Vec1D"/> with the given value.
        /// </summary>
        /// <param name="i">The x value.</param>
        public Point1D(int i)
        {
            X = i;
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
        			default: throw new IndexOutOfRangeException(index + " out of Point1D range.");
        		}
        	}
        	set
        	{
        		switch(index)
        		{
        			case 0: X = value; break;
        			default: throw new IndexOutOfRangeException(index + " out of Point1D range.");
        		}
        	}
        }
        
        public override string ToString()
		{
			return string.Format("Point1D({0})", X);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is Point1D)
        	{
        		return this == (Point1D)obj;
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
        /// Implicit cast to the data type.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The value.</returns>
        public static implicit operator int(Point1D point)
        {
            return point.X;
        }

        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="val">The value to cast.</param>
        /// <returns>The point.</returns>
        public static implicit operator Point1D(int val)
        {
            return new Point1D(val);
        }
        
        /// <summary>
        /// Implicit cast to <see cref="Vec1D"/>.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The resulting point.</returns>
        public static implicit operator Vec1D(Point1D point)
        {
        	return new Vec1D(point.X);
        }

        /// <summary>
        /// Explicit cast to <see cref="Point2D"/>. Fills missing data with 0's.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The resulting point.</returns>
        public static implicit operator Point2D(Point1D point)
        {
            return new Point2D(point.X, 0);
        }

		/// <summary>
        /// Negates this point.
        /// </summary>
        /// <param name="point">The point to negate.</param>
        /// <returns>The negated point.</returns>
        public static Point1D operator -(Point1D point)
        {
            return new Point1D(-point.X);
        }

        /// <summary>
        /// Adds the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The sum point.</returns>
        public static Point1D operator +(Point1D point, Point1D point2)
        {
            return new Point1D(point.X + point2.X);
        }

        /// <summary>
        /// Subtracts the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The difference point.</returns>
        public static Point1D operator -(Point1D point, Point1D point2)
        {
            return new Point1D(point.X - point2.X);
        }

        /// <summary>
        /// Multiplies the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The product point.</returns>
        public static Point1D operator *(Point1D point, Point1D point2)
        {
            return new Point1D(point.X * point2.X);
        }

        /// <summary>
        /// Divides the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The quotient point.</returns>
        public static Point1D operator /(Point1D point, Point1D point2)
        {
            return new Point1D(point.X / point2.X);
        }
        
        /// <summary>
        /// Modulos the given points.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The modulo point.</returns>
        public static Point1D operator %(Point1D point, Point1D point2)
        {
            return new Point1D(point.X % point2.X);
        }
        
        /// <summary>
        /// True if the points are equal.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(Point1D point, Point1D point2)
        {
        	return point.X == point2.X;
        }
        
        /// <summary>
        /// True if the points are not equal.
        /// </summary>
        /// <param name="point">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(Point1D point, Point1D point2)
        {
        	return !(point == point2);
        }
	}
}
