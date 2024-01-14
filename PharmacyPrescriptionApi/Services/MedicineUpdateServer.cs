// MedicineUpdateService.cs
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PharmacyPrescriptionApi.Data;
using Microsoft.Extensions.DependencyInjection;

namespace PharmacyPrescriptionApi.Services
{
    public class MedicineUpdateService : BackgroundService
    {
        private readonly ILogger<MedicineUpdateService> _logger;
        private readonly IServiceProvider _services;
        private readonly string _medicineListUrl = "https://www.tick.gov.tr/dinamikmodul/43";

        public MedicineUpdateService(ILogger<MedicineUpdateService> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<PharmacyDbContext>();
                    var httpClient = scope.ServiceProvider.GetRequiredService<HttpClient>();

                    try
                    {
                        var response = await httpClient.GetStringAsync(_medicineListUrl);
                        var medicines = JArray.Parse(response); // Assuming the response is a JSON array

                        foreach (var medicine in medicines)
                        {
                            // Assume each medicine is just a string. Adjust accordingly if it's more complex.
                            var medicineName = medicine.ToString();
                            // Add or update the medicine in the database
                        }

                        await dbContext.SaveChangesAsync(stoppingToken);
                        _logger.LogInformation("Medicine list updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error updating medicine list.");
                    }
                }

                // Calculate the delay until the next update (1 week in this case)
                var nextRun = DateTime.Now.AddDays(7);
                var delay = nextRun.Subtract(DateTime.Now);
                await Task.Delay(delay, stoppingToken);
            }
        }
    }
}
