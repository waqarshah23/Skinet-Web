using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTO_Server.Extensions.AuthToken;
using PTO_Server.Extensions.Logger;
using Core.Models;
using PTO_Server.Repository.UserAuth;

namespace PTO_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {

        private readonly IUserAuth _userAuthRepo;
        private readonly ITokenService _tokenService;
        private readonly ILoggerManager _logger;

        public TokenController(IUserAuth userAuthRepo, ITokenService tokenService, ILoggerManager loggerManager)
        {
            _userAuthRepo = userAuthRepo;
            _tokenService = tokenService;
            _logger = loggerManager;
        }
        [HttpPost, Route("Refresh")]
        public async Task<IActionResult> Refresh(TokenApiModel tokenApiModel)
        {
            try
            {
                if (tokenApiModel == null)
                {
                    return BadRequest("Invalid Client Request");
                }
                var principal = _tokenService.GetPrincipalFromExpiredToken(tokenApiModel.AccessToken);
                var email = principal.Identity.Name;

                var user = await _userAuthRepo.Authenticate_User(email);
                if (user == null || user.RefreshToken != tokenApiModel.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    return BadRequest("Invalid Client Request");
                }

                var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
                var newRefreshToken = _tokenService.GenerateRefreshToken();

                user.RefreshToken = newRefreshToken;
                await _userAuthRepo.Update(user);

                return Ok(new AuthenticatedResponse()
                {
                    Token = newAccessToken,
                    RefreshToken = newRefreshToken,
                    found = true,
                    UserName = user.Email_Address
                });
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }

        }

        [HttpPost, Authorize]
        [Route("revoke")]
        public async Task<IActionResult >Revoke()
        {
            try
            {
                var email = User.Identity.Name;
                var user = await _userAuthRepo.Authenticate_User(email);
                if (user == null) return BadRequest();
                _logger.LogInfo("User authenticated");
                user.RefreshToken = null;
                await _userAuthRepo.Update(user);
                _logger.LogInfo("User refresh token updated");
                return NoContent();
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            
            }
        }
    }
}
