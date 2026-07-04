using EcommerceAPI.Models;

namespace EcommerceAPI.Interfaces;

public interface IUserRepository
{
    User? GetByEmail(string email);

    User? GetById(int id);

    User Add(User user);
}