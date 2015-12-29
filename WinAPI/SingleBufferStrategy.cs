namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// A buffer strategy for a Frame.
	/// </summary>
	public class SingleBufferStrategy : FrameBufferStrategy
	{
		private readonly FrameBuffer Buffer;
		
		/// <summary>
		/// Creates a new <see cref="SingleBufferStrategy"/>.
		/// </summary>
		public SingleBufferStrategy()
		{
			Buffer = new FrameBuffer();
		}
		
		public override FrameBuffer GetDisplayFrame()
		{
			//always match buffer bounds to current window bounds
			Buffer.Image.Resize(Width, Height);
			return Buffer;
		}
		
		public override FrameBuffer GetRenderFrame()
		{
			return Buffer;
		}
		
		public override FrameBuffer[] GetBuffers()
		{
			return new []{Buffer};
		}
	}
}
