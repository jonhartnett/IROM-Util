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
    	public static readonly ARGB Clear = new ARGB(0, 0, 0, 0);
    	public static readonly ARGB White = new ARGB(255, 255, 255, 255);
    	public static readonly ARGB Grey = new ARGB(255, 127, 127, 127);
    	public static readonly ARGB Black = new ARGB(255, 0, 0, 0);
    	public static readonly ARGB Red = new ARGB(255, 255, 0, 0);
    	public static readonly ARGB Orange = new ARGB(255, 255, 127, 0);
    	public static readonly ARGB Yellow = new ARGB(255, 255, 255, 0);
    	public static readonly ARGB Green = new ARGB(255, 0, 255, 0);
    	public static readonly ARGB Teal = new ARGB(255, 0, 255, 255);
    	public static readonly ARGB LightBlue = new ARGB(255, 0, 127, 255);
    	public static readonly ARGB Blue = new ARGB(255, 0, 0, 255);
    	public static readonly ARGB Purple = new ARGB(255, 127, 0, 255);
    	public static readonly ARGB Magenta = new ARGB(255, 255, 0, 255);
    	public static readonly ARGB Pink = new ARGB(255, 255, 0, 127);
    	
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
        	return new ARGB((byte)Math.Min(color.A + color2.A, 255), 
        	                (byte)Math.Min(color.R + color2.R, 255), 
        	                (byte)Math.Min(color.G + color2.G, 255), 
        	                (byte)Math.Min(color.B + color2.B, 255));
        }

        /// <summary>
        /// Subtracts the given <see cref="ARGB"/>s. Alpha is equal to the left <see cref="ARGB"/>'s.
        /// </summary>
        /// <param name="color">The first <see cref="ARGB"/>.</param>
        /// <param name="color2">The second <see cref="ARGB"/>.</param>
        /// <returns>The difference <see cref="ARGB"/>.</returns>
        public static ARGB operator -(ARGB color, ARGB color2)
        {
        	return new ARGB((byte)Math.Max(color.A - color2.A, 0), 
        	                (byte)Math.Max(color.R - color2.R, 0),
        	                (byte)Math.Max(color.G - color2.G, 0),
        	                (byte)Math.Max(color.B - color2.B, 0));
        }

        /// <summary>
        /// Multiplies the given <see cref="ARGB"/> and value. Alpha is unchanged.
        /// </summary>
        /// <param name="color">The <see cref="ARGB"/>.</param>
        /// <param name="val">The value.</param>
        /// <returns>The product <see cref="ARGB"/>.</returns>
        public static ARGB operator *(ARGB color, double val)
        {
        	return new ARGB((byte)Util.Clip((int)(color.A * val), 0, 255), 
        	                (byte)Util.Clip((int)(color.R * val), 0, 255), 
        	                (byte)Util.Clip((int)(color.G * val), 0, 255), 
        	                (byte)Util.Clip((int)(color.B * val), 0, 255));
        }
        
        /// <summary>
        /// Multiplies the given <see cref="ARGB"/> and <see cref="Vec4D"/>.
        /// </summary>
        /// <param name="color">The <see cref="ARGB"/>.</param>
        /// <param name="vec">The <see cref="Vec4D"/>.</param>
        /// <returns>The product <see cref="ARGB"/>.</returns>
        public static ARGB operator *(ARGB color, Vec4D vec)
        {
        	return new ARGB((byte)Util.Clip((int)(color.A * vec.W), 0, 255),
        				    (byte)Util.Clip((int)(color.R * vec.X), 0, 255),
        	                (byte)Util.Clip((int)(color.G * vec.Y), 0, 255), 
        	                (byte)Util.Clip((int)(color.B * vec.Z), 0, 255));
        }

        /// <summary>
        /// Divides the given <see cref="ARGB"/> and val. Alpha is unchanged.
        /// </summary>
        /// <param name="color">The <see cref="ARGB"/>.</param>
        /// <param name="val">The value.</param>
        /// <returns>The quotient <see cref="ARGB"/>.</returns>
        public static ARGB operator /(ARGB color, double val)
        {
            return new ARGB((byte)Util.Clip((int)(color.A / val), 0, 255), 
        	                (byte)Util.Clip((int)(color.R / val), 0, 255), 
        	                (byte)Util.Clip((int)(color.G / val), 0, 255), 
        	                (byte)Util.Clip((int)(color.B / val), 0, 255));
        }
        
        /// <summary>
        /// Divides the given <see cref="ARGB"/> and <see cref="Vec4D"/>.
        /// </summary>
        /// <param name="color">The <see cref="ARGB"/>.</param>
        /// <param name="vec">The <see cref="Vec4D"/>.</param>
        /// <returns>The quotient <see cref="ARGB"/>.</returns>
        public static ARGB operator /(ARGB color, Vec4D vec)
        {
        	return new ARGB((byte)Util.Clip((int)(color.A / vec.W), 0, 255),
        				    (byte)Util.Clip((int)(color.R / vec.X), 0, 255),
        	                (byte)Util.Clip((int)(color.G / vec.Y), 0, 255), 
        	                (byte)Util.Clip((int)(color.B / vec.Z), 0, 255));
        }
        
        /// <summary>
        /// Blends the given <see cref="ARGB"/>s.
        /// </summary>
        /// <param name="dest">The background <see cref="ARGB"/>.</param>
        /// <param name="src">The foreground <see cref="ARGB"/>.</param>
        /// <returns>The blended <see cref="ARGB"/>.</returns>
        public static ARGB operator &(ARGB dest, ARGB src)
        {
        	if(src.A == 255) return src;
        	if(src.A == 0) return dest;
        	double srcA = src.A / 255D;
        	return new ARGB((byte)(src.A + dest.A),
        					(byte)((src.R * srcA) + (dest.R * (1 - srcA))),
        	                (byte)((src.G * srcA) + (dest.G * (1 - srcA))), 
        	                (byte)((src.B * srcA) + (dest.B * (1 - srcA))));
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
        
        static ARGB()
		{
			AutoConfig.SetParser<ARGB>(TryParse);
		}
        
        public static bool TryParse(string str, out ARGB result)
        {
        	result = default(ARGB);
        	str = str.Trim();
        	if(str.Length < "(0,0,0,0)".Length || str[0] != '(' || str[str.Length - 1] != ')') return false;
        	//remove ( and )
        	str = str.Substring(1, str.Length - 2);
        	string[] parts = str.Split(',');
        	if(parts.Length != 4) return false;
        	if(!byte.TryParse(parts[0], out result.A) || 
        	   !byte.TryParse(parts[1], out result.R) || 
        	   !byte.TryParse(parts[2], out result.G) || 
        	   !byte.TryParse(parts[3], out result.B)) return false;
        	return true;
        }
    }
}
