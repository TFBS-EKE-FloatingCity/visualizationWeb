using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace VisualizationWeb.Helpers
{
    public static class UnitCalc
    {
        public static double PrefixToNumber(string value)
        {
            var val_arr = Regex.Split(value, @"([a-zA-Z]+)");
            
            switch (val_arr[1].ToLower())
            {
                case "w":
                    return Convert.ToDouble(val_arr[0]);
                case "kw":
                    return Convert.ToDouble(val_arr[0]) * 1000;
                case "mw":
                    return Convert.ToDouble(val_arr[0]) * 1000000;
                case "gw":
                    return Convert.ToDouble(val_arr[0]) * 1000000000;
                default:
                    return Convert.ToDouble(val_arr[0]);
            }
        }

        public static string NumberToPrefix(double value)
        {
            int digits = 0;
            string valueString = "";

            while (value > 1)
            {
                if (digits >= 3) break;

                digits++;
                value /= 1000;
            }

            if (digits >= 3)
            {
                valueString = value.ToString() + " GW";
            }
            else if (digits == 2)
            {
                valueString = value.ToString() + " MW";
            }
            else if (digits == 1)
            {
                valueString = value.ToString() + " kW";
            }
            else
            {
                valueString = value.ToString() + " W";
            }
            
           

            return valueString;
        }
    }
}