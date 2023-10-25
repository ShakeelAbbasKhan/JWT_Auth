using JWT_Auth.Models;

namespace JWT_Auth.Data
{
    public interface IUserRepository
    {
        User Create (User user);
        User GetUserByEmail(string email);
        User GetById(int id);
    }
}
