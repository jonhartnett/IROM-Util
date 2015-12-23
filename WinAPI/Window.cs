namespace IROM.Util
{
	using System;
	using System.Windows.Forms;
	using System.Runtime.InteropServices;
	using System.ComponentModel;
	
	/// <summary>
	/// Arguments for a <see cref="Window"/> resize event.
	/// </summary>
	public class ResizeEventArgs : EventArgs
	{
		/// <summary>
		/// The new window content area size.
		/// </summary>
		public Point2D Size;
		
		public ResizeEventArgs(Point2D size)
		{
			Size = size;
		}
	}
	
	//handler for resize events
	public delegate void ResizeEventHandler(object sender, ResizeEventArgs args);
	
	/// <summary>
	/// A managed wrapper for an unmanaged WinAPI Window.
	/// </summary>
	public unsafe class Window : NativeWindow, IDisposable
	{
		//WS_OVERLAPPEDWINDOW | WS_TABSTOP
		private const int DEFAULT_WINDOW_STYLE = 0xCF0000;
		
		/// <summary>
		/// The standard cursor for the OS.
		/// </summary>
		private static readonly IntPtr STANDARD_CURSOR = LoadCursor(IntPtr.Zero, 32512);
		
		/// <summary>
		/// The handle of this <see cref="Window"/>.
		/// </summary>
		public new IntPtr Handle;
		
		/// <summary>
		/// True if this <see cref="Window"/> has been started. (displayed)
		/// </summary>
		private bool Started = false;
		
		/// <summary>
		/// The current <see cref="Window"/> bounds.
		/// </summary>
		private Rectangle BaseBounds;
		
		/// <summary>
		/// The frame buffer type.
		/// </summary>
		private Type BufferType;
		
		/// <summary>
		/// The frame buffer instance.
		/// </summary>
		private FrameBufferStrategy Buffer;
		
		/// <summary>
		/// True if this object has already been disposed.
		/// </summary>
		private bool Disposed = false;
		
		/// <summary>
		/// The driving <see cref="MessageLoop"/>
		/// </summary>
		private MessageLoop MessageLoopObj;
		
		/// <summary>
		/// True if the mouse is within the client area.
		/// </summary>
		private bool IsMouseInWindow = false;
		
		/// <summary>
		/// The previous mouse location.
		/// </summary>
		private Point2D PrevMouseCoords;
		
		/// <summary>
		/// True if fullscreen.
		/// </summary>
		private bool BaseFullscreen = false;
		
		/// <summary>
		/// The previous maximized value when fullscreen.
		/// </summary>
		private bool SavedStateMaximized;
		
		/// <summary>
		/// The previous style value when fullscreen.
		/// </summary>
		private uint SavedStateStyle;
		
		/// <summary>
		/// The previous extended style value when fullscreen.
		/// </summary>
		private uint SavedStateExStyle;
		
		/// <summary>
		/// The previous bounds value when fullscreen.
		/// </summary>
		private Rectangle SavedStateBounds;
		
		/// <summary>
		/// The old cursor to restore.
		/// </summary>
		private IntPtr OldCursor;
		
		/// <summary>
		/// The states of inputs.
		/// </summary>
		public readonly InputState InputStates;
		
		/// <summary>
		/// True if this <see cref="Window"/> is the current input target.
		/// </summary>
		public bool IsFocused
		{
			get;
			private set;
		}
		
		/// <summary>
		/// The bounds of this <see cref="Window"/>.
		/// </summary>
		public Rectangle Bounds 
		{
			get{return BaseBounds;}
			set
			{
				//make sure size is at least 1 by 1
				value.EnsureSize(1, 1);
				BaseBounds = value;
			}
		}
		
		/// <summary>
		/// The position of this <see cref="Window"/>.
		/// </summary>
		public Point2D Position 
		{
			get{return BaseBounds.Position;}
			set
			{
				BaseBounds.Position = value;
			}
		}
		
		/// <summary>
		/// The size of this <see cref="Window"/>.
		/// </summary>
		public Point2D Size
		{
			get{return BaseBounds.Size;}
			set
			{
				value = VectorUtil.Max(value, new Point2D(1, 1));
				BaseBounds.Size = value;
			}
		}
		
		/// <summary>
		/// The x coord of this <see cref="Window"/>.
		/// </summary>
		public int X
		{
			get{return BaseBounds.X;}
			set
			{
				BaseBounds.X = value;
			}
		}
		
		/// <summary>
		/// The y coord of this <see cref="Window"/>.
		/// </summary>
		public int Y
		{
			get{return BaseBounds.Position.Y;}
			set
			{
				BaseBounds.Y = value;
			}
		}
		
		/// <summary>
		/// The width of this <see cref="Window"/>.
		/// </summary>
		public int Width
		{
			get{return BaseBounds.Width;}
			set
			{
				value = Math.Max(value, 1);
				BaseBounds.Width = value;
			}
		}
		
		/// <summary>
		/// The height of this <see cref="Window"/>.
		/// </summary>
		public int Height
		{
			get{return BaseBounds.Height;}
			set
			{
				value = Math.Max(value, 1);
				BaseBounds.Height = value;
			}
		}
		
		/// <summary>
		/// Called when this <see cref="Window"/> is resized.
		/// </summary>
		public event ResizeEventHandler OnResize;
		
		/// <summary>
		/// Called when this <see cref="Window"/> is X'ed. Does not actually close unless Exit() is called.
		/// </summary>
		public event EventHandler OnClose;
		
		/// <summary>
		/// Invoked whenever the mouse enters the window.
		/// </summary>
		public event EventHandler OnMouseEnter;
		
		/// <summary>
		/// Invoked whenever the mouse exits the window.
		/// </summary>
		public event EventHandler OnMouseExit;
		
		/// <summary>
		/// Invoked whenever a <see cref="MouseButton"/> is pressed.
		/// </summary>
		public event EventHandler<MouseButtonEventArgs> OnMousePress;
		
		/// <summary>
		/// Invoked whenever a <see cref="MouseButton"/> is released.
		/// </summary>
		public event EventHandler<MouseButtonEventArgs> OnMouseRelease;
		
		/// <summary>
		/// Invoked whenever the mouse is moved.
		/// </summary>
		public event EventHandler<MouseMoveEventArgs> OnMouseMove;
		
		/// <summary>
		/// Invoked whenever the mouse wheel is moved.
		/// </summary>
		public event EventHandler<MouseWheelEventArgs> OnMouseWheel;
		
		/// <summary>
		/// Invoked whenever a <see cref="KeyboardButton"/> is pressed.
		/// </summary>
		public event EventHandler<KeyEventArgs> OnKeyPress;
		
		/// <summary>
		/// Invoked whenever a <see cref="KeyboardButton"/> is released.
		/// </summary>
		public event EventHandler<KeyEventArgs> OnKeyRelease;
		
		/// <summary>
		/// Invoked whenever a character is typed..
		/// </summary>
		public event EventHandler<CharEventArgs> OnCharTyped;
		
		/// <summary>
		/// True if this window is fullscreen.
		/// </summary>
		public bool Fullscreen
		{
			get
			{
				return BaseFullscreen;
			}
			set
			{
				if(Started)
				{
					// Save current window state if not already fullscreen.
					if (!BaseFullscreen) 
					{
					    // Save current window information.  We force the window into restored mode
					    // before going fullscreen because Windows doesn't seem to hide the
					    // taskbar if the window is in the maximized state.
					    SavedStateMaximized = IsZoomed(Handle);
					    if(SavedStateMaximized)
					    {
					    	SendMessage(Handle, /*WM_SYSCOMMAND*/0x0112, /*SC_RESTORE*/0xF120, IntPtr.Zero);
					    }
					    SavedStateStyle = GetWindowLong(Handle, /*GWL_STYLE*/-16);
					    Assert(SavedStateStyle != 0);
					    SavedStateExStyle = GetWindowLong(Handle, /*GWL_EXSTYLE*/-20);
					    Assert(SavedStateExStyle != 0);
					    bool result = GetWindowRect(Handle, out SavedStateBounds);
					    Assert(result);
					}
					
					BaseFullscreen = value;
					
					if(BaseFullscreen) 
					{
					    // Set new window style and size.
					    SetWindowLong(Handle, /*GWL_STYLE*/-16, SavedStateStyle & /*~(WS_CAPTION | WS_THICKFRAME)*/~0x00C40000U);
					    SetWindowLong(Handle, /*GWL_EXSTYLE*/-20, SavedStateExStyle & /*~(WS_EX_DLGMODALFRAME | WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE | WS_EX_STATICEDGE)*/~0x00020301U);
					
					    // On expand, if we're given a window_rect, grow to it, otherwise do
					    // not resize.
					    MONITORINFO monitor_info = default(MONITORINFO);
				    	monitor_info.Size = sizeof(MONITORINFO);
				    	bool result = GetMonitorInfo(MonitorFromWindow(Handle, /*MONITOR_DEFAULTTONEAREST*/2), ref monitor_info);
				    	Assert(result);
				    	result = SetWindowPos(Handle, IntPtr.Zero, monitor_info.Monitor.X, monitor_info.Monitor.Y, 
				    	                      monitor_info.Monitor.Width, monitor_info.Monitor.Height, /*SWP_NOZORDER | SWP_NOACTIVATE | SWP_FRAMECHANGED*/0x0034);
				    	Assert(result);
					}else 
					{
					    // Reset original window style and size.  The multiple window size/moves
					    // here are ugly, but if SetWindowPos() doesn't redraw, the taskbar won't be
					    // repainted.  Better-looking methods welcome.
					    SetWindowLong(Handle, /*GWL_STYLE*/-16, SavedStateStyle);
					    SetWindowLong(Handle, /*GWL_EXSTYLE*/-20, SavedStateExStyle);
						
				    	// On restore, resize to the previous saved rect size.
				    	bool result = SetWindowPos(Handle, IntPtr.Zero, SavedStateBounds.X, SavedStateBounds.Y, SavedStateBounds.Width, SavedStateBounds.Height, /*SWP_NOZORDER | SWP_NOACTIVATE | SWP_FRAMECHANGED*/0x0034);
				    	Assert(result);
				    	
					    if(SavedStateMaximized)
					    {
					      	SendMessage(Handle, /*WM_SYSCOMMAND*/0x0112, /*SC_MAXIMIZE*/0xF030, IntPtr.Zero);
					    }else
					    {
					    	//for some reason this is necessary or the window cannot be interacted with
					    	SendMessage(Handle, /*WM_SYSCOMMAND*/0x0112, /*SC_RESTORE*/0xF120, IntPtr.Zero);
					    }
					}
				}else
				{
					BaseFullscreen = value;
				}
			}
		}
		
		/// <summary>
		/// Creates a new <see cref="Window"/> with a size of 100x100.
		/// </summary>
		public Window() : this(100, 100)
		{
			
		}
		
		/// <summary>
		/// Creates a new <see cref="Window"/> with the given size.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		public Window(int width, int height) : this(width, height, typeof(DoubleBufferStrategy))
		{
			
		}
		
		/// <summary>
		/// Creates a new <see cref="Window"/> with the given size and frame buffer type.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="frameBufferType">The frame buffer type class.</param>
		public Window(int width, int height, Type frameBufferType)
		{
			InputStates = new InputState(this);
			Width = width;
			Height = height;
			if(!frameBufferType.IsSubclassOf(typeof(FrameBufferStrategy)) || frameBufferType.GetConstructor(Type.EmptyTypes) == null)
			{
				throw new Exception("[Window] Frame buffer type must be a subclass of FrameBuffer and have a default constructor");
			}
			BufferType = frameBufferType;
			ToCenter();
			OnResize += ((sender, args) => Buffer.Resize(args.Size.X, args.Size.Y));
			OnMouseEnter += (sender, e) => OldCursor = SetCursor(STANDARD_CURSOR);
			OnMouseExit += (sender, e) => SetCursor(OldCursor);
		}
		
		~Window()
		{
			UDispose();
		}
		
		/// <summary>
		/// Centers the <see cref="Window"/> on the primary screen.
		/// </summary>
		public void ToCenter()
		{
			Rectangle rect = (Rectangle)Screen.PrimaryScreen.Bounds;
			Position = rect.Position + ((rect.Size - Size) / 2);
		}
		
		/// <summary>
		/// Updates the bounds of the window.
		/// </summary>
		public void UpdateBounds()
		{
			//we we've start we need to update window.
			//otherwise it will get picked up automatically
			if(Started)
			{
				Rectangle dimension = GetWindowDimensions();
				bool success = MoveWindow(Handle, dimension.X, dimension.Y, dimension.Width, dimension.Height, false);
				Assert(success);
			}
		}
		
		/// <summary>
		/// Returns the dimensions of the window for the current client area size.
		/// </summary>
		/// <returns></returns>
		private Rectangle GetWindowDimensions()
		{
			Rectangle dimension = Bounds;
			bool success = AdjustWindowRectEx(out dimension, DEFAULT_WINDOW_STYLE, false, 0);
			Assert(success);
			return dimension;
		}
		
		public void Dispose()
		{
			UDispose();
			GC.SuppressFinalize(this);
		}
		
		private void UDispose()
		{
			if(!Disposed)
			{
				if(Started)
				{
					DestroyHandle();
				}
				Disposed = true;
			}
		}
		
		/// <summary>
		/// Returns the <see cref="Image"/> to render to this frame.
		/// </summary>
		/// <returns>The render target.</returns>
		public FrameBuffer GetRenderBuffer()
		{
			return Buffer != null ? Buffer.GetRenderFrame() : null;
		}
		
		/// <summary>
		/// Starts this Window. (shows and begins event handling)
		/// </summary>
		public void Start()
		{
			if(Started)
			{
				throw new Exception("Window already started!");
			}
			
			//specify the creation parameters
			CreateParams paras = new CreateParams();
			paras.Caption = "";
			paras.ClassStyle = 0x0003;//CS_HREDRAW | CS_VREDRAW
			paras.Style = DEFAULT_WINDOW_STYLE;
			Rectangle dimension = GetWindowDimensions();
			paras.X = dimension.X;
			paras.Y = dimension.Y;
			paras.Width = dimension.Width;
			paras.Height = dimension.Height;
			
			//create buffer if null
			Buffer = (FrameBufferStrategy)BufferType.GetConstructor(Type.EmptyTypes).Invoke(Type.EmptyTypes);
			
			//start message loop
			MessageLoopObj = new MessageLoop();
			MessageLoopObj.Start<object>(() => {CreateHandle(paras); return null;});
			
			Started = true;
			
			//if pending fullscreen, actually perform action
			if(Fullscreen)
			{
				Fullscreen = true;
			}
			
			ShowWindow(Handle, 1);//WINDOW_SHOW_NORMAL
		}
		
		/// <summary>
		/// Kills this <see cref="Window"/>.
		/// </summary>
		public void Stop()
		{
			MessageLoopObj.Stop();
			Dispose();
		}
		
		/// <summary>
		/// Makes this <see cref="Window"/> exit on a OnClose event.
		/// </summary>
		public void ExitOnClose()
		{
			OnClose += (sender, args) => Stop();
		}
		
		/// <summary>
		/// Sets the title of this <see cref="Window"/>.
		/// </summary>
		/// <param name="title">The title string.</param>
		public void SetTitle(string title)
		{
			if(Started)
			{
				bool success = SetWindowText(Handle, title);
				Assert(success);
			}
		}
		
		/// <summary>
		/// Refreshs the screen.
		/// </summary>
		public void Refresh()
		{
			if(Started)
			{
				bool success = RedrawWindow(Handle, IntPtr.Zero, IntPtr.Zero, 0x0001);//RDW_INVALIDATE
				Assert(success);
			}
		}
		
		protected override void OnHandleChange()
        {
            Handle = base.Handle;
        }
		
		protected override void WndProc(ref Message m)
        {
			try
			{
				int code = m.Msg;
				DualParameter wParam = (DualParameter)m.WParam;
				DualParameter lParam = (DualParameter)m.LParam;
	            switch (m.Msg)
	            {
	                case WM_CLOSE:
	            	{
	            		if(OnClose != null) OnClose(this, EventArgs.Empty);
	            		return;
	            	}
	            	case WM_SIZE:
	            	{
	            		Size = new Point2D(lParam.LowWord, lParam.HighWord);
	            		if(OnResize != null) OnResize(this, new ResizeEventArgs(Size));
	            		break;
	            	}
	            	case WM_PAINT:
	            	{	
	            		PaintData data;
	            		IntPtr screenDC = BeginPaint(Handle, out data);
	            		Assert(screenDC != IntPtr.Zero);
	            		
	            		//blit display frame to screen
	            		DIBImage frame = Buffer.GetDisplayFrame();
	            		bool success = BitBlt(screenDC, 0, 0, frame.Width, frame.Height, frame.Section.DC.Handle, 0, 0, SRCCOPY);
	            		Assert(success);
	            		
	            		EndPaint(Handle, ref data);
	            		return;
	            	}
	            	case WM_SETFOCUS:
	        		{
	            		IsFocused = true;
	            		break;
	        		}
	            	case WM_KILLFOCUS:
	        		{
	        			IsFocused = false;
	        			break;
	        		}
	            	case WM_MOUSEMOVE:
					{
	            		//get move info
	            		Point2D mouseCoords = (Point2D)lParam;
						Point2D delta = mouseCoords - PrevMouseCoords;
						//if mouse was previously outside window
						if(!IsMouseInWindow)
						{
							IsMouseInWindow = true;
							if(OnMouseEnter != null) OnMouseEnter(this, EventArgs.Empty);
							//suppress inaccurate delta value
							delta = new Point2D(0, 0);
							//start tracking mouse so we recieve WM_MOUSELEAVE
							TRACKMOUSEEVENT eventTrack = new TRACKMOUSEEVENT(Handle);
							bool success = TrackMouseEvent(ref eventTrack);
							Assert(success);
						}
			    		
						//call event
						if(OnMouseMove != null)
						{
							MouseMoveEventArgs args = new MouseMoveEventArgs(mouseCoords, delta);
							OnMouseMove(this, args);
							PrevMouseCoords = mouseCoords;
							//consume if told
							if(args.Consumed) return;
						}else
						{
							PrevMouseCoords = mouseCoords;
						}
	            		break;
	            	}
	            	case WM_MOUSELEAVE:
	        		{
	            		IsMouseInWindow = false;
	            		if(OnMouseExit != null) OnMouseExit(this, EventArgs.Empty);
	            		break;
	        		}
	            	case WM_LBUTTONDOWN:
	        		{
	            		if(OnMousePress != null) 
	            		{
		            		MouseButtonEventArgs args = new MouseButtonEventArgs((Point2D)lParam, MouseButton.LEFT, true);
		            		OnMousePress(this, args);
		            		//consume if told
							if(args.Consumed) return;
	            		}
	            		break;
	        		}
	            	case WM_LBUTTONUP:
	        		{
	            		if(OnMouseRelease != null) 
	            		{
		            		MouseButtonEventArgs args = new MouseButtonEventArgs((Point2D)lParam, MouseButton.LEFT, false);
		            		OnMouseRelease(this, args);
		            		//consume if told
							if(args.Consumed) return;
	            		}
	            		break;
	        		}
	            	case WM_RBUTTONDOWN:
	        		{
	            		if(OnMousePress != null) 
	            		{
		            		MouseButtonEventArgs args = new MouseButtonEventArgs((Point2D)lParam, MouseButton.RIGHT, true);
		            		OnMousePress(this, args);
		            		//consume if told
							if(args.Consumed) return;
	            		}
	            		break;
	        		}
	            	case WM_RBUTTONUP:
	        		{
	            		if(OnMouseRelease != null) 
	            		{
		            		MouseButtonEventArgs args = new MouseButtonEventArgs((Point2D)lParam, MouseButton.RIGHT, false);
		            		OnMouseRelease(this, args);
		            		//consume if told
							if(args.Consumed) return;
	            		}
	            		break;
	        		}
	            	case WM_MBUTTONDOWN:
	        		{
	            		if(OnMousePress != null) 
	            		{
		            		MouseButtonEventArgs args = new MouseButtonEventArgs((Point2D)lParam, MouseButton.MIDDLE, true);
		            		OnMousePress(this, args);
		            		//consume if told
							if(args.Consumed) return;
	            		}
	            		break;
	        		}
	            	case WM_MBUTTONUP:
	        		{
	            		if(OnMouseRelease != null) 
	            		{
		            		MouseButtonEventArgs args = new MouseButtonEventArgs((Point2D)lParam, MouseButton.MIDDLE, false);
		            		OnMouseRelease(this, args);
		            		//consume if told
							if(args.Consumed) return;
	            		}
	            		break;
	        		}
	            	case WM_XBUTTONDOWN:
	        		{
	            		if(OnMousePress != null) 
	            		{
		            		MouseButton button = (wParam.HighWord == 0x0001 ? MouseButton.EXTRA_1 : MouseButton.EXTRA_2);
		            		MouseButtonEventArgs args = new MouseButtonEventArgs((Point2D)lParam, button, true);
		            		OnMousePress(this, args);
		            		//consume if told
							if(args.Consumed) return;
	            		}
	            		break;
	        		}
	            	case WM_XBUTTONUP:
	        		{
	            		if(OnMouseRelease != null) 
	            		{
		            		MouseButton button = (wParam.HighWord == 0x0001 ? MouseButton.EXTRA_1 : MouseButton.EXTRA_2);
		            		MouseButtonEventArgs args = new MouseButtonEventArgs((Point2D)lParam, button, false);
		            		OnMouseRelease(this, args);
		            		//consume if told
							if(args.Consumed) return;
	            		}
	            		break;
	        		}
	            	case WM_MOUSEWHEEL:
	        		{
	            		if(OnMouseWheel != null) 
	            		{
		            		MouseWheelEventArgs args = new MouseWheelEventArgs((Point2D)lParam, (wParam.HighWord / WHEEL_DELTA));
		            		
		            		//fix coords because for whatever reason wheel coords are screen coords not window like all the rest of the events
		            		Point2D windowCoords = new Point2D(0, 0);
		            		ClientToScreen(Handle, out windowCoords);
		            		args.Coords -= windowCoords;
		            		
		            		OnMouseWheel(this, args);
		            		//consume if told
							if(args.Consumed) return;
	            		}
	            		break;
	        		}
	            	case WM_KEYDOWN:
	            	case WM_SYSKEYDOWN:
	        		{
	            		if(OnKeyPress != null) 
	            		{
		            		//only post event if not a repeat
		            		if(((lParam.Number >> 30) & 1) == 0)
		            		{
		            			KeyEventArgs args = new KeyEventArgs((KeyboardButton)wParam.Number, true);
		            			OnKeyPress(this, args);
		            			//consume if told
								if(args.Consumed) return;
		            		}
	            		}
	            		break;
	        		}
	            	case WM_KEYUP:
	            	case WM_SYSKEYUP:
	        		{
	            		if(OnKeyRelease != null) 
	            		{
		            		//only post event if not a repeat
		            		if(((lParam.Number >> 30) & 1) == 1)
		            		{
		            			KeyEventArgs args = new KeyEventArgs((KeyboardButton)wParam.Number, false);
		            			OnKeyRelease(this, args);
		            			//consume if told
								if(args.Consumed) return;
		            		}
	            		}
	            		break;
	        		}
	            	case WM_CHAR:
	        		{
	            		if(OnCharTyped != null) 
	            		{
		            		CharEventArgs args = new CharEventArgs((char)wParam.Number, ((lParam.Number >> 30) & 1) == 1);
		            		//ok if char events are repeats
		            		OnCharTyped(this, args);
		            		//consume if told
							if(args.Consumed) return;
	            		}
	            		break;
	        		}
	            }
			}catch(Exception ex)
			{
				Console.WriteLine(ex);
			}
            base.WndProc(ref m);
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
		
		//winapi constants
		private const int WM_CLOSE = 0x0010;
		private const int WM_SIZE = 0x0005;
		private const int WM_PAINT = 0x000F;
		private const int WM_SETFOCUS = 0x0007;
		private const int WM_KILLFOCUS = 0x0008;
		
		private const int WM_MOUSEMOVE = 0x0200;
		private const int WM_MOUSEWHEEL = 0x020A;
		
		private const int WM_LBUTTONDOWN = 0x0201;
		private const int WM_LBUTTONUP = 0x0202;
		private const int WM_RBUTTONDOWN = 0x0204;
		private const int WM_RBUTTONUP = 0x0205;
		private const int WM_MBUTTONDOWN = 0x0207;
		private const int WM_MBUTTONUP = 0x0208;
		private const int WM_XBUTTONDOWN = 0x020B;
		private const int WM_XBUTTONUP = 0x020C;
		
		private const int WM_MOUSELEAVE = 0x02A3;
		
		private const int WM_KEYDOWN  = 0x100;
		private const int WM_KEYUP = 0x101;
		private const int WM_SYSKEYUP = 0x105;
		private const int WM_SYSKEYDOWN = 0x104;
		private const int WM_CHAR = 0x0102;
		
		private const int WHEEL_DELTA = 120;
		
		private const int SRCCOPY = 0xCC0020;
		
		//winapi methods
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool IsZoomed(IntPtr window);
		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr window, uint message, uint wParam, IntPtr lParam);
		[DllImport("user32.dll", SetLastError=true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetWindowRect(IntPtr window, out Rectangle rect);
		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint GetWindowLong(IntPtr window, int index);
		[DllImport("user32.dll")]
		private static extern uint SetWindowLong(IntPtr window, int index, uint value);
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetMonitorInfo(IntPtr monitor, ref MONITORINFO info);
		[DllImport("user32.dll")]
		private static extern IntPtr MonitorFromWindow(IntPtr window, uint flags);
		[DllImport("user32.dll", SetLastError=true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetWindowPos(IntPtr window, IntPtr windowInsertAfter, int x, int y, int cx, int cy, uint flags);
		[DllImport("user32.dll")]
		private static extern IntPtr LoadCursor(IntPtr file, int name);
		[DllImport("user32.dll")]
    	private static extern IntPtr SetCursor(IntPtr cursor);
		
		[DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr window, int showCommands);
		[DllImport("user32.dll", SetLastError=true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool AdjustWindowRectEx(out Rectangle window, int style, bool menu, int exStyle);
		[DllImport("user32.dll", SetLastError=true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool MoveWindow(IntPtr window, int x, int y, int width, int height, bool repaint);
		[DllImport("user32.dll", SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ClientToScreen(IntPtr window, out Point2D coords);
		[DllImport("user32.dll", SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowText(IntPtr window, string title);
		[DllImport("user32.dll", SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool RedrawWindow(IntPtr window, IntPtr rect, IntPtr region, uint flags);
        [DllImport("user32.dll", SetLastError=true)]
        private static extern IntPtr BeginPaint(IntPtr window, out PaintData lpPaint);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EndPaint(IntPtr window, [In] ref PaintData lpPaint);
        [DllImport("gdi32.dll", SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool BitBlt(IntPtr dest, int destX, int destY, int width, int height, IntPtr src, int srcX, int srcY, int operationCode);
        [DllImport("user32.dll")]
        private static extern IntPtr SetCapture(IntPtr window);
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ReleaseCapture();
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool TrackMouseEvent(ref TRACKMOUSEEVENT eventTrack);
        
        //winapi structs
		[StructLayout(LayoutKind.Sequential)]
		private struct MONITORINFO
		{
		    public int Size;
		    public Rectangle Monitor;
		    public Rectangle Work;
		    public uint Flags;
		}
		
		[StructLayout(LayoutKind.Explicit)]
	    private struct DualParameter
	    {
	        [FieldOffset(0)]
	        public int Number;
	
	        [FieldOffset(0)]
	        public short LowWord;
	
	        [FieldOffset(2)]
	        public short HighWord;
	        
	        public static explicit operator DualParameter(IntPtr pointer)
	        {
	        	return new DualParameter(){Number = pointer.ToInt32()};
	        }
	        
	        public static explicit operator Point2D(DualParameter param)
	        {
	        	return new Point2D(param.LowWord, param.HighWord);
	        }
	    }
	    
		[StructLayout(LayoutKind.Sequential)]
	    private struct PaintData
	    {
	        public IntPtr hdc;
	        public bool fErase;
	        public Point2D Min;
	        public Point2D Max;
	        public bool fRestore;
	        public bool fIncUpdate;
	        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	        public byte[] rgbReserved;
	    }
	    
	    [StructLayout(LayoutKind.Sequential)]
	    private struct TRACKMOUSEEVENT
	    {
	    	public int Size;
	    	public int Flags;
	    	public IntPtr WindowObj;
	    	public int HoverTime;
	    	
	    	public TRACKMOUSEEVENT(IntPtr window)
	    	{
	    		Size = sizeof(TRACKMOUSEEVENT);
	    		Flags = 0x00000002;//TIME_LEAVE
	    		WindowObj = window;
	    		HoverTime = 0;
	    	}
	    }
	}
}
