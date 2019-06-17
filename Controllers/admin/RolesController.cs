using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoreCore.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StoreCore.Controllers.admin
{
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<UsersController> _logger;
        private readonly IEmailSender _emailSender;

        public RolesController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<UsersController> logger,
            IEmailSender emailSender
            )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Roles.ToListAsync());
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationRole = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationRole == null)
            {
                return NotFound();
            }

            return View(applicationRole);
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationRole applicationRole)
        {

            if(applicationRole.Name == null)
            {
                ModelState.AddModelError("Name", "O campo Name é obrigatório.");
                return View(applicationRole);
            }

            if (ModelState.IsValid)
            {
                if (!CheckRoleName(applicationRole.Name, applicationRole.Id))
                {
                    //applicationRole.Id = Guid.NewGuid();
                    //applicationRole.NormalizedName = applicationRole.Name;
                    //_context.Add(applicationRole);
                    await _roleManager.CreateAsync(applicationRole);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Name", "Este Nome já está em uso");
                    return View(applicationRole);
                }
            }
            return View(applicationRole);


            //if (ModelState.IsValid)
            //{
            //    applicationRole.Id = Guid.NewGuid();
            //    _context.Add(applicationRole);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(applicationRole);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationRole = await _context.Roles.FindAsync(id);
            if (applicationRole == null)
            {
                return NotFound();
            }
            return View(applicationRole);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ApplicationRole applicationRole)
        {
            if (id != applicationRole.Id)
            {
                return NotFound();
            }

            if(applicationRole.Name == null)
            {
                ModelState.AddModelError("Name", "O campo Name é obrigatório.");
                return View(applicationRole);
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (!CheckRoleName(applicationRole.Name, applicationRole.Id))
                    {
                        _context.Update(applicationRole);
                        //applicationRole.NormalizedName = applicationRole.Name;
                        //await _roleManager.UpdateAsync(applicationRole);
                        await _roleManager.UpdateNormalizedRoleNameAsync(applicationRole);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ModelState.AddModelError("Name", "Este Nome já está em uso");
                        return View(applicationRole);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationRoleExists(applicationRole.Id))
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
            return View(applicationRole);
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationRole = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationRole == null)
            {
                return NotFound();
            }

            return View(applicationRole);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var applicationRole = await _context.Roles.FindAsync(id);
            _context.Roles.Remove(applicationRole);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationRoleExists(Guid id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }

        public bool CheckRoleName(string name, Guid id)
        {
            var DoesExistcategory = (from u in _context.Roles
                                     where u.Name == name
                                     where u.Id != id
                                     select u).FirstOrDefault();
            if (DoesExistcategory != null)
                return true;
            else
                return false;
        }
    }
}
