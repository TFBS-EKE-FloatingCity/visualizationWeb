using System;

namespace Simulation.Library.Calculations
{
   public static class InterpolationHelper
    {
        //Returns searched Value between 2 Function positions
        public static decimal GetValue(long x1, long y1, long x2, long y2, decimal givenX)
        {
            if (x1 == x2 && givenX == x1 && y1 == y2)
            {
                return (int)y1;
            }
            return Math.Round(y1 + ((y2 - (decimal)y1) / (x2 - x1)) * (givenX - x1));
        }

        /// <summary>
        /// Linearly interpolates between a and b by x. Used when the Factor is known but the Value is unknown.
        /// </summary>
        /// <param name="min">The 0% value</param>
        /// <param name="max">The 100% value</param>
        /// <param name="x">The percentage factor where 0.0 is 0% and 1.0 is 100%</param>
        /// <returns>Returns the value of the factor x</returns>
        public static decimal Lerp(decimal min, decimal max, decimal x)
        {
            return min + x * (max - min);
        }

        /// <summary>
        /// Linearly interpolates between a and b by x. Used when the Value is known but the Factor is unknown.
        /// </summary>
        /// <param name="min">The 0% value</param>
        /// <param name="max">The 100% value</param>
        /// <param name="x">The actual value</param>
        /// <returns>Returns the factor of the value x</returns>
        public static decimal InverseLerp(decimal min, decimal max, decimal x)
        {
            return max - min == 0 ? 0 : (x - min) / (max - min);
        }
    }
}
