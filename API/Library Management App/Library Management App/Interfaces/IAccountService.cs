using Library_Management_App.DTOs;
using Library_Management_App.Models;

namespace Library_Management_App.Interfaces
{
    public interface IAccountService
    {
        public dynamic Register(string username, string password);
        AppUser Login(LoginDto loginDto);
    }
}
