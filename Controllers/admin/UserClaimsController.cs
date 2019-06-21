using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreCore.Data;

namespace StoreCore.Controllers.admin
{
    public class UserClaimsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserClaimsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserClaims
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicationUserClaim.ToListAsync());
        }

        // GET: UserClaims/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUserClaim = await _context.ApplicationUserClaim
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUserClaim == null)
            {
                return NotFound();
            }

            return View(applicationUserClaim);
        }

        // GET: UserClaims/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: UserClaims/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ClaimType,ClaimValue")] ApplicationUserClaim applicationUserClaim)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationUserClaim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUserClaim);
        }

        // GET: UserClaims/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUserClaim = await _context.ApplicationUserClaim.FindAsync(id);
            if (applicationUserClaim == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View(applicationUserClaim);
        }

        // POST: UserClaims/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ClaimType,ClaimValue")] ApplicationUserClaim applicationUserClaim)
        {
            if (id != applicationUserClaim.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationUserClaim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserClaimExists(applicationUserClaim.Id))
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
            return View(applicationUserClaim);
        }

        // GET: UserClaims/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUserClaim = await _context.ApplicationUserClaim
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUserClaim == null)
            {
                return NotFound();
            }

            return View(applicationUserClaim);
        }

        // POST: UserClaims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicationUserClaim = await _context.ApplicationUserClaim.FindAsync(id);
            _context.ApplicationUserClaim.Remove(applicationUserClaim);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserClaimExists(int id)
        {
            return _context.ApplicationUserClaim.Any(e => e.Id == id);
        }
    }
}
