using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftServe_BackEnd.Models
{
    [Table("event")]
    public class Event
    {
        [Key] 
        public int Id;
        
        [Required] 
        [Column("created_by")]
        [MaxLength(20)]
        public User CreatedBy;
        
        [Required]
        [Column("name")]
        [MaxLength(100)]
        public string EventName;
        
        [Required]
        [Column("type")]
        public VolunteerType TypeOfVolunteer;

        [Required]
        [Column("place")]
        [MaxLength(100)]
        public string Location;
        
        [Required]
        [Column("description")]
        public string AdditionalInfo;
        
        [Required]
        [Column("date")]
        public DateTime CarryingOutTime;

    }
}