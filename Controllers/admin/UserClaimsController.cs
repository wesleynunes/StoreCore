using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreCore.Data;
using StoreCore.ViewModels;

namespace StoreCore.Controllers.admin
{
    [Route("Admin/UserClaims")]
    public class UserClaimsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserClaimsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //// GET: UserClaims
        //[HttpGet("")]
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.ApplicationUserClaim.ToListAsync());
        //}


        // GET: UserClaims
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            List<UserClaimViewModel> listUserClaims = new List<UserClaimViewModel>();

            var UserClaimList = await (from uc in _context.UserClaims
                                       join u in _context.Users on uc.UserId equals u.Id
                                       select new
                                       {
                                           uc.Id,
                                           uc.UserId,
                                           uc.ClaimType,
                                           uc.ClaimValue,
                                           u.UserName,
                                       }).ToListAsync();

            foreach (var item in UserClaimList)
            {
                UserClaimViewModel dataList = new UserClaimViewModel
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    UserName = item.UserName,
                    ClaimType = item.ClaimType,
                    ClaimValue = item.ClaimValue
                };

                listUserClaims.Add(dataList);
            }
            return View(listUserClaims);
        }

        //// GET: UserClaims/Details/5
        //[HttpGet("Details")]
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    //var applicationUserClaim = await _context.ApplicationUserClaim.FirstOrDefaultAsync(m => m.Id == id);

        //    ApplicationUserClaim applicationUserClaim = _context.ApplicationUserClaim.Find(id);

        //    if (applicationUserClaim == null)
        //    {
        //        return NotFound();
        //    }

        //    var DetailsUserClaim = await    from uc in _context.UserClaims
        //                                    join u in _context.Users on uc.UserId equals u.Id
        //                                    select new
        //                                    {
        //                                        uc.Id,
        //                                        uc.UserId,
        //                                        uc.ClaimType,
        //                                        uc.ClaimValue,
        //                                        u.UserName,
        //                                    })


        //    return View();
        //}




        // GET: UserClaims/Details/5
        [HttpGet("Details")]
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
        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: UserClaims/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ClaimType,ClaimValue")] ApplicationUserClaim applicationUserClaim)
        {
            if (ModelState.IsValid)
            {
                applicationUserClaim.ClaimValue = applicationUserClaim.ClaimType;
                _context.Add(applicationUserClaim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUserClaim);
        }

        // GET: UserClaims/Edit/5
        [HttpGet("Edit")]
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
        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ApplicationUserClaim applicationUserClaim)
        {
            if (id != applicationUserClaim.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    applicationUserClaim.ClaimValue = applicationUserClaim.ClaimType;
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
        [HttpGet("Delete")]
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
        [HttpPost("Delete"), ActionName("Delete")]
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
