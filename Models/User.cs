using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //  [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmationPassword { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string ProfilePicture { get; set; }

        public List<User> friends;
        public List<List> lists;

        public User()
        {
        }

        public User(int id, string username, string emailAddress, string password, string profilePicture, List<User> friends, List<List> lists)
        {
            this.ID = id;
            this.Username = username;
            this.EmailAddress = emailAddress;
            this.Password = password;
            this.ProfilePicture = profilePicture;
            this.friends.AddRange(friends);
            this.lists.AddRange(lists);
        }
    }
}