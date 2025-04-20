using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.Model
{
    [Table("Dentists")]
    public class Dentist
    {
        [Key]
        public int DentistId { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public DateTime Dob { get; set; }

        [Required, MaxLength(100)]
        public string Specialization { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
