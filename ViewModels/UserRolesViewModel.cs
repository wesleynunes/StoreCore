using StoreCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreCore.ViewModels
{
    public class UserRolesViewModel
    {

        //public IEnumerable<ApplicationUser> User { get; set; }

        //public IEnumerable<ApplicationRole> Role { get; set; }

        //public IEnumerable<ApplicationUserRole> UserRoles { get; set; }

        public Guid UserRoleId { get; set; }

        public Guid UserId { get; set; }

        public Guid RoleId { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ApplicationRole Role { get; set; }

    }
}
