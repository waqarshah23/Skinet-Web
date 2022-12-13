using Microsoft.AspNetCore.Mvc;
using PTO_Server.Extensions.AuthToken;
using PTO_Server.Extensions.Logger;
using Core.Models;
using PTO_Server.Repository.UserAuth;
using System.Security.Claims ;

namespace PTO_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuth _userAuthRepo;
        private readonly ILoggerManager _logger;
        private readonly ITokenService _tokenService;
        public AuthController(IUserAuth userAuthRepo, ILoggerManager logger, ITokenService tokenService)
        {
            _userAuthRepo = userAuthRepo;
            _logger = logger;
            _tokenService = tokenService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel user)
        {
            var response = new AuthenticatedResponse();
            try
            {
                if (user is null)
                {
                    _logger.LogError("Invalid User Request, Null user params");
                    return BadRequest("Invalid Request");
                }
                var AuthUser = await _userAuthRepo.Authenticate_User(user.EmailAddress);
                response.UserName = AuthUser.Email_Address;
                if (AuthUser == null)
                {
                    _logger.LogInfo("User Not Found");
                    response.found = false;
                    response.Token = "";
                    response.RefreshToken = "";
                    return Unauthorized(response);
                }
          
                _logger.LogInfo("User authenticated");
           
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, AuthUser.Email_Address),
                        new Claim(ClaimTypes.Role, AuthUser.Type)
                    };

                var accessToken = _tokenService.GenerateAccessToken(claims);
                var refreshToken = _tokenService.GenerateRefreshToken();
                AuthUser.RefreshToken = refreshToken;
                AuthUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
                await _userAuthRepo.Update(AuthUser);
                response.Token = accessToken;
                response.RefreshToken = refreshToken;
                response.found = true;
                return Ok(response);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }
    }
}
