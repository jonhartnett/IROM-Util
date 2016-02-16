namespace IROM.Util
{
	using System;
	using System.Collections.Generic;
	
	/// <summary>
	/// Scans shapes for rendering.
	/// </summary>
	public class Scanner
	{
		private static readonly FastLinkedList<Scanner> scanners = new FastLinkedList<Scanner>();
		
		/// <summary>
		/// Returns a scanner for the given height.
		/// </summary>
		/// <param name="height">The height of the target.</param>
		/// <returns>A scanner obj.</returns>
		public static Scanner Get(int height)
		{
			lock(scanners)
			{
				foreach(var node in scanners.GetNodes())
				{
					if(node.Value.Length >= height)
					{
						scanners.Remove(node);
						return node.Value;
					}
				}
			}
			Scanner result = scanners.Pop();
			if(result == null) result = new Scanner();
			result.EnsureSize(height);
			return result;
		}
		
		/// <summary>
		/// Returns a scanner to the pool after it's done being used.
		/// </summary>
		/// <param name="scanner">The scanner to return.</param>
		public static void Put(Scanner scanner)
		{
			scanners.AddSorted(scanner, (a, b) => a.Length - b.Length);
		}
		
		public struct Scan
		{
			public float min;
			public float max;
		}
		
		/// <summary>
		/// The scan lines in the shape.
		/// </summary>
		private Scan[] scans;
		
		/// <summary>
		/// The minimum y scan index.
		/// </summary>
		public int yMin;
		
		/// <summary>
		/// The maximum y scan index.
		/// </summary>
		public int yMax;
		
		public bool isYMinClipped = false;
		public bool isYMaxClipped = false;
		
		/// <summary>
		/// Returns the scan at the given level.
		/// </summary>
		public Scan this[int y]
		{
			get
			{
				return scans[y+1];
			}
			set
			{
				scans[y+1] = value;
			}
		}
		
		/// <summary>
		/// Returns the length.
		/// </summary>
		public int Length
		{
			get{return scans.Length - 2;}
		}
		
		private Scanner(){}
		
		/// <summary>
		/// Ensures that the Scanner has sufficient size for the given height.
		/// </summary>
		/// <param name="height">The map height.</param>
		public void EnsureSize(int height)
		{
			if(scans == null || scans.Length < height + 2)
			{
				scans = new Scan[height+ 2];
			}
		}
	}
}
