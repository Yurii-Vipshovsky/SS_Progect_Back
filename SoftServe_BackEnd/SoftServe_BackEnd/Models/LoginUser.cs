using System.ComponentModel.DataAnnotations;

namespace SoftServe_BackEnd.Models
{
    public class LoginUser
    {
        [Required]
        public string LoginString;

        [Required]
        public string Password;
    }
}