using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Infra.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(int id)
            : base($"No user found with ID = {id}") { }
    }
}
