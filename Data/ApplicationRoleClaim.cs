﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreCore.Data
{
    public class ApplicationRoleClaim : IdentityRoleClaim<Guid>
    {
        //public virtual ApplicationRole Role { get; set; }
    }
}
