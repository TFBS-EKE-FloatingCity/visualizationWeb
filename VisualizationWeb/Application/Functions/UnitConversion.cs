using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Application.Functions
{
   public static class UnitConversion
   {
      public static double PrefixToNumber(string stringValue)
      {
         var val_arr = Regex.Split(stringValue, @"([a-zA-Z]+)");

         if (val_arr.Count() < 2)
         {
            return Convert.ToDouble(val_arr[0]);
         }

         double value = Convert.ToDouble(val_arr[0]);

         switch (val_arr[1].ToLower())
         {
            case "w":
               return value;

            case "kw":
               return value * 1000;

            case "mw":
               return value * 1000000;

            case "gw":
               return value * 1000000000;

            default:
               return value;
         }
      }

      public static string NumberToPrefix(double value)
      {
         int digits = 0;

         while (value > 10)
         {
            if (digits >= 4) break;

            digits++;
            value /= 1000;
         }

         string[] units = new string[] { "W", "kW", "MW", "GW", "TW" };

         return value.ToString() + units[digits];
      }
   }
}