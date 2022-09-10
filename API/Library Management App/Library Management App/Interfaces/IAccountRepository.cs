using Library_Management_App.Models;

namespace Library_Management_App.Interfaces
{
    public interface IAccountRepository
    {
        public string Register(AppUser user);
        bool UserExists(string username);
    }
}
