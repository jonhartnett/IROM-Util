namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// A buffer strategy for a Frame.
	/// </summary>
	public class TripleBufferStrategy : FrameBufferStrategy
	{
		private readonly DIBImage Screen;
		private FrameBuffer FrontBuffer;
		private FrameBuffer MiddleBuffer;
		private FrameBuffer BackBuffer;
		
		/// <summary>
		/// Creates a new <see cref="TripleBufferStrategy"/>.
		/// </summary>
		public TripleBufferStrategy()
		{
			Screen = new DIBImage(1, 1);
			FrontBuffer = new FrameBuffer();
			MiddleBuffer = new FrameBuffer();
			BackBuffer = new FrameBuffer();
		}
		
		/// <summary>
		/// Gets the current frame to be displayed.
		/// </summary>
		/// <returns>The display frame.</returns>
		public override DIBImage GetDisplayFrame()
		{
			lock(this) Util.Swap(ref FrontBuffer, ref MiddleBuffer);
			//always match screen bounds to current window bounds
			if(Screen.Width != Width || Screen.Height != Height)
			{
				Screen.Resize(Width, Height);
			}
			Screen.Copy(FrontBuffer.Image);
			return Screen;
		}
		
		/// <summary>
		/// Gets the current frame for rendering.
		/// </summary>
		/// <returns>The render frame.</returns>
		public override FrameBuffer GetRenderFrame()
		{
			lock(this) Util.Swap(ref BackBuffer, ref MiddleBuffer);
			//always match buffer bounds to current window bounds
			BackBuffer.Image.Resize(Width, Height);
			return BackBuffer;
		}
	}
}
