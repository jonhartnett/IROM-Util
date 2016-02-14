namespace IROM.Util
{
	using System;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Runtime.InteropServices;
	
	/// <summary>
	/// A color image with <see cref="ARGB"/> storage. Channel data is in bytes.
	/// </summary>
	public unsafe sealed class Image : DataMap2D<ARGB>, IDisposable
    {
		/// <summary>
		/// True if this Image is backed by a DIBSection.
		/// </summary>
		private bool isDIBSection;
		
		/// <summary>
		/// The internal data array.
		/// </summary>
		private ARGB* data;
		
		/// <summary>
		/// The native buffer.	
		/// </summary>
		private NativeBuffer buffer;
		
        /// <summary>
        /// Creates a new <see cref="Image"/> with the given initial size. Images with dimensions of 0 are discouraged.
        /// </summary>
        /// <param name="w">The initial width.</param>
        /// <param name="h">The initial height.</param>
        public Image(int w, int h) : this(w, h, false)
        {
        	
        }
        
        /// <summary>
        /// Creates a new <see cref="Image"/> with the given initial size. Images with dimensions of 0 are discouraged.
        /// </summary>
        /// <param name="w">The initial width.</param>
        /// <param name="h">The initial height.</param>
        /// <param name="isDIB">True if the image is backed by a DIBSection.</param>
        public Image(int w, int h, bool isDIB)
		{
        	isDIBSection = isDIB;
        	Resize(w, h);
		}

		public void Dispose()
		{
			buffer.Dispose();
		}
		
        public new ARGB this[int x, int y]
        {
        	[MethodImpl(MethodImplOptions.AggressiveInlining)]
        	get
			{
        		return *(data + x + y * Width);
			}
        	[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				*(data + x + y * Width) = value;
			}
        }
        
        /// <summary>
        /// Returns the raw pointer to the <see cref="ARGB"/> data array.
        /// </summary>
        /// <returns>The array pointer.</returns>
        public ARGB* GetRawData()
        {
            return data;
        }

        /// <summary>
        /// Returns the <see cref="IChannelManager2D{T}">IChannelManager2D</see> instance. Must be cast to <see cref="IChannelManager2D{T}">IChannelManager&lt;byte&gt;</see> for use.
        /// </summary>
        /// <returns>The channel manager.</returns>
        public override object GetChannelManager()
        {
        	return new ImageChannelManager(this);
        }
        
        protected override ARGB BaseGet(int x, int y)
        {
        	return *(data + x + y * Width);
        }
        
        protected override void BaseSet(int x, int y, ARGB value)
        {
        	*(data + x + y * Width) = value;
        }
        
        protected override void BaseResize(int width, int height)
        {
        	if(buffer != null) buffer.Dispose();
        	data = NativeBuffer.CreateBuffer(out buffer, width, height, isDIBSection);
        }
        
        public override bool UnsafeOperationsSupported()
        {
        	return true;
        }
        
        public unsafe override void* BeginUnsafeOperation()
        {
        	return (void*)data;
        }
        
        public override void EndUnsafeOperation(){}
        
        /// <summary>
        /// Returns the <see cref="DeviceContext"/> for this DIB-backed <see cref="Image"/>.
        /// </summary>
        /// <returns>The <see cref="DeviceContext"/>.</returns>
        internal IntPtr GetContext()
        {
        	return buffer.Context.Handle;
        }
        
        public unsafe static Image LoadImage(string path)
        {
        	using(Bitmap bitmap = new Bitmap(path))
        	{
	        	return FromBitmap(bitmap);
        	}
        }
        
        public unsafe static Image FromBitmap(Bitmap bitmap)
        {
        	//convert to 32bppArgb
        	if(bitmap.PixelFormat != PixelFormat.Format32bppArgb && bitmap.PixelFormat != PixelFormat.Format32bppRgb && 
        	   bitmap.PixelFormat != PixelFormat.Format32bppPArgb && bitmap.PixelFormat != PixelFormat.Format24bppRgb)
	        {
        		using(Bitmap clone = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppRgb))
        		{
			        using (Graphics gr = Graphics.FromImage(clone))
			        {
			        	gr.DrawImage(bitmap, new Rectangle{Size = clone.Size});
			        }
			        return FromBitmap(clone);
        		}
	        }
	        
        	Image image = new Image(bitmap.Width, bitmap.Height);
        	BitmapData data = bitmap.LockBits(new Rectangle{Size = bitmap.Size}, ImageLockMode.ReadOnly, bitmap.PixelFormat);
    
        	if(bitmap.PixelFormat == PixelFormat.Format24bppRgb)
        	{
        		byte* p1 = (byte*)data.Scan0;
	    		byte* p2 = (byte*)image.BeginUnsafeOperation();
	    		byte* end = p1 + (bitmap.Width * bitmap.Height * 3);
	    		while(p1 != end)
	    		{
	    			*p2++ = *p1++;
	    			*p2++ = *p1++;
	    			*p2++ = *p1++;
	    			p2++;
	    		}
	    		image.EndUnsafeOperation();
        	}else
        	{
	    		int* p1 = (int*)data.Scan0;
	    		int* p2 = (int*)image.BeginUnsafeOperation();
	    		int* end = p1 + (bitmap.Width * bitmap.Height);
	    		while(p1 != end)
	    		{
	    			*p2++ = *p1++;
	    		}
	    		image.EndUnsafeOperation();
        	}
    		
        	return image;
        }
        
        public unsafe static void SaveImage(string path, Image image)
        {
        	using(Bitmap bitmap = ToBitmap(image))
        	{
	    		bitmap.Save(path);
        	}
        }
        
        public unsafe static Bitmap ToBitmap(Image image)
        {
        	Bitmap bitmap = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppRgb);
        	BitmapData data = bitmap.LockBits(new Rectangle{Size = bitmap.Size}, ImageLockMode.ReadOnly, bitmap.PixelFormat);
	    
    		int* p1 = (int*)image.BeginUnsafeOperation();
    		int* p2 = (int*)data.Scan0;
    		int* end = p1 + (image.Width * image.Height);
    		while(p1 != end)
    		{
    			*p2++ = *p1++;
    		}
    		image.EndUnsafeOperation();
    		
    		return bitmap;
        }

        /// <summary>
        /// The <see cref="IChannelManager2D{T}">IChannelManager2D</see> instance for Images.
        /// </summary>
        private class ImageChannelManager : IChannelManager2D<byte>
        {
            /// <summary>
            /// Strong types.
            /// </summary>
            private static readonly string[] StrongTypes = {"ALPHA", "RED", "GREEN", "BLUE"};
            /// <summary>
            /// Weak types.
            /// </summary>
            private static readonly string[] WeakTypes = {"GREY"};
            /// <summary>
            /// All types.
            /// </summary>
            private static readonly string[] AllTypes = {"ALPHA", "RED", "GREEN", "BLUE", "GREY"};
            /// <summary>
        	/// The parent <see cref="Image"/>.
        	/// </summary>
            private readonly Image Parent;
            
            //static channel instances
            private readonly ImageAlphaChannel AlphaChannel;
            private readonly ImageRedChannel RedChannel;
            private readonly ImageGreenChannel GreenChannel;
            private readonly ImageBlueChannel BlueChannel;
            private readonly ImageGreyChannel GreyChannel;
            
            internal ImageChannelManager(Image img)
            {
                Parent = img;
                AlphaChannel = new ImageAlphaChannel(Parent);
                RedChannel = new ImageRedChannel(Parent);
                GreenChannel = new ImageGreenChannel(Parent);
                BlueChannel = new ImageBlueChannel(Parent);
                GreyChannel = new ImageGreyChannel(Parent);
            }
            
			public bool HasType(string type)
			{
				return StrongTypes.Contains(type);
			}
			
			public bool HasWeakType(string type)
			{
				return WeakTypes.Contains(type);
			}
			
			public string[] GetTypes()
			{
				return StrongTypes;
			}
			
			public string[] GetAllTypes()
			{
				return AllTypes;
			}
			
			public DataMap2D<byte> GetChannel(string type)
			{
				switch(type)
				{
					case "Alpha": return AlphaChannel;
					case "RED": return RedChannel;
					case "GREEN": return GreenChannel;
					case "BLUE": return BlueChannel;
					case "GREY": return GreyChannel;
					default: throw new NotSupportedException();
				}
			}
        }
        
        /// <summary>
        /// The base channel class for <see cref="Image"/>s.
        /// </summary>
        public abstract class ImageChannel : DataMap2D<byte>
        {
            internal readonly Image Parent;

            internal ImageChannel(Image img)
            {
                Parent = img;
                Width = img.Width;
                Height = img.Height;
            }
	    	
        	public override object GetChannelManager()
        	{
        		throw new NotImplementedException("Image channels do not have channels themselves.");
        	}
        	
        	protected override void BaseResize(int width, int height)
        	{
        		throw new NotImplementedException("Image channel cannot be individually resized. Use Image.Resize().");
        	}
        	
        	public override bool UnsafeOperationsSupported()
	        {
	        	return true;
	        }
	        
	        public unsafe override void* BeginUnsafeOperation()
	        {
	        	return (void*)Parent.data;
	        }
	        
	        public override void EndUnsafeOperation(){}
	        
	        public override int GetRawDataStride()
	        {
	        	return 4;
        	}
        }
        
        /// <summary>
        /// The Alpha channel for <see cref="Image"/>s.
        /// </summary>
        public class ImageAlphaChannel : ImageChannel
        {
            internal ImageAlphaChannel(Image img) : base(img)
            {
            }

	    	public new byte this[int x, int y]
	    	{
	    		[MethodImpl(MethodImplOptions.AggressiveInlining)]
	        	get
				{
	        		return (*(Parent.data + x + y * Width)).A;
				}
	        	[MethodImpl(MethodImplOptions.AggressiveInlining)]
				set
				{
					(*(Parent.data + x + y * Width)).A = value;
				}
	    	}
	    	
	    	protected override byte BaseGet(int x, int y)
	        {
	        	return this[x, y];
	        }
	        
	        protected override void BaseSet(int x, int y, byte value)
	        {
	        	this[x, y] = value;
	        }
	        
	        public override int GetRawDataOffset()
	        {
	        	return 3;
	        }
        }
        
        /// <summary>
        /// The RED channel for <see cref="Image"/>s.
        /// </summary>
        public class ImageRedChannel : ImageChannel
        {
            internal ImageRedChannel(Image img) : base(img)
            {
            }

	    	public new byte this[int x, int y]
	    	{
	    		[MethodImpl(MethodImplOptions.AggressiveInlining)]
	        	get
				{
	        		return (*(Parent.data + x + y * Width)).R;
				}
	        	[MethodImpl(MethodImplOptions.AggressiveInlining)]
				set
				{
					(*(Parent.data + x + y * Width)).R = value;
				}
	    	}
	    	
	    	protected override byte BaseGet(int x, int y)
	        {
	        	return this[x, y];
	        }
	        
	        protected override void BaseSet(int x, int y, byte value)
	        {
	        	this[x, y] = value;
	        }
	        
	        public override int GetRawDataOffset()
	        {
	        	return 2;
	        }
        }
        
        /// <summary>
        /// The GREEN channel for <see cref="Image"/>s.
        /// </summary>
        public class ImageGreenChannel : ImageChannel
        {
            internal ImageGreenChannel(Image img) : base(img)
            {
            }

	    	public new byte this[int x, int y]
	    	{
	    		[MethodImpl(MethodImplOptions.AggressiveInlining)]
	    		get
	    		{
	    			return (*(Parent.data + x + y * Width)).G;
	    		}
	    		[MethodImpl(MethodImplOptions.AggressiveInlining)]
	    		set
	    		{
	    			(*(Parent.data + x + y * Width)).G = value;
	    		}
	    	}
	    	
	    	protected override byte BaseGet(int x, int y)
	        {
	        	return this[x, y];
	        }
	        
	        protected override void BaseSet(int x, int y, byte value)
	        {
	        	this[x, y] = value;
	        }
	        
	        public override int GetRawDataOffset()
	        {
	        	return 1;
	        }
        }
        
        /// <summary>
        /// The BLUE channel for <see cref="Image"/>s.
        /// </summary>
        public class ImageBlueChannel : ImageChannel
        {
            internal ImageBlueChannel(Image img) : base(img)
            {
            }

	    	public new byte this[int x, int y]
	    	{
	    		[MethodImpl(MethodImplOptions.AggressiveInlining)]
	    		get
	    		{
	    			return (*(Parent.data + x + y * Width)).B;
	    		}
	    		[MethodImpl(MethodImplOptions.AggressiveInlining)]
	    		set
	    		{
	    			(*(Parent.data + x + y * Width)).B = value;
	    		}
	    	}
	    	
	    	protected override byte BaseGet(int x, int y)
	        {
	        	return this[x, y];
	        }
	        
	        protected override void BaseSet(int x, int y, byte value)
	        {
	        	this[x, y] = value;
	        }
	        
	        public override int GetRawDataOffset()
	        {
	        	return 0;
	        }
        }
        
        /// <summary>
        /// The GREY channel for <see cref="Image"/>s.
        /// </summary>
        public class ImageGreyChannel : ImageChannel
        {
            internal ImageGreyChannel(Image img) : base(img)
            {
            }
	    	
	    	public new byte this[int x, int y]
	    	{
	    		[MethodImpl(MethodImplOptions.AggressiveInlining)]
	    		get
	    		{
	    			return (byte)Parent[x, y].RGB.Greyscale.R;
	    		}
	    		[MethodImpl(MethodImplOptions.AggressiveInlining)]
	    		set
	    		{
	    			ARGB color = Parent[x, y];
	    			color.RGB = new RGB(value, value, value);
	    			Parent[x, y] = color;
	    		}
	    	}
	    	
	    	protected override byte BaseGet(int x, int y)
	        {
	        	return this[x, y];
	        }
	        
	        protected override void BaseSet(int x, int y, byte value)
	        {
	        	this[x, y] = value;
	        }
	        
	        public override bool UnsafeOperationsSupported()
	        {
	        	return false;
	        }
        }
    }
}
