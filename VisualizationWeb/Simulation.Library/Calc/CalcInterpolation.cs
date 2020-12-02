using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Library.Calc
{
    public class CalcInterpolation
    {
        //Calculates the Missing Value to a given Time between two known Points
        private int calcInterpolationValue (int valueA, int valueB, int timeA, int timeB, int timeX)
        {
            int interPolationValue = valueA + ((timeX - timeA) / (timeA - timeB)) * (valueB - valueA);
            return interPolationValue;
        }
    }
}
