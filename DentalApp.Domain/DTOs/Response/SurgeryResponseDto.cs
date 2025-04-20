using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.DTOs.Response
{
    public class SurgeryResponseDto
    {
        public int SurgeryId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressSummary { get; set; }
    }
}
