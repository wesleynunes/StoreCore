using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoreCore.Data;
using StoreCore.Services;
using StoreCore.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StoreCore.Controllers.admin
{
    [Route("Admin/Users")]
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
        [HttpGet("")]
        //[HttpGet("Index")]
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
        [HttpGet("Details")]
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
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }


        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel viewmodel)
        {            
            if (ModelState.IsValid)
            {

                var user = new ApplicationUser { UserName = viewmodel.Email, Email = viewmodel.Email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, viewmodel.Password);

                if (result.Succeeded)
                {
                    TempData["MessageOk"] = "Usuário cadastrado com sucesso";
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


        // GET: Users/Edit/5
        [HttpGet("Edit")]
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
        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ApplicationUser applicationUser)
        {
            //pega id do usuario 
            ApplicationUser userId  = _context.Users.Find(applicationUser.Id);

            //validação campo vazio
            if (applicationUser.UserName == null)
            {
                ModelState.AddModelError("UserName", "O campo Usuário é obrigatório.");
                return View(applicationUser);
            }

            // validação id invalido
            if (id != applicationUser.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    userId.UserName = applicationUser.UserName;
                    userId.Email = applicationUser.UserName;
                    userId.EmailConfirmed = applicationUser.EmailConfirmed;
                    userId.TwoFactorEnabled = applicationUser.TwoFactorEnabled;
                    userId.LockoutEnd = applicationUser.LockoutEnd;
                    userId.LockoutEnabled = applicationUser.LockoutEnabled;
                    userId.AccessFailedCount = applicationUser.AccessFailedCount;

                    if (!CheckUserName(applicationUser.UserName, applicationUser.Id))
                    {
                        await _userManager.UpdateAsync(userId);
                        await _context.SaveChangesAsync();
                        TempData["MessageOk"] = "Usuário atualizado com sucesso";
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "Este Usuário já está em uso");
                        return View(applicationUser);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUser);
        }


        // GET: Users/Edit/5
        [HttpGet("ChangePassword")]
        public async Task<IActionResult> ChangePassword(Guid? id)
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
        [HttpPost("ChangePassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(Guid id, ApplicationUser applicationUser)
        {
            //pega id do usuario 
            ApplicationUser userId = _context.Users.Find(applicationUser.Id);

            if (applicationUser.PasswordHash == null)
            {
                ModelState.AddModelError("PasswordHash", "O campo senha é obrigatório.");
                return View(applicationUser);
            }           

            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                userId.PasswordHash = Hash.HashPassword(applicationUser.PasswordHash);

                await _userManager.UpdateAsync(userId);
                await _context.SaveChangesAsync();
                TempData["MessageOk"] = "Senha alterada com sucesso";
                return RedirectToAction(nameof(Index));
               
            }
            return View(applicationUser);
        }

        // GET: Users/Delete/5
        [HttpGet("Delete")]
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
        [HttpPost("Delete"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var applicationUser = await _context.Users.FindAsync(id);
            _context.Users.Remove(applicationUser);
            await _context.SaveChangesAsync();
            TempData["MessageOk"] = "Usuário deletado";
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        public bool CheckUserName(string name, Guid id)
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
