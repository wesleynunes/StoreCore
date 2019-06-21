using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoreCore.Data;

namespace StoreCore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(u => {
                u.ToTable("Users"); //altera o nome da tabela                
            });

            builder.Entity<ApplicationRole>(r => {
                r.ToTable("Roles"); //altera o nome da tabela
            });

            builder.Entity<IdentityUserRole<Guid>>(ur =>
            {
                ur.ToTable("UserRoles"); // altera o nome da tabela
                //ur.HasKey(k => k.UserId);
                //ur.Property(p => p.Discriminator).HasColumnType("LONGTEXT");
            });

            builder.Entity<IdentityUserLogin<Guid>>(ul =>
            {
                ul.ToTable("UserLogins"); // altera o nome da tabela 

            });

            builder.Entity<IdentityUserClaim<Guid>>(uc =>
            {
                uc.ToTable("UserClaims"); // altera o nome da tabela                
            });

            builder.Entity<IdentityRoleClaim<Guid>>(rc =>
            {
                rc.ToTable("RoleClaims"); // altera o nome da tabela
            });

            builder.Entity<IdentityUserToken<Guid>>(ut =>
            {
                ut.ToTable("UserTokens"); // altera o nome da tabela               
            });

        }
          

        public DbSet<ApplicationUserRole> ApplicationUserRole { get; set; }
          

        public DbSet<StoreCore.Data.ApplicationUserClaim> ApplicationUserClaim { get; set; }
    }
}
