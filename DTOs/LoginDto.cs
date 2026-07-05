using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs;

public class LoginDto
{
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = "";

    [Required]
    [MinLength(6)]
    [MaxLength(100)]
    public string Password { get; set; } = "";
}