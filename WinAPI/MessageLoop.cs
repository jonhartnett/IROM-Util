namespace IROM.Util
{
	using System;
	using System.Windows.Forms;
	using System.Runtime.InteropServices;
	using System.ComponentModel;
	using System.Threading;
	
	/// <summary>
	/// A class for quickly creating unmanaged message loops.
	/// </summary>
	public class MessageLoop
	{
		/// <summary>
		/// The last ID used.
		/// </summary>
		private static int LastID = -1;
		
		/// <summary>
		/// The unique ID of this <see cref="MessageLoop"/>.
		/// </summary>
		public int ID;
		
		/// <summary>
		/// The <see cref="Thread"/> that drives this <see cref="MessageLoop"/>
		/// </summary>
		public Thread MessageThread;
		
		/// <summary>
		/// A delegate for performing thread-specific initialization.
		/// </summary>
		public delegate T InitLoop<T>();
		
		/// <summary>
		/// Starts this <see cref="MessageLoop"/>, performing the required initialization delegate.
		/// </summary>
		/// <returns>The result of the initialization.</returns>
		public T Start<T>(InitLoop<T> initDel)
		{
			//lock to wait for init completion
			object waitLock = new object();
			T result = default(T);
			ID = Interlocked.Add(ref LastID, 1);
			MessageThread = new Thread(() => MessageLoopFunc(initDel, waitLock, ref result));
			MessageThread.Name = "UtilLib Message Thread " + ID;
            MessageThread.IsBackground = true;
            MessageThread.Priority = ThreadPriority.Highest;
			lock(waitLock)
            {
            	//starting inside lock ensures it doesn't finish initing before we get to the wait.
            	MessageThread.Start();
            	Monitor.Wait(waitLock);
            }
			return result;
		}
		
		/// <summary>
		/// Exit's this <see cref="MessageLoop"/>.
		/// </summary>
		public void Stop()
		{
			MessageThread.Abort();
		}
		
		private static void MessageLoopFunc<T>(InitLoop<T> initDel, object waitLock, ref T result)
		{
			//wrap entire task in a try-catch to ensure errors are reported
            try
            {
            	result = initDel();
            	//signal to the waiting thread
            	lock(waitLock)
            	{
            		Monitor.Pulse(waitLock);
            	}
            	
            	//enter eternal message loop
				Message message;
	            while(true)
	            {
            		//get the next message
	            	if(GetMessage(out message))
	            	{
            			//process
            			ProcessMessage(message);
	            	}else
	            	{
	            		break;
	            	}
	            }
            }catch(ThreadAbortException)
            {
            	//do nothing, simple means we we're exited.
            }catch(Exception ex)
            {
                //report exception
                Console.WriteLine(ex);
                //kill program
                //Environment.Exit(1);
            }
		}
		
		/// <summary>
		/// Gets the next message for this thread.
		/// </summary>
		/// <param name="message">The message output.</param>
		/// <returns>False if no more messages.</returns>
		private static bool GetMessage(out Message message)
		{
			sbyte code = GetMessage(out message, IntPtr.Zero, 0, 0);
			Assert(code != -1);
			return code != 0;
		}
		
		/// <summary>
		/// Processes the given message.
		/// </summary>
		/// <param name="message">The message.</param>
		private static void ProcessMessage(Message message)
		{
			TranslateMessage(ref message);
			DispatchMessage(ref message);
		}
		
		/// <summary>
		/// Throws a <see cref="Win32Exception"/> if the given condition is not true.
		/// </summary>
		/// <param name="test">The condition.</param>
		private static void Assert(bool test)
		{
			if(!test)
			{
				throw new Win32Exception();
			}
		}
		
		[DllImport("user32.dll", SetLastError = true)]
        private static extern sbyte GetMessage(out Message message, IntPtr window, uint wMsgFilterMin, uint wMsgFilterMax);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool TranslateMessage([In] ref Message message);
        [DllImport("user32.dll")]
        private static extern IntPtr DispatchMessage([In] ref Message message);
	}
}
