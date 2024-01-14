using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using PharmacyPrescriptionApi.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace PharmacyPrescriptionApi.Services
{
    public class PaymentCalculationService : BackgroundService
    {
        private readonly ILogger<PaymentCalculationService> _logger;
        private readonly IServiceProvider _services;
        private readonly IConfiguration _configuration;

        public PaymentCalculationService(ILogger<PaymentCalculationService> logger, IServiceProvider services, IConfiguration configuration)
        {
            _logger = logger;
            _services = services;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Payment Calculation Service running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork(stoppingToken);

                _logger.LogInformation("Payment Calculation Service is waiting for the next run.");

                // Run the task every day at 1:00 AM
                var nextRun = DateTime.Today.AddDays(1).AddHours(1);
                var delay = nextRun.Subtract(DateTime.Now);

                await Task.Delay(delay, stoppingToken);
            }
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Payment Calculation Service is working.");

            using (var scope = _services.CreateScope())
            {
                var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();

                await scopedProcessingService.CalculatePayments(stoppingToken);
            }
        }

        public interface IScopedProcessingService
        {
            Task CalculatePayments(CancellationToken stoppingToken);
        }

        public class ScopedProcessingService : IScopedProcessingService
        {
            private readonly PharmacyDbContext _context;
            private readonly IConfiguration _configuration;
            private readonly ILogger<ScopedProcessingService> _logger; // Added logger
            public ScopedProcessingService(PharmacyDbContext context, IConfiguration configuration, ILogger<ScopedProcessingService> logger)
            {
                _context = context;
                _configuration = configuration;
                _logger = logger; // Initialize the logger
            }

            public async Task CalculatePayments(CancellationToken stoppingToken)
            {
                // Logic to calculate payments goes here
                // ...

                // After calculating payments, send emails
                foreach (var pharmacy in _context.Pharmacies.ToList())
                {
                    var totalAmount = _context.Prescriptions
                        .Where(p => p.PharmacyId == pharmacy.PharmacyId)
                        .SelectMany(p => p.PrescriptionMedicines)
                        .Sum(pm => pm.Medicine.Price * pm.Quantity);

                    var emailContent = $"You have submitted prescriptions today. Total amount is {totalAmount}.";
                    await SendEmailAsync(pharmacy.Email, "Daily Prescription Summary", emailContent, stoppingToken);
                }
            }

            private async Task SendEmailAsync(string to, string subject, string body, CancellationToken stoppingToken)
            {
                using (var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpHost"]))
                {
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_configuration["EmailSettings:From"]),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true,
                    };
                    mailMessage.To.Add(to);

                    smtpClient.Port = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                    smtpClient.Credentials = new System.Net.NetworkCredential(_configuration["EmailSettings:SmtpUser"], _configuration["EmailSettings:SmtpPass"]);
                    smtpClient.EnableSsl = true;

                    try
                    {
                        await smtpClient.SendMailAsync(mailMessage, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        // Log the exception
                        _logger.LogError(ex, "An error occurred while sending email to {Email}.", to);
                    }
                }
            }
        }
    }
}