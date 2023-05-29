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
    public class LicenceTypesController : Controller
    {
        private readonly AppDBContext _context;

        public LicenceTypesController(AppDBContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "admin, employee, guest")]

        // GET: LicenceTypes
        public async Task<IActionResult> Index()
        {
            TempData.Clear();
              return View(await _context.LicenceType.ToListAsync());
        }
        [Authorize(Roles = "admin, employee, guest")]
        // GET: LicenceTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LicenceType == null)
            {
                return NotFound();
            }

            var licenceType = await _context.LicenceType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (licenceType == null)
            {
                return NotFound();
            }

            return View(licenceType);
        }
        [Authorize(Roles = "admin")]
        // GET: LicenceTypes/Create
        public IActionResult Create(bool fromSelectLicenceType)
        {
            if (fromSelectLicenceType == true)
                TempData["fromSelectLicenceType"] = true;
            return View();
        }
        [Authorize(Roles = "admin")]
        // POST: LicenceTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] LicenceType licenceType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(licenceType);
                await _context.SaveChangesAsync();
                if ((bool?)TempData["fromSelectLicenceType"] == true)
                   return RedirectToAction("SelectLicenceType", "LicenceAdding");
                return RedirectToAction(nameof(Index));
            }
            return View(licenceType);
        }
        [Authorize(Roles = "admin")]
        // GET: LicenceTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LicenceType == null)
            {
                return NotFound();
            }

            var licenceType = await _context.LicenceType.FindAsync(id);
            if (licenceType == null)
            {
                return NotFound();
            }
            return View(licenceType);
        }

        // POST: LicenceTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] LicenceType licenceType)
        {
            if (id != licenceType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(licenceType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LicenceTypeExists(licenceType.Id))
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
            return View(licenceType);
        }

        // GET: LicenceTypes/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LicenceType == null)
            {
                return NotFound();
            }

            var licenceType = await _context.LicenceType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (licenceType == null)
            {
                return NotFound();
            }

            return View(licenceType);
        }

        // POST: LicenceTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LicenceType == null)
            {
                return Problem("Entity set 'AppDBContext.LicenceType'  is null.");
            }
            var licenceType = await _context.LicenceType.Include(t => t.Licences).FirstOrDefaultAsync(m => m.Id == id);
            if (licenceType != null)
            {
                _context.LicenceType.Remove(licenceType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LicenceTypeExists(int id)
        {
          return _context.LicenceType.Any(e => e.Id == id);
        }
    }
}
