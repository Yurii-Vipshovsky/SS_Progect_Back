using System.ComponentModel.DataAnnotations;

namespace SoftServe_BackEnd.Models
{
    public class LoginUser
    {
        [Required]
        public string Email{ get; set; }

        [Required]
        public string Password{ get; set; }
    }
}