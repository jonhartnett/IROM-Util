namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// A render buffer for a Frame.
	/// </summary>
	public class FrameBuffer
	{
		//the actual buffer image
		public Image Image;
		//id of the last frame rendered
		public ulong LastFrameId;
		
		public FrameBuffer()
		{
			Image = new Image(1, 1);
			LastFrameId = 0;
		}
	}
	
	/// <summary>
	/// A buffer strategy for a Frame.
	/// </summary>
	public abstract class FrameBufferStrategy
	{
		protected int Width = 1;
		protected int Height = 1;

		/// <summary>
		/// Resizes this FrameBuffer.
		/// </summary>
		public void Resize(int w, int h)
		{
			Width = w;
			Height = h;
		}
		
		/// <summary>
		/// Gets the current frame to be displayed.
		/// </summary>
		/// <returns>The display frame.</returns>
		public abstract DIBImage GetDisplayFrame();
		
		/// <summary>
		/// Gets the current frame for rendering.
		/// </summary>
		/// <returns>The render frame.</returns>
		public abstract FrameBuffer GetRenderFrame();
	}
}
