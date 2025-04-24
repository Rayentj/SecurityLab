using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.DTOs.Request
{
    public class BillRequestDto
    {
        [Required]
        public int AppointmentId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public bool IsPaid { get; set; }
    }
}
