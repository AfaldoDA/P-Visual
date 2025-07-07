using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace GuestRegistrationApp.Models
{
    public static class DatabaseHelper
    {
        private static string connectionString = "Server=localhost;Database=guest_registration;Uid=root;Pwd=;";

        public static IDbConnection GetConnection()
        {
            try
            {
                return new MySqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting connection: {ex.Message}");
                throw;
            }
        }
    }
}
