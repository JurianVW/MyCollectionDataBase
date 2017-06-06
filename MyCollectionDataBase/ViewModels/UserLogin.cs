using Models;
using System.ComponentModel.DataAnnotations;

namespace MyCollectionDataBase.ViewModels
{
    public class UserLogin : User
    {
        [Required(ErrorMessage = "Username is required")]
        public override string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public override string Password { get; set; }
    }
}