namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Defines a variety of utility methods for colors.
	/// </summary>
	public static class ColorUtil
	{
		/// <summary>
		/// Returns a random RGB color.
		/// </summary>
		/// <returns>A random RGB color from entire RGB range.</returns>
		public static RGB NextRGB(this Random rand)
		{
			byte[] buffer = new byte[3];
			rand.NextBytes(buffer);
			return new RGB(buffer[0], buffer[1], buffer[2]);
		}
		
		/// <summary>
		/// Returns a random ARGB color.
		/// </summary>
		/// <returns>A random ARGB color from entire ARGB range.</returns>
		public static ARGB NextARGB(this Random rand)
		{
			byte[] buffer = new byte[4];
			rand.NextBytes(buffer);
			return new ARGB(buffer[0], buffer[1], buffer[2], buffer[3]);
		}
		
        /// <summary>
        /// Returns the component-wise max of the given colors.
        /// </summary>
        /// <param name="vecs">The colors.</param>
        /// <returns>The component-wise max color.</returns>
        public static RGB Max(params RGB[] vecs)
        {
        	RGB max = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].R > max.R)
        		{
        			max.R = vecs[i].R;
        		}
        		if(vecs[i].G > max.G)
        		{
        			max.G = vecs[i].G;
        		}
        		if(vecs[i].B > max.B)
        		{
        			max.B = vecs[i].B;
        		}
        	}
            return max;
        }
        
        /// <summary>
        /// Returns the component-wise max of the given colors.
        /// </summary>
        /// <param name="vecs">The colors.</param>
        /// <returns>The component-wise max color.</returns>
        public static ARGB Max(params ARGB[] vecs)
        {
        	ARGB max = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].A > max.A)
        		{
        			max.A = vecs[i].A;
        		}
        		if(vecs[i].R > max.R)
        		{
        			max.R = vecs[i].R;
        		}
        		if(vecs[i].G > max.G)
        		{
        			max.G = vecs[i].G;
        		}
        		if(vecs[i].B > max.B)
        		{
        			max.B = vecs[i].B;
        		}
        	}
            return max;
        }
        
        /// <summary>
        /// Returns the component-wise max of the given colors.
        /// </summary>
        /// <param name="vecs">The colors.</param>
        /// <returns>The component-wise max color.</returns>
        public static RGB Min(params RGB[] vecs)
        {
        	RGB min = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].R < min.R)
        		{
        			min.R = vecs[i].R;
        		}
        		if(vecs[i].G < min.G)
        		{
        			min.G = vecs[i].G;
        		}
        		if(vecs[i].B < min.B)
        		{
        			min.B = vecs[i].B;
        		}
        	}
            return min;
        }
        
        /// <summary>
        /// Returns the component-wise max of the given colors.
        /// </summary>
        /// <param name="vecs">The colors.</param>
        /// <returns>The component-wise max color.</returns>
        public static ARGB Min(params ARGB[] vecs)
        {
        	ARGB min = vecs[0];
        	for(int i = 1; i < vecs.Length; i++)
        	{
        		if(vecs[i].A < min.A)
        		{
        			min.A = vecs[i].A;
        		}
        		if(vecs[i].R < min.R)
        		{
        			min.R = vecs[i].R;
        		}
        		if(vecs[i].G < min.G)
        		{
        			min.G = vecs[i].G;
        		}
        		if(vecs[i].B < min.B)
        		{
        			min.B = vecs[i].B;
        		}
        	}
            return min;
        }
        
        /// <summary>
        /// Interpolates the given bytes.
        /// </summary>
        /// <param name="bottom">The bottom value.</param>
        /// <param name="top">The top value.</param>
        /// <param name="mu">The mu value.</param>
        /// <param name="func">The interpolation function.</param>
        /// <returns>The interpolated byte.</returns>
        private static byte InterpByte(byte bottom, byte top, double mu, InterpFunction func)
        {
        	return (byte)Util.Clip(func(bottom, top, mu), 0, 255);
        }
        
        /// <summary>
        /// Interpolates between the given colors using the given interpolation function.
        /// </summary>
        /// <param name="bottom">The bottom value.</param>
        /// <param name="top">The top value.</param>
        /// <param name="mu">The mu value.</param>
        /// <param name="func">The interpolation function to use.</param>
        /// <returns>The resulting color.</returns>
        public static RGB Interpolate(RGB bottom, RGB top, double mu, InterpFunction func)
        {
        	return new RGB(InterpByte(bottom.R, top.R, mu, func), InterpByte(bottom.G, top.G, mu, func), InterpByte(bottom.B, top.B, mu, func));
        }
        
        /// <summary>
        /// Interpolates between the given colors using the given interpolation function.
        /// </summary>
        /// <param name="bottom">The bottom value.</param>
        /// <param name="top">The top value.</param>
        /// <param name="mu">The mu value.</param>
        /// <param name="func">The interpolation function to use.</param>
        /// <returns>The resulting color.</returns>
        public static ARGB Interpolate(ARGB bottom, ARGB top, double mu, InterpFunction func)
        {
        	return new ARGB(InterpByte(bottom.A, top.A, mu, func), InterpByte(bottom.R, top.R, mu, func), InterpByte(bottom.G, top.G, mu, func), InterpByte(bottom.B, top.B, mu, func));
        }
        
        /// <summary>
        /// Interpolates the given bytes.
        /// </summary>
        /// <param name="past">The past value.</param>
        /// <param name="bottom">The bottom value.</param>
        /// <param name="top">The top value.</param>
        /// <param name="future">The future value.</param>
        /// <param name="mu">The mu value.</param>
        /// <param name="interpFunc">The extended interpolation function.</param>
        /// <returns>The interpolated byte.</returns>
        private static byte InterpByte(byte past, byte bottom, byte top, byte future, double mu, ExtendedInterpFunction interpFunc)
        {
        	return (byte)Util.Clip(interpFunc(past, bottom, top, future, mu), 0, 255);
        }
        
        /// <summary>
        /// Interpolates between the given colors using the given extended interpolation function.
        /// </summary>
        /// <param name="past">The past value.</param>
        /// <param name="bottom">The bottom value.</param>
        /// <param name="top">The top value.</param>
        /// <param name="future">The future value.</param>
        /// <param name="mu">The mu value.</param>
        /// <param name="func">The extended interpolation function to use.</param>
        /// <returns>The resulting color.</returns>
        public static RGB Interpolate(RGB past, RGB bottom, RGB top, RGB future, double mu, ExtendedInterpFunction func)
        {
        	return new RGB(InterpByte(past.R, bottom.R, top.R, future.R, mu, func), 
        	               InterpByte(past.G, bottom.G, top.G, future.G, mu, func), 
        	               InterpByte(past.B, bottom.B, top.B, future.B, mu, func));
        }
        
        /// <summary>
        /// Interpolates between the given colors using the given extended interpolation function.
        /// </summary>
        /// <param name="past">The past value.</param>
        /// <param name="bottom">The bottom value.</param>
        /// <param name="top">The top value.</param>
        /// <param name="future">The future value.</param>
        /// <param name="mu">The mu value.</param>
        /// <param name="func">The extended interpolation function to use.</param>
        /// <returns>The resulting color.</returns>
        public static ARGB Interpolate(ARGB past, ARGB bottom, ARGB top, ARGB future, double mu, ExtendedInterpFunction func)
        {
        	return new ARGB(InterpByte(past.A, bottom.A, top.A, future.A, mu, func), 
        	                InterpByte(past.R, bottom.R, top.R, future.R, mu, func),
        	                InterpByte(past.G, bottom.G, top.G, future.G, mu, func), 
        	                InterpByte(past.B, bottom.B, top.B, future.B, mu, func));
        }
	}
}
