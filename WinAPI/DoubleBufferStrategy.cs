namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// A buffer strategy for a Frame.
	/// </summary>
	public class DoubleBufferStrategy : FrameBufferStrategy
	{
		private readonly DIBImage Screen;
		private FrameBuffer FrontBuffer;
		private FrameBuffer BackBuffer;
		
		/// <summary>
		/// Creates a new <see cref="DoubleBufferStrategy"/>.
		/// </summary>
		public DoubleBufferStrategy()
		{
			Screen = new DIBImage(1, 1);
			FrontBuffer = new FrameBuffer();
			BackBuffer = new FrameBuffer();
		}
		
		/// <summary>
		/// Gets the current frame to be displayed.
		/// </summary>
		/// <returns>The display frame.</returns>
		public override DIBImage GetDisplayFrame()
		{
			//always match screen bounds to current window bounds
			Screen.Resize(Width, Height);
			lock(this) Screen.Copy(FrontBuffer.Image);
			return Screen;
		}
		
		/// <summary>
		/// Gets the current frame for rendering.
		/// </summary>
		/// <returns>The render frame.</returns>
		public override FrameBuffer GetRenderFrame()
		{
			lock(this) Util.Swap(ref BackBuffer, ref FrontBuffer);
			//always match buffer bounds to current window bounds
			BackBuffer.Image.Resize(Width, Height);
			return BackBuffer;
		}
	}
}
