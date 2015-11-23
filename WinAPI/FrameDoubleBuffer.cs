namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// A buffer strategy for a Frame.
	/// </summary>
	public class FrameDoubleBuffer : FrameBuffer
	{
		private readonly DIBImage Screen;
		private Image FrontBuffer;
		private Image BackBuffer;
		
		/// <summary>
		/// Creates a new <see cref="FrameTripleBuffer"/>.
		/// </summary>
		public FrameDoubleBuffer()
		{
			Screen = new DIBImage(1, 1);
			FrontBuffer = new Image(1, 1);
			BackBuffer = new Image(1, 1);
		}
		
		/// <summary>
		/// Gets the current frame to be displayed.
		/// </summary>
		/// <returns>The display frame.</returns>
		public DIBImage GetDisplayFrame()
		{
			//always match screen bounds to current window bounds
			if(Screen.Width != Width || Screen.Height != Height)
			{
				Screen.Resize(Width, Height);
			}
			lock(this) Screen.Copy(FrontBuffer);
			return Screen;
		}
		
		/// <summary>
		/// Gets the current frame for rendering.
		/// </summary>
		/// <returns>The render frame.</returns>
		public Image GetRenderFrame()
		{
			lock(this) Util.Swap(ref BackBuffer, ref FrontBuffer);
			//always match buffer bounds to current window bounds
			if(BackBuffer.Width != Width || BackBuffer.Height != Height)
			{
				BackBuffer.Resize(Width, Height);
			}
			return BackBuffer;
		}
	}
}
