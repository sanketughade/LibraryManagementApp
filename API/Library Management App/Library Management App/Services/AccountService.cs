using Library_Management_App.Interfaces;
using Library_Management_App.Models;
using Library_Management_App.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace Library_Management_App.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// Register a new user to database.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Register(string username, string password)
        {
            string status = string.Empty;
            if (UserExists(username))
            {
                status = "Username already exists. Try using a different username.";
            }
            else
            {
                using var hmac = new HMACSHA512();

                var user = new AppUser
                {
                    UserName = username,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                    PasswordSalt = hmac.Key
                };
                status = _accountRepository.Register(user);
            }
            return status;
        }

        /// <summary>
        /// Check if the username exists into the database or not.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private bool UserExists(string username)
        {
            return _accountRepository.UserExists(username);
        }
    }
}
