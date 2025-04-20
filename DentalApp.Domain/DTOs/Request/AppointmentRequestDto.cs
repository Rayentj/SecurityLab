using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.DTOs.Request
{
    public class AppointmentRequestDto
    {
        public DateTime DateTime { get; set; }
        public int SurgeryId { get; set; }
        public int DentistId { get; set; }
        public int PatientId { get; set; }
    }
}
