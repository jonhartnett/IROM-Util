namespace IROM.Util
{
	using System;
	using System.Linq;
	
    /// <summary>
    /// Simple util class for misc. methods.
    /// </summary>
    public static class Util
    {
    	/// <summary>
    	/// Simply swaps the values of the given variables.
    	/// </summary>
    	/// <param name="var1">The first variable.</param>
    	/// <param name="var2">The second variable.</param>
    	public static void Swap<T>(ref T var1, ref T var2)
    	{
    		T temp = var1;
    		var1 = var2;
    		var2 = temp;
    	}
    	
        /// <summary>
        /// Clips the given value to between min and max.
        /// </summary>
        /// <param name="value">The value to clip.</param>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <returns>The clipped value.</returns>
        public static T Clip<T>(T value, T min, T max) where T : struct
        {
            if(Operator<T>.LessThanOrEquals(value, min))
            {
                return min;
            }else
            if(Operator<T>.GreaterThanOrEquals(value, max))
            {
                return max;
            }else
            {
                return value;
            }
        }

        /// <summary>
        /// Wraps the given value to between the min (inclusive) and max (exclusive).
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <returns>The wrapped value.</returns>
        public static T Wrap<T>(T value, T min, T max) where T: struct
        {
            //dif = max - min;
            T dif = Operator<T>.Subtract(max, min);

            //naive implementation
            /*
            while(value < min) value += dif;
            while(value >= max) value -= dif;
             */

            //fast implementation
            //value -= min;
            value = Operator<T>.Subtract(value, min);
            //value %= dif;
            value = Operator<T>.Modulo(value, dif);
            //if(value < 0)
            if(Operator<T>.LessThan(value, Cast<int, T>.CastVal(0)))
            {
                //value += dif;
                value = Operator<T>.Add(value, dif);
            }
            //value += min;
            value = Operator<T>.Add(value, min);
            return value;
        }

        /// <summary>
        /// Returns the max of the given values.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="values">The values.</param>
        /// <returns>The max value.</returns>
        public static T Max<T>(params T[] values) where T : IComparable
        {
        	T max = values[0];
        	for(int i = 1; i < values.Length; i++)
        	{
        		if(Operator<T>.GreaterThan(values[i], max))
        		{
        			max = values[i];
        		}
        	}
            return max;
        }

        /// <summary>
        /// Returns the min of the given values.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="values">The values.</param>
        /// <returns>The min value.</returns>
        public static T Min<T>(params T[] values) where T : IComparable
        {
            T min = values[0];
        	for(int i = 1; i < values.Length; i++)
        	{
        		if(Operator<T>.LessThan(values[i], min))
        		{
        			min = values[i];
        		}
        	}
            return min;
        }

        /// <summary>
        /// Performs a power function with integer exponents.
        /// Faster than Math.Pow.
        /// </summary>
        /// <param name="b">The base.</param>
        /// <param name="e">The exponent.</param>
        /// <returns>The result.</returns>
        public static double IPow(double b, int e)
        {
            if(e >= 0)
            {
                double result = 1;
                while(e != 0)
                {
                    if ((e & 1) == 1) result *= b;
                    e >>= 1;
                    b *= b;
                }
                return result;
            }else
            {
                return 1D / IPow(b, -e);
            }
        }
        
        /// <summary>
        /// Performs a power function with integer exponents.
        /// Faster than Math.Pow.
        /// </summary>
        /// <param name="b">The base.</param>
        /// <param name="e">The exponent.</param>
        /// <returns>The result.</returns>
        public static int IPow(int b, int e)
        {
            if(e >= 0)
            {
                int result = 1;
                while(e != 0)
                {
                    if ((e & 1) == 1) result *= b;
                    e >>= 1;
                    b *= b;
                }
                return result;
            }else
            {
            	throw new ArgumentException("Exponent must be >= 0 for integer pow");
            }
        }

        /// <summary>
        /// Rounds the given value to the given number of decimal digits. 
        /// Negative digits round to multiples of 10.
        /// /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="digits">The num digits.</param>
        /// <returns>The rounded number.</returns>
        public static double RoundTo(double value, int digits)
        {
            double multi = IPow(10, digits);
            return Math.Round(value * multi) / multi;
        }

        /// <summary>
        /// Clips the given angle to between the given angle measures. 
        /// Takes into account the wrap around at 0-2PI.
        /// </summary>
        /// <param name="angle">The angle measure.</param>
        /// <param name="min">The min (most counter-clockwise) angle.</param>
        /// <param name="max">The max (most clockwise) angle.</param>
        /// <returns>The clipped angle.</returns>
        public static double AngleClip(double angle, double min, double max)
        {
            angle = Wrap(angle, 0, Math.PI * 2);
            min = Wrap(min, 0, Math.PI * 2);
            max = Wrap(max, 0, Math.PI * 2);
            if(min < max)
            {
                if(angle < min || angle > max)
                {
                    if(AngleDif(angle, min) < AngleDif(angle, max))
                    {
                        angle = min;
                    }else
                    {
                        angle = max;
                    }
                }
            }else
            {
                if (angle > max && angle < min)
                {
                    if (AngleDif(angle, min) < AngleDif(angle, max))
                    {
                        angle = min;
                    }else
                    {
                        angle = max;
                    }
                }
            }
            return angle;
        }

        /// <summary>
        /// Returns the difference between the two given angles. 
        /// Takes into account the wrap around at 0-2PI.
        /// </summary>
        /// <param name="a1">The first angle.</param>
        /// <param name="a2">The second angle.</param>
        /// <returns>The angle difference.</returns>
        public static double AngleDif(double a1, double a2)
        {
            return WrapDif(a1, a2, 0, Math.PI * 2);
        }

        /// <summary>
        /// Returns the difference between the two given coords. Uses wrapping between the max and min.
        /// </summary>
        /// <param name="c1">The first coord.</param>
        /// <param name="c2">The second coord.</param>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <returns>The wrapped difference.</returns>
        public static double WrapDif(double c1, double c2, double min, double max)
        {
            c1 -= min;
            c2 -= min;
            max -= min;
            double d = Math.Abs(c1 - c2) % max;
            if(d > (max + min) / 2)
            {
                d = max - d;
            }
            d += min;
            return d;
        }
        
        /// <summary>
        /// Checks equality with a small fudge factor.
        /// </summary>
        /// <param name="val">The first value.</param>
        /// <param name="val2">The second value.</param>
        /// <param name="fudge">The max acceptible difference (inclusive).</param>
        /// <returns>True if equal.</returns>
        public static bool Equal(double val, double val2, double fudge)
        {
        	return Math.Abs(val - val2) <= fudge;
        }
    }
}