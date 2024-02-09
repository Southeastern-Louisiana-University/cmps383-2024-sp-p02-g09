using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Selu383.SP24.Api.Data;
using Selu383.SP24.Api.Features.Hotels;
using Selu383.SP24.Api.Features.Users;

namespace Selu383.SP24.Api.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly DbSet<User> users;
        private readonly DataContext dataContext;

      

        public AuthenticationController( SignInManager<User> _signInManager, UserManager<User> _userManager)
        {
            this._signInManager = _signInManager;
            this._userManager = _userManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto dto) 
        {
            if (string.IsNullOrEmpty(dto.UserName))
            {
                return BadRequest("Username cannot be null or empty.");
            }

            var user= await _userManager.FindByNameAsync(dto.UserName);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password,false);

            if (!result.Succeeded)
            {
                return BadRequest();
            }
            await _signInManager.SignInAsync(user, true);

           var roles = await _userManager.GetRolesAsync(user);

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Roles = roles.ToArray(),
            };

            return Ok(userDto);
 
        }
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Nobody is logged in, return 401 Unauthorized
                return Unauthorized();
            }
            var user = User.Identity.Name;

            var resultDto = await GetUDto(_userManager.Users).SingleAsync(x => x.UserName == user);

            return Ok(resultDto);
        }

        private static IQueryable<UserDto> GetUDto(IQueryable<User> users)
        {
            return users.Select(x => new UserDto
            {
                Id = x.Id,
                UserName = x.UserName,
                Roles = x.Roles.Select(y => y.Role!.Name).ToArray()
            });
        }

        [HttpPost]
        [Route("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();    

        }


      

    }
}
