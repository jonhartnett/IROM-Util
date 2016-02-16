namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Simple int polygon struct.
	/// </summary>
	public struct Polygon : IRenderableShape
	{
		/// <summary>
		/// The <see cref="Polygon"/> verticies.
		/// </summary>
		public Vec2D[] Vertices;
		
		public Polygon(params Vec2D[] verts)
		{
			Vertices = verts;
		}
        
        public override string ToString()
		{
        	System.Text.StringBuilder builder = new System.Text.StringBuilder();
        	builder.Append("Polygon {");
        	foreach(var vert in Vertices)
        	{
        		builder.Append(string.Format("({0}, {1})", vert.X, vert.Y));
        	}
        	builder.Append("}");
        	return builder.ToString();
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is Polygon)
        	{
        		Polygon poly = (Polygon)obj;
        		return this == poly;
        	}else
        	{
        		return false;
        	}
        }
        
        public override int GetHashCode()
        {
        	// disable NonReadonlyReferencedInGetHashCode
        	uint[] values = new uint[Vertices.Length];
        	for(int i = 0; i < Vertices.Length; i++)
        	{
        		values[i] = (uint)Vertices[i].GetHashCode();
        	}
        	return (int)Hash.PerformStaticHash(values);
        }
        
        public void Scan(Scanner scanner, Rectangle clip)
		{
			scanner.yMin = int.MaxValue;
			scanner.yMax = int.MinValue;
			for(int i = 0; i < Vertices.Length; i++)
			{
				scanner.yMin = Math.Min(scanner.yMin, (int)Vertices[i].Y);
				scanner.yMax = Math.Max(scanner.yMax, (int)Vertices[i].Y);
			}
			//clip to clip, but with leeway
			if(scanner.yMin < clip.Min.Y - 1) scanner.yMin = clip.Min.Y - 1;
			if(scanner.yMax > clip.Max.Y + 1) scanner.yMax = clip.Max.Y + 1;
			for(int y = scanner.yMin; y <= scanner.yMax; y++)
			{
				scanner[y] = new Scanner.Scan{min = float.PositiveInfinity, max = float.NegativeInfinity};
			}
			for(int i = 0; i < Vertices.Length; i++)
			{
				ScanLine(scanner, Vertices[i], Vertices[(i + 1) % Vertices.Length]);
			}
			//true clip
			if(scanner.yMin < clip.Min.Y)
			{
				scanner.yMin = clip.Min.Y;
				scanner.isYMinClipped = true;
			}
			if(scanner.yMax > clip.Max.Y)
			{
				scanner.yMax = clip.Max.Y;
				scanner.isYMaxClipped = true;
			}
		}
        
        private void ScanLine(Scanner scanner, Vec2D v1, Vec2D v2)
		{
			if((int)v1.Y == (int)v2.Y)
			{
				int ly = (int)v1.Y;
				if(ly > scanner.yMin && ly < scanner.yMax)
				{
					double minX = Math.Min(v1.X, v2.X);
					double maxX = Math.Max(v1.X, v2.X);
					Scanner.Scan scan = scanner[ly];
					scan.min = Math.Min(scan.min, (float)minX);
					scan.max = Math.Max(scan.max, (float)maxX);
					scanner[ly] = scan;
				}
				return;
			}
			
			//v1.Y always less than v2.Y
			if(v1.Y > v2.Y)
			{
				Util.Swap(ref v1, ref v2);
			}
			//skip if out of bounds
			if(v2.Y < scanner.yMin || v1.Y > scanner.yMax) return;
			
			double x = v1.X;
			double dx = (v2.X - v1.X) / ((int)v2.Y - (int)v1.Y);
			int y = (int)v1.Y;
			int maxY = Math.Min((int)v2.Y, scanner.yMax);
			//start at beginning
			if(y < scanner.yMin)
			{
				x += dx * (scanner.yMin - y);
				y = scanner.yMin;
			}
			for(; y <= maxY; y++, x += dx)
			{
				Scanner.Scan scan = scanner[y];
				scan.min = Math.Min(scan.min, (float)x);
				scan.max = Math.Max(scan.max, (float)x);
				scanner[y] = scan;
			}
        }
        
        /// <summary>
        /// True if the polys are equal.
        /// </summary>
        /// <param name="poly">The first poly.</param>
        /// <param name="poly2">The second poly.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(Polygon poly, Polygon poly2)
        {
        	if(poly.Vertices.Length != poly2.Vertices.Length) return false;
        	for(int i = 0; i < poly.Vertices.Length; i++)
        	{
        		if(poly.Vertices[i] != poly2.Vertices[i])
        			return false;
        	}
        	return true;
        }
        
        /// <summary>
        /// True if the rects are not equal.
        /// </summary>
        /// <param name="rect">The first rect.</param>
        /// <param name="rect2">The second rect.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(Polygon rect, Polygon rect2)
        {
        	return !(rect == rect2);
        }
	}
}