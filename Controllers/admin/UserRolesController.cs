using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreCore.Data;
using StoreCore.ViewModels;

namespace StoreCore.Controllers.admin
{
    public class UserRolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRolesController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }


        //// GET: UserRoles
        //public async Task<IActionResult> Index()
        //{
        //    UserRolesViewModel userRoleViewModel = new UserRolesViewModel();

        //    return View(userRoleViewModel);

        //    //return View(await _context.ApplicationUserRole.ToListAsync());
        //}

        // GET: UserRoles
        public async Task<IActionResult> Index()
        {
            List<UserRolesViewModel> listUserRoles = new List<UserRolesViewModel>();

            var UserRoleList = await (from ur in _context.UserRoles
                                      join u in _context.Users on ur.UserId equals u.Id
                                      join r in _context.Roles on ur.RoleId equals r.Id
                                      select new
                                      {
                                          u.UserName,
                                          r.Name,
                                          ur.UserId,
                                          ur.RoleId
                                      }).ToListAsync();

            foreach (var item in UserRoleList)
            {
                UserRolesViewModel dataList = new UserRolesViewModel
                {
                    UserName = item.UserName,
                    Name = item.Name,
                    UserId = item.UserId,
                    RoleId = item.RoleId
                };

                listUserRoles.Add(dataList);
            }
            return View(listUserRoles);
        }


        // GET: UserRoles/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var details = _context.UserRoles

            .Select(p => new UserRolesViewModel
            {
                UserId = p.UserId,
                RoleId = p.RoleId,
            }).Where(p => p.UserId == id).FirstOrDefault();//Executes the query

            return View(details);
        }


        // GET: UserRoles/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return View();
        }

        // POST: UserRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUserRole applicationUserRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationUserRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUserRole);
        }

        // GET: UserRoles/Edit/5
        //public async Task<IActionResult> Edit(Guid? id)
        public IActionResult Edit(Guid? id)
        {

            var details = _context.UserRoles

            .Select(p => new UserRolesViewModel
            {
                UserId = p.UserId,
                RoleId = p.RoleId,
            }).Where(p => p.UserId == id).FirstOrDefault();//Executes the query

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return View(details);
           
        }

        // POST: UserRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ApplicationUserRole applicationUserRole)
        {
            if (id != applicationUserRole.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationUserRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserRoleExists(applicationUserRole.UserId))
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
            return View(applicationUserRole);
        }

        // GET: UserRoles/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            var details = _context.UserRoles

             .Select(p => new UserRolesViewModel
             {
                 UserId = p.UserId,
                 RoleId = p.RoleId,
             }).Where(p => p.UserId == id).FirstOrDefault();//Executes the query

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return View(details);
        }

        // POST: UserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            var details = _context.UserRoles

            .Select(p => new UserRolesViewModel
            {
                UserId = p.UserId,
                RoleId = p.RoleId,
            }).Where(p => p.UserId == id).FirstOrDefault();//Executes the query

            var applicationUserRole = await _context.ApplicationUserRole.FindAsync(id);
            _context.ApplicationUserRole.Remove(applicationUserRole);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
        private bool ApplicationUserRoleExists(Guid id)
        {
            return _context.ApplicationUserRole.Any(e => e.UserId == id);
        }
    }
}
