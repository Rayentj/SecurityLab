using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.DTOs.Request
{
    public class SurgeryRequestDto
    {
        [Required, StringLength(50)]
        public string Name { get; set; }
        [Required, Phone]
        public string PhoneNumber { get; set; }
        [Required, StringLength(50)]
        public int AddressId { get; set; }
    }
}
