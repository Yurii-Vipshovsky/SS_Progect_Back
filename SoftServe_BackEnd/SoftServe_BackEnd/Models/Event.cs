﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
        public TypeOfVolunteer Type { get; set; }
        public Client CreatedByNavigation;
        public ICollection<Order> Orders;
        
        [NotMapped]
        public string StringType
        {
            get => Type.ToString();
        }
    }
}