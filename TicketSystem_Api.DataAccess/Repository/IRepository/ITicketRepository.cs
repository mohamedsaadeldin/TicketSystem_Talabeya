
using TicketSystem_Api.Models;

namespace TicketSystem_Api.DataAccess.Repository.IRepository
{
    public interface ITicketRepository
    {
        Task<Ticket> CreateTicketAsync(Ticket ticket);
        Task<IEnumerable<Ticket>> GetTicketsAsync(int page, int pageSize);
        Task IsHandhandled(Ticket ticket);
        Task UpdateHandhandledAsync(Ticket ticket);
        Task<Ticket> GetTicketByIdAsync(int id);
        Task SaveAsync();
        public Task<int> GetTicketsCount();
    }
}
