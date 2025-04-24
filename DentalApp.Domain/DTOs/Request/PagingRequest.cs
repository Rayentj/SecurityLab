using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.DTOs.Request
{
    public class PagingRequest
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public string SortBy { get; set; } = "Id"; // Can be overridden
        public bool Desc { get; set; } = false;
    }

}
