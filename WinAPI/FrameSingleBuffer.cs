namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// A buffer strategy for a Frame.
	/// </summary>
	public class FrameSingleBuffer : FrameBuffer
	{
		private readonly DIBImage Screen;
		private readonly Image Buffer;
		
		/// <summary>
		/// Creates a new <see cref="FrameTripleBuffer"/>.
		/// </summary>
		public FrameSingleBuffer()
		{
			Screen = new DIBImage(1, 1);
			Buffer = new Image(1, 1);
		}
		
		/// <summary>
		/// Gets the current frame to be displayed.
		/// </summary>
		/// <returns>The display frame.</returns>
		public DIBImage GetDisplayFrame()
		{
			lock(this) return Screen;
		}
		
		/// <summary>
		/// Gets the current frame for rendering.
		/// </summary>
		/// <returns>The render frame.</returns>
		public Image GetRenderFrame()
		{
			//always match buffer bounds to current window bounds
			if(Buffer.Width != Width || Buffer.Height != Height)
			{
				Buffer.Resize(Width, Height);
			}
			lock(this) 
			{
				//always match screen bounds to current window bounds
				if(Screen.Width != Width || Screen.Height != Height)
				{
					Screen.Resize(Width, Height);
				}
				Screen.Copy(Buffer);
			}
			return Buffer;
		}
	}
}
