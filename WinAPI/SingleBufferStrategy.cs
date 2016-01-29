namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// A buffer strategy for rendering with one buffer.
	/// </summary>
	public class SingleBufferStrategy : RenderBufferStrategy
	{
		private readonly RenderBuffer Buffer;
		
		/// <summary>
		/// Creates a new <see cref="SingleBufferStrategy"/>.
		/// </summary>
		public SingleBufferStrategy()
		{
			Buffer = new RenderBuffer();
		}
		
		public override RenderBuffer GetDisplayBuffer()
		{
			return Buffer;
		}
		
		public override RenderBuffer GetRenderBuffer()
		{
			//always match buffer bounds to current window bounds
			Buffer.Image.Resize(Width, Height);
			return Buffer;
		}
		
		public override RenderBuffer[] GetBuffers()
		{
			return new []{Buffer};
		}
	}
}
