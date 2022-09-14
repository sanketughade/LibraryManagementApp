using Library_Management_App.DTOs;
using Library_Management_App.Models;

namespace Library_Management_App.Interfaces
{
    public interface IAccountRepository
    {
        public void Register(AppUser user);
        bool UserExists(string username);
        AppUser GetUser(LoginDto loginDto);
    }
}
