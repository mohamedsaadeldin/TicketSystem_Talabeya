using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using TicketSystem_Api.DataAccess.Data;
using TicketSystem_Api.DataAccess.Repository.IRepository;
using TicketSystem_Api.Models;

namespace TicketSystem_Api.DataAccess.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _db;
        public TicketRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Ticket> CreateTicketAsync(Ticket ticket)
        {
            await _db.AddAsync(ticket);
            await _db.SaveChangesAsync();
            return ticket;
        }


        public async Task<Ticket> GetTicketByIdAsync(int id)
        {
            return await _db.Tickets.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Ticket>> GetTicketsAsync(int page, int pageSize)
        {
            return _db.Tickets.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task IsHandhandled(Ticket ticket)
        {
            ticket.IsHandled = true;
            _db.Update(ticket);
            await _db.SaveChangesAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task UpdateHandhandledAsync(Ticket ticket)
        {
            _db.Update(ticket);
            await _db.SaveChangesAsync();
        }

        public async Task<int> GetTicketsCount()
        {
            return await _db.Tickets.CountAsync();
        }

    }
}
