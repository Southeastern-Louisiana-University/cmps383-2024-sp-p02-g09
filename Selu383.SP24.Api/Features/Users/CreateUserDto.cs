namespace Selu383.SP24.Api.Features.Users
{
    public class CreateUserDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string[] Roles { get; set; }
    }
}
