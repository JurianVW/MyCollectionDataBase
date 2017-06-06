using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace MyCollectionDataBase.ViewModels
{
    public class UserSettings : User
    {
        public override int ID { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public override string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public override string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public override string Password { get; set; }

        public override string ProfilePicture { get; set; }
    }
}