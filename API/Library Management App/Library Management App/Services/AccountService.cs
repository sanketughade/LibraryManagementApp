using Library_Management_App.DTOs;
using Library_Management_App.Interfaces;
using Library_Management_App.Models;
using Library_Management_App.Repositories;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;

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
        public dynamic Register(string username, string password)
        {
            AppUser appUser = null;
            try
            {
                string status = string.Empty;
                if (UserExists(username))
                {
                    status = "Username already exists. Try using a different username.";
                    return status;
                }
                else
                {
                    using var hmac = new HMACSHA512();

                    appUser = new AppUser
                    {
                        UserName = username,
                        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                        PasswordSalt = hmac.Key
                    };
                    _accountRepository.Register(appUser);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return appUser;
        }

        /// <summary>
        /// Login a user into application.
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        public AppUser Login(LoginDto loginDto)
        {
            AppUser appUser = _accountRepository.GetUser(loginDto);
            if(appUser == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
            }

            using var hmac = new HMACSHA512(appUser.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for(int i = 0; i <computedHash.Length; i++)
            {
                if (computedHash[i] != appUser.PasswordHash[i])
                {
                    throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
                }
            }
            return (appUser);
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
