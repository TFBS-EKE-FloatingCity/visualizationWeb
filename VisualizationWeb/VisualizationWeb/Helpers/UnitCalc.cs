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

            if(val_arr.Count() < 2)
            {
                return Convert.ToDouble(val_arr[0]);
            }

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

            while (value > 10)
            {
                if (digits >= 4) break;

                digits++;
                value /= 1000;
            }

            string[] units = new string[] { "W", "kW", "MW", "GW", "TW" };

            valueString = value.ToString() + units[digits];

            return valueString;
        }
    }
}