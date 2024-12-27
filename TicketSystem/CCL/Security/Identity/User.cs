using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL.Security.Identity
{
    public class User
    {
        public int UserId { get; init; }
        public List<Role> Roles { get; init; }

        public User(int userId, params Role[] roles)
        {
            UserId = userId;
            Roles = new List<Role>(roles);
        }
    }
}
