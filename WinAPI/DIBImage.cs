namespace IROM.Util
{
	using System;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// An image with <see cref="DIBSection"/> storage that can be quickly blitted to the screen.
	/// </summary>
	public unsafe sealed class DIBImage : DataMap2D<RGB>
	{
		/// <summary>
		/// The <see cref="DIBSection"/> for this <see cref="DIBImage"/>
		/// </summary>
		public DIBSection Section
		{
			get;
			internal set;
		}
		
		/// <summary>
		/// The pointer to the internal RGB data.
		/// </summary>
		public RGB* Data
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Creates a new <see cref="DIBImage"/> with the given dimensions.
		/// </summary>
		/// <param name="width">The width</param>
		/// <param name="height">The height</param>
		public DIBImage(int width, int height) : base(width, height)
		{
			
		}
		
		/// <summary>
		/// Unimplemented.
		/// </summary>
		public override Object GetChannelManager()
        {
            throw new NotImplementedException("Not implemented for purposes of performance. Channel operations should not be performed on DIBImages.");
        }
		
		protected override RGB BaseGet(int x, int y)
        {
        	throw new NotImplementedException("Not implemented for purposes of performance. Use this[int].");
        }
        
        protected override void BaseSet(int x, int y, RGB value)
        {
        	throw new NotImplementedException("Not implemented for purposes of performance. Use this[int].");
        }
        
        /// <summary>
        /// Unimplemented. Use this[int]
        /// </summary>
		public new RGB this[int x, int y]
        {
			get
			{
				throw new NotImplementedException("Not implemented for purposes of performance. Use this[int].");
			}
			set
			{
				throw new NotImplementedException("Not implemented for purposes of performance. Use this[int].");
			}
		}
        
        /// <summary>
        /// Indexer for the pixel at the given index. Index is simply "x + (y * width)". 
        /// Value is not calculated internally so the JIT compiler will inline this method.
        /// </summary>
        /// <param name="index">The pixel index.</param>
        /// <returns>The pixel.</returns>
		public RGB this[int index]
        {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return *(RGB*)(Data + index);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				*(RGB*)(Data + index) = value;
			}
		}
        
        protected override void BaseResize(int w, int h)
        {
        	Section = new DIBSection(w, h);
        	Data = (RGB*)Section.Data;
        }
        
        public override bool UnsafeOperationsSupported()
        {
        	return true;
        }
        
        public unsafe override void* BeginUnsafeOperation()
        {
        	return (void*)Data;
        }
        
        public override void EndUnsafeOperation()
        {
        	
        }
	}
}
