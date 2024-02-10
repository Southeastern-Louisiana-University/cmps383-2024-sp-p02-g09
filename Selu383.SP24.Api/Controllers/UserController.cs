using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Selu383.SP24.Api.Data;
using Selu383.SP24.Api.Features.Users;

namespace Selu383.SP24.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;

        public UserController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;

        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CreateUserDto>> createUser(CreateUserDto dto)
        {
            if ((dto.Roles.Length==0) || (dto.UserName==null))
            {
                return BadRequest();
            }

            var roles = await roleManager.Roles.Select(x => x.Name).ToListAsync();
            foreach (var role in dto.Roles)
            {
                if(!roles.Contains(role))
                {
                    return BadRequest($"{role} does not exits.");

                }
            }

            var existingUser = await userManager.FindByNameAsync(dto.UserName);
            if(existingUser != null)
            {
                return BadRequest($"The username {dto.UserName} is already taken");
            }

            var userCreated = new User
            {
                UserName = dto.UserName,

            };

            var validatePassword = await userManager.CreateAsync(userCreated, dto.Password);

            if (!validatePassword.Succeeded)
            {
                return BadRequest();
            }

            var validateRole = await userManager.AddToRolesAsync(userCreated, dto.Roles);
            if (!validateRole.Succeeded)
            {
                return BadRequest();
            }

            var newUser = new UserDto
            {
                Id = userCreated.Id,
                UserName = dto.UserName,
                Roles = dto.Roles,

            };

            return Ok(newUser);
        }
    }
}
