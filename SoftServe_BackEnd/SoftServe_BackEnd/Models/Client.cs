using System;
using System.Collections.Generic;

#nullable disable

namespace SoftServe_BackEnd.Models
{
    public class Client
    {
        public Client()
        {
            Events = new HashSet<Event>();
        }

        public string Login { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
        public string City { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsOrganization { get; set; }
        public string Site { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}