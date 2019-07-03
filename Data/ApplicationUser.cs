﻿using Microsoft.AspNetCore.Identity;
using StoreCore.Models.admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreCore.Data
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        //public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
        //public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
        //public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
        //public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
