namespace IROM.Util
{
	using System;
	using System.Linq;
	
    /// <summary>
    /// A complex number struct.
    /// </summary>
    public struct Complex
    {
        /// <summary>
        /// The real part.
        /// </summary>
        public double Real;
        /// <summary>
        /// The imaginary part.
        /// </summary>
        public double Imaginary;

        /// <summary>
        /// Creates a new <see cref="Complex"/> with the given values.
        /// </summary>
        /// <param name="real">The real value.</param>
        /// <param name="imaginary">The imaginary value.</param>
        public Complex(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }
        
        public override string ToString()
		{
			return string.Format("{0} + {1}i", Real, Imaginary);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is Complex)
        	{
        		Complex com = (Complex)obj;
        		return this == com;
        	}else
        	{
        		return false;
        	}
        }
        
        public override int GetHashCode()
        {
        	// disable NonReadonlyReferencedInGetHashCode
        	return (int)Hash.PerformStaticHash((uint)Real.GetHashCode(), (uint)Imaginary.GetHashCode());
        }

        /// <summary>
        /// Explicit cast to <see cref="Vec2D"/>.
        /// </summary>
        /// <param name="com">The complex to cast.</param>
        /// <returns>The vec.</returns>
        public static explicit operator Vec2D(Complex com)
        {
            return new Vec2D(com.Real, com.Imaginary);
        }

        /// <summary>
        /// Explicit cast from <see cref="Vec2D"/>.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The complex.</returns>
        public static explicit operator Complex(Vec2D vec)
        {
            return new Complex(vec.X, vec.Y);
        }

        /// <summary>
        /// Explicit cast to value type.
        /// </summary>
        /// <param name="com">The complex to cast.</param>
        /// <returns>The value.</returns>
        public static explicit operator double(Complex com)
        {
            return com.Real;
        }

        /// <summary>
        /// Implicit cast from value type.
        /// </summary>
        /// <param name="value">The value to cast.</param>
        /// <returns>The complex.</returns>
        public static implicit operator Complex(double value)
        {
            return new Complex(value, 0);
        }

        /// <summary>
        /// Negates this complex.
        /// </summary>
        /// <param name="com">The complex to negate.</param>
        /// <returns>The negated complex.</returns>
        public static Complex operator -(Complex com)
        {
            return new Complex(-com.Real, -com.Imaginary);
        }

        /// <summary>
        /// Adds the given complexes.
        /// </summary>
        /// <param name="com">The first complex.</param>
        /// <param name="com2">The second complex.</param>
        /// <returns>The sum complex.</returns>
        public static Complex operator +(Complex com, Complex com2)
        {
            return new Complex(com.Real + com2.Real, com.Imaginary + com2.Imaginary);
        }

        /// <summary>
        /// Subtracts the given complexes.
        /// </summary>
        /// <param name="com">The first complex.</param>
        /// <param name="com2">The second complex.</param>
        /// <returns>The difference complex.</returns>
        public static Complex operator -(Complex com, Complex com2)
        {
            return new Complex(com.Real - com2.Real, com.Imaginary - com2.Imaginary);
        }

        /// <summary>
        /// Multiplies the given complexes.
        /// </summary>
        /// <param name="com">The first complex.</param>
        /// <param name="com2">The second complex.</param>
        /// <returns>The product complex.</returns>
        public static Complex operator *(Complex com, Complex com2)
        {
        	double r = (com.Real * com2.Real) - (com.Imaginary * com2.Imaginary);
            double i = (com.Imaginary * com2.Real) + (com.Real * com2.Imaginary);
            return new Complex(r, i);
        }

        /// <summary>
        /// Multiplies the given complex and value..
        /// </summary>
        /// <param name="com">The complex.</param>
        /// <param name="val">The value.</param>
        /// <returns>The product complex.</returns>
        public static Complex operator *(Complex com, double val)
        {
            return new Complex(com.Real * val, com.Imaginary * val);
        }

        /// <summary>
        /// Divides the given complexes.
        /// </summary>
        /// <param name="com">The first complex.</param>
        /// <param name="com2">The second complex.</param>
        /// <returns>The quotient complex.</returns>
        public static Complex operator /(Complex com, Complex com2)
        {
        	double div = (com2.Real * com2.Real) + (com2.Imaginary * com2.Imaginary);
            double r = (com.Real * com2.Real) - (com.Imaginary * com2.Imaginary) / div;
            double i = (com.Imaginary * com2.Real) + (com.Real * com2.Imaginary) / div;
            return new Complex(r, i);
        }

        /// <summary>
        /// Multiplies the given complex and value.
        /// </summary>
        /// <param name="com">The complex.</param>
        /// <param name="val">The value.</param>
        /// <returns>The quotient complex.</returns>
        public static Complex operator /(Complex com, double val)
        {
            return new Complex(com.Real / val, com.Imaginary / val);
        }
        
        /// <summary>
        /// True if the complexes are equal.
        /// </summary>
        /// <param name="com">The first complex.</param>
        /// <param name="com2">The second complex.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(Complex com, Complex com2)
        {
        	return com.Real == com2.Real && com.Imaginary == com2.Imaginary;
        }
        
        /// <summary>
        /// True if the given numbers are equal.
        /// </summary>
        /// <param name="com">The complex.</param>
        /// <param name="val">The number.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(Complex com, double val)
        {
        	return com.Real == val && com.Imaginary == 0;
        }
        
        /// <summary>
        /// True if the complexes are not equal.
        /// </summary>
        /// <param name="com">The first complex.</param>
        /// <param name="com2">The second complex.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(Complex com, Complex com2)
        {
        	return !(com == com2);
        }
        
        /// <summary>
        /// True if the given numbers are not equal.
        /// </summary>
        /// <param name="com">The complex.</param>
        /// <param name="val">The number.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(Complex com, double val)
        {
        	return !(com == val);
        }
    }
}
