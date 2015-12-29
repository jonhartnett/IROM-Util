namespace IROM.Util
{
	using System;
	using System.Linq;
	
	/// <summary>
	/// A generic 2D map of data. Supports data reading/writing, per channel access, and resizing.
	/// </summary>
	/// <typeparam name="T">The data type.</typeparam>
    public abstract class DataMap2D<T> where T : struct
    {
    	private Point2D BaseSize;
    	
		/// <summary>
		/// Gets the size of this map. For resizing, see <see cref="Resize"/>.
		/// </summary>
		public Point2D Size 
		{
			get
    		{
    			return BaseSize;
    		}
    		private set
    		{
    			BaseSize = value;
    		}
		}
    	
    	/// <summary>
    	/// Gets the width (x-size) of this map. For resizing, see <see cref="Resize"/>.
    	/// </summary>
        public int Width
        {
            get
            {
            	return BaseSize.X;
            }
            protected set
            {
            	BaseSize.X = value;
            }
        }
        
        /// <summary>
        /// Gets the height (y-size) of this map. For resizing, see <see cref="Resize"/>.
        /// </summary>
        public int Height
        {
            get
            {
            	return BaseSize.Y;
            }
            protected set
            {
            	BaseSize.Y = value;
            }
        }
        
        /// <summary>
        /// Creates a new empty <see cref="DataMap2D{T}">DataMap2D</see>. Leaves sizing up to the calling constructor.
        /// </summary>
        protected DataMap2D()
        {
        	
        }
    	
        /// <summary>
        /// Creates a new <see cref="DataMap2D{T}">DataMap2D</see> with the given initial size. Maps with dimensions of 0 are highly discouraged.
        /// </summary>
        /// <param name="width">The initial width.</param>
        /// <param name="height">The initial height.</param>
        protected DataMap2D(int width, int height)
        {
            Resize(width, height);
        }
        
        /// <summary>
        /// Creates a new <see cref="DataMap2D{T}">DataMap2D</see> with the given initial size. Maps with dimensions of 0 are highly discouraged.
        /// </summary>
        /// <param name="size">The initial size.</param>
        protected DataMap2D(Point2D size)
        {
        	Resize(size);
        }
        
        /// <summary>
    	/// Accesses the data value at the given coordinates.
    	/// </summary>
    	/// <param name="x">The x coord.</param>
    	/// <param name="y">The y coord.</param>
    	/// <returns>The data value.</returns>
        public T this[int x, int y]
        {
            get
            {
            	return BaseGet(x, y);
            }
            
            set
            {
            	BaseSet(x, y, value);
            }
        }
        
        /// <summary>
    	/// Accesses the data value at the given coordinates.
    	/// </summary>
    	/// <param name="coords">The coords.</param>
    	/// <returns>The data value.</returns>
        public T this[Point2D coords]
        {
            get
            {
            	return this[coords.X, coords.Y];
            }
            
            set
            {
            	this[coords.X, coords.Y] = value;
            }
        }

        /// <summary>
        /// Returns the <see cref="IChannelManager2D{T}">IChannelManager2D</see> instance. Must be cast to a specific type for usage. See specific DataMap implementation for data type details.
        /// </summary>
        /// <returns>The channel manager.</returns>
        public abstract object GetChannelManager();
        
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
        /// Resizes the underlying data structure of this <see cref="DataMap2D{T}">DataMap2D</see>.
        /// Width and Height still contain the old size at the time of the call.
        /// </summary>
        /// <param name="width">The new width.</param>
        /// <param name="height">The new height.</param>
        protected abstract void BaseResize(int width, int height);

        /// <summary>
        /// Resizes this <see cref="DataMap2D{T}">DataMap2D</see>.
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
        /// Resizes this <see cref="DataMap2D{T}">DataMap2D</see>.
        /// </summary>
        /// <param name="size">The new size.</param>
        public void Resize(Point2D size)
        {
        	Resize(size.X, size.Y);
        }
        
        /// <summary>
        /// Returns true if this <see cref="DataMap2D{T}">DataMap2D</see> supports unsafe operations.
        /// Unsafe operations, if implemented correctly, significantly improve the speed of most <see cref="DataMapExtensions"/> methods.
        /// To register an unsafe operation delegate, use <see cref="DataMapExtensions"/> register methods.
        /// </summary>
        /// <returns>True if unsafe operations are supported.</returns>
        public virtual bool UnsafeOperationsSupported()
        {
        	return false;
        }
        
        /// <summary>
        /// Begins an unsafe copy to or from this <see cref="DataMap2D{T}">DataMap2D</see>. 
        /// Returns the pointer to the underlying data, assumed to be in the format of an array of the map's type with "index = x + (y * width)" access.
        /// </summary>
        /// <returns>The data pointer.</returns>
        public unsafe virtual void* BeginUnsafeOperation()
        {
        	throw new NotImplementedException("Types that allow unsafe copying should override BeginUnsafeCopy()!");
        }
        
        /// <summary>
        /// Ends an unsafe copy to or from this <see cref="DataMap2D{T}">DataMap2D</see>. 
        /// </summary>
        public virtual void EndUnsafeOperation()
        {
        	throw new NotImplementedException("Types that allow unsafe copying should override EndUnsafeCopy()!");
        }
        
        /// <summary>
        /// The offset from the beginning of the RawData object for the start of the data.
        /// Returns 0 by default.
        /// </summary>
        /// <returns>The data offset.</returns>
        public virtual int GetRawDataOffset()
        {
        	return 0;
        }
        
        /// <summary>
        /// The distance between data points in the array.
        /// Returns 1 by default.
        /// </summary>
        /// <returns>The data stride.</returns>
        public virtual int GetRawDataStride()
        {
        	return 1;
        }
    }
}
