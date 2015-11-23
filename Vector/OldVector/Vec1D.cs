namespace IROM.Util
{
	using System;
	using System.Linq;
	
    /// <summary>
    /// Simple 1D vector struct.
    /// </summary>
    /// <typeparam name="T">The data type.</typeparam>
    public struct Vec1D<T> where T : struct
    {
    	/// <summary>
    	/// The origin value for this vector type.
    	/// </summary>
    	public static readonly Vec1D<T> Zero = new Vec1D<T>(default(T));
    	
        /// <summary>
        /// The x value.
        /// </summary>
        public T X;

        /// <summary>
        /// Creates a new <see cref="Vec1D{T}">Vec1D</see> with the given value.
        /// </summary>
        /// <param name="i">The x value.</param>
        public Vec1D(T i)
        {
            X = i;
        }
        
        public override string ToString()
		{
			return string.Format("Vec1D ({0})", X);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is Vec1D<T>)
        	{
        		Vec1D<T> vec = (Vec1D<T>)obj;
        		return this == vec;
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
        /// Casts this <see cref="Vec1D{T}">Vec1D</see> to another type.
        /// </summary>
        /// <returns>The cast vec.</returns>
        public Vec1D<T2> TypeCast<T2>() where T2 : struct
        {
        	return new Vec1D<T2>(Cast<T, T2>.CastVal(X));
        }
        
        /// <summary>
        /// Component-wise clips this <see cref="Vec1D{T}">Vec1D</see> to between min and max.
        /// </summary>
        /// <param name="min">The min vec.</param>
        /// <param name="max">The max vec.</param>
        /// <returns>The component-wise clipped vec.</returns>
        public Vec1D<T> Clip(Vec1D<T> min, Vec1D<T> max)
        {
        	return new Vec1D<T>(Util.Clip(X, min.X, max.X));
        }
        
        /// <summary>
        /// Component-wise wraps this <see cref="Vec1D{T}">Vec1D</see> to between min (inclusive) and max (exclusive).
        /// </summary>
        /// <param name="min">The min vec.</param>
        /// <param name="max">The max vec.</param>
        /// <returns>The component-wise wrapped vec.</returns>
        public Vec1D<T> Wrap(Vec1D<T> min, Vec1D<T> max)
        {
        	return new Vec1D<T>(Util.Wrap(X, min.X, max.X));
        }

        /// <summary>
        /// Implicit cast to the data type.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The value.</returns>
        public static implicit operator T(Vec1D<T> vec)
        {
            return vec.X;
        }

        /// <summary>
        /// Implicit cast from the data type.
        /// </summary>
        /// <param name="val">The value to cast.</param>
        /// <returns>The vec.</returns>
        public static implicit operator Vec1D<T>(T val)
        {
            return new Vec1D<T>(val);
        }

        /// <summary>
        /// Explicit cast to <see cref="Vec2D{T}">Vec2D</see>. Fills missing data with defaults.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static explicit operator Vec2D<T>(Vec1D<T> vec)
        {
            return new Vec2D<T>(vec.X, default(T));
        }

        /// <summary>
        /// Negates this vec.
        /// </summary>
        /// <param name="vec">The vec to negate.</param>
        /// <returns>The negated vec.</returns>
        public static Vec1D<T> operator -(Vec1D<T> vec)
        {
            return new Vec1D<T>(Operator<T>.Negate(vec.X));
        }

        /// <summary>
        /// Adds the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The sum vec.</returns>
        public static Vec1D<T> operator +(Vec1D<T> vec, Vec1D<T> vec2)
        {
            return new Vec1D<T>(Operator<T>.Add(vec.X, vec2.X));
        }

        /// <summary>
        /// Subtracts the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The difference vec.</returns>
        public static Vec1D<T> operator -(Vec1D<T> vec, Vec1D<T> vec2)
        {
            return new Vec1D<T>(Operator<T>.Subtract(vec.X, vec2.X));
        }

        /// <summary>
        /// Multiplies the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The product vec.</returns>
        public static Vec1D<T> operator *(Vec1D<T> vec, Vec1D<T> vec2)
        {
            return new Vec1D<T>(Operator<T>.Multiply(vec.X, vec2.X));
        }

        /// <summary>
        /// Multiplies the given vec and value.
        /// </summary>
        /// <param name="vec">The vec.</param>
        /// <param name="val">The value.</param>
        /// <returns>The product vec.</returns>
        public static Vec1D<T> operator *(Vec1D<T> vec, T val)
        {
            return new Vec1D<T>(Operator<T>.Multiply(vec.X, val));
        }

        /// <summary>
        /// Divides the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The quotient vec.</returns>
        public static Vec1D<T> operator /(Vec1D<T> vec, Vec1D<T> vec2)
        {
            return new Vec1D<T>(Operator<T>.Divide(vec.X, vec2.X));
        }

        /// <summary>
        /// Divides the given vec and value.
        /// </summary>
        /// <param name="vec">The vec.</param>
        /// <param name="val">The value.</param>
        /// <returns>The quotient vec.</returns>
        public static Vec1D<T> operator /(Vec1D<T> vec, T val)
        {
            return new Vec1D<T>(Operator<T>.Divide(vec.X, val));
        }
        
        /// <summary>
        /// Modulos the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The modulo vec.</returns>
        public static Vec1D<T> operator %(Vec1D<T> vec, Vec1D<T> vec2)
        {
            return new Vec1D<T>(Operator<T>.Modulo(vec.X, vec2.X));
        }

        /// <summary>
        /// Modulos the given vec and value.
        /// </summary>
        /// <param name="vec">The vec.</param>
        /// <param name="val">The value.</param>
        /// <returns>The modulo vec.</returns>
        public static Vec1D<T> operator %(Vec1D<T> vec, T val)
        {
            return new Vec1D<T>(Operator<T>.Modulo(vec.X, val));
        }
        
        /// <summary>
        /// True if the vecs are equal.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(Vec1D<T> vec, Vec1D<T> vec2)
        {
        	return Operator<T>.Equals(vec.X, vec2.X);
        }
        
        /// <summary>
        /// True if the vecs are not equal.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(Vec1D<T> vec, Vec1D<T> vec2)
        {
        	return !(vec == vec2);
        }
    }
}
