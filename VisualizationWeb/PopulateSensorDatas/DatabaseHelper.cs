using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;

namespace PopulateSensorDatas
{
    public class DatabaseHelper
    {
        #region variables
        //initialize variables
        static int Runs = 0;

        //if you need more Sensors, set range and dont forget to add another case in PopulateDataBase-method
        static List<int> SensorIDs = Enumerable.Range(1, 15).ToList();

        public Timer timer = new Timer(PopulateDataBase, null, 0, 1000);

        //Simulation start time
        static DateTime SimStartTime = new DateTime(2020, 03, 05, 07, 0, 0);

        //Simulation end time
        static DateTime SimEndTime = new DateTime(2020, 03, 05, 08, 0, 0);

        //Factor from real to sim time
        static long Factor = 10;

        static string StartTimeActual = DateTime.Now.ToString();
        #endregion

        #region sql
        static string CONNECTION = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FloatingCity;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static SqlConnection con = new SqlConnection(CONNECTION);

        static SqlCommand command = new SqlCommand
        {
            CommandText = "INSERT INTO [SensorDatas] ([RealTime], [SimulationTime], [SensorID], [SValue]) " +
                           "VALUES (@RealTime, @SimulationTime, @SensorID, @SValue)"
        };
        #endregion  

        static public void PopulateDataBase(object stateinfo)
        {
            //test for 20 times
            if (SimStartTime < SimEndTime)
            {
                Stopwatch stopwatch = new Stopwatch();

                //initial start
                if (stopwatch.IsRunning != true)
                {
                    stopwatch.Start();
                }

                Random random = new Random();

                float SValue = 0;

                double SimulationValue(float minparameter, float maxparameter)
                {
                    return random.NextDouble() * (maxparameter - minparameter) + minparameter;
                }

                //Check if connection is open. If already open, keep it that way
                if (con == null || con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                foreach (var item in SensorIDs)
                {
                    command.Connection = con;
                    command.Parameters.Clear();

                    //if you expanded the range in "static List<int> SensorIDs" you have to set the cases accordingly
                    switch (item)
                    {
                        case 1:
                            SValue = (float)SimulationValue(100, 500);
                            break;
                        case 2:
                            SValue = (float)SimulationValue(100, 500);
                            break;
                        case 3:
                            SValue = (float)SimulationValue(100, 500);
                            break;
                        case 4:
                            SValue = (float)SimulationValue(5, 10);
                            break;
                        case 5:
                            SValue = (float)SimulationValue(5, 10);
                            break;
                        case 6:
                            SValue = (float)SimulationValue(5, 10);
                            break;
                        case 7:
                            SValue = (float)SimulationValue(1000, 5000);
                            break;
                        case 8:
                            SValue = (float)SimulationValue(1000, 5000);
                            break;
                        case 9:
                            SValue = (float)SimulationValue(1000, 5000);
                            break;
                        case 10:
                            SValue = (float)SimulationValue(1000, 5000);
                            break;
                        case 11:
                            SValue = (float)SimulationValue(1000, 5000);
                            break;
                        case 12:
                            SValue = (float)SimulationValue(1000, 5000);
                            break;
                        case 13:
                            SValue = (float)SimulationValue(50, 100);
                            break;
                        case 14:
                            SValue = (float)SimulationValue(50, 100);
                            break;
                        case 15:
                            SValue = (float)SimulationValue(50, 100);
                            break;
                        default:
                            break;
                    }
                    command.Parameters.AddWithValue("@RealTime", DateTime.Now);
                    command.Parameters.AddWithValue("@SensorID", item);
                    TimeSpan difference = DateTime.Now.Subtract(DateTime.Parse(StartTimeActual));
                    var result = TimeSpan.FromTicks(difference.Ticks * Factor);
                    command.Parameters.AddWithValue("@SimulationTime", SimStartTime.Add(result));
                    command.Parameters.AddWithValue("@SValue", SValue);
                    if (con == null || con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    command.ExecuteNonQuery();
                }
                con.Close();
                Runs++;
            }
        }

    }


}
