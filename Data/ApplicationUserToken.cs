﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreCore.Data
{
    public class ApplicationUserToken : IdentityUserToken<Guid>
    {
        //public virtual ApplicationUser User { get; set; }
    }
}
