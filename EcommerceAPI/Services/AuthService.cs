namespace EcommerceAPI.Services
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using EcommerceAPI.DTO.User;
    using EcommerceAPI.Models;
    using Microsoft.IdentityModel.Tokens;

    public class AuthService
    {

        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task Register(RegisterUserDto dto)
        {
            var exists = _context.Users.Any(u => u.Email == dto.Email);
            if (exists)
                throw new Exception("Email already exists");

            var user = new Users
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<AuthResponseDto> Login(LoginDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            var token = GenerateJwt(user);//Error: CS1503

            return new AuthResponseDto { Token = token };
        }

        private string GenerateJwt(Users user)//Error:CS0246
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])//Error:CS8604
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim("UserId", user.Id.ToString()),//Error: CS1503 Argumento 1: no se puede convertir de 'string' 'System.IO.BinaryReader'
            new Claim(ClaimTypes.Email, user.Email) //Error: CS1503 Argumento 1: no se puede convertir de 'string' 'System.IO.BinaryReader'
        };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}