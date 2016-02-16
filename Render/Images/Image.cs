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
	public unsafe sealed class Image : DataMap<ARGB>, IUnsafeMap, IDisposable
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
		/// The alpha channel.
		/// </summary>
		public AlphaChannel ChannelAlpha;
		
		/// <summary>
		/// The red channel.
		/// </summary>
		public RedChannel ChannelRed;
		
		/// <summary>
		/// The green channel.
		/// </summary>
		public GreenChannel ChannelGreen;
		
		/// <summary>
		/// The blue channel.
		/// </summary>
		public BlueChannel ChannelBlue;
		
		/// <summary>
		/// The grey channel.
		/// </summary>
		public GreyChannel ChannelGrey;
		
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
        
        /// <summary>
        /// Returns the <see cref="DeviceContext"/> for this DIB-backed <see cref="Image"/>.
        /// </summary>
        /// <returns>The <see cref="DeviceContext"/>.</returns>
        internal IntPtr GetContext()
        {
        	return buffer.Context.Handle;
        }

		public byte* BeginUnsafeOperation()
		{
			return (byte*)data;
		}
		
		public void EndUnsafeOperation()
		{
			//do nothing
		}
		
		public int GetStride()
		{
			return sizeof(ARGB);
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
	    		byte* p2 = (byte*)image.data;
	    		byte* end = p1 + (bitmap.Width * bitmap.Height * 3);
	    		while(p1 != end)
	    		{
	    			*p2++ = *p1++;
	    			*p2++ = *p1++;
	    			*p2++ = *p1++;
	    			p2++;
	    		}
        	}else
        	{
	    		int* p1 = (int*)data.Scan0;
	    		int* p2 = (int*)image.data;
	    		int* end = p1 + (bitmap.Width * bitmap.Height);
	    		while(p1 != end)
	    		{
	    			*p2++ = *p1++;
	    		}
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
	    
    		int* p1 = (int*)image.data;
    		int* p2 = (int*)data.Scan0;
    		int* end = p1 + (image.Width * image.Height);
    		while(p1 != end)
    		{
    			*p2++ = *p1++;
    		}
    		
    		return bitmap;
        }
        
        /// <summary>
        /// Represents a single channel in an <see cref="Image"/>.
        /// </summary>
        private abstract class Channel : DataMap<byte>, IUnsafeMap
        {
        	/// <summary>
        	/// The parent <see cref="Image"/>.
        	/// </summary>
        	public readonly Image Parent;
        	public Channel(Image parent){Parent = parent;}
	        protected override void BaseResize(int width, int height){throw new InvalidOperationException("Image channels cannot be resized individually, use Image.Resize!");}
			public byte* BeginUnsafeOperation(){return (byte*)Parent.data;}
			public void EndUnsafeOperation(){}
			public int GetStride(){return Parent.GetStride();}
        }
        
        public class RedChannel : Channel
        {
        	public RedChannel(Image parent):base(parent){}
        	
        	public new byte this[int x, int y]
	        {
	        	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	        	get{return (*(Parent.data + x + y * Width)).R;}
	        	[MethodImpl(MethodImplOptions.AggressiveInlining)]
				set{(*(Parent.data + x + y * Width)).R = value;}
	        }
	        
	        protected override byte BaseGet(int x, int y)
	        {
	        	return (*(Parent.data + x + y * Width)).R;
	        }
	        
	        protected override void BaseSet(int x, int y, byte value)
	        {
	        	(*(Parent.data + x + y * Width)).R = value;
	        }
        }
        
        public class GreenChannel : Channel
        {
        	public GreenChannel(Image parent):base(parent){}
        	
        	public new byte this[int x, int y]
	        {
	        	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	        	get{return (*(Parent.data + x + y * Width)).G;}
	        	[MethodImpl(MethodImplOptions.AggressiveInlining)]
				set{(*(Parent.data + x + y * Width)).G = value;}
	        }
	        
	        protected override byte BaseGet(int x, int y)
	        {
	        	return (*(Parent.data + x + y * Width)).G;
	        }
	        
	        protected override void BaseSet(int x, int y, byte value)
	        {
	        	(*(Parent.data + x + y * Width)).G = value;
	        }
        }
        
        public class BlueChannel : Channel
        {
        	public BlueChannel(Image parent):base(parent){}
        	
        	public new byte this[int x, int y]
	        {
	        	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	        	get{return (*(Parent.data + x + y * Width)).B;}
	        	[MethodImpl(MethodImplOptions.AggressiveInlining)]
				set{(*(Parent.data + x + y * Width)).B = value;}
	        }
	        
	        protected override byte BaseGet(int x, int y)
	        {
	        	return (*(Parent.data + x + y * Width)).B;
	        }
	        
	        protected override void BaseSet(int x, int y, byte value)
	        {
	        	(*(Parent.data + x + y * Width)).B = value;
	        }
        }
        
        public class AlphaChannel : Channel
        {
        	public AlphaChannel(Image parent):base(parent){}
        	
        	public new byte this[int x, int y]
	        {
	        	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	        	get{return (*(Parent.data + x + y * Width)).A;}
	        	[MethodImpl(MethodImplOptions.AggressiveInlining)]
				set{(*(Parent.data + x + y * Width)).A = value;}
	        }
	        
	        protected override byte BaseGet(int x, int y)
	        {
	        	return (*(Parent.data + x + y * Width)).A;
	        }
	        
	        protected override void BaseSet(int x, int y, byte value)
	        {
	        	(*(Parent.data + x + y * Width)).A = value;
	        }
        }
        
        /// <summary>
        /// Represents a grey channel in an <see cref="Image"/>.
        /// </summary>
        private abstract class GreyChannel : DataMap<byte>
        {
        	/// <summary>
        	/// The parent <see cref="Image"/>.
        	/// </summary>
        	public readonly Image Parent;
        	public GreyChannel(Image parent){Parent = parent;}
        	
        	public new byte this[int x, int y]
	        {
	        	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	        	get
	        	{
	        		ARGB* target = Parent.data + x + y * Width;
					return (byte)(((*target).R + (*target).G + (*target).B) / 3);
	        	}
	        	[MethodImpl(MethodImplOptions.AggressiveInlining)]
				set
				{
					ARGB* target = Parent.data + x + y * Width;
					(*target).R = (*target).G = (*target).B = value;
				}
	        }
	        
	        protected override byte BaseGet(int x, int y)
	        {
	        	ARGB* target = Parent.data + x + y * Width;
					return (byte)(((*target).R + (*target).G + (*target).B) / 3);
	        }
	        
	        protected override void BaseSet(int x, int y, byte value)
	        {
	        	ARGB* target = Parent.data + x + y * Width;
				(*target).R = (*target).G = (*target).B = value;
	        }
        	
	        protected override void BaseResize(int width, int height){throw new InvalidOperationException("Image channels cannot be resized individually, use Image.Resize!");}
        }
    }
}
