using System;

namespace DoppelgangstersOnline
{
    public class Settings
    {
        public static string GetConnectionString()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                return Environment.GetEnvironmentVariable("DefaultConnection");
            }

            var dbHost = Environment.GetEnvironmentVariable("dbhost");
            var dbUser = Environment.GetEnvironmentVariable("dbuser");
            var dbPass = Environment.GetEnvironmentVariable("dbpass");
            var dbPort = Environment.GetEnvironmentVariable("dbport");
            var dbName = Environment.GetEnvironmentVariable("db");

            return $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPass}";
        }
    }
}
