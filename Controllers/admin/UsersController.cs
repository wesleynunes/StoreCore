using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoreCore.Data;
using StoreCore.Services;
using StoreCore.ViewModels;

namespace StoreCore.Controllers.admin
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UsersController> _logger;
        private readonly IEmailSender _emailSender;

        public UsersController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<UsersController> logger,
            IEmailSender emailSender
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        // GET: Users
        public async Task<IActionResult> Index(
            string sortOrder, 
            string searchString,
            string currentFilter,
            int? pageNumber)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CurrentSort"] = sortOrder;
           

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var users = from s in _context.Users
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.UserName.Contains(searchString)
                                       || s.Email.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(s => s.UserName);
                    break;               
                default:
                    users = users.OrderBy(s => s.UserName);
                    break;
            }
            int pageSize = 3;
            return View(await PaginatedList<ApplicationUser>.CreateAsync(users.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
                       

        // GET: Users/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }


        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel viewmodel)
        {            
            if (ModelState.IsValid)
            {

                var user = new ApplicationUser { UserName = viewmodel.Email, Email = viewmodel.Email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, viewmodel.Password);

                if (result.Succeeded)
                {
                    TempData["MessageUser"] = "Usuário cadastrado com sucesso";
                    //return View(viewmodel);
                    return RedirectToAction("Index", "Users");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(viewmodel);


        }


        //// POST: Users/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        applicationUser.Id = Guid.NewGuid();
        //        _context.Add(applicationUser);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(applicationUser);
        //}

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.Users.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            return View(applicationUser);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ApplicationUser applicationUser)
        {

            // pegar usuario logado id ou username 
            //var Userlogin = await _userManager.GetUserAsync(User);
            //var userId = await _userManager.GetUserIdAsync(user);


            ApplicationUser userId  = _context.Users.Find(applicationUser.Id);

            

            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!CheckUser(applicationUser.UserName, applicationUser.Id))
                {
                    userId.UserName = applicationUser.UserName;
                    userId.Email = applicationUser.Email;
                    userId.EmailConfirmed = applicationUser.EmailConfirmed;
                    userId.PasswordHash = Hash.HashPassword(applicationUser.PasswordHash);
                    userId.TwoFactorEnabled = applicationUser.TwoFactorEnabled;
                    userId.LockoutEnd = applicationUser.LockoutEnd;
                    userId.LockoutEnabled = applicationUser.LockoutEnabled;
                    userId.AccessFailedCount = applicationUser.AccessFailedCount;

                    if (userId.PasswordHash != null)
                    {
                        userId.PasswordHash = Hash.HashPassword(applicationUser.PasswordHash);
                    }
                    
                    //_context.Update(userId);
                    await _userManager.UpdateAsync(userId);
                    await _context.SaveChangesAsync();                   
                    TempData["MessagePanelCategory"] = "Categoria atualizada com sucesso";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Name", "Esta categoria já está em uso");
                    return View(applicationUser);
                }
            }
            return View(applicationUser);
        }


        //// POST: Users/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(Guid id, [Bind("Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser)
        //{
        //    if (id != applicationUser.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(applicationUser);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ApplicationUserExists(applicationUser.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(applicationUser);
        //}

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var applicationUser = await _context.Users.FindAsync(id);
            _context.Users.Remove(applicationUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        public bool CheckUser(string name, Guid id)
        {
            var DoesExistcategory = (from u in _context.Users
                                     where u.UserName == name
                                     where u.Id != id
                                     select u).FirstOrDefault();
            if (DoesExistcategory != null)
                return true;
            else
                return false;
        }
    }
}
