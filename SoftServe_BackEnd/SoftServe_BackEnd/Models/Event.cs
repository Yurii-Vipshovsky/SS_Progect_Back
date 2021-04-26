using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftServe_BackEnd.Models
{
    [Table("event")]
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required] 
        [Column("created_by")]
        public User CreatedBy { get; set; }
        
        [Required]
        [Column("name")]
        [MaxLength(100)]
        public string EventName { get; set; }
        
        [Required]
        [Column("type")]
        public VolunteerType TypeOfVolunteer { get; set; }

        [Required]
        [Column("place")]
        [MaxLength(100)]
        public string Location { get; set; }
        
        [Required]
        [Column("description")]
        public string AdditionalInfo { get; set; }
        
        [Required]
        [Column("date")]
        public DateTime CarryingOutTime { get; set; }

    }
}