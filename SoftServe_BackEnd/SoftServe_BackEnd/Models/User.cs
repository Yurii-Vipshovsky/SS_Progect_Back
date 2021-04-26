using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SoftServe_BackEnd.Models
{
    [Table("client")]
    public class User: IdentityUser
    {
        [Required]
        [Column("login")]
        [MaxLength(20)]
        public override string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Column("email")]
        public override string Email { get; set; }
        
        [Required]
        [Column("name")]
        [MaxLength(50)]
        public string FullName{ get; set; }
        
        [Column("birthday")]
        public DateTime Birthday{ get; set; }
        
        [Column("city")]
        [MaxLength(20)]
        public string City{ get; set; }

        [Required]
        [Column("password")]
        [MaxLength(50)]
        public override string PasswordHash { get; set; }

        [Required]
        [Column("phone_number")]
        public override string PhoneNumber { get; set; }
        
        [Required] 
        [Column("is_organization")]
        public bool IsOrganization{ get; set; }
        
        [Column("site")]
        [MaxLength(50)]
        public string SiteUrl{ get; set; }
    }
}