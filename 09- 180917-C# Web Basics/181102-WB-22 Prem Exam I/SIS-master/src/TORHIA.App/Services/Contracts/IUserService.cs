using TORSHIA.App.Models.Binding;
using TORSHIA.Models;

namespace TORSHIA.App.Services.Contracts
{
    public interface IUserService
    {
        bool HasUsername(string username);

        User GetUserByUsernameAndPassword(LoginBindingModel model);

        void CreateUser(RegisterBindingModel model);
    }
}
