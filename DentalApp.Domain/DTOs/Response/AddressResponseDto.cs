using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.DTOs.Response
{
    public class AddressResponseDto
    {
        public int AddressId { get; set; }
        public string FullAddress { get; set; }
        public List<string> PatientNames { get; set; }
        public List<string> SurgeryNames { get; set; }
    }
}
