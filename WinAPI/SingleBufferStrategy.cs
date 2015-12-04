namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// A buffer strategy for a Frame.
	/// </summary>
	public class SingleBufferStrategy : FrameBufferStrategy
	{
		private readonly DIBImage Screen;
		private readonly FrameBuffer Buffer;
		
		/// <summary>
		/// Creates a new <see cref="SingleBufferStrategy"/>.
		/// </summary>
		public SingleBufferStrategy()
		{
			Screen = new DIBImage(1, 1);
			Buffer = new FrameBuffer();
		}
		
		/// <summary>
		/// Gets the current frame to be displayed.
		/// </summary>
		/// <returns>The display frame.</returns>
		public override DIBImage GetDisplayFrame()
		{
			return Screen;
		}
		
		/// <summary>
		/// Gets the current frame for rendering.
		/// </summary>
		/// <returns>The render frame.</returns>
		public override FrameBuffer GetRenderFrame()
		{
			//always match buffer bounds to current window bounds
			Buffer.Image.Resize(Width, Height);
			//always match screen bounds to current window bounds
			Screen.Resize(Width, Height);
			Screen.Copy(Buffer.Image);
			return Buffer;
		}
	}
}
