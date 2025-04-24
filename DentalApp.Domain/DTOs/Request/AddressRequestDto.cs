using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.DTOs.Request
{
    public class AddressRequestDto{


    [Required, StringLength(50)]
    public string Street { get; set; }
        [Required, StringLength(30)]
        public string City { get; set; }
        [Required, StringLength(20)]
        public string State { get; set; }
        [Required, StringLength(10)]
        public string Zip { get; set; }
    
    }
}
