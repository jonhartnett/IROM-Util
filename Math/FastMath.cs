namespace IROM.Util
{
	using System;
	using System.Linq;
	
    /// <summary>
    /// Stores functions for fast sin, cos, and tan, asin, and acos approximations.
    /// </summary>
    public static class FastMath
    {
	    //one revolution
	    private const double Rev = System.Math.PI * 2;
	    //half rev
	    private const double HRev = Rev / 2;
	    //quarter rev
	    private const double QRev = Rev / 4;
	    //the sample size to use (higher uses more memory but is more precise)
        private static readonly int Samples = 512;
        private static readonly int SampleMulti = Samples - 1;
	    //the resolution of the cos samples
        private static readonly double Resolution = QRev / SampleMulti;
	    //inverse cos resolution
        private static readonly double InvResolution = 1 / Resolution;
	    //list of cos and acos values, all other functions can be calculated off these
	    private static readonly float[] CosSamples = new float[Samples];
        private static readonly float[] ACosSamples = new float[Samples];

	    static FastMath()
	    {
		    for(int i = 0; i < Samples; i++)
		    {
                CosSamples[i] = (float)System.Math.Cos(Resolution * i);
                ACosSamples[i] = (float)System.Math.Acos(i / (double)SampleMulti);
            }
	    }
	
	    /// <summary>
        /// Calculates cosine values.
	    /// </summary>
	    /// <param name="rads">The radian measure.</param>
	    /// <returns>The cosine.</returns>
	    public static float Cos(double rads)
	    {
		    if(rads < 0)//flip to >0
		    {
			    rads = -rads;
		    }
		    rads %= Rev;//mod by 360
		    if(rads > HRev)//if >180, map 360->0 and 180->180
		    {
			    rads = Rev - rads;
		    }
		    if(rads <= QRev)//if <90
		    {
                return CosSamples[(int)(rads * InvResolution)];
		    }else
		    {
                return -CosSamples[(int)((HRev - rads) * InvResolution)];
		    }
	    }

        /// <summary>
        /// Calculates sine values.
        /// </summary>
        /// <param name="rads">The radian measure.</param>
        /// <returns>The sine.</returns>
	    public static float Sin(double rads)
	    {
		    //sin = cos - 90
		    return Cos(rads - QRev);
	    }

        /// <summary>
        /// Calculates tangent values.
        /// </summary>
        /// <param name="rads">The radian measure.</param>
        /// <returns>The tangent.</returns>
	    public static float Tan(double rads)
	    {
		    //tan = sin / cos
		    return Sin(rads) / Cos(rads);
	    }

        /// <summary>
        /// Calculates arccosine values.
        /// </summary>
        /// <param name="cosine">The cosine value.</param>
        /// <returns>The radian arccosine measure.</returns>
	    public static float ACos(double cosine)
	    {
            if(cosine < 0)
            {
                return (float)(HRev - ACos(-cosine));
            }
            return ACosSamples[(int)(cosine * SampleMulti)];
	    }

        /// <summary>
        /// Calculates arcsine values.
        /// </summary>
        /// <param name="sine">The sine value.</param>
        /// <returns>The radian arcsine measure.</returns>
        public static float ASin(double sine)
        {
            return (float)(ACos(-sine) - QRev);
        }   
    }
}
