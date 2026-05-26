using Microsoft.EntityFrameworkCore;
using vet_data.Context;


namespace WebVeterinaria.BackgroundServices
{
    public class CitaCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CitaCleanupService> _logger;

        public CitaCleanupService(IServiceScopeFactory scopeFactory, ILogger<CitaCleanupService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<VetDbContext>();

                    var hoy = DateOnly.FromDateTime(DateTime.UtcNow);

                    var citasVencidas = await context.Citas
                        .Where(c => c.Date < hoy
                            && c.Status == "pendiente"
                            && c.deleted_at == null)
                        .ToListAsync(stoppingToken);

                    if (citasVencidas.Any())
                    {
                        foreach (var cita in citasVencidas)
                            cita.deleted_at = DateTime.UtcNow;

                        await context.SaveChangesAsync(stoppingToken);
                        _logger.LogInformation($"{citasVencidas.Count} citas vencidas marcadas como eliminadas.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error en CitaCleanupService: {ex.Message}");
                }

                // Espera hasta la medianoche para volver a ejecutar
                var ahora = DateTime.Now;
                var medianoche = ahora.Date.AddDays(1);
                var espera = medianoche - ahora;
                await Task.Delay(espera, stoppingToken);
            }
        }
    }
}
