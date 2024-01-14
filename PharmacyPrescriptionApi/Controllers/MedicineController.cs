using Microsoft.AspNetCore.Mvc;
using PharmacyPrescriptionApi.Models;
using PharmacyPrescriptionApi.Data;
using System.Linq;

namespace PharmacyPrescriptionApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MedicineController : ControllerBase
    {
        private readonly PharmacyDbContext _context;

        public MedicineController(PharmacyDbContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public IActionResult SearchMedicine(string name)
        {
            var medicines = _context.Medicines
                                .Where(m => m.Name.Contains(name))
                                .ToList();

            if (medicines == null || !medicines.Any())
            {
                return NotFound("No medicines found with the provided name.");
            }

            return Ok(medicines);
        }
    }
}
