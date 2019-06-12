using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreCore.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.UI.Services;
using StoreCore.Areas.Services;
using StoreCore.Interface;

namespace StoreCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    Configuration.GetConnectionString("DefaultConnection")));
         
            services.AddIdentity<ApplicationUser, ApplicationRole>(options => {

                // Lockout settings 
                // Se no controller login na class OnPostAsync no lockoutOnFailure estiver como false mudar para true
                // Bloquea o acesso do usuario se tiver muitas tentativas de erro 
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                
                
                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;

            })
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultUI(UIFramework.Bootstrap4)
              .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });


            //Autenticacao Facebook
            //Para registrar os tokens na tabela tem que alterar o controller ExternalLogin no metodo OnPostConfirmationAsync e criar um foreach 
            //foreach (var token in info.AuthenticationTokens)
            //{
            //  await _userManager.SetAuthenticationTokenAsync(user, info.LoginProvider, token.Name, token.Value);
            //}
            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                facebookOptions.ClaimActions.MapJsonKey(ClaimTypes.Locality, "locale");
                facebookOptions.SaveTokens = true;
            })

            //Autenticacao Google
            .AddGoogle(googleOptions =>
             {
                 googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                 googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                 googleOptions.SaveTokens = true;
             })

            //Autenticacao Microsoft
            .AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = Configuration["Authentication:Microsoft:ClientId"];
                microsoftOptions.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
                microsoftOptions.SaveTokens = true;
            });

            // Configuração email
            // Instalar pelo nuget o pacote SendGrid para gerenciar emails
            // using Microsoft.AspNetCore.Identity.UI.Services;
            // using WebPWrecover.Services;
            // Comentar a linha no controller register no OnPostAsync Areas/Identity/Pages/Account/Register.cshtml.cs.
            // Linha comentada await _signInManager.SignInAsync(user, isPersistent: false);
            // Esta linha nao permite o usuario logar automaticamente depois de se registrar
            //services.AddSingleton<IEmailSender, EmailSender>();
            //services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

                      

            // Add Database Initializer
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDbInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            // Add Database Initializer
            dbInitializer.Initialize();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
