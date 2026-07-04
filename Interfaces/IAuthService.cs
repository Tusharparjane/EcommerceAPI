using EcommerceAPI.DTOs;

public interface IAuthService
{
    UserDto Register(RegisterDto dto);

    string? Login(LoginDto dto);
}