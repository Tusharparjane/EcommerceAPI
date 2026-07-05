using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs;

public class RegisterDto
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string FirstName { get; set; } = "";

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string LastName { get; set; } = "";

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = "";

    [Required]
    [MinLength(6)]
    [MaxLength(100)]
    public string Password { get; set; } = "";
}