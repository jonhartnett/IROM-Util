namespace IROM.Util
{
	using System;
	using System.Linq;
	
    /// <summary>
    /// Allows blazing-fast approximate calculations of sqrts in a certain interval.
    /// </summary>
    public class SqrtTable
    {
	    private readonly double Min;
	    private readonly double Max;
	    private readonly double Inc;
	    //list of sqrt values
	    private readonly double[] SqrtSamples;
	
        /// <summary>
        /// Creates a new SqrtTable.
        /// </summary>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <param name="samples">The number of samples.</param>
	    public SqrtTable(double min, double max, int samples)
	    {
		    Min = min;
		    Max = max;
		    Inc = (Max - Min) / (samples - 1);
            SqrtSamples = new double[samples];
            for (int i = 0; i < SqrtSamples.Length; i++)
		    {
                SqrtSamples[i] = System.Math.Sqrt(Min + (i * Inc));
		    }
	    }
	
        /// <summary>
        /// Basic sqrt approximation.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The sqrt value.</returns>
	    public double Sqrt(double value)
	    {
		    if(value < Min || value > Max)
		    {
                return Math.Sqrt(value);
		    }
		    int index = (int)((value - Min) / Inc);
            return SqrtSamples[index];
	    }

        /// <summary>
        /// Lerped sqrt approximation. More accurate than sqrt().
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The sqrt value.</returns>
        public double LerpSqrt(double value)
        {
            if (value < Min || value > Max)
            {
                return Math.Sqrt(value);
            }
            double mu = (value - Min) / Inc;
            int index = (int)mu;
            //if exact value
            // disable once CompareOfFloatsByEqualityOperator
            if(index == mu)
            {
                return SqrtSamples[index];
            } 
            mu -= index;
            return Interpolation.Linear(SqrtSamples[index], SqrtSamples[index + 1], mu);
        }
    }
}
