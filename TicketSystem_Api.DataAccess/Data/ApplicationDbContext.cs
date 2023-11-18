using Microsoft.EntityFrameworkCore;
using TicketSystem_Api.Models;

namespace TicketSystem_Api.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {  
        }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
