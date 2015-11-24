namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// A buffer strategy for a Frame.
	/// </summary>
	public abstract class FrameBuffer
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
		public abstract Image GetRenderFrame();
	}
}
