namespace IROM.Util
{
	using System;
	using System.Linq;
	using System.Runtime.InteropServices;
	
    /// <summary>
    /// A byte RGB color. Laid out in memory as (Blue,Green,Red,Padding) to be compatible with WinAPI and to align with integer bounds.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct RGB
    {
    	private const uint MASK = 0x00FFFFFFu;
    	public static readonly RGB White = new RGB(255, 255, 255);
    	public static readonly RGB Grey = new RGB(127, 127, 127);
    	public static readonly RGB Black = new RGB(0, 0, 0);
    	public static readonly RGB Red = new RGB(255, 0, 0);
    	public static readonly RGB Orange = new RGB(255, 127, 0);
    	public static readonly RGB Yellow = new RGB(255, 255, 0);
    	public static readonly RGB Green = new RGB(0, 255, 0);
    	public static readonly RGB Teal = new RGB(0, 255, 255);
    	public static readonly RGB LightBlue = new RGB(0, 127, 255);
    	public static readonly RGB Blue = new RGB(0, 0, 255);
    	public static readonly RGB Purple = new RGB(127, 0, 255);
    	public static readonly RGB Magenta = new RGB(255, 0, 255);
    	public static readonly RGB Pink = new RGB(255, 0, 127);
    	
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
        /// The value as an integer.
        /// </summary>
        [FieldOffset(0)]
        public uint IntValue;
        
        /// <summary>
        /// Returns the greyscale version of this <see cref="RGB"/>
        /// </summary>
        public RGB Greyscale
        {
        	get
        	{
        		byte grey = (byte)((R + G + B) / 3D);
        		return new RGB(grey, grey, grey);
        	}
        }

        /// <summary>
        /// Creates a new <see cref="RGB"/>.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        public RGB(byte r, byte g, byte b)
        {
        	this.IntValue = 0;//clear memory
            this.R = r;
            this.G = g;
            this.B = b;
        }
        
        public override string ToString()
		{
			return string.Format("RGB ({0}, {1}, {2})", R, G, B);
		}
		
		public override bool Equals(object obj)
        {
        	if(obj is RGB)
        	{
        		RGB col = (RGB)obj;
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
        /// Component-wise clips this <see cref="RGB"/> to between min and max.
        /// </summary>
        /// <param name="min">The min color.</param>
        /// <param name="max">The max color.</param>
        /// <returns>The component-wise clipped color.</returns>
        public RGB Clip(RGB min, RGB max)
        {
        	return new RGB(Util.Clip(R, min.R, max.R), Util.Clip(G, min.G, max.G), Util.Clip(B, min.B, max.B));
        }
        
        /// <summary>
        /// Implicit cast to <see cref="ARGB"/>.
        /// </summary>
        /// <param name="color">The <see cref="RGB"/> to cast.</param>
        /// <returns>The result.</returns>
        public static implicit operator ARGB(RGB color)
        {
            return new ARGB(255, color);
        }
        
        /// <summary>
        /// Explicit cast to int.
        /// </summary>
        /// <param name="color">The <see cref="ARGB"/> to cast.</param>
        /// <returns>The int value.</returns>
        public static explicit operator int(RGB color)
        {
			return (int)(color.IntValue & 0x00FFFFFF);
        }

        /// <summary>
        /// Explicit cast from int.
        /// </summary>
        /// <param name="color">The int to cast.</param>
        /// <returns>The result.</returns>
        public static implicit operator RGB(int color)
        {
        	return new RGB(){IntValue = ((uint)color & 0x00FFFFFF)};
        }
        
        /// <summary>
        /// Explicit cast to uint.
        /// </summary>
        /// <param name="color">The <see cref="ARGB"/> to cast.</param>
        /// <returns>The uint value.</returns>
        public static explicit operator uint(RGB color)
        {
			return color.IntValue & 0x00FFFFFF;
        }

        /// <summary>
        /// Explicit cast from uint.
        /// </summary>
        /// <param name="color">The uint to cast.</param>
        /// <returns>The result.</returns>
        public static implicit operator RGB(uint color)
        {
        	return new RGB(){IntValue = (color & 0x00FFFFFF)};
        }
        
        /// <summary>
        /// Adds the given <see cref="RGB"/>s.
        /// </summary>
        /// <param name="color">The first <see cref="RGB"/>.</param>
        /// <param name="color2">The second <see cref="RGB"/>.</param>
        /// <returns>The sum <see cref="RGB"/>.</returns>
        public static RGB operator +(RGB color, RGB color2)
        {
        	return new RGB((byte)Math.Min(color.R + color2.R, 255), 
        	               (byte)Math.Min(color.G + color2.G, 255), 
        	               (byte)Math.Min(color.B + color2.B, 255));
        }

        /// <summary>
        /// Subtracts the given <see cref="RGB"/>s.
        /// </summary>
        /// <param name="color">The first <see cref="RGB"/>.</param>
        /// <param name="color2">The second <see cref="RGB"/>.</param>
        /// <returns>The difference <see cref="RGB"/>.</returns>
        public static RGB operator -(RGB color, RGB color2)
        {
        	return new RGB((byte)Math.Max(color.R - color2.R, 0), 
        	               (byte)Math.Max(color.G - color2.G, 0), 
        	               (byte)Math.Max(color.B - color2.B, 0));
        }

        /// <summary>
        /// Multiplies the given <see cref="RGB"/> and value.
        /// </summary>
        /// <param name="color">The <see cref="RGB"/>.</param>
        /// <param name="val">The value.</param>
        /// <returns>The product <see cref="RGB"/>.</returns>
        public static RGB operator *(RGB color, double val)
        {
        	return new RGB((byte)Util.Clip((int)(color.R * val), 0, 255), 
        	               (byte)Util.Clip((int)(color.G * val), 0, 255), 
        	               (byte)Util.Clip((int)(color.B * val), 0, 255));
        }
        
        /// <summary>
        /// Multiplies the given <see cref="RGB"/> and <see cref="Vec3D"/>.
        /// </summary>
        /// <param name="color">The <see cref="RGB"/>.</param>
        /// <param name="vec">The <see cref="Vec3D"/>.</param>
        /// <returns>The product <see cref="RGB"/>.</returns>
        public static RGB operator *(RGB color, Vec3D vec)
        {
        	return new RGB((byte)Util.Clip((int)(color.R * vec.X), 0, 255), 
        	               (byte)Util.Clip((int)(color.G * vec.Y), 0, 255), 
        	               (byte)Util.Clip((int)(color.B * vec.Z), 0, 255));
        }

        /// <summary>
        /// Divides the given <see cref="RGB"/> and val.
        /// </summary>
        /// <param name="color">The <see cref="RGB"/>.</param>
        /// <param name="val">The value.</param>
        /// <returns>The quotient <see cref="RGB"/>.</returns>
        public static RGB operator /(RGB color, double val)
        {
            return new RGB((byte)Util.Clip((int)(color.R / val), 0, 255), 
        	               (byte)Util.Clip((int)(color.G / val), 0, 255), 
        	               (byte)Util.Clip((int)(color.B / val), 0, 255));
        }
        
        /// <summary>
        /// Divides the given <see cref="RGB"/> and <see cref="Vec3D"/>.
        /// </summary>
        /// <param name="color">The <see cref="RGB"/>.</param>
        /// <param name="vec">The <see cref="Vec3D"/>.</param>
        /// <returns>The quotient <see cref="RGB"/>.</returns>
        public static RGB operator /(RGB color, Vec3D vec)
        {
        	return new RGB((byte)Util.Clip((int)(color.R / vec.X), 0, 255), 
        	               (byte)Util.Clip((int)(color.G / vec.Y), 0, 255), 
        	               (byte)Util.Clip((int)(color.B / vec.Z), 0, 255));
        }
        
        /// <summary>
        /// Blends the given <see cref="ARGB"/>s. Assumes the background is opaque.
        /// </summary>
        /// <param name="dest">The background <see cref="ARGB"/>.</param>
        /// <param name="src">The foreground <see cref="ARGB"/>.</param>
        /// <returns>The blended <see cref="ARGB"/>.</returns>
        public static RGB operator &(RGB dest, ARGB src)
        {
        	double srcA = src.A / 255D;
        	return new RGB((byte)((src.R * srcA) + (dest.R * (1 - srcA))), 
        	               (byte)((src.G * srcA) + (dest.G * (1 - srcA))), 
        	               (byte)((src.B * srcA) + (dest.B * (1 - srcA))));
        }
        
        /// <summary>
        /// True if the <see cref="RGB"/>s are equal.
        /// </summary>
        /// <param name="color">The first <see cref="RGB"/>.</param>
        /// <param name="color2">The second <see cref="RGB"/>.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(RGB color, RGB color2)
        {
        	return (color.IntValue & MASK) == (color2.IntValue & MASK);
        }
        
        /// <summary>
        /// True if the <see cref="RGB"/>s are not equal.
        /// </summary>
        /// <param name="color">The first <see cref="RGB"/>.</param>
        /// <param name="color2">The second <see cref="RGB"/>.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(RGB color, RGB color2)
        {
        	return !(color == color2);
        }
    }
}
