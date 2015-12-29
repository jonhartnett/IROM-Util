namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Simple double view struct.
	/// </summary>
	public struct Viewport
	{
		/// <summary>
		/// The <see cref="Viewport"/> minimum coordinates (inclusive).
		/// </summary>
		public Vec2D Min;
		
		/// <summary>
		/// The <see cref="Viewport"/> maximum coordinates (exclusive).
		/// </summary>
		public Vec2D Max;
		
		/// <summary>
		/// The <see cref="Viewport"/> position.
		/// </summary>
		public Vec2D Position
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
		/// The <see cref="Viewport"/> size.
		/// </summary>
		public Vec2D Size
		{
			get{return Max - Min;}
			set{Max = Min + value;}
		}
		
		/// <summary>
		/// The <see cref="Viewport"/> x.
		/// </summary>
		public double X
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
		/// The <see cref="Viewport"/> y.
		/// </summary>
		public double Y
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
		/// The <see cref="Viewport"/> width.
		/// </summary>
		public double Width
		{
			get{return Max.X - Min.X;}
			set{Max.X = Min.X + value;}
		}
		
		/// <summary>
		/// The <see cref="Viewport"/> height.
		/// </summary>
		public double Height
		{
			get{return Max.Y - Min.Y;}
			set{Max.Y = Min.Y + value;}
		}
        
        public override string ToString()
		{
			return string.Format("Viewport ({0}, {1}), ({2}, {3})", Min.X, Min.Y, Max.X, Max.Y);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is Viewport)
        	{
        		Viewport view = (Viewport)obj;
        		return this == view;
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
        /// Returns true if this <see cref="Viewport"/> is a valid, space-filling view.
        /// </summary>
        /// <returns>True if valid.</returns>
        public bool IsValid()
        {
        	return Min.X <= Max.X && Min.Y <= Max.Y;
        }
        
        /// <summary>
        /// Ensures this <see cref="Viewport"/> is at least the given size.
        /// </summary>
        /// <param name="width">The min allowed width.</param>
        /// <param name="height">The min allowed height.</param>
        public void EnsureSize(double width, double height)
        {
        	EnsureSize(new Vec2D(width, height));
        }
        
        /// <summary>
        /// Ensures this <see cref="Viewport"/> is at least the given size.
        /// </summary>
        /// <param name="size">The min allowed size.</param>
        public void EnsureSize(Vec2D size)
        {
        	Size = VectorUtil.Max(Size, size);
        }
        
         /// <summary>
        /// Returns true if the given x coordinate is within this <see cref="Viewport"/>'s bounds.
        /// </summary>
        /// <param name="xcoord">The coordinate.</param>
        /// <returns>True if within, else false.</returns>
        public bool ContainsX(double xcoord)
        {
        	return xcoord >= Min.X && xcoord <= Max.X;
        }
        
        /// <summary>
        /// Returns true if the given y coordinate is within this <see cref="Viewport"/>'s bounds.
        /// </summary>
        /// <param name="ycoord">The coordinate.</param>
        /// <returns>True if within, else false.</returns>
        public bool ContainsY(double ycoord)
        {
        	return ycoord >= Min.Y && ycoord <= Max.Y;
        }
        
        /// <summary>
        /// Returns true if the given <see cref="Vec2D"/> is within this <see cref="Viewport"/>'s bounds.
        /// </summary>
        /// <param name="vec">The vec.</param>
        /// <returns>True if within, else false.</returns>
        public bool Contains(Vec2D vec)
        {
        	return ContainsX(vec.X) && ContainsY(vec.Y);
        }
        
        /// <summary>
        /// Returns true if the given <see cref="Viewport"/> is entirely within this <see cref="Viewport"/>'s bounds.
        /// </summary>
        /// <param name="view">The rectangle.</param>
        /// <returns>True if within, else false.</returns>
        public bool Contains(Viewport view)
        {
        	return view.Min.X >= Min.X && view.Min.Y >= Min.Y && view.Max.X <= Max.X && view.Max.Y <= Max.Y;
        }
        
		/// <summary>
        /// Implicit cast from <see cref="Vec2D"/>.
        /// Resulting <see cref="Viewport"/> extends into quadrant 1 from the origin.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting view.</returns>
        public static explicit operator Viewport(Vec2D vec)
        {
        	return new Viewport{Position = 0, Size = vec};
        }
        
        /// <summary>
        /// Adds the given vec to the view's position.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="vec">The vec.</param>
        /// <returns>The moved view.</returns>
        public static Viewport operator +(Viewport view, Vec2D vec)
        {
        	//passed by value, so we can just add vec to view pos and pass it back
        	view.Position += vec;
            return view;
        }

        /// <summary>
        /// Subtracts the given vec from the view's position.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="vec">The vec.</param>
        /// <returns>The moved view.</returns>
        public static Viewport operator -(Viewport view, Vec2D vec)
        {
        	//passed by value, so we can just add vec to view pos and pass it back
        	view.Position -= vec;
            return view;
        }

        /// <summary>
        /// Multiplies the given view's size by the given value, keeping it centered.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="val">The value.</param>
        /// <returns>The resized view.</returns>
        public static Viewport operator *(Viewport view, double val)
        {
        	Vec2D middle = (view.Max + view.Min) / 2;
        	view.Min -= middle;
        	view.Min *= val;
        	view.Min += middle;
        	view.Max -= middle;
        	view.Max *= val;
        	view.Max += middle;
            return view;
        }

        /// <summary>
        /// Divides the given view's size by the given value, keeping it centered.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="val">The value.</param>
        /// <returns>The resized view.</returns>
        public static Viewport operator /(Viewport view, double val)
        {
        	Vec2D middle = (view.Max + view.Min) / 2;
        	view.Min -= middle;
        	view.Min /= val;
        	view.Min += middle;
        	view.Max -= middle;
        	view.Max /= val;
        	view.Max += middle;
            return view;
        }
        
        /// <summary>
        /// Multiplies the given view's size by the given value, about the given vector.
        /// </summary>
        /// <param name="val">The value.</param>
        /// <param name="vec">The anchor vec.</param>
        /// <returns>The resized view.</returns>
        public Viewport ResizeAbout(double val, Vec2D vec)
        {
        	Viewport view = this;
        	view.Min -= vec;
        	view.Min *= val;
        	view.Min += vec;
        	view.Max -= vec;
        	view.Max *= val;
        	view.Max += vec;
        	return view;
        }
        
        /// <summary>
        /// True if the views are equal.
        /// </summary>
        /// <param name="view">The first view.</param>
        /// <param name="view2">The second view.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(Viewport view, Viewport view2)
        {
        	return (view.Min == view2.Min) && (view.Max == view2.Max);
        }
        
        /// <summary>
        /// True if the views are not equal.
        /// </summary>
        /// <param name="view">The first view.</param>
        /// <param name="view2">The second view.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(Viewport view, Viewport view2)
        {
        	return !(view == view2);
        }
	}
}