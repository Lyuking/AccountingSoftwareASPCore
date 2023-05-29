using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AccountingSoftware.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace AccountingSoftware.Controllers
{
    public class LicenceDetailsController : Controller
    {
        private readonly AppDBContext _context;

        public LicenceDetailsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: LicenceDetails
        [Authorize(Roles = "admin, employee, guest")]

        public async Task<IActionResult> Index()
        {
            TempData.Clear();
              return View(await _context.LicenceDetailses.ToListAsync());
        }

        // GET: LicenceDetails/Details/5
        [Authorize(Roles = "admin, employee, guest")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LicenceDetailses == null)
            {
                return NotFound();
            }

            var licenceDetails = await _context.LicenceDetailses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (licenceDetails == null)
            {
                return NotFound();
            }

            return View(licenceDetails);
        }
        [Authorize(Roles = "admin, employee")]
        // GET: LicenceDetails/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "admin, employee")]
        // POST: LicenceDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateStart,DateEnd,Price,Count")] LicenceDetails licenceDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(licenceDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(licenceDetails);
        }
        [Authorize(Roles = "admin, employee")]
        // GET: LicenceDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LicenceDetailses == null)
            {
                return NotFound();
            }

            var licenceDetails = await _context.LicenceDetailses.FindAsync(id);
            if (licenceDetails == null)
            {
                return NotFound();
            }
            return View(licenceDetails);
        }
        [Authorize(Roles = "admin, employee")]
        // POST: LicenceDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Key, DateStart,DateEnd,Price,Count")] LicenceDetails licenceDetails)
        {
            if (id != licenceDetails.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(licenceDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LicenceDetailsExists(licenceDetails.Id))
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
            return View(licenceDetails);
        }

        // GET: LicenceDetails/Delete/5
        [Authorize(Roles = "admin, employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LicenceDetailses == null)
            {
                return NotFound();
            }

            var licenceDetails = await _context.LicenceDetailses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (licenceDetails == null)
            {
                return NotFound();
            }

            return View(licenceDetails);
        }

        // POST: LicenceDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, employee")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LicenceDetailses == null)
            {
                return Problem("Entity set 'AppDBContext.LicenceDetailses'  is null.");
            }
            var licenceDetails = await _context.LicenceDetailses.Include(s => s.Licences).ThenInclude(s => s.Softwares).FirstOrDefaultAsync(t => t.Id == id);
            if (licenceDetails.Licences != null)
            {
                _context.Licences.Remove(licenceDetails.Licences.FirstOrDefault());
                if(licenceDetails.Licences.First().Softwares != null)
                {
                    _context.Softwares.Remove(licenceDetails.Licences.First().Softwares.First());
                }
            }               
            if (licenceDetails != null)
            {
                _context.LicenceDetailses.Remove(licenceDetails);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LicenceDetailsExists(int id)
        {
          return _context.LicenceDetailses.Any(e => e.Id == id);
        }
    }
}
