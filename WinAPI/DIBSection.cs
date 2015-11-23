namespace IROM.Util
{
	using System;
	using System.ComponentModel;
	using System.Runtime.InteropServices;
	
	/// <summary>
	/// A managed wrapper for an unmanaged WinAPI DIBSection.
	/// </summary>
	public unsafe class DIBSection : IDisposable
	{
		/// <summary>
		/// The <see cref="DeviceContext"/> for this DIBSection.
		/// </summary>
		public DeviceContext DC;
		
		/// <summary>
		/// The handle of this <see cref="DIBSection"/>.
		/// </summary>
		public IntPtr Handle;
		
		/// <summary>
		/// The raw data pointer of this <see cref="DIBSection"/>.
		/// </summary>
		public int* Data
		{
			get;
			protected set;
		}
		
		/// <summary>
		/// The width of this <see cref="DIBSection"/>.
		/// </summary>
		public int Width
		{
			get;
			protected set;
		}
		
		/// <summary>
		/// The height of this <see cref="DIBSection"/>.
		/// </summary>
		public int Height
		{
			get;
			protected set;
		}
		
		/// <summary>
		/// True if this object has already been disposed.
		/// </summary>
		private bool Disposed = false;
		
		/// <summary>
		/// Creates a new <see cref="DIBSection"/> with the given dimensions.
		/// </summary>
		/// <param name="w">The width.</param>
		/// <param name="h">The height.</param>
		public DIBSection(int w, int h) : this(new DeviceContext(), w, h)
		{
			
		}
		
		/// <summary>
		/// Creates a new <see cref="DIBSection"/> with the given dimensions and <see cref="DeviceContext"/>.
		/// </summary>
		/// <param name="context">The <see cref="DeviceContext"/> instance.</param>
		/// <param name="w">The width.</param>
		/// <param name="h">The height.</param>
		public DIBSection(DeviceContext context, int w, int h)
		{
			//set context
			DC = context;
			//create info
			BITMAPINFO info = new BITMAPINFO();
			//init with size
        	info.init(w, h);
        	IntPtr pixels;
        	//create DIB
        	Handle = CreateDIBSection(DC.Handle, ref info, DIB_RGB_COLORS, out pixels, IntPtr.Zero, 0);
        	Assert(Handle != IntPtr.Zero);
        	//set data
        	Data = (int*)pixels;
        	//select the DIB into the DC
        	DC.Push(Handle);
		}
		
		~DIBSection()
		{
			UDispose();
		}
		
		public void Dispose()
		{
			UDispose();
			GC.SuppressFinalize(this);
		}
		
		/// <summary>
		/// Disposes of unmanaged resources.
		/// </summary>
		private void UDispose()
		{
			if(!Disposed)
			{
				bool result = DeleteObject(Handle);
				try
				{
					Assert(result);
				}catch(Win32Exception ex)
				{
					Console.WriteLine(ex);
				}
				Disposed = true;
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
        private static extern IntPtr CreateDIBSection(IntPtr deviceContext, ref BITMAPINFO info, uint usage, out IntPtr bits, IntPtr section, uint offset);
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
