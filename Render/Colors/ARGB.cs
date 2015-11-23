namespace IROM.Util
{
	using System;
	using System.Linq;
	using System.Runtime.InteropServices;
	
    /// <summary>
    /// A byte ARGB color.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct ARGB
    {
        /// <summary>
        /// The alpha component.
        /// </summary>
        [FieldOffset(3)]
        public byte A;
        
        /// <summary>
        /// The red component.
        /// </summary>
        [FieldOffset(2)]
        public byte R;
        
        /// <summary>
        /// The green component.
        /// </summary>
        [FieldOffset(1)]
        public byte G;
        
        /// <summary>
        /// The blue component.
        /// </summary>
        [FieldOffset(0)]
        public byte B;
        
        /// <summary>
        /// The color part.
        /// </summary>
        [FieldOffset(0)]
        public RGB RGB;
        
        /// <summary>
        /// The value as an integer.
        /// </summary>
        [FieldOffset(0)]
        private uint IntValue;
        
        /// <summary>
        /// Returns the greyscale version of this <see cref="RGB"/>
        /// </summary>
        public ARGB Greyscale
        {
        	get
        	{
        		byte grey = (byte)((R + G + B) / 3D);
        		return new ARGB(A, grey, grey, grey);
        	}
        }

        /// <summary>
        /// Creates a new <see cref="ARGB"/>.
        /// </summary>
        /// <param name="a">The alpha component.</param>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        public ARGB(byte a, byte r, byte g, byte b) : this()
        {
            this.A = a;
            this.R = r;
            this.G = g;
            this.B = b;
        }
        
        /// <summary>
        /// Creates a new <see cref="ARGB"/>.
        /// </summary>
        /// <param name="a">The alpha component.</param>
        /// <param name="rgb">The color component.</param>
        public ARGB(byte a, RGB rgb) : this()
        {
            this.RGB = rgb;
            this.A = a;
        }
        
        public override string ToString()
		{
			return string.Format("ARGB ({0}, {1}, {2}, {3})", A, R, G, B);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is ARGB)
        	{
        		ARGB col = (ARGB)obj;
        		return this == col;
        	}else
        	{
        		return false;
        	}
        }
        
        public override int GetHashCode()
        {
        	// disable NonReadonlyReferencedInGetHashCode
        	return (int)Hash.PerformStaticHash((uint)IntValue);
        }
        
        /// <summary>
        /// Component-wise clips this <see cref="ARGB"/> to between min and max.
        /// </summary>
        /// <param name="min">The min color.</param>
        /// <param name="max">The max color.</param>
        /// <returns>The component-wise clipped color.</returns>
        public ARGB Clip(ARGB min, ARGB max)
        {
        	return new ARGB(Util.Clip(A, min.A, max.A), Util.Clip(R, min.R, max.R), Util.Clip(G, min.G, max.G), Util.Clip(B, min.B, max.B));
        }
        
        /// <summary>
        /// Implicit cast to <see cref="RGB"/>.
        /// </summary>
        /// <param name="color">The <see cref="ARGB"/> to cast.</param>
        /// <returns>The result.</returns>
        public static explicit operator RGB(ARGB color)
        {
            return color.RGB;
        }

        /// <summary>
        /// Explicit cast to int.
        /// </summary>
        /// <param name="color">The <see cref="ARGB"/> to cast.</param>
        /// <returns>The int value.</returns>
        public static explicit operator int(ARGB color)
        {
        	return (int)color.IntValue;
        }

        /// <summary>
        /// Explicit cast from int.
        /// </summary>
        /// <param name="color">The int to cast.</param>
        /// <returns>The result.</returns>
        public static implicit operator ARGB(int color)
        {
        	return new ARGB(){IntValue = (uint)color};
        }
        
        /// <summary>
        /// Explicit cast to uint.
        /// </summary>
        /// <param name="color">The <see cref="ARGB"/> to cast.</param>
        /// <returns>The uint value.</returns>
        public static explicit operator uint(ARGB color)
        {
            return color.IntValue;
        }

        /// <summary>
        /// Explicit cast from uint.
        /// </summary>
        /// <param name="color">The uint to cast.</param>
        /// <returns>The result.</returns>
        public static implicit operator ARGB(uint color)
        {
        	return new ARGB(){IntValue = color};
        }
        
        /// <summary>
        /// Adds the given <see cref="ARGB"/>s. Alpha is equal to the left <see cref="ARGB"/>'s.
        /// </summary>
        /// <param name="color">The first <see cref="ARGB"/>.</param>
        /// <param name="color2">The second <see cref="ARGB"/>.</param>
        /// <returns>The sum <see cref="ARGB"/>.</returns>
        public static ARGB operator +(ARGB color, ARGB color2)
        {
        	return new ARGB(color.A, (byte)(color.R + color2.R), (byte)(color.G + color2.G), (byte)(color.B + color2.B));
        }

        /// <summary>
        /// Subtracts the given <see cref="ARGB"/>s. Alpha is equal to the left <see cref="ARGB"/>'s.
        /// </summary>
        /// <param name="color">The first <see cref="ARGB"/>.</param>
        /// <param name="color2">The second <see cref="ARGB"/>.</param>
        /// <returns>The difference <see cref="ARGB"/>.</returns>
        public static ARGB operator -(ARGB color, ARGB color2)
        {
        	return new ARGB(color.A, (byte)(color.R - color2.R), (byte)(color.G - color2.G), (byte)(color.B - color2.B));
        }

        /// <summary>
        /// Multiplies the given <see cref="ARGB"/> and value. Alpha is unchanged.
        /// </summary>
        /// <param name="color">The <see cref="ARGB"/>.</param>
        /// <param name="val">The value.</param>
        /// <returns>The product <see cref="ARGB"/>.</returns>
        public static ARGB operator *(ARGB color, double val)
        {
        	return new ARGB(color.A, (byte)(color.R * val), (byte)(color.G * val), (byte)(color.B * val));
        }

        /// <summary>
        /// Divides the given <see cref="ARGB"/> and val. Alpha is unchanged.
        /// </summary>
        /// <param name="color">The <see cref="ARGB"/>.</param>
        /// <param name="val">The value.</param>
        /// <returns>The quotient <see cref="ARGB"/>.</returns>
        public static ARGB operator /(ARGB color, double val)
        {
            return new ARGB(color.A, (byte)(color.R / val), (byte)(color.G / val), (byte)(color.B / val));
        }
        
        /// <summary>
        /// True if the <see cref="ARGB"/>s are equal.
        /// </summary>
        /// <param name="color">The first <see cref="ARGB"/>.</param>
        /// <param name="color2">The second <see cref="ARGB"/>.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(ARGB color, ARGB color2)
        {
        	return color.IntValue == color2.IntValue;
        }
        
        /// <summary>
        /// True if the <see cref="ARGB"/>s are not equal.
        /// </summary>
        /// <param name="color">The first <see cref="ARGB"/>.</param>
        /// <param name="color2">The second <see cref="ARGB"/>.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(ARGB color, ARGB color2)
        {
        	return !(color == color2);
        }
    }
}
