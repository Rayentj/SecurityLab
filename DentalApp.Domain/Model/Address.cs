using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.Model
{
    [Table("Addresses")]
    public class Address
    {

        [Key]
        public int AddressId { get; set; }

        [Required, MaxLength(100)]
        public string Street { get; set; }

        [Required, MaxLength(50)]
        public string City { get; set; }

        [Required, MaxLength(50)]
        public string State { get; set; }

        [Required, MaxLength(10)]
        public string Zip { get; set; }

        public ICollection<Patient> Patients { get; set; }
        public ICollection<Surgery> Surgeries { get; set; }
    }
}
