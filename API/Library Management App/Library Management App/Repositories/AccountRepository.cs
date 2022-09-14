using Library_Management_App.DTOs;
using Library_Management_App.Interfaces;
using Library_Management_App.Models;
using Npgsql;

namespace Library_Management_App.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IConfiguration _configuration;

        public AccountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Register(AppUser user)
        {
            try
            {
                using(NpgsqlConnection connection = new NpgsqlConnection())
                {
                    connection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "INSERT INTO AppUser(username,passwordhash,passwordsalt) VALUES (@username,@passwordhash,@passwordsalt)";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new NpgsqlParameter("@username", user.UserName));
                    cmd.Parameters.Add(new NpgsqlParameter("@passwordhash", user.PasswordHash));
                    cmd.Parameters.Add(new NpgsqlParameter("@passwordsalt", user.PasswordSalt));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public AppUser GetUser(LoginDto loginDto)
        {
            AppUser? appUser = null;
            try
            {
                using(NpgsqlConnection connection = new NpgsqlConnection())
                {
                    connection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT id, username, passwordhash, passwordsalt FROM appuser WHERE username = @username";
                    cmd.Parameters.Add(new NpgsqlParameter("@username", loginDto.Username));
                    NpgsqlDataReader dataReader = cmd.ExecuteReader();

                    if(dataReader != null)
                    {
                        while(dataReader.Read())
                        {
                            appUser = new AppUser();
                            appUser.Id = (int)dataReader["id"];
                            appUser.UserName = dataReader["username"].ToString();
                            appUser.PasswordHash = (byte[])dataReader["passwordhash"];
                            appUser.PasswordSalt = (byte[])dataReader["passwordsalt"];
                        }
                    }
                }
            }
            catch (Exception exception)
            {

                
            }
            return appUser;
        }

        #region Helper Methods
        public bool UserExists(string username)
        {
            bool isUserExists = false;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    connection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT EXISTS(SELECT username FROM appuser WHERE username = @username) AS isuserexists";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new NpgsqlParameter("@username", username));
                    NpgsqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        string result = dataReader["isuserexists"].ToString();
                        isUserExists = result == "False" ? false : true;
                    }
                }
            }
            catch (Exception exception)
            {

            }
            return isUserExists;
        }
        #endregion


    }
}
