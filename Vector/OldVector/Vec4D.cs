namespace IROM.Util
{
	using System;
	using System.Linq;
	
    /// <summary>
    /// Simple 4D vector struct.
    /// </summary>
    /// <typeparam name="T">The data type.</typeparam>
    public struct Vec4D<T> where T : struct
    {
    	/// <summary>
    	/// The origin value for this vector type.
    	/// </summary>
    	public static readonly Vec4D<T> Zero = new Vec4D<T>(default(T), default(T), default(T), default(T));
    	
        /// <summary>
        /// The x value.
        /// </summary>
        public T X;
        /// <summary>
        /// The y value.
        /// </summary>
        public T Y;
        /// <summary>
        /// The z value.
        /// </summary>
        public T Z;
        /// <summary>
        /// The w value.
        /// </summary>
        public T W;

        /// <summary>
        /// Creates a new <see cref="Vec4D{T}">Vec4D</see> with the given values.
        /// </summary>
        /// <param name="i">The x value.</param>
        /// <param name="j">The y value.</param>
        /// <param name="k">The z value.</param>
        /// <param name="l">The w value.</param>
        public Vec4D(T i, T j, T k, T l)
        {
            X = i;
            Y = j;
            Z = k;
            W = l;
        }
        
        public override string ToString()
		{
			return string.Format("Vec1D ({0}, {1}, {2}, {3})", X, Y, Z, W);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is Vec4D<T>)
        	{
        		Vec4D<T> vec = (Vec4D<T>)obj;
        		return this == vec;
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
        /// Casts this <see cref="Vec4D{T}">Vec4D</see> to another type.
        /// </summary>
        /// <returns>The cast vec.</returns>
        public Vec4D<T2> TypeCast<T2>() where T2 : struct
        {
        	return new Vec4D<T2>(Cast<T, T2>.CastVal(X), Cast<T, T2>.CastVal(Y), Cast<T, T2>.CastVal(Z), Cast<T, T2>.CastVal(W));
        }
        
        /// <summary>
        /// Component-wise clips this <see cref="Vec4D{T}">Vec4D</see> to between min and max.
        /// </summary>
        /// <param name="min">The min vec.</param>
        /// <param name="max">The max vec.</param>
        /// <returns>The component-wise clipped vec.</returns>
        public Vec4D<T> Clip(Vec4D<T> min, Vec4D<T> max)
        {
        	return new Vec4D<T>(Util.Clip(X, min.X, max.X), Util.Clip(Y, min.Y, max.Y), Util.Clip(Z, min.Z, max.Z), Util.Clip(W, min.W, max.W));
        }
        
        /// <summary>
        /// Component-wise wraps this <see cref="Vec4D{T}">Vec4D</see> to between min (inclusive) and max (exclusive).
        /// </summary>
        /// <param name="min">The min vec.</param>
        /// <param name="max">The max vec.</param>
        /// <returns>The component-wise wrapped vec.</returns>
        public Vec4D<T> Wrap(Vec4D<T> min, Vec4D<T> max)
        {
        	return new Vec4D<T>(Util.Wrap(X, min.X, max.X), Util.Wrap(Y, min.Y, max.Y), Util.Wrap(Z, min.Z, max.Z), Util.Wrap(W, min.W, max.W));
        }

        /// <summary>
        /// Explicit cast to <see cref="Vec3D{T}">Vec3D</see>. Drops extra data.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static explicit operator Vec3D<T>(Vec4D<T> vec)
        {
            return new Vec3D<T>(vec.X, vec.Y, vec.Z);
        }

        /// <summary>
        /// Negates this vec.
        /// </summary>
        /// <param name="vec">The vec to negate.</param>
        /// <returns>The negated vec.</returns>
        public static Vec4D<T> operator -(Vec4D<T> vec)
        {
            return new Vec4D<T>(Operator<T>.Negate(vec.X), Operator<T>.Negate(vec.Y), Operator<T>.Negate(vec.Z) , Operator<T>.Negate(vec.W));
        }

        /// <summary>
        /// Adds the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The sum vec.</returns>
        public static Vec4D<T> operator +(Vec4D<T> vec, Vec4D<T> vec2)
        {
            return new Vec4D<T>(Operator<T>.Add(vec.X, vec2.X), Operator<T>.Add(vec.Y, vec2.Y), Operator<T>.Add(vec.Z, vec2.Z), Operator<T>.Add(vec.W, vec2.W));
        }

        /// <summary>
        /// Subtracts the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The difference vec.</returns>
        public static Vec4D<T> operator -(Vec4D<T> vec, Vec4D<T> vec2)
        {
            return new Vec4D<T>(Operator<T>.Subtract(vec.X, vec2.X), Operator<T>.Subtract(vec.Y, vec2.Y), Operator<T>.Subtract(vec.Z, vec2.Z), Operator<T>.Subtract(vec.W, vec2.W));
        }

        /// <summary>
        /// Multiplies the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The product vec.</returns>
        public static Vec4D<T> operator *(Vec4D<T> vec, Vec4D<T> vec2)
        {
            return new Vec4D<T>(Operator<T>.Multiply(vec.X, vec2.X), Operator<T>.Multiply(vec.Y, vec2.Y), Operator<T>.Multiply(vec.Z, vec2.Z), Operator<T>.Multiply(vec.W, vec2.W));
        }

        /// <summary>
        /// Multiplies the given vec and value.
        /// </summary>
        /// <param name="vec">The vec.</param>
        /// <param name="val">The value.</param>
        /// <returns>The product vec.</returns>
        public static Vec4D<T> operator *(Vec4D<T> vec, T val)
        {
            return new Vec4D<T>(Operator<T>.Multiply(vec.X, val), Operator<T>.Multiply(vec.Y, val), Operator<T>.Multiply(vec.Z, val), Operator<T>.Multiply(vec.W, val));
        }

        /// <summary>
        /// Divides the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The quotient vec.</returns>
        public static Vec4D<T> operator /(Vec4D<T> vec, Vec4D<T> vec2)
        {
            return new Vec4D<T>(Operator<T>.Divide(vec.X, vec2.X), Operator<T>.Divide(vec.Y, vec2.Y), Operator<T>.Divide(vec.Z, vec2.Z), Operator<T>.Divide(vec.W, vec2.W));
        }

        /// <summary>
        /// Divides the given vec and value.
        /// </summary>
        /// <param name="vec">The vec.</param>
        /// <param name="val">The value.</param>
        /// <returns>The quotient vec.</returns>
        public static Vec4D<T> operator /(Vec4D<T> vec, T val)
        {
            return new Vec4D<T>(Operator<T>.Divide(vec.X, val), Operator<T>.Divide(vec.Y, val), Operator<T>.Divide(vec.Z, val), Operator<T>.Divide(vec.W, val));
        }
        
        /// <summary>
        /// Modulos the given vecs.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>The modulo vec.</returns>
        public static Vec4D<T> operator %(Vec4D<T> vec, Vec4D<T> vec2)
        {
            return new Vec4D<T>(Operator<T>.Modulo(vec.X, vec2.X), Operator<T>.Modulo(vec.Y, vec2.Y), Operator<T>.Modulo(vec.Z, vec2.Z), Operator<T>.Modulo(vec.W, vec2.W));
        }

        /// <summary>
        /// Modulos the given vec and value.
        /// </summary>
        /// <param name="vec">The vec.</param>
        /// <param name="val">The value.</param>
        /// <returns>The modulo vec.</returns>
        public static Vec4D<T> operator %(Vec4D<T> vec, T val)
        {
            return new Vec4D<T>(Operator<T>.Modulo(vec.X, val), Operator<T>.Modulo(vec.Y, val), Operator<T>.Modulo(vec.Z, val), Operator<T>.Modulo(vec.W, val));
        }
        
        /// <summary>
        /// True if the vecs are equal.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(Vec4D<T> vec, Vec4D<T> vec2)
        {
        	return Operator<T>.Equals(vec.X, vec2.X) && Operator<T>.Equals(vec.Y, vec2.Y) && Operator<T>.Equals(vec.Z, vec2.Z) && Operator<T>.Equals(vec.W, vec2.W);
        }
        
        /// <summary>
        /// True if the vecs are not equal.
        /// </summary>
        /// <param name="vec">The first vec.</param>
        /// <param name="vec2">The second vec.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(Vec4D<T> vec, Vec4D<T> vec2)
        {
        	return !(vec == vec2);
        }
    }
}
