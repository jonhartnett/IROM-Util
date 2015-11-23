namespace IROM.Util
{
	using System;
	using System.ComponentModel;
	using System.Runtime.InteropServices;
	using System.Collections.Generic;
	
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
		/// True if this object has already been disposed.
		/// </summary>
		private bool Disposed = false;
		
		/// <summary>
		/// The handle of this <see cref="DeviceContext"/>.
		/// </summary>
		public IntPtr Handle
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Creates a new <see cref="DeviceContext"/> compatible with the primary screen.
		/// </summary>
		public DeviceContext()
		{
			Handle = CreateCompatibleDC(IntPtr.Zero);
			Assert(Handle != IntPtr.Zero);
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
			UDispose();
		}
		
		/// <summary>
		/// Creates a new <see cref="DeviceContext"/> compatible with the given <see cref="DeviceContext"/>.
		/// </summary>
		/// <param name="deviceContext">The <see cref="DeviceContext"/></param>
		public DeviceContext(DeviceContext deviceContext)
		{
			Handle = CreateCompatibleDC(deviceContext.Handle);
			Assert(Handle != IntPtr.Zero);
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
				//only delete if it is our Handle.
				if(Owned)
				{
					bool result = DeleteDC(Handle);
					try
					{
						Assert(result);
					}catch(Win32Exception ex)
					{
						Console.WriteLine(ex);
					}
				}
				Disposed = true;
			}
		}
		
		/// <summary>
		/// Pushs a new object onto this <see cref="DeviceContext"/>'s stack.
		/// </summary>
		/// <param name="obj"></param>
		public void Push(IntPtr obj)
		{
			IntPtr old = SelectObject(Handle, obj);
			Assert(old != IntPtr.Zero);
			//save old on stack
			ObjStack.Push(old);
		}
		
		public void Pop()
		{
			//replace with prev object
			IntPtr old = SelectObject(Handle, ObjStack.Pop());
			Assert(old != IntPtr.Zero);
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
