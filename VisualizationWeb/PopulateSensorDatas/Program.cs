using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopulateSensorDatas
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseHelper dh = new DatabaseHelper();


            #region SetSimulationStartDateTime
            //Console.WriteLine("Enter SimulationStartTime (dd/mm/YYYY hh:mm:ss)");

            //string datestring = Console.ReadLine();

            //if (DateTime.TryParse(datestring, out DateTime dateValue))
            //{
            //    Console.WriteLine("Converted '{0}' to {1}.", datestring, dateValue);
            //    //dh.SimulationTime = dateValue;
            //}
            //else
            //{
            //    Console.WriteLine("Unable to convert '{0}' to a date.", datestring);
            //}
            Console.WriteLine("Data in DB");

            Console.ReadKey();
            #endregion  

            #region MinValue
            //Console.WriteLine("Enter minimum-value:");
            //string minimum = Console.ReadLine();

            //if (float.TryParse(minimum, out float minimumValue))
            //{
            //    Console.WriteLine("Converted '{0} to {1}'", minimum, minimumValue);
            //}
            //else
            //{
            //    Console.WriteLine("Not able to convert number");
            //}
            #endregion

            #region MaxValue
            //Console.WriteLine("Enter maximum-value:");
            //string maximum = Console.ReadLine();

            //if (float.TryParse(minimum, out float maximumValue))
            //{
            //    Console.WriteLine("Converted '{0} to {1}'", maximum, maximumValue);
            //    float simValue = (float)SimulationValue(minimumValue, maximumValue);
            //}
            //else
            //{
            //    Console.WriteLine("Not able to convert number");
            //}
            #endregion
        }
    }
}
