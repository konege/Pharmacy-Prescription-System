using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyPrescriptionApi.Models
{
    public class PrescriptionMedicine
    {
        [Key]
        public int PrescriptionMedicineId { get; set; }

        [Required]
        [ForeignKey("Prescription")]
        public int PrescriptionId { get; set; }

        [Required]
        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }

        public int Quantity { get; set; }

        // Navigation properties
        public Prescription Prescription { get; set; }
        public Medicine Medicine { get; set; }
    }
}
