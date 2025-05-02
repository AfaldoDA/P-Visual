// using System.Data;
// using MySql.Data.MySqlClient;
// using Dapper;

// public static class DatabaseHelper
// {
//     private static string connectionString = "Server=localhost;Database=guest_registration;Uid=root;Pwd=;";

//     public static IDbConnection GetConnection()
//     {
//         return new MySqlConnection(connectionString);  // Ensure you're using MySqlConnection here
//     }

//     public static void InitializeDatabase()
//     {
//         using (var connection = GetConnection())
//         {
//             connection.Open();

//             // Check if the table exists
//             var tableExists = connection.ExecuteScalar<int>(
//                 "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'guest_registration' AND table_name = 'Guests';"
//             );

//             if (tableExists == 0)
//             {
//                 connection.Execute(@"
//                     CREATE TABLE Guests (
//                         Id INT AUTO_INCREMENT PRIMARY KEY,
//                         Name VARCHAR(100) NOT NULL,
//                         Address VARCHAR(200),
//                         Phone VARCHAR(20),
//                         InvitationCode VARCHAR(20) UNIQUE,
//                         RegistrationDate DATETIME DEFAULT CURRENT_TIMESTAMP
//                     );
//                 ");
//             }
//         }
//     }
// }
using System;
using System.Data;
using MySql.Data.MySqlClient;
using Dapper;

public static class DatabaseHelper
{
    private static string connectionString = "Server=localhost;Database=guest_registration;Uid=root;Pwd=;";

    public static IDbConnection GetConnection()
    {
        try
        {
            return new MySqlConnection(connectionString);  // Ensure you're using MySqlConnection here
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting connection: {ex.Message}");
            throw;
        }
    }

    public static void InitializeDatabase()
    {
        try
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                // Check if the table exists
                var tableExists = connection.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'guest_registration' AND table_name = 'Guests';"
                );

                if (tableExists == 0)
                {
                    // Create the table if it doesn't exist
                    connection.Execute(@"
                        CREATE TABLE Guests (
                            Id INT AUTO_INCREMENT PRIMARY KEY,
                            Name VARCHAR(100) NOT NULL,
                            Address VARCHAR(200),
                            Phone VARCHAR(20),
                            InvitationCode VARCHAR(20) UNIQUE,
                            RegistrationDate DATETIME DEFAULT CURRENT_TIMESTAMP
                        );
                    ");
                    Console.WriteLine("Table 'Guests' created successfully.");
                }
                else
                {
                    Console.WriteLine("Table 'Guests' already exists.");
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"MySQL Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
