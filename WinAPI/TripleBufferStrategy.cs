namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// A buffer strategy for a Frame.
	/// </summary>
	public class TripleBufferStrategy : FrameBufferStrategy
	{
		private FrameBuffer FrontBuffer;
		private FrameBuffer MiddleBuffer;
		private FrameBuffer BackBuffer;
		
		/// <summary>
		/// Creates a new <see cref="TripleBufferStrategy"/>.
		/// </summary>
		public TripleBufferStrategy()
		{
			FrontBuffer = new FrameBuffer();
			MiddleBuffer = new FrameBuffer();
			BackBuffer = new FrameBuffer();
		}
		
		public override FrameBuffer GetDisplayFrame()
		{
			lock(this) Util.Swap(ref FrontBuffer, ref MiddleBuffer);
			//always match screen bounds to current window bounds
			FrontBuffer.Image.Resize(Width, Height);
			return FrontBuffer;
		}
		
		public override FrameBuffer GetRenderFrame()
		{
			lock(this) Util.Swap(ref BackBuffer, ref MiddleBuffer);
			//always match buffer bounds to current window bounds
			BackBuffer.Image.Resize(Width, Height);
			return BackBuffer;
		}
		
		public override FrameBuffer[] GetBuffers()
		{
			return new []{FrontBuffer, MiddleBuffer, BackBuffer};
		}
	}
}
