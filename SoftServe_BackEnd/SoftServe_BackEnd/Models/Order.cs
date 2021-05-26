namespace SoftServe_BackEnd.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int EventId { get; set; }
        public string UserLogin { get; set; }

        public virtual Event Event { get; set; }
        public virtual Client UserLoginNavigation { get; set; }
    }
}
