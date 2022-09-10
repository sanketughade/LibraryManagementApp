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

        public string Register(AppUser user)
        {
            string status = string.Empty;
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
                status = "User registered successfully";
            }
            catch (Exception exception)
            {
                status = "User registration Failed.\n" + exception.ToString();
            }
            return status;
        }

        public bool UserExists(string username)
        {
            bool isUserExists = false;
            try
            {
                using(NpgsqlConnection connection = new NpgsqlConnection())
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
            catch(Exception exception)
            {

            }
            return isUserExists;
        }
    }
}
