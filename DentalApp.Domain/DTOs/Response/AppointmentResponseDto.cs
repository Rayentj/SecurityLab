using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.DTOs.Response
{
    public class AppointmentResponseDto
    {
        public int AppointmentId { get; set; }
        public DateTime DateTime { get; set; }
        public string DentistName { get; set; }
        public string PatientName { get; set; }
        public string SurgeryName { get; set; }

        public string Status { get; set; } = "Pending"; // or use an enum

    }
}
