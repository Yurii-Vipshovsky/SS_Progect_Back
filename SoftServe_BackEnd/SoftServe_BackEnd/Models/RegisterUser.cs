using System;
using System.ComponentModel.DataAnnotations;

namespace SoftServe_BackEnd.Models
{
    public class RegisterUser
    {
        [Required]
        public string NickName;
        
        [Required]
        [EmailAddress]
        public string Email;
        
        [Required]
        public string FullName;
        
        [Required]
        public DateTime Birthday;
        
        [Required]
        public string City;
        
        [Required]
        public string Password;
    }
}