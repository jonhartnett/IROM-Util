namespace IROM.Util
{
	using System;
	using System.Drawing;
	using System.Linq;
	
    /// <summary>
    /// Simple 2D vector struct.
    /// </summary>
    /// <typeparam name="T">The data type.</typeparam>
    public struct Vec2D<T> where T : struct
    {
    	/// <summary>
    	/// The origin value for this vector type.
    	/// </summary>
    	public static readonly Vec2D<T> Zero = new Vec2D<T>(default(T), default(T));
    	
        /// <summary>
        /// The x value.
        /// </summary>
        public T X;
        /// <summary>
        /// The y value.
        /// </summary>
        public T Y;

        /// <summary>
        /// Creates a new <see cref="Vec2D{T}">Vec2D</see> with the given values.
        /// </summary>
        /// <param name="i">The x value.</param>
        /// <param name="j">The y value.</param>
        public Vec2D(T i, T j)
        {
            X = i;
            Y = j;
        }
        
        public override string ToString()
		{
			return string.Format("Vec2D ({0}, {1})", X, Y);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is Vec2D<T>)
        	{
        		Vec2D<T> vec = (Vec2D<T>)obj;
        		return this == vec;
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
        /// Returns the squared length of this <see cref="Vec2D{T}">Vec2D</see>.
        /// </summary>
        /// <returns>The squared length.</returns>
        public T LengthSq()
        {
        	return Operator<T>.Add(Operator<T>.Multiply(X, X), Operator<T>.Multiply(Y, Y));
        }
        
        /// <summary>
        /// Returns the length of this <see cref="Vec2D{T}">Vec2D</see>.
        /// </summary>
        /// <returns>The length.</returns>
        public T Length()
        {
        	return Cast<double, T>.CastVal(Math.Sqrt(Cast<T, double>.CastVal(LengthSq())));
        }
        
        public Vec2D<T> Normalized()
        {
        	return this / Length();
        }
        
        /// <summary>
        /// Casts this <see cref="Vec2D{T}">Vec2D</see> to another type.
        /// </summary>
        /// <returns>The cast vec.</returns>
        public Vec2D<T2> TypeCast<T2>() where T2 : struct
        {
        	return new Vec2D<T2>(Cast<T, T2>.CastVal(X), Cast<T, T2>.CastVal(Y));
        }
        
        /// <summary>
        /// Component-wise clips this <see cref="Vec2D{T}">Vec2D</see> to between min and max.
        /// </summary>
        /// <param name="min">The min vec.</param>
        /// <param name="max">The max vec.</param>
        /// <returns>The component-wise clipped vec.</returns>
        public Vec2D<T> Clip(Vec2D<T> min, Vec2D<T> max)
        {
        	return new Vec2D<T>(Util.Clip(X, min.X, max.X), Util.Clip(Y, min.Y, max.Y));
        }
        
        /// <summary>
        /// Component-wise wraps this <see cref="Vec2D{T}">Vec2D</see> to between min (inclusive) and max (exclusive).
        /// </summary>
        /// <param name="min">The min vec.</param>
        /// <param name="max">The max vec.</param>
        /// <returns>The component-wise wrapped vec.</returns>
        public Vec2D<T> Wrap(Vec2D<T> min, Vec2D<T> max)
        {
        	return new Vec2D<T>(Util.Wrap(X, min.X, max.X), Util.Wrap(Y, min.Y, max.Y));
        }
        
        /// <summary>
        /// Rotates this <see cref="Vec2D{T}">Vec2D</see> the given number of radians..
        /// </summary>
        /// <param name="theta">The angle in radians.</param>
        /// <returns>The component-wise wrapped vec.</returns>
        public Vec2D<T> Rotate(double theta)
        {
        	double x = Cast<T, double>.CastVal(X);
        	double y = Cast<T, double>.CastVal(Y);
        	double cos = Math.Cos(theta);
        	double sin = Math.Sin(theta);
        	return new Vec2D<T>(Cast<double, T>.CastVal((x * cos) - (y * sin)), Cast<double, T>.CastVal((x * sin) + (y * cos)));
        }
        
        /// <summary>
        /// Implicit cast to <see cref="Point"/>.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting point.</returns>
        public static explicit operator Point(Vec2D<T> vec)
        {
        	if(typeof(T) != typeof(int))
        	{
        		throw new InvalidCastException("Cannot cast Vec2D<" + typeof(T).Name + "> to System.Drawing.Point. Only Vec2D<int> can be cast in this way");
        	}
        	return new Point(Cast<T, int>.CastVal(vec.X), Cast<T, int>.CastVal(vec.Y));
        }
        
        /// <summary>
        /// Implicit cast from <see cref="Point"/>.
        /// </summary>
        /// <param name="point">The point to cast.</param>
        /// <returns>The resulting vec</returns>
        public static explicit operator Vec2D<T>(Point point)
        {
        	if(typeof(T) != typeof(int))
        	{
        		throw new InvalidCastException("Cannot cast System.Drawing.Point to Vec2D<" + typeof(T).Name + ">. It can only be cast to Vec2D<int> in this way");
        	}
        	return new Vec2D<T>(Cast<int, T>.CastVal(point.X), Cast<int, T>.CastVal(point.Y));
        }

        /// <summary>
        /// Explicit cast to <see cref="Vec1D{T}">Vec1D</see>. Drops extra data.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static explicit operator Vec1D<T>(Vec2D<T> vec)
        {
            return new Vec1D<T>(vec.X);
        }

        /// <summary>
        /// Explicit cast to <see cref="Vec3D{T}">Vec3D</see>. Fills missing data with defaults.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static explicit operator Vec3D<T>(Vec2D<T> vec)
        {
            return new Vec3D<T>(vec.X, vec.Y, default(T));
        }

        /// <summary>
        /// Negates this vec.
        /// </summary>
        /// <param name="vec">The vec to negate.</param>
        /// <returns>The negated vec.</returns>
        public static Vec2D<T> operator -(Vec2D<T> vec)
        {
            return new Vec2D<T>(Operator<T>.Negate(vec.X), Operator<T>.Negate(vec.Y));
        }

        /// <summary>
        /// Adds the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The sum vec.</returns>
        public static Vec2D<T> operator +(Vec2D<T> vec, Vec2D<T> vec2)
        {
            return new Vec2D<T>(Operator<T>.Add(vec.X, vec2.X), Operator<T>.Add(vec.Y, vec2.Y));
        }

        /// <summary>
        /// Subtracts the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The difference vec.</returns>
        public static Vec2D<T> operator -(Vec2D<T> vec, Vec2D<T> vec2)
        {
            return new Vec2D<T>(Operator<T>.Subtract(vec.X, vec2.X), Operator<T>.Subtract(vec.Y, vec2.Y));
        }

        /// <summary>
        /// Multiplies the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The product vec.</returns>
        public static Vec2D<T> operator *(Vec2D<T> vec, Vec2D<T> vec2)
        {
            return new Vec2D<T>(Operator<T>.Multiply(vec.X, vec2.X), Operator<T>.Multiply(vec.Y, vec2.Y));
        }

        /// <summary>
        /// Multiplies the given vec and value.
        /// </summary>
        /// <param name="vec">The vec.</param>
        /// <param name="val">The value.</param>
        /// <returns>The product vec.</returns>
        public static Vec2D<T> operator *(Vec2D<T> vec, T val)
        {
            return new Vec2D<T>(Operator<T>.Multiply(vec.X, val), Operator<T>.Multiply(vec.Y, val));
        }

        /// <summary>
        /// Divides the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The quotient vec.</returns>
        public static Vec2D<T> operator /(Vec2D<T> vec, Vec2D<T> vec2)
        {
            return new Vec2D<T>(Operator<T>.Divide(vec.X, vec2.X), Operator<T>.Divide(vec.Y, vec2.Y));
        }

        /// <summary>
        /// Divides the given vec and value.
        /// </summary>
        /// <param name="vec">The vec.</param>
        /// <param name="val">The value.</param>
        /// <returns>The quotient vec.</returns>
        public static Vec2D<T> operator /(Vec2D<T> vec, T val)
        {
            return new Vec2D<T>(Operator<T>.Divide(vec.X, val), Operator<T>.Divide(vec.Y, val));
        }
        
        /// <summary>
        /// Modulos the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The modulo vec.</returns>
        public static Vec2D<T> operator %(Vec2D<T> vec, Vec2D<T> vec2)
        {
            return new Vec2D<T>(Operator<T>.Modulo(vec.X, vec2.X), Operator<T>.Modulo(vec.Y, vec2.Y));
        }

        /// <summary>
        /// Modulos the given vec and value.
        /// </summary>
        /// <param name="vec">The vec.</param>
        /// <param name="val">The value.</param>
        /// <returns>The modulo vec.</returns>
        public static Vec2D<T> operator %(Vec2D<T> vec, T val)
        {
            return new Vec2D<T>(Operator<T>.Modulo(vec.X, val), Operator<T>.Modulo(vec.Y, val));
        }
        
        /// <summary>
        /// True if the vecs are equal.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(Vec2D<T> vec, Vec2D<T> vec2)
        {
        	return Operator<T>.Equals(vec.X, vec2.X) && Operator<T>.Equals(vec.Y, vec2.Y);
        }
        
        /// <summary>
        /// True if the vecs are not equal.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(Vec2D<T> vec, Vec2D<T> vec2)
        {
        	return !(vec == vec2);
        }
    }
}
