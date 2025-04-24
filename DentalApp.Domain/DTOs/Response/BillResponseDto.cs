using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.DTOs.Response
{
    public class BillResponseDto
    {
        public int BillId { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PatientName { get; set; }
        public string AppointmentSummary { get; set; }
    }
}
