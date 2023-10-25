using JWT_Auth.Models;

namespace JWT_Auth.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public User Create(User user)
        {
            _context.users.Add(user);
            user.Id = _context.SaveChanges();
            return user;
        }

        public User GetUserByEmail(string email)
        {
            return _context.users.FirstOrDefault(u => u.Email == email);
        }

        public User GetById(int id)
        {
            return _context.users.FirstOrDefault(u => u.Id == id);
        }
    }
}
