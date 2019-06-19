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

        public UserRolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //// GET: UserRoles
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.ApplicationUserRole.ToListAsync());
        //}

        // GET: UserRoles
        public async Task<IActionResult> Index()
        {
            List<UserRolesViewModel> listUserRoles = new List<UserRolesViewModel>();

            var  UserRoleList = await (from ur in _context.UserRoles
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
        public async Task<IActionResult> Details(UserRolesViewModel viewModel)
        {
            if (viewModel == null)
            {
                return NotFound();
            }

            //UserRolesViewModel estudante = _context.UserRoles.Find(viewModel.UserId);

            //UserRolesViewModel details = _context.ApplicationUserRole.Find(viewModel.UserId);

            //UserRolesViewModel userId = await _context.ApplicationUserRole.Find(viewModel.UserId);


            //UserRolesViewModel details = await _context.ApplicationUserRole.FirstOrDefaultAsync(m => m.UserId == id);

            //if (details == null)
            //{
            //    return NotFound();
            //}

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
                    RoleId = item.RoleId,
                    

                };

                listUserRoles.Add(dataList);
            }
            return View(listUserRoles);
           
        }

        //// GET: UserRoles/Details/5
        //public async Task<IActionResult> Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var applicationUserRole = await _context.ApplicationUserRole.FirstOrDefaultAsync(m => m.UserId == id);
        //    if (applicationUserRole == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(applicationUserRole);
        //}

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
                applicationUserRole.UserRoleId = Guid.NewGuid();
                _context.Add(applicationUserRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUserRole);
        }

        // GET: UserRoles/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUserRole = await _context.ApplicationUserRole.FindAsync(id);
            if (applicationUserRole == null)
            {
                return NotFound();
            }
            return View(applicationUserRole);
        }

        // POST: UserRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserRoleId,UserId,RoleId")] ApplicationUserRole applicationUserRole)
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
            if (id == null)
            {
                return NotFound();
            }

            var applicationUserRole = await _context.ApplicationUserRole.FirstOrDefaultAsync(m => m.UserId == id);
            if (applicationUserRole == null)
            {
                return NotFound();
            }

            return View(applicationUserRole);
        }

        // POST: UserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
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
