
namespace TicketSystem_Api.Utility
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Ticket, TicketDto>().ReverseMap();
            CreateMap<Ticket, TicketChangeStatusDto>().ReverseMap();
        }
    }
}
