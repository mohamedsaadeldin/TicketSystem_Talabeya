namespace TicketSystem_Api
{
    public class TicketHandlingService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TicketHandlingService> _logger;

        public TicketHandlingService(IServiceScopeFactory scopeFactory, ILogger<TicketHandlingService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            await MarkTicketsAsHandledAsync();
        }

        private async Task MarkTicketsAsHandledAsync()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var ticketRepository = scope.ServiceProvider.GetRequiredService<ITicketRepository>();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var currentDateTime = DateTime.Now;
                var tickets = await dbContext.Tickets.ToListAsync();
                foreach (var ticket in tickets)
                {
                    var elapsedMinutes = (currentDateTime - ticket.CreationDateTime).TotalMinutes;

                    if (elapsedMinutes >= 60 && !ticket.IsHandled)
                    {
                        ticket.IsHandled = true;
                        try
                        {
                            await ticketRepository.UpdateHandhandledAsync(ticket);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Error updating ticket ID {ticket.Id}: {ex.Message}", ex);
                        }
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}