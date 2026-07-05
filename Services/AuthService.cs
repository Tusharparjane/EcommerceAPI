using Microsoft.Extensions.Logging;
using BCrypt.Net;
using EcommerceAPI.DTOs;
using EcommerceAPI.Interfaces;
using EcommerceAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EcommerceAPI.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
     IUserRepository userRepository,
     IConfiguration configuration,
     ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public UserDto Register(RegisterDto dto)
    {
        var existingUser = _userRepository.GetByEmail(dto.Email);

        if (existingUser != null)
        {
            _logger.LogWarning(
                "Registration failed. Email {Email} already exists.",
                dto.Email);

            throw new Exception("Email already exists.");
        }

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "Customer"
        };

        _userRepository.Add(user);
        _logger.LogInformation(
    "User {Email} registered successfully.",
    user.Email);

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role
        };
    }

    public string? Login(LoginDto dto)
    {
        var user = _userRepository.GetByEmail(dto.Email);

        if (user == null)
        {
            _logger.LogWarning(
                "Login failed. User with email {Email} not found.",
                dto.Email);

            return null;
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            _logger.LogWarning(
                "Login failed. Invalid password for {Email}.",
                dto.Email);

            return null;
        }

        // JWT token will be added in the next step.
        var claims = new[]
 {
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(ClaimTypes.Email, user.Email),
    new Claim(ClaimTypes.Role, user.Role)
};

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(
                Convert.ToDouble(_configuration["Jwt:DurationInMinutes"])
            ),
            signingCredentials: credentials
        );
        _logger.LogInformation(
    "User {Email} logged in successfully.",
    user.Email);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}