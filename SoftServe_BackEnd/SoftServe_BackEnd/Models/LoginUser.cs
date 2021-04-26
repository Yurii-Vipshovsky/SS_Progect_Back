using System.ComponentModel.DataAnnotations;

namespace SoftServe_BackEnd.Models
{
    public class LoginUser
    {
        [Required]
        public string LoginString{ get; set; }

        [Required]
        public string Password{ get; set; }
    }
}