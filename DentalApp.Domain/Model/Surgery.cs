using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.Model
{
    [Table("Surgeries")]
    public class Surgery
    {
        [Key]
        public int SurgeryId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(20)]
        public string PhoneNumber { get; set; }

        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public Address Address { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
