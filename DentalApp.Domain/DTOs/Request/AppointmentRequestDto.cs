using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.DTOs.Request
{
    public class AppointmentRequestDto
    {
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public int SurgeryId { get; set; }
        [Required]
        public int DentistId { get; set; }
        [Required]
        public int PatientId { get; set; }
    }
}
