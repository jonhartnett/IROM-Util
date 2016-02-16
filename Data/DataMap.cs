namespace IROM.Util
{
	using System;
	using System.Linq;
	
	/// <summary>
	/// A generic 2D map of data. Supports data reading/writing, per channel access, and resizing.
	/// </summary>
	/// <typeparam name="T">The data type.</typeparam>
    public abstract class DataMap<T> where T : struct
    {
    	private Point2D BaseSize;
    	
		/// <summary>
		/// Gets the size of this map. For resizing, see <see cref="Resize"/>.
		/// </summary>
		public Point2D Size 
		{
			get{return BaseSize;}
    		protected set{BaseSize = value;}
		}
    	
    	/// <summary>
    	/// Gets the width (x-size) of this map. For resizing, see <see cref="Resize"/>.
    	/// </summary>
        public int Width
        {
            get{return BaseSize.X;}
            protected set{BaseSize.X = value;}
        }
        
        /// <summary>
        /// Gets the height (y-size) of this map. For resizing, see <see cref="Resize"/>.
        /// </summary>
        public int Height
        {
            get{return BaseSize.Y;}
            protected set{BaseSize.Y = value;}
        }
        
        /// <summary>
        /// Creates a new empty <see cref="DataMap{T}">DataMap</see>. Leaves sizing up to the calling constructor.
        /// </summary>
        protected DataMap()
        {
        	
        }
    	
        /// <summary>
        /// Creates a new <see cref="DataMap{T}">DataMap</see> with the given initial size. Maps with dimensions of 0 are highly discouraged.
        /// </summary>
        /// <param name="width">The initial width.</param>
        /// <param name="height">The initial height.</param>
        protected DataMap(int width, int height)
        {
            Resize(width, height);
        }
        
        //       !!!!! Override this with new modifier to allow faster access if the compiler can determine the type at compile time.=
        
        /// <summary>
    	/// Accesses the data value at the given coordinates.
    	/// </summary>
    	/// <param name="x">The x coord.</param>
    	/// <param name="y">The y coord.</param>
    	/// <returns>The data value.</returns>
        public T this[int x, int y]
        {
            get{return BaseGet(x, y);}
            set{BaseSet(x, y, value);}
        }
        
        /// <summary>
        /// Returns the data value at the given coordinates.
        /// </summary>
        /// <param name="x">The x coord.</param>
        /// <param name="y">The y coord.</param>
        /// <returns>The data value.</returns>
        protected abstract T BaseGet(int x, int y);
        
        /// <summary>
        /// Sets the data value at the given coordinates.
        /// </summary>
        /// <param name="x">The x coord.</param>
        /// <param name="y">The y coord.</param>
        /// <param name="value">The value.</param>
        protected abstract void BaseSet(int x, int y, T value);
        
        /// <summary>
        /// Resizes the underlying data structure of this <see cref="DataMap{T}">DataMap</see>.
        /// Width and Height still contain the old size at the time of the call.
        /// </summary>
        /// <param name="width">The new width.</param>
        /// <param name="height">The new height.</param>
        protected abstract void BaseResize(int width, int height);

        /// <summary>
        /// Resizes this <see cref="DataMap{T}">DataMap</see>.
        /// </summary>
        /// <param name="width">The new width.</param>
        /// <param name="height">The new height.</param>
        public void Resize(int width, int height)
        {
            if (width < 0 || height < 0)
            {
            	throw new ArgumentException("Invalid size: (" + width + " " + height + ")");
            }
            // if already this size, don't bother
            if (Width == width && Height == height)
            {
            	return;
            }
            BaseResize(width, height);
            Width = width;
            Height = height;
        }
        
        /// <summary>
        /// Returns a set of rendering contexts to be used for rendering on this <see cref="DataMap{T}">DataMap</see>.
        /// The lowest index context with a non-null delegate of the relavent type is used for the actual rendering.
        /// </summary>
        /// <returns>The content array.</returns>
        protected internal RenderContext<T>[] GetContexts()
        {
        	return new []{UnsafeRenderer<T>.Instance, BasicRenderer<T>.Instance};
        }
    }
}
