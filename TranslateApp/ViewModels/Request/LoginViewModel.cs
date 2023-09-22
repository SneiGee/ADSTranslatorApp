using System.ComponentModel.DataAnnotations;

namespace TranslateApp.ViewModels.Request;

public class LoginViewModel
{
    [Display(Name = "Email Address")]
    [Required(ErrorMessage = "Email address is required")]
    public string? EmailAddress { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}