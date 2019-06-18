using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreCore.Data
{
    public class ApplicationRole : IdentityRole<Guid>
    {

        //public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        //public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }

        public string Description { get; set; }

        public ApplicationRole() : base()
        {
        }
        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}
