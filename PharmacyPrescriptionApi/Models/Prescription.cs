using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyPrescriptionApi.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [ForeignKey("Pharmacy")]
        public int PharmacyId { get; set; }

        // Navigation property for Entity Framework Core
        public Pharmacy Pharmacy { get; set; }

        // This will reference the collection of medicine items in the prescription
        public List<PrescriptionMedicine> PrescriptionMedicines { get; set; }
    }
}
