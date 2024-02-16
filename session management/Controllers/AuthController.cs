using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using session_management.Data;
using session_management.DTO;
using session_management.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;


namespace session_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SessionManagementDbContext dbContext;
        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration, SessionManagementDbContext dbContext)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<ActionResult<UserModelDTO>> RegisterUser([FromBody] UserModelDTO registerUserDto)
        {
            var existingUser = await userManager.FindByEmailAsync(registerUserDto.Email);

            if (existingUser != null)
            {
                return BadRequest("User already exists");
            }

            var userModel = new UserModel
            {
                Username = registerUserDto.Username,
                Email = registerUserDto.Email,
                Password = HashPassword(registerUserDto.Password), // Hash the password
                PhoneNumber = registerUserDto.PhoneNumber,
                Address = registerUserDto.Address,
                IsBlocked = false,
                IsEmailVerified = false
            };

            dbContext.Users.Add(userModel);
            await dbContext.SaveChangesAsync();

            var identityUser = new IdentityUser
            {
                UserName = userModel.Username,
                Email = userModel.Email
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerUserDto.Password);

            if (identityResult.Succeeded)
            {
                identityResult = await userManager.AddToRoleAsync(identityUser, "User");

                if (identityResult.Succeeded)
                {
                    // Map the UserModel to UserModelDTO or return the UserModel directly
                    var userModelDTO = new UserModelDTO
                    {
                        UserID = userModel.UserID,
                        Username = userModel.Username,
                        Email = userModel.Email,
                        PhoneNumber = userModel.PhoneNumber,
                        Address = userModel.Address,
                        IsBlocked = userModel.IsBlocked,
                        IsEmailVerified = userModel.IsEmailVerified
                    };

                    return Ok(userModelDTO);
                }
                else
                {
                    return BadRequest("Error occurred while adding User role");
                }
            }

            return BadRequest("Error occurred");
        }





        [HttpPost]
        [Route("RegisterAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequestDto registerRequestDto)
        {
            return await Register(registerRequestDto, "Admin");
        }

        private async Task<IActionResult> Register(RegisterRequestDto registerRequestDto, string roleName)
        {
            var existingUser = await userManager.FindByEmailAsync(registerRequestDto.Username);

            if (existingUser != null)
            {
                return BadRequest("User already exists");
            }

            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                identityResult = await userManager.AddToRoleAsync(identityUser, roleName);

                if (identityResult.Succeeded)
                {
                    return Ok($"User Registered as {roleName} Successfully");
                }
                else
                {
                    // Handle the error, possibly log it
                    return BadRequest($"Error occurred while adding {roleName} role");
                }
            }

            return BadRequest("Error occurred");
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (checkPasswordResult)
                {
                    // Creating Token
                    var token = GenerateJwtToken(user, configuration);

                    return Ok(new { Token = token });
                }
            }
            return BadRequest("Username or password incorrect");
        }



        private string GenerateJwtToken(IdentityUser user, IConfiguration configuration)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email),
        // Add more claims as needed
    };

            // Add roles to claims if available
            var userRoles = userManager.GetRolesAsync(user).Result; // Note: Using Result here to synchronously get the roles
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["Jwt:ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

    }
}