using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace PopulateSensorDatas
{
    class DatabaseHelper
    {
        public int Factor;

        static int SensorID = 1;

        public static DateTime SimulationTime;

        //Timer
        readonly Timer t = new Timer(PopulateDataBase, null, 0, 1000);

        //SQL
        static string CONNECTION = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FloatingCity;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static SqlConnection con = new SqlConnection(CONNECTION);

        static SqlCommand command = new SqlCommand
        {
            CommandText = "INSERT INTO [SensorDatas] ([RealTime], [SimulationTime], [SensorID], [SValue]) " +
                           "VALUES (@RealTime, @SimulationTime, @SensorID, @SValue)"
        };

        static public void PopulateDataBase(object stateinfo)
        {
            Random random = new Random();

            if (SensorID == 7)
            {
                SensorID = 1;
            }

            float SValue = 0;

            switch (SensorID)
            {
                case 1:
                    SValue = (float)SimulationValue(1, 1);
                    break;
                case 2:
                    SValue = (float)SimulationValue(1, 1);
                    break;
                case 3:
                    SValue = (float)SimulationValue(1, 1);
                    break;
                case 4:
                    SValue = (float)SimulationValue(1, 1);
                    break;
                case 5:
                    SValue = (float)SimulationValue(1, 1);
                    break;
                case 6:
                    SValue = (float)SimulationValue(1, 1);
                    break;
                default:
                    break;
            }
            double SimulationValue(float minparameter, float maxparameter)
            {
                return random.NextDouble() * (maxparameter - minparameter) + minparameter;
            }

            //Check if connection is open. If already open, keep it that way
            if (con == null || con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            command.Parameters.AddWithValue("@RealTime", DateTime.Now);
            command.Parameters.AddWithValue("@SimulationTime", SimulationTime);
            command.Parameters.AddWithValue("@SensorID", SensorID);
            command.Parameters.AddWithValue("@SValue", SValue);
            command.ExecuteNonQuery();
            con.Close();
            SensorID++;
        }

    }


}
