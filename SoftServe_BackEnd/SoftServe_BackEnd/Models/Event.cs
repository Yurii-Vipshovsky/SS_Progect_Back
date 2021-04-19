using System;
using System.ComponentModel.DataAnnotations;

namespace SoftServe_BackEnd.Models
{
    public class Event
    {
        [Required] 
        public bool IsOrganization;

        public string OrganizationName;
        public string SiteUrl;

        public string FullName;
        [Phone]
        public string PhoneNumber;
        
        [Required]
        [EmailAddress]
        public string Email;
        
        [Required]
        public string EventName;
        
        [Required]
        public VolunteerType TypeOfVolunteer;
        
        [Required]
        public string Location;
        
        [Required]
        public DateTime CarryingOutTime;
        
        [Required]
        public string AdditionalInfo;
    }
}