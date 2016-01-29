namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// A buffer strategy for rendering with two buffers.
	/// </summary>
	public class DoubleBufferStrategy : RenderBufferStrategy
	{
		private RenderBuffer FrontBuffer;
		private RenderBuffer BackBuffer;
		
		/// <summary>
		/// Creates a new <see cref="DoubleBufferStrategy"/>.
		/// </summary>
		public DoubleBufferStrategy()
		{
			FrontBuffer = new RenderBuffer();
			BackBuffer = new RenderBuffer();
		}
		
		public override RenderBuffer GetDisplayBuffer()
		{
			return FrontBuffer;
		}
		
		public override RenderBuffer GetRenderBuffer()
		{
			BackBuffer = System.Threading.Interlocked.Exchange(ref FrontBuffer, BackBuffer);
			//always match buffer bounds to current window bounds
			BackBuffer.Image.Resize(Width, Height);
			return BackBuffer;
		}
		
		public override RenderBuffer[] GetBuffers()
		{
			return new []{FrontBuffer, BackBuffer};
		}
	}
}
