using Microsoft.EntityFrameworkCore;
using Core.Models;

namespace PTO_Server.Repository.UserAuth
{
    public class UserAuth: Repository<Users>,IUserAuth
    {
        public async Task<Users> Authenticate_User(string email)
        {
            if(string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("email");
            }
            var authUser = await _context.Users.Where(u => u.Email_Address == email).FirstOrDefaultAsync();
            return authUser;
        }
    }
}
