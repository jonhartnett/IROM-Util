namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// A buffer strategy for rendering with three buffers.
	/// </summary>
	public class TripleBufferStrategy : RenderBufferStrategy
	{
		private RenderBuffer FrontBuffer;
		private RenderBuffer MiddleBuffer;
		private RenderBuffer BackBuffer;
		
		/// <summary>
		/// Creates a new <see cref="TripleBufferStrategy"/>.
		/// </summary>
		public TripleBufferStrategy()
		{
			FrontBuffer = new RenderBuffer();
			MiddleBuffer = new RenderBuffer();
			BackBuffer = new RenderBuffer();
		}
		
		public override RenderBuffer GetDisplayBuffer()
		{
			FrontBuffer = System.Threading.Interlocked.Exchange(ref MiddleBuffer, FrontBuffer);
			return FrontBuffer;
		}
		
		public override RenderBuffer GetRenderBuffer()
		{
			BackBuffer = System.Threading.Interlocked.Exchange(ref MiddleBuffer, BackBuffer);
			//always match buffer bounds to current window bounds
			BackBuffer.Image.Resize(Width, Height);
			return BackBuffer;
		}
		
		public override RenderBuffer[] GetBuffers()
		{
			return new []{FrontBuffer, MiddleBuffer, BackBuffer};
		}
	}
}
