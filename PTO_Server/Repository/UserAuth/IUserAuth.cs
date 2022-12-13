using Core.Models;

namespace PTO_Server.Repository.UserAuth
{
    public interface IUserAuth: IRepository<Users>
    {
        Task<Users> Authenticate_User(string email);
    }
}
