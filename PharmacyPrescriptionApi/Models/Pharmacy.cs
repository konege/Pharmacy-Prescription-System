using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PharmacyPrescriptionApi.Models
{
    public class Pharmacy
    {
        [Key]
        public int PharmacyId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // Assuming each pharmacy has unique email and password for authentication
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Navigation property for Entity Framework Core
        public List<Prescription> Prescriptions { get; set; }
    }
}
