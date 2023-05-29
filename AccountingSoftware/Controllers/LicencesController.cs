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
    public class LicencesController : Controller
    {
        private readonly AppDBContext _context;

        public LicencesController(AppDBContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "admin, employee, guest")]
        // GET: Licences
        public async Task<IActionResult> Index()
        {
            TempData.Clear();
            var appDBContext = _context.Licences.Include(l => l.Employee).Include(l => l.LicenceDetails).Include(l => l.LicenceType);
            return View(await appDBContext.ToListAsync());
        }
        [Authorize(Roles = "admin, employee, guest")]
        // GET: Licences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Licences == null)
            {
                return NotFound();
            }

            var licence = await _context.Licences
                .Include(l => l.Employee)
                .Include(l => l.LicenceDetails)
                .Include(l => l.LicenceType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (licence == null)
            {
                return NotFound();
            }

            return View(licence);
        }
        [Authorize(Roles = "admin, employee")]
        // GET: Licences/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id");
            ViewData["LicenceDetailsId"] = new SelectList(_context.LicenceDetailses, "Id", "Id");
            ViewData["LicenceTypeId"] = new SelectList(_context.LicenceType, "Id", "Id");
            return View();
        }

        // POST: Licences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin, employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId,LicenceTypeId,LicenceDetailsId")] Licence licence)
        {
            if (ModelState.IsValid)
            {
                _context.Add(licence);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", licence.EmployeeId);
            ViewData["LicenceDetailsId"] = new SelectList(_context.LicenceDetailses, "Id", "Id", licence.LicenceDetailsId);
            ViewData["LicenceTypeId"] = new SelectList(_context.LicenceType, "Id", "Id", licence.LicenceTypeId);
            return View(licence);
        }
        [Authorize(Roles = "admin, employee")]
        // GET: Licences/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Licences == null)
            {
                return NotFound();
            }

            var licence = await _context.Licences.FindAsync(id);
            if (licence == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", licence.EmployeeId);
            ViewData["LicenceDetailsId"] = new SelectList(_context.LicenceDetailses, "Id", "Id", licence.LicenceDetailsId);
            ViewData["LicenceTypeId"] = new SelectList(_context.LicenceType, "Id", "Id", licence.LicenceTypeId);
            return View(licence);
        }
        // POST: Licences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin, employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,LicenceTypeId,LicenceDetailsId")] Licence licence)
        {
            if (id != licence.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(licence);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LicenceExists(licence.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", licence.EmployeeId);
            ViewData["LicenceDetailsId"] = new SelectList(_context.LicenceDetailses, "Id", "Id", licence.LicenceDetailsId);
            ViewData["LicenceTypeId"] = new SelectList(_context.LicenceType, "Id", "Id", licence.LicenceTypeId);
            return View(licence);
        }

        // GET: Licences/Delete/5
        [Authorize(Roles = "admin, employee")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Licences == null)
            {
                return NotFound();
            }

            var licence = await _context.Licences
                .Include(l => l.Employee)
                .Include(l => l.LicenceDetails)
                .Include(l => l.LicenceType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (licence == null)
            {
                return NotFound();
            }

            return View(licence);
        }

        // POST: Licences/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin, employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Licences == null)
            {
                return Problem("Entity set 'AppDBContext.Licences'  is null.");
            }
            var licence = await _context.Licences.FindAsync(id);
            if (licence != null)
            {
                _context.Licences.Remove(licence);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LicenceExists(int id)
        {
          return _context.Licences.Any(e => e.Id == id);
        }
    }
}
