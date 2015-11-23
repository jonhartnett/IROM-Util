namespace IROM.Util
{
	using System;
	using System.Linq;
	
	/// <summary>
	/// Stores methods for interpolating (or lerping) between values.
	/// </summary>
    public static class Interpolation
    {
        /// <summary>
        /// Interpolates between the given values.
        /// </summary>
        /// <param name="bottom">The value at 0.</param>
        /// <param name="top">The value at 1.</param>
        /// <param name="mu">The distance.</param>
        /// <returns>The interpolated value.</returns>
        public delegate double InterpFunction(double bottom, double top, double mu);
        
        /// <summary>
        /// Interpolates between the given values.
        /// </summary>
        /// <param name="past">The value at -1.</param>
        /// <param name="bottom">The value at 0.</param>
        /// <param name="top">The value at 1.</param>
        /// <param name="future">The value at 2.</param>
        /// <param name="mu">The distance.</param>
        /// <returns>The interpolated value.</returns>
        public delegate double ExtendedInterpFunction(double past, double bottom, double top, double future, double mu);
    
        /// <summary>
        /// Linear interpolation function.
        /// Neither first nor second derivative is continuous.
        /// </summary>
        /// <param name="bottom">The value at 0.</param>
        /// <param name="top">The value at 1.</param>
        /// <param name="mu">The distance.</param>
        /// <returns>The interpolated value.</returns>
        public static double Linear(double bottom, double top, double mu)
        {
            return (bottom * (1 - mu)) + (top * mu);
        }

        /// <summary>
        /// Cosine interpolation function.
        /// First derivative is continuous but not the second.
        /// </summary>
        /// <param name="bottom">The value at 0.</param>
        /// <param name="top">The value at 1.</param>
        /// <param name="mu">The distance.</param>
        /// <returns>The interpolated value.</returns>
        public static double Cosine(double bottom, double top, double mu)
        {
            return Linear(bottom, top, (1 - System.Math.Cos(mu * System.Math.PI)) / 2);
        }

        /// <summary>
        /// Cubic interpolation function.
        /// First and second derivative are continuous.
        /// </summary>
        /// <param name="past">The value at -1.</param>
        /// <param name="bottom">The value at 0.</param>
        /// <param name="top">The value at 1.</param>
        /// <param name="future">The value at 2.</param>
        /// <param name="mu">The distance.</param>
        /// <returns>The interpolated value.</returns>
        public static double Cubic(double past, double bottom, double top, double future, double mu)
        {
            double muSq = mu * mu;
            double a0 = (future - top) - (past - bottom);
            double a1 = (past - bottom) - a0;
            double a2 = (top - past);
            double a3 = bottom;
            return (a0 * mu * muSq) + (a1 * muSq) + (a2 * mu) + a3;
        }

        /// <summary>
        /// Catmull Rom interpolation function.
        /// First and second derivative are continuous.
        /// </summary>
        /// <param name="past">The value at -1.</param>
        /// <param name="bottom">The value at 0.</param>
        /// <param name="top">The value at 1.</param>
        /// <param name="future">The value at 2.</param>
        /// <param name="mu">The distance.</param>
        /// <returns>The interpolated value.</returns>
        public static double CatmullRom(double past, double bottom, double top, double future, double mu)
        {
            double muSq = mu * mu;
            double a0 = ((future / 2) - (top * 3 / 2)) - ((past / 2) - (bottom * 3 / 2));
            double a1 = (past - (bottom * 5 / 2)) - ((future / 2) - (top * 2));
            double a2 = (top - past) / 2;
            double a3 = bottom;
            return (a0 * mu * muSq) + (a1 * muSq) + (a2 * mu) + a3;
        }
    }
}
