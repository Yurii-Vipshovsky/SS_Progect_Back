using System.ComponentModel.DataAnnotations;

namespace SoftServe_BackEnd.Models
{
    public class LoginUser
    {
        [Required]
        [EmailAddress] 
        public string Email;

        [Required]
        public string Password;
    }
}