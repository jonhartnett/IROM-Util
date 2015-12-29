namespace IROM.Util
{
	using System;
	using System.ComponentModel;
	using System.Runtime.InteropServices;
	
	/// <summary>
	/// A managed wrapper for an unmanaged ARGB buffer.
	/// Either backed by DIBSection or Marshal.AllocHGlobal
	/// </summary>
	public unsafe class NativeBuffer : IDisposable
	{
		/// <summary>
		/// Creates a new <see cref="NativeBuffer"/> with the given dimensions.
		/// </summary>
		/// <param name="buffer">The storage location for the buffer object.</param>
		/// <param name="w">The width.</param>
		/// <param name="h">The height.</param>
		/// <param name="isDIB">True if the buffer should be DIB backed, false for Marshal.</param>
		/// <returns>Returns the data buffer.</returns>
		public static ARGB* CreateBuffer(out NativeBuffer buffer, int w, int h, bool isDIB)
		{
			ARGB* pixels;
			IntPtr handle;
			DeviceContext context = null;
			if(isDIB)
			{
				context = new DeviceContext();
				//create info
				BITMAPINFO info = new BITMAPINFO();
				//init with size
	        	info.init(w, h);
	        	//create DIB
	        	handle = CreateDIBSection(context.Handle, ref info, DIB_RGB_COLORS, out pixels, IntPtr.Zero, 0);
	        	Assert(handle != IntPtr.Zero);
	        	//select the DIB into the DC
	        	context.Push(handle);
			}else
			{
				handle = Marshal.AllocHGlobal(w * h * 4);
				pixels = (ARGB*)handle;
			}
			//create buffer wrapper
			buffer = new NativeBuffer{isDIB = isDIB, Handle = handle, Context = context};
        	//return the data
        	return pixels;
		}
		
		/// <summary>
		/// True if this buffer is based on DIB, false for Marshal.
		/// </summary>
		private bool isDIB;
		
		/// <summary>
		/// The <see cref="DeviceContext"/> instance. Only non-null in DIB buffers.
		/// </summary>
		internal DeviceContext Context;
		
		/// <summary>
		/// The handle of this <see cref="NativeBuffer"/>.
		/// </summary>
		private IntPtr Handle;
		
		private NativeBuffer(){}
		
		~NativeBuffer()
		{
			Dispose();
		}
		
		/// <summary>
		/// Disposes of unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			IntPtr handle = System.Threading.Interlocked.Exchange(ref Handle, IntPtr.Zero);
			if(handle != IntPtr.Zero)
			{
				if(isDIB)
				{
					bool result = DeleteObject(handle);
					try
					{
						Assert(result);
					}catch(Win32Exception ex){Console.WriteLine(ex);}
				}else
				{
					Marshal.FreeHGlobal(handle);
				}
			}
		}
		
		/// <summary>
		/// Throws a <see cref="Win32Exception"/> if the given condition is not true.
		/// </summary>
		/// <param name="test">The condition.</param>
		private static void Assert(bool test)
		{
			if(!test)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
		}
		
		//winapi constant
		private const int DIB_RGB_COLORS = 0;
		
		//winapi methods
		[DllImport("gdi32.dll", SetLastError = true)]
		private static extern IntPtr CreateDIBSection(IntPtr deviceContext, ref BITMAPINFO info, uint usage, out ARGB* pixels, IntPtr section, int offset);
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr obj);
        
        //winapi structs
        [StructLayout(LayoutKind.Sequential)]
	    private struct BITMAPINFO
	    {
	    	public BITMAPINFOHEADER header;
	    	public byte rgbBlue;
	        public byte rgbGreen;
	        public byte rgbRed;
	        public byte rgbReserved;
	    	
	    	public void init(int w, int h)
	    	{
	    		header.init(w, h);
	    	}
	    }
	    
	    [StructLayout(LayoutKind.Sequential)]
	    private struct BITMAPINFOHEADER
	    {
	    	//winapi constant
	    	private const uint BI_RGB = 0;
	    	
	    	public uint size;
	    	public int width;
	    	public int height;
	    	public ushort planes;
	    	public ushort bitCount;
	    	public uint compression;
	        public uint sizeImage;
	        public int xPelsPerMeter;
	        public int yPelsPerMeter;
	        public uint clrUsed;
	        public uint clrImportant;
	        
	        public void init(int w, int h)
	        {
	        	size = (uint)Marshal.SizeOf(this);
	        	width = w;
	        	height = -h;//negative so we are a top-down image
	        	planes = 1;//uneeded (always 1)
	        	bitCount = 32;//bits per pixel
	        	compression = BI_RGB;
	        }
	    }
	}
}
