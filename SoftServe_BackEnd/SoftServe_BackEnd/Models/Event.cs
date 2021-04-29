using System;

#nullable disable

namespace SoftServe_BackEnd.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }

        public virtual Client CreatedByNavigation { get; set; }
    }
}