namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// A buffer strategy for a Frame.
	/// </summary>
	public class FrameTripleBuffer : FrameBuffer
	{
		private readonly DIBImage Screen;
		private Image FrontBuffer;
		private Image MiddleBuffer;
		private Image BackBuffer;
		
		/// <summary>
		/// Creates a new <see cref="FrameTripleBuffer"/>.
		/// </summary>
		public FrameTripleBuffer()
		{
			Screen = new DIBImage(1, 1);
			FrontBuffer = new Image(1, 1);
			MiddleBuffer = new Image(1, 1);
			BackBuffer = new Image(1, 1);
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
			Screen.Copy(FrontBuffer);
			return Screen;
		}
		
		/// <summary>
		/// Gets the current frame for rendering.
		/// </summary>
		/// <returns>The render frame.</returns>
		public override Image GetRenderFrame()
		{
			lock(this) Util.Swap(ref BackBuffer, ref MiddleBuffer);
			//always match buffer bounds to current window bounds
			if(BackBuffer.Width != Width || BackBuffer.Height != Height)
			{
				BackBuffer.Resize(Width, Height);
			}
			return BackBuffer;
		}
	}
}
