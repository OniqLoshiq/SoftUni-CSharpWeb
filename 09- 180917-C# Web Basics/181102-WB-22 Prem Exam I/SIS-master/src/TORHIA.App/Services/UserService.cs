using System.Linq;
using TORSHIA.App.Models.Binding;
using TORSHIA.App.Services.Contracts;
using TORSHIA.Data;
using TORSHIA.Models;
using TORSHIA.Models.Enums;

namespace TORSHIA.App.Services
{
    public class UserService : IUserService
    {
        private readonly TorshiaDbContext context;

        public UserService(TorshiaDbContext context)
        {
            this.context = context;
        }

        public void CreateUser(RegisterBindingModel model)
        {
            User user = new User
            {
                Username = model.Username,
                Password = model.Password,
                Email = model.Email,
                Role = (this.context.Users.Any() ? UserRole.User : UserRole.Admin)
            };

            this.context.Users.Add(user);
            this.context.SaveChanges();
        }

        public User GetUserByUsernameAndPassword(LoginBindingModel model)
        {
            return this.context.Users.SingleOrDefault(u => u.Username == model.Username && u.Password == model.Password);
        }

        public bool HasUsername(string username)
        {
            return this.context.Users.Any(u => u.Username == username);
        }
    }
}
