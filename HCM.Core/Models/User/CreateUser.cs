using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCM.Core.Models.User
{
    public class CreateUser
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public int EmployeeId { get; set; }
    }
}
