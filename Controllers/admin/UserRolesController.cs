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
    [Route("Admin/UserRoles")]
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

              
        // GET: UserRoles
        [HttpGet("")]
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
                                          ur.RoleId,                                      
                                      }).ToListAsync();

            foreach (var item in UserRoleList)
            {
                UserRolesViewModel dataList = new UserRolesViewModel
                {
                    UserRoleId = item.UserId,
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
        [HttpGet("Details")]
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var UserRoleList = from ur in _context.UserRoles
                               join u in _context.Users on ur.UserId equals u.Id
                               join r in _context.Roles on ur.RoleId equals r.Id
                               where ur.UserId == id
                               select new
                               {
                                   u.UserName,
                                   r.Name,
                                   ur.UserId,
                                   ur.RoleId,
                               };


            var details = UserRoleList.Select(p => new UserRolesViewModel
            {
                UserId =    p.UserId,
                RoleId =    p.RoleId,
                UserName =  p.UserName,
                Name    =   p.Name,
            }).Where(p => p.UserId == id).FirstOrDefault();

            return View(details);
        }


        // GET: UserRoles/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return View();
        }

        // POST: UserRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Create")]
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
              

        // GET: UserRoles/Delete/5
        [HttpGet("Delete")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var UserRoleList = from ur in _context.UserRoles
                               join u in _context.Users on ur.UserId equals u.Id
                               join r in _context.Roles on ur.RoleId equals r.Id
                               where ur.UserId == id
                               select new
                               {
                                   u.UserName,
                                   r.Name,
                                   ur.UserId,
                                   ur.RoleId,
                               };

            var delete = UserRoleList.Select(p => new UserRolesViewModel
            {
                UserId = p.UserId,
                RoleId = p.RoleId,
                UserName = p.UserName,
                Name = p.Name,
            }).Where(p => p.UserId == id).FirstOrDefault();

            return View(delete);
        }


        // POST: UserRoles/Delete/5
        [HttpPost("Delete"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid UserId, Guid RoleId)
        {
            var userRole = await _context.ApplicationUserRole.FindAsync(UserId, RoleId);

            _context.ApplicationUserRole.Remove(userRole);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}
