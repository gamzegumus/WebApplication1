using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data
{
    public class Users:IdentityUser
    {
        //[MaxLength(50)]
        //public string Name { get; set; } = null!;

        //[MaxLength(50)]
        //public string Surname { get; set; } = null!;


        //[MaxLength(50)]
        //public string Username { get; set; } = null!;
        //public string EMail { get; set; }
        //public string Password { get; set; }
        //public string Password_tekrar { get; set; }
        //public object RefreshToken { get; internal set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
