using System;
using System.ComponentModel.DataAnnotations;

namespace SoftServe_BackEnd.Models
{
    public class RegisterUser
    {
        [Required]
        public string NickName{ get; set; }
        
        [Required]
        [EmailAddress]
        public string Email{ get; set; }
        
        [Required]
        public string FullName{ get; set; }
        
        [Required]
        public DateTime Birthday{ get; set; }
        
        [Required]
        public string City{ get; set; }
        
        [Required]
        public string Password{ get; set; }

        [Required]
        public string PhoneNumber{ get; set; }
        
        [Required] 
        public bool IsOrganization{ get; set; }
        
        [MaxLength(50)]
        public string SiteUrl{ get; set; }
    }
}