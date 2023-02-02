using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormNazarRomanyuk.Helpers
{
    public class HelperConfiguration
    {
        public static string GetConnectionString()
        {
            IConfigurationBuilder builder =
              new ConfigurationBuilder().AddJsonFile("Config.json", true, true);
            IConfigurationRoot config = builder.Build();
            string connectionString = config["SqlClase"];
            return connectionString;
        }
    }
}
