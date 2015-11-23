namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Simple rectangle struct.
	/// </summary>
	public struct Rectangle<T> where T : struct
	{
		static Rectangle()
		{
			if(!(typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte) || typeof(T) == typeof(ushort) || typeof(T) == typeof(short) ||
			     typeof(T) == typeof(uint) || typeof(T) == typeof(int) || typeof(T) == typeof(ulong) || typeof(T) == typeof(long) ||
			     typeof(T) == typeof(float) || typeof(T) == typeof(double) || typeof(T) == typeof(decimal)))
		    {
			     throw new TypeLoadException("Rectangle<T> must be of an integral or floating point type!");
		    }
		}
		
		/// <summary>
		/// The <see cref="Rectangle{T}">Rectangle</see> minimum coordinates (inclusive).
		/// </summary>
		public Vec2D<T> Min;
		
		/// <summary>
		/// The <see cref="Rectangle{T}">Rectangle</see> maximum coordinates (exclusive).
		/// </summary>
		public Vec2D<T> Max;
		
		/// <summary>
		/// The <see cref="Rectangle{T}">Rectangle</see> position.
		/// </summary>
		public Vec2D<T> Position
		{
			get{return Min;}
			set
			{
				Max = (Max - Min) + value;
				Min = value;
			}
		}
		
		/// <summary>
		/// The <see cref="Rectangle{T}">Rectangle</see> size.
		/// </summary>
		public Vec2D<T> Size
		{
			get{return Max - Min;}
			set{Max = Min + value;}
		}
		
		/// <summary>
		/// The <see cref="Rectangle{T}">Rectangle</see> x.
		/// </summary>
		public T X
		{
			get{return Min.X;}
			set
			{
				Max.X = Operator<T>.Add(Operator<T>.Subtract(Max.X, Min.X), value);
				Min.X = value;
			}
		}
		
		/// <summary>
		/// The <see cref="Rectangle{T}">Rectangle</see> y.
		/// </summary>
		public T Y
		{
			get{return Min.Y;}
			set
			{
				Max.Y = Operator<T>.Add(Operator<T>.Subtract(Max.Y, Min.Y), value);
				Min.Y = value;
			}
		}
		
		/// <summary>
		/// The <see cref="Rectangle{T}">Rectangle</see> width.
		/// </summary>
		public T Width
		{
			get{return Operator<T>.Subtract(Max.X, Min.X);}
			set{Max.X = Operator<T>.Add(value, Min.X);}
		}
		
		/// <summary>
		/// The <see cref="Rectangle{T}">Rectangle</see> height.
		/// </summary>
		public T Height
		{
			get{return Operator<T>.Subtract(Max.Y, Min.Y);}
			set{Max.Y = Operator<T>.Add(value, Min.Y);}
		}
		
		/// <summary>
        /// Creates a new <see cref="Rectangle{T}">Rectangle</see> with the given values.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        public Rectangle(T x, T y, T w, T h) : this(new Vec2D<T>(x, y), new Vec2D<T>(Operator<T>.Add(x, w), Operator<T>.Add(y, h)))
        {
           	
        }
        
        /// <summary>
        /// Creates a new <see cref="Rectangle{T}">Rectangle</see> with the given values.
        /// </summary>
        /// <param name="min">The minimum coordinates.</param>
        /// <param name="max">The maximum coordinates.</param>
        public Rectangle(Vec2D<T> min, Vec2D<T> max)
        {
        	Min = min;
        	Max = max;
        }
        
        public override string ToString()
		{
			return string.Format("Rectangle ({0}, {1}), ({2}, {3})", Min.X, Min.Y, Max.X, Max.Y);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is Rectangle<T>)
        	{
        		Rectangle<T> rect = (Rectangle<T>)obj;
        		return this == rect;
        	}else
        	{
        		return false;
        	}
        }
        
        public override int GetHashCode()
        {
        	// disable NonReadonlyReferencedInGetHashCode
        	return (int)Hash.PerformStaticHash((uint)Min.GetHashCode(), (uint)Max.GetHashCode());
        }
        
        /// <summary>
        /// Returns true if this <see cref="Rectangle{T}">Rectangle</see> is a valid, space-filling rectangle.
        /// </summary>
        /// <returns>True if valid.</returns>
        public bool IsValid()
        {
        	return Operator<T>.LessThan(Min.X, Max.X) && Operator<T>.LessThan(Min.Y, Max.Y);
        }
        
        /// <summary>
        /// Ensures this <see cref="Rectangle{T}">Rectangle</see> is at least the given size.
        /// </summary>
        /// <param name="width">The min allowed width.</param>
        /// <param name="height">The min allowed height.</param>
        public void EnsureSize(T width, T height)
        {
        	EnsureSize(new Vec2D<T>(width, height));
        }
        
        /// <summary>
        /// Ensures this <see cref="Rectangle{T}">Rectangle</see> is at least the given size.
        /// </summary>
        /// <param name="size">The min allowed size.</param>
        public void EnsureSize(Vec2D<T> size)
        {
        	Max = VectorUtil.Max(Max, Min + size);
        }
        
        /// <summary>
        /// Returns true if the given <see cref="Vec2D{T}">Vec2D</see> is within this <see cref="Rectangle{T}">Rectangle</see>'s bounds.
        /// </summary>
        /// <param name="vec">The vec.</param>
        /// <returns>True if within, else false.</returns>
        public bool Contains(Vec2D<T> vec)
        {
        	return Operator<T>.GreaterThanOrEquals(vec.X, Min.X) && Operator<T>.GreaterThanOrEquals(vec.Y, Min.Y) && Operator<T>.LessThan(vec.X, Max.X) && Operator<T>.LessThan(vec.Y, Max.Y);
        }
        
        /// <summary>
        /// Casts this <see cref="Rectangle{T}">Rectangle</see> to another type.
        /// </summary>
        /// <returns>The cast rect.</returns>
        public Rectangle<T2> TypeCast<T2>() where T2 : struct
        {
        	return new Rectangle<T2>(Min.TypeCast<T2>(), Max.TypeCast<T2>());
        }
        
        /// <summary>
        /// Explicit cast to <see cref="System.Drawing.Rectangle"/>.
        /// </summary>
        /// <param name="rect">The rect to cast.</param>
        /// <returns>The resulting rect.</returns>
        public static explicit operator System.Drawing.Rectangle(Rectangle<T> rect)
        {
        	return new System.Drawing.Rectangle(Cast<T, int>.CastVal(rect.X), Cast<T, int>.CastVal(rect.Y), Cast<T, int>.CastVal(rect.Width), Cast<T, int>.CastVal(rect.Height));
        }
        
        /// <summary>
        /// Explicit cast from <see cref="System.Drawing.Rectangle"/>.
        /// </summary>
        /// <param name="rect">The rect to cast.</param>
        /// <returns>The resulting rect.</returns>
        public static explicit operator Rectangle<T>(System.Drawing.Rectangle rect)
        {
        	return new Rectangle<T>(Cast<int, T>.CastVal(rect.X), Cast<int, T>.CastVal(rect.Y), Cast<int, T>.CastVal(rect.Width), Cast<int, T>.CastVal(rect.Height));
        }
        
        /// <summary>
        /// Implicit cast from <see cref="Vec2D{T}">Vec2D</see>.
        /// Resulting <see cref="Rectangle{T}">Rectangle</see> extends into quadrant 1 from the origin.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting rect.</returns>
        public static explicit operator Rectangle<T>(Vec2D<T> vec)
        {
        	return new Rectangle<T>(Vec2D<T>.Zero, vec);
        }
        
        /// <summary>
        /// Adds the given vec to the rect's position.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="vec">The vec.</param>
        /// <returns>The moved rectangle.</returns>
        public static Rectangle<T> operator +(Rectangle<T> rect, Vec2D<T> vec)
        {
        	//passed by value, so we can just add vec to rect pos and pass it back
        	rect.Min += vec;
        	rect.Max += vec;
            return rect;
        }

        /// <summary>
        /// Subtracts the given vec from the rect's position.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="vec">The vec.</param>
        /// <returns>The moved rectangle.</returns>
        public static Rectangle<T> operator -(Rectangle<T> rect, Vec2D<T> vec)
        {
        	//passed by value, so we can just add vec to rect pos and pass it back
        	rect.Min -= vec;
        	rect.Max -= vec;
            return rect;
        }

        /// <summary>
        /// Multiplies the given rect's size by the given value, keeping it centered.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="val">The value.</param>
        /// <returns>The resized rectangle.</returns>
        public static Rectangle<T> operator *(Rectangle<T> rect, T val)
        {
        	Vec2D<T> middle = (rect.Max + rect.Min) / Cast<int, T>.CastVal(2);
        	rect.Min -= middle;
        	rect.Min *= val;
        	rect.Min += middle;
        	rect.Max -= middle;
        	rect.Max *= val;
        	rect.Max += middle;
            return rect;
        }

        /// <summary>
        /// Divided the given rect's size by the given value, keeping it centered.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="val">The value.</param>
        /// <returns>The resized rectangle.</returns>
        public static Rectangle<T> operator /(Rectangle<T> rect, T val)
        {
        	Vec2D<T> middle = (rect.Max + rect.Min) / Cast<int, T>.CastVal(2);
        	rect.Min -= middle;
        	rect.Min /= val;
        	rect.Min += middle;
        	rect.Max -= middle;
        	rect.Max /= val;
        	rect.Max += middle;
            return rect;
        }
        
        /// <summary>
        /// True if the rects are equal.
        /// </summary>
        /// <param name="rect">The first rect.</param>
        /// <param name="rect2">The second rect.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(Rectangle<T> rect, Rectangle<T> rect2)
        {
        	return (rect.Min == rect2.Min) && (rect.Max == rect2.Max);
        }
        
        /// <summary>
        /// True if the rects are not equal.
        /// </summary>
        /// <param name="rect">The first rect.</param>
        /// <param name="rect2">The second rect.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(Rectangle<T> rect, Rectangle<T> rect2)
        {
        	return !(rect == rect2);
        }
	}
}
