using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Library.Calculations
{
    public static class CalcInterpolation
    {
        //Returns searched Value between 2 Function positions
        public static int GetValue(int startY, decimal startX, int endX, decimal endY, decimal xGiven)
        {
            return (int)Math.Round(startY + ((endX - startY) / (endY - startX)) * (xGiven - startX));
        }
    }
}
