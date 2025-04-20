using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.DTOs.Request
{
    public class DentistRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime Dob { get; set; }
        public string Specialization { get; set; }
    }
}
