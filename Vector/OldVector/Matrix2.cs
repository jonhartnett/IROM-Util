namespace IROM.Util
{
	using System;
	using System.Drawing;
	using System.Linq;
	using GlmNet;
	
    /// <summary>
    /// Simple 2x2 matrix struct.
    /// </summary>
    /// <typeparam name="T">The data type.</typeparam>
    public struct Matrix2<T> where T : struct
    {
    	static Matrix2()
		{
			if(!(typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte) || typeof(T) == typeof(ushort) || typeof(T) == typeof(short) ||
			     typeof(T) == typeof(uint) || typeof(T) == typeof(int) || typeof(T) == typeof(ulong) || typeof(T) == typeof(long) ||
			     typeof(T) == typeof(float) || typeof(T) == typeof(double) || typeof(T) == typeof(decimal)))
		    {
			     throw new TypeLoadException("Matrix2<T> must be of an integral or floating point type!");
		    }
		}
    	
    	// disable StaticFieldInGenericType
    	private static readonly T Zero = Cast<int, T>.CastVal(0);
    	private static readonly T One = Cast<int, T>.CastVal(1);
    	/// <summary>
    	/// The identity value for this matrix type.
    	/// </summary>
    	public static readonly Matrix2<T> Identity = new Matrix2<T>(One, Zero, Zero, One);
    	
        /// <summary>
        /// The 0x0 value.
        /// </summary>
        public T E00;
        /// <summary>
        /// The 1x0 value.
        /// </summary>
        public T E10;
        /// <summary>
        /// The 0x1 value.
        /// </summary>
        public T E01;
        /// <summary>
        /// The 1x1 value.
        /// </summary>
        public T E11;

        /// <summary>
        /// Creates a new <see cref="Matrix2{T}">Matrix2</see> with the given values.
        /// </summary>
        /// <param name="e00">The 0x0 value.</param>
        /// <param name="e10">The 1x0 value.</param>
        /// <param name="e01">The 0x1 value.</param>
        /// <param name="e11">The 1x1 value.</param>
        public Matrix2(T e00, T e10, T e01, T e11)
        {
        	E00 = e00;
        	E10 = e10;
        	E01 = e01;
        	E11 = e11;
        }
        
        public override string ToString()
		{
			return string.Format("Matrix2 {{{0}, {1}}, {{2}, {3}}}", E00, E10, E01, E11);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is Matrix2<T>)
        	{
        		Matrix2<T> vec = (Matrix2<T>)obj;
        		return this == vec;
        	}else
        	{
        		return false;
        	}
        }
        
        public override int GetHashCode()
        {
        	// disable NonReadonlyReferencedInGetHashCode
        	return (int)Hash.PerformStaticHash((uint)E00.GetHashCode(), (uint)E10.GetHashCode(), (uint)E01.GetHashCode(), (uint)E11.GetHashCode());
        }

        /// <summary>
        /// Negates this vec.
        /// </summary>
        /// <param name="vec">The vec to negate.</param>
        /// <returns>The negated vec.</returns>
        public static Vec2D<T> operator -(Vec2D<T> vec)
        {
        	mat2 m = new mat2();
        	glm.cross(default(vec3), default(vec3));
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
