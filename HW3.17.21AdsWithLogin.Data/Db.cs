using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace HW3._17._21AdsWithLogin.Data
{
    public class Db
    {
        private readonly string _connectionString;
        public Db(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Ad> GetAllAds()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT a.*,u.Name 'PostedBy' from Ads a
JOIN Users u
ON u.Id = a.UserId
ORDER BY DateCreated DESC";
            var ads = new List<Ad>();
            connection.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ads.Add(new Ad
                {
                    Id = (int)reader["id"],
                    Description = (string)reader["description"],
                    DateCreated = (DateTime)reader["datecreated"], 
                    PhoneNumber = (string)reader["phonenumber"], 
                    PostedBy = (string)reader["postedBy"], 
                    UserId = (int)reader["userId"]
                });
            }
            return ads;
        }

        public void NewAd(Ad ad)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Ads VALUES 
(@description, @date, @phonenumber, @userId) SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@description", ad.Description);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            cmd.Parameters.AddWithValue("@phonenumber", ad.PhoneNumber);
            cmd.Parameters.AddWithValue("@userId", ad.UserId);
            connection.Open();
            ad.Id = (int)(decimal)cmd.ExecuteScalar();
        }

        public void CreateNewAccount(User user, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Users VALUES (@name, @email, @hashpassword);";
            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@email", user.Email);
            user.HashPassword = BCrypt.Net.BCrypt.HashPassword(password);
            cmd.Parameters.AddWithValue("@hashpassword", user.HashPassword);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public User Login(string email, string password)
        {
            //  var user = GetUserByEmail(email);
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.HashPassword);
            return isValid ? user : null;
        }

        public User GetByEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Users WHERE Email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            connection.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return new User
            {
                Id = (int)reader["Id"],
                Name = (string)reader["Name"],
                Email = (string)reader["Email"],
                HashPassword = (string)reader["HashPassword"]
            };
        }

        public void DeleteAd(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM Ads
WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
