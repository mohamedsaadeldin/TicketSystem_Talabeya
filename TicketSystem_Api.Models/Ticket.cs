
namespace TicketSystem_Api.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public DateTime CreationDateTime { get; set; } = DateTime.Now;
        public string PhoneNumber { get; set; }
        public string Governorate { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public bool IsHandled { get; set; } = false;
    }
}
