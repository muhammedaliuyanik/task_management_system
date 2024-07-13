using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TaskManagementSystem.Helpers;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.DTOs;
using System.Linq;
using System; // Bu satırı ekleyin

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;

        public UserController(DatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserDto userDto)
        {
            if (_context.Users.Any(u => u.Username == userDto.Username || u.Email == userDto.Email))
            {
                return BadRequest("Username or Email already exists.");
            }

            var user = new User
            {
                Username = userDto.Username,
                Password = PasswordHelper.HashPassword(userDto.Password),
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Role = UserRole.Employee,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(user);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDto loginDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == loginDto.Username);

            if (user == null || !PasswordHelper.VerifyPassword(loginDto.Password, user.Password))
            {
                return Unauthorized();
            }

            var token = JwtHelper.GenerateToken(user, _configuration["AppSettings:Secret"]);
            return Ok(new { Token = token });
        }

        [HttpPut("updateProfile")]
        public IActionResult UpdateProfile([FromBody] UserProfile profile)
        {
            var existingProfile = _context.UserProfiles.FirstOrDefault(p => p.UserId == profile.UserId);

            if (existingProfile == null)
            {
                return NotFound("Profile not found.");
            }

            existingProfile.Address = profile.Address;
            existingProfile.PhoneNumber = profile.PhoneNumber;
            existingProfile.ProfilePicture = profile.ProfilePicture;

            _context.SaveChanges();

            return Ok(existingProfile);
        }

        [HttpPut("updateUser")]
        public IActionResult UpdateUser([FromBody] User updatedUser)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == updatedUser.UserId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Username = updatedUser.Username;
            user.Email = updatedUser.Email;
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Role = updatedUser.Role;
            user.UpdatedDate = DateTime.Now;

            _context.SaveChanges();

            return Ok(user);
        }
    }
}
