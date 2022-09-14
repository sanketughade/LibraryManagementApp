using Library_Management_App.Models;

namespace Library_Management_App.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
