namespace IROM.Util
{
	using System;
	using System.Collections.Generic;
	
	/// <summary>
	/// A render buffer for rendering.
	/// </summary>
	public class RenderBuffer
	{
		//the actual buffer image
		public Image Image;
		//id of the last frame rendered
		public ulong LastFrameId;
		
		public RenderBuffer()
		{
			Image = new Image(1, 1, true);
			LastFrameId = 0;
		}
	}
	
	/// <summary>
	/// A buffer strategy for rendering.
	/// </summary>
	public abstract class RenderBufferStrategy
	{
		protected int Width = 1;
		protected int Height = 1;

		/// <summary>
		/// Resizes this <see cref="RenderBufferStrategy"/>.
		/// </summary>
		public void Resize(int w, int h)
		{
			Width = w;
			Height = h;
		}
		
		/// <summary>
		/// Gets the current buffer to be displayed.
		/// </summary>
		/// <returns>The display buffer.</returns>
		public abstract RenderBuffer GetDisplayBuffer();
		
		/// <summary>
		/// Gets the current buffer for rendering.
		/// </summary>
		/// <returns>The render buffer.</returns>
		public abstract RenderBuffer GetRenderBuffer();
		
		/// <summary>
		/// Returns the buffers used by this strategy.
		/// </summary>
		public abstract RenderBuffer[] GetBuffers();
	}
}
