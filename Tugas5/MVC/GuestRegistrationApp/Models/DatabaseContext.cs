using Dapper;
using MySql.Data.MySqlClient;
using System.Data;

namespace GuestRegistrationApp.Models
{
    public class DatabaseContext
    {
        private readonly IDbConnection _connection;
        
        public DatabaseContext(IDbConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<Guest> GetAllGuests()
        {
            return _connection.Query<Guest>(
                "SELECT * FROM Guests ORDER BY RegistrationDate DESC");
        }

        public int AddGuest(Guest guest)
        {
            var sql = @"INSERT INTO Guests 
                        (Name, Address, Phone, InvitationCode, RegistrationDate) 
                        VALUES (@Name, @Address, @Phone, @InvitationCode, @RegistrationDate);
                        SELECT LAST_INSERT_ID();";
            
            return _connection.ExecuteScalar<int>(sql, guest);
        }

        public bool UpdateGuest(Guest guest)
        {
            var affectedRows = _connection.Execute(
                @"UPDATE Guests SET 
                Name = @Name, 
                Address = @Address, 
                Phone = @Phone 
                WHERE Id = @Id", guest);
                
            return affectedRows > 0;
        }

        public bool DeleteGuest(int id)
        {
            var affectedRows = _connection.Execute(
                "DELETE FROM Guests WHERE Id = @Id", 
                new { Id = id });
                
            return affectedRows > 0;
        }
    }
}