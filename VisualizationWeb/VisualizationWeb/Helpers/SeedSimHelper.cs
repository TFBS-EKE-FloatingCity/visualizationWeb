using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VisualizationWeb.Models;

namespace VisualizationWeb.Helpers
{
    public class SeedSimHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();

        SqlConnection con = new SqlConnection(@"Data Source = (LocalDb)\MSSQLLocalDB; AttachDbFilename =| DataDirectory |\FloatingCity.mdf; Initial Catalog = FloatingCity; Integrated Security = True");

        //static to prevent same random number getting used twice
        static Random random = new Random();

        //Methods are creating random double to simulate data for database
        public double SimulatedWind(float minimum, float maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        public double SimulatedSun(float minimum, float maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        public double SimulatedConsumption(float minimum, float maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        string sqlcmd;

        public Task<int> ExecuteAsync()
        {
            return Task.Run(() =>
            {
                using (con)
                using (var newCommand = new SqlCommand(sqlcmd, con))
                {
                    newCommand.CommandType = CommandType.Text;
                    con.Open();
                    return newCommand.ExecuteNonQuery();
                }
            });
        }


    }
}