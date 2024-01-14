using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PharmacyPrescriptionApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserCredentials userCredentials)
        {
            // You should validate the user credentials in a real application
            // For demo, we assume the credentials are valid
            if (IsValidUser(userCredentials))
            {
                var token = GenerateToken(userCredentials);
                return Ok(token);
            }
            return Unauthorized();
        }

        private bool IsValidUser(UserCredentials creds)
        {
            // Validate the user credentials with your user management system here
            // This should check if such a user exists and if the password matches
            return true; // Placeholder for demonstration
        }

        private string GenerateToken(UserCredentials creds)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, creds.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            // Add more claims as needed
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120), // Token expiration time
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public class UserCredentials
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}

