namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Interface for making a shape renderable.
	/// </summary>
	public interface IRenderableShape
	{
		/// <summary>
		/// Scans the shape with the given clip into the given scanner object.
		/// </summary>
		/// <param name="scanner">The scanner to scan into.</param>
		/// <param name="clip">The clip.</param>
		void Scan(Scanner scanner, Rectangle clip);
	}
}
