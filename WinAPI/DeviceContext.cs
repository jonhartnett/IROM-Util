namespace IROM.Util
{
	using System;
	using System.ComponentModel;
	using System.Runtime.InteropServices;
	using System.Collections.Generic;
	using System.Threading;
	
	/// <summary>
	/// A managed wrapper for an unmanaged WinAPI device context.
	/// </summary>
	public class DeviceContext : IDisposable
	{
		/// <summary>
		/// True if Handle was created by the current instance.
		/// </summary>
		private readonly bool Owned = true;
		
		/// <summary>
		/// The backing object stack.
		/// </summary>
		private readonly Stack<IntPtr> ObjStack = new Stack<IntPtr>();
		
		/// <summary>
		/// The backing handle var.
		/// </summary>
		private IntPtr baseHandle;
		
		/// <summary>
		/// The handle of this <see cref="DeviceContext"/>.
		/// </summary>
		public IntPtr Handle
		{
			get{return baseHandle;}
			private set{baseHandle = value;}
		}
		
		/// <summary>
		/// Creates a new <see cref="DeviceContext"/> compatible with the primary screen.
		/// </summary>
		public DeviceContext()
		{
			Handle = CreateCompatibleDC(IntPtr.Zero);
			WinAPIUtils.Assert(Handle != IntPtr.Zero);
		}
		
		/// <summary>
		/// Creates a new <see cref="DeviceContext"/> with the given handle.
		/// </summary>
		/// <param name="handle">The handle to use.</param>
		public DeviceContext(IntPtr handle)
		{
			Handle = handle;
			Owned = false;
		}
		
		~DeviceContext()
		{
			Dispose();
		}
		
		/// <summary>
		/// Creates a new <see cref="DeviceContext"/> compatible with the given <see cref="DeviceContext"/>.
		/// </summary>
		/// <param name="deviceContext">The <see cref="DeviceContext"/></param>
		public DeviceContext(DeviceContext deviceContext)
		{
			Handle = CreateCompatibleDC(deviceContext.Handle);
			WinAPIUtils.Assert(Handle != IntPtr.Zero);
		}
		
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			lock(this)
			{
				if(Handle == IntPtr.Zero) return;
				while(ObjStack.Count > 0)
				{
					Pop();
				}
				//only delete if it is our Handle.
				if(Owned)
				{
					bool result = DeleteDC(Handle);
					Handle = IntPtr.Zero;
					try
					{
						WinAPIUtils.Assert(result);
					}catch(Win32Exception ex)
					{
						Console.WriteLine(ex);
					}
				}
			}
		}
		
		/// <summary>
		/// Pushs a new object onto this <see cref="DeviceContext"/>'s stack.
		/// </summary>
		/// <param name="obj"></param>
		public void Push(IntPtr obj)
		{
			if(Monitor.TryEnter(this))
			{
				IntPtr old = SelectObject(Handle, obj);
				WinAPIUtils.Assert(old != IntPtr.Zero);
				//save old on stack
				ObjStack.Push(old);
				Monitor.Exit(this);
			}
		}
		
		public void Pop()
		{
			if(Monitor.TryEnter(this))
			{
				//replace with prev object
				SelectObject(Handle, ObjStack.Pop());
				Monitor.Exit(this);
			}
		}
		
		//winapi methods
		[DllImport("gdi32.dll", SetLastError = true)]
        private static extern IntPtr CreateCompatibleDC([In] IntPtr deviceContext);
        [DllImport("gdi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteDC([In] IntPtr deviceContext);
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern IntPtr SelectObject([In] IntPtr deviceContext, [In] IntPtr obj);
	}
}
