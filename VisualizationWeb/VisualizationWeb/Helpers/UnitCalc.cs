using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisualizationWeb.Helpers
{
    public static class UnitCalc
    {
        public static double PrefixToNumber(string value)
        {
            return 0.0;
        }

        public static string NumberToPrefix(double value)
        {
            int digits = 0;
            string valueString = "";

            while (value > 1)
            {
                digits++;
                value /= 10;
            }

            if (digits < 3)
            {
                valueString = value.ToString() + " W";
            } 
            else if (digits < 6)
            {
                valueString = value.ToString() + " kW";
            }
            else if (digits >= 6)
            {
                valueString = value.ToString() + " MW";
            } 
            else if(digits >= 9)
            {
                valueString = value.ToString() + " GW";
            }

            return valueString;
        }
    }
}