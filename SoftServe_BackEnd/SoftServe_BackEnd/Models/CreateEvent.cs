namespace SoftServe_BackEnd.Models
{
    public class CreateEvent
    {
        public string CreatedBy { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Description { get; set; }
        public TypeOfVolunteer Type { get; set; }
    }
}