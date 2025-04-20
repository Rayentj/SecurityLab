using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.DTOs.Request
{
    public class SurgeryRequestDto
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int AddressId { get; set; }
    }
}
