namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// A buffer strategy for a Frame.
	/// </summary>
	public class DoubleBufferStrategy : FrameBufferStrategy
	{
		private FrameBuffer FrontBuffer;
		private FrameBuffer BackBuffer;
		
		/// <summary>
		/// Creates a new <see cref="DoubleBufferStrategy"/>.
		/// </summary>
		public DoubleBufferStrategy()
		{
			FrontBuffer = new FrameBuffer();
			BackBuffer = new FrameBuffer();
		}
		
		public override FrameBuffer GetDisplayFrame()
		{
			//always match buffer bounds to current window bounds
			Util.Swap(ref BackBuffer, ref FrontBuffer);
			FrontBuffer.Image.Resize(Width, Height);
			return FrontBuffer;
		}
		
		public override FrameBuffer GetRenderFrame()
		{
			return BackBuffer;
		}
		
		public override FrameBuffer[] GetBuffers()
		{
			return new []{FrontBuffer, BackBuffer};
		}
	}
}
