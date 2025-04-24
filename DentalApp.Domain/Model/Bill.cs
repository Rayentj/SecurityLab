using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.Model
{
    [Table("Bills")]
    public class Bill
    {
        [Key]
        public int BillId { get; set; }

        [Required]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        [Required]
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public bool IsPaid { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
