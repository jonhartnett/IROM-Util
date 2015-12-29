namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Simple int rectangle struct.
	/// </summary>
	public struct Rectangle
	{
		/// <summary>
		/// The <see cref="Rectangle"/> minimum coordinates (inclusive).
		/// </summary>
		public Point2D Min;
		
		/// <summary>
		/// The <see cref="Rectangle"/> maximum coordinates (inclusive).
		/// </summary>
		public Point2D Max;
		
		/// <summary>
		/// The <see cref="Rectangle"/> position.
		/// </summary>
		public Point2D Position
		{
			get{return Min;}
			set
			{
				Max -= Min;
				Min = value;
				Max += Min;
			}
		}
		
		/// <summary>
		/// The <see cref="Rectangle"/> size.
		/// </summary>
		public Point2D Size
		{
			get{return Max - Min + 1;}
			set{Max = Min + value - 1;}
		}
		
		/// <summary>
		/// The <see cref="Rectangle"/> x.
		/// </summary>
		public int X
		{
			get{return Min.X;}
			set
			{
				Max.X -= Min.X;
				Min.X = value;
				Max.X += Min.X;
			}
		}
		
		/// <summary>
		/// The <see cref="Rectangle"/> y.
		/// </summary>
		public int Y
		{
			get{return Min.Y;}
			set
			{
				Max.Y -= Min.Y;
				Min.Y = value;
				Max.Y += Min.Y;
			}
		}
		
		/// <summary>
		/// The <see cref="Rectangle"/> width.
		/// </summary>
		public int Width
		{
			get{return Max.X - Min.X + 1;}
			set{Max.X = Min.X + value - 1;}
		}
		
		/// <summary>
		/// The <see cref="Rectangle"/> height.
		/// </summary>
		public int Height
		{
			get{return Max.Y - Min.Y + 1;}
			set{Max.Y = Min.Y + value - 1;}
		}
        
        public override string ToString()
		{
			return string.Format("Rectangle ({0}, {1}), ({2}, {3})", Min.X, Min.Y, Max.X, Max.Y);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is Rectangle)
        	{
        		Rectangle rect = (Rectangle)obj;
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
        /// Returns true if this <see cref="Rectangle"/> is a valid, space-filling rectangle.
        /// </summary>
        /// <returns>True if valid.</returns>
        public bool IsValid()
        {
        	return Min.X <= Max.X && Min.Y <= Max.Y;
        }
        
        /// <summary>
        /// Ensures this <see cref="Rectangle"/> is at least the given size.
        /// </summary>
        /// <param name="width">The min allowed width.</param>
        /// <param name="height">The min allowed height.</param>
        public void EnsureSize(int width, int height)
        {
        	EnsureSize(new Point2D(width, height));
        }
        
        /// <summary>
        /// Ensures this <see cref="Rectangle"/> is at least the given size.
        /// </summary>
        /// <param name="size">The min allowed size.</param>
        public void EnsureSize(Point2D size)
        {
        	Size = VectorUtil.Max(Size, size);
        }
        
        /// <summary>
        /// Returns true if the given x coordinate is within this <see cref="Rectangle"/>'s bounds.
        /// </summary>
        /// <param name="xcoord">The coordinate.</param>
        /// <returns>True if within, else false.</returns>
        public bool ContainsX(int xcoord)
        {
        	return xcoord >= Min.X && xcoord <= Max.X;
        }
        
        /// <summary>
        /// Returns true if the given y coordinate is within this <see cref="Rectangle"/>'s bounds.
        /// </summary>
        /// <param name="ycoord">The coordinate.</param>
        /// <returns>True if within, else false.</returns>
        public bool ContainsY(int ycoord)
        {
        	return ycoord >= Min.Y && ycoord <= Max.Y;
        }
        
        /// <summary>
        /// Returns true if the given <see cref="Point2D"/> is within this <see cref="Rectangle"/>'s bounds.
        /// </summary>
        /// <param name="vec">The vec.</param>
        /// <returns>True if within, else false.</returns>
        public bool Contains(Point2D vec)
        {
        	return ContainsX(vec.X) && ContainsY(vec.Y);
        }
        
        /// <summary>
        /// Returns true if the given <see cref="Rectangle"/> is entirely within this <see cref="Rectangle"/>'s bounds.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <returns>True if within, else false.</returns>
        public bool Contains(Rectangle rect)
        {
        	return rect.Min.X >= Min.X && rect.Min.Y >= Min.Y && rect.Max.X <= Max.X && rect.Max.Y <= Max.Y;
        }
        
        /// <summary>
        /// Explicit cast to <see cref="System.Drawing.Rectangle"/>.
        /// </summary>
        /// <param name="rect">The rect to cast.</param>
        /// <returns>The resulting rect.</returns>
        public static implicit operator System.Drawing.Rectangle(Rectangle rect)
        {
        	return new System.Drawing.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }
        
        /// <summary>
        /// Explicit cast from <see cref="System.Drawing.Rectangle"/>.
        /// </summary>
        /// <param name="rect">The rect to cast.</param>
        /// <returns>The resulting rect.</returns>
        public static implicit operator Rectangle(System.Drawing.Rectangle rect)
        {
        	return new Rectangle{X = rect.X, Y = rect.Y, Width = rect.Width, Height = rect.Height};
        }
        
		/// <summary>
        /// Implicit cast from <see cref="Point2D"/>.
        /// Resulting <see cref="Rectangle"/> extends into quadrant 1 from the origin.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting rect.</returns>
        public static explicit operator Rectangle(Point2D vec)
        {
        	return new Rectangle{Position = 0, Size = vec};
        }
        
        /// <summary>
        /// Adds the given vec to the rect's position.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="vec">The vec.</param>
        /// <returns>The moved rectangle.</returns>
        public static Rectangle operator +(Rectangle rect, Point2D vec)
        {
        	//passed by value, so we can just add vec to rect pos and pass it back
        	rect.Position += vec;
            return rect;
        }

        /// <summary>
        /// Subtracts the given vec from the rect's position.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="vec">The vec.</param>
        /// <returns>The moved rectangle.</returns>
        public static Rectangle operator -(Rectangle rect, Point2D vec)
        {
        	//passed by value, so we can just add vec to rect pos and pass it back
        	rect.Position -= vec;
            return rect;
        }

        /// <summary>
        /// Multiplies the given rect's size by the given value, keeping it centered.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="val">The value.</param>
        /// <returns>The resized rectangle.</returns>
        public static Rectangle operator *(Rectangle rect, double val)
        {
        	Point2D middle = (rect.Max + rect.Min) / 2.0;
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
        public static Rectangle operator /(Rectangle rect, double val)
        {
        	Point2D middle = (rect.Max + rect.Min) / 2.0;
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
        public static bool operator ==(Rectangle rect, Rectangle rect2)
        {
        	return (rect.Min == rect2.Min) && (rect.Max == rect2.Max);
        }
        
        /// <summary>
        /// True if the rects are not equal.
        /// </summary>
        /// <param name="rect">The first rect.</param>
        /// <param name="rect2">The second rect.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(Rectangle rect, Rectangle rect2)
        {
        	return !(rect == rect2);
        }
	}
}