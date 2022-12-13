using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PTO_Server.Repository;
using Core.Models;
using PTO_Server.Enums;
using Microsoft.Extensions.Caching.Memory;
using PTO_Server.Extensions.Logger;

namespace PTO_Server.Controllers
{
    [Route("api/[controller]"), Authorize(Roles = UserRoles.admin)] //Authorize
    [ApiController]
    public class UsersController : ControllerBase
    {
        private const string userListKey = "AllUsers"; 
        private readonly IRepository<Users> _userRepo;
        private readonly ILoggerManager _logger;
        private IMemoryCache _cache;
        public UsersController( IRepository<Users> userRepo, IMemoryCache cache, ILoggerManager logger)
        {
            _userRepo = userRepo;
            _cache = cache;
            _logger = logger;
        }

        [HttpGet, Route("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUsers()
        {
            // List<Users> users = new List<Users>();
            IEnumerable<Users> userlist;
            if (_cache.TryGetValue(userListKey, out userlist))
            {
                _logger.LogInfo("User List found in cache");
            }
            else
            {
                _logger.LogInfo("User List not found in cache, fetching from Database");
                userlist = await _userRepo.GetListAsync();
                if (userlist == null)
                {
                    return NotFound();
                }
                else
                {
                    _setUserListCache(userlist);
                }
            }
            
            return Ok(new { Users = userlist });
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUserById(int id)
        {
            var user =  await _userRepo.GetById(id);
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPost, Route("AddUser")]
        public async Task<IActionResult> AddUser(Users user)
        {
            if(user == null)
            {
                return BadRequest("Invalid Client Request");
            }
            var status = await _userRepo.Add(user);
            if (status)
            {
                var userlist = await _userRepo.GetListAsync();
                if(userlist != null && userlist.Count() > 0)
                {
                    _setUserListCache(userlist);
                }
                return Ok(new
                {
                    Users = userlist,
                    status = status,
                    msg = "new User Added"
                });
            }
            return Ok(status);
        }
        [HttpPut, Route("updateUser")]
        public async Task<IActionResult> updateUser(Users user)
        {
            if (user == null)
            {
                return BadRequest("invalid Client Request: empty User Param");
            }
            var status = await _userRepo.Update(user);
            if (status)
            {
                var userlist = await _userRepo.GetListAsync();
                if (userlist != null && userlist.Count() > 0)
                {
                    _setUserListCache(userlist);
                }
                return Ok(new
                {
                    Users = userlist,
                    status = status,
                    msg = "User Updated"
                });
            }
            return Ok(status);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var status = await _userRepo.Delete(id);
            return Ok(status);
        }
        private void _setUserListCache(IEnumerable<Users> userlist)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                .SetPriority(CacheItemPriority.Normal);

            _cache.Set(userListKey, userlist, cacheEntryOptions);
        }

        private void _removeCacheItem(string key)
        {
            _cache.Remove(key);
        }
    }
}
