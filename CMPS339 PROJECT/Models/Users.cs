using System.ComponentModel.DataAnnotations;

namespace CMPS339_PROJECT.Models
{
    public class Users
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Username { get; set; }
        public string Password { get; set; }

        public string IsActive { get; set; }
    }
    public class UsersDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Username { get; set; }
        public string Password { get; set; }

        public string IsActive { get; set; }
    }


    public class UsersGetDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string IsActive { get; set; }
    }

    public class UserssCreateDto
    {
        [Required]
        [MaxLength(30)]
        public string Username { get; set; }
        public string Password { get; set; }

        public string IsActive { get; set; }
    }


    public class UsersCreateUpdateDto
    {
        [Required]
        [MaxLength(30)]
        public string Username { get; set; }
        public string Password { get; set; }

        public string IsActive { get; set; }

    }

}

