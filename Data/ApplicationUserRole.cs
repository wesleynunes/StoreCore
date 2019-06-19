using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreCore.Data
{
    public class ApplicationUserRole : IdentityUserRole<Guid>
    {
        public Guid UserRoleId { get; set; }

        //public virtual ApplicationUser User { get; set; }
        //public virtual ApplicationRole Role { get; set; }       
    }
}
