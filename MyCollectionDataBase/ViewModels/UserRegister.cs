using System.ComponentModel.DataAnnotations;
using Models;

namespace MyCollectionDataBase.ViewModels
{
    public class UserRegister : User
    {
        [Required(ErrorMessage = "Username is required")]
        public override string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public override string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public override string Password { get; set; }

        [Required(ErrorMessage = "Confirmation Password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmationPassword { get; set; }
    }
}