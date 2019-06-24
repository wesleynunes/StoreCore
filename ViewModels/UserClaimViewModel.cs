using StoreCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreCore.ViewModels
{
    public class UserClaimViewModel
    {

        public int Id { get; set; }

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ApplicationUserClaim UserClaim { get; set; }

    }
}
