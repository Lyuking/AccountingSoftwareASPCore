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
    public class SubjectAreasController : Controller
    {
        private readonly AppDBContext _context;

        public SubjectAreasController(AppDBContext context)
        {
            _context = context;
        }

        // GET: SubjectAreas
        [Authorize(Roles = "admin, employee, guest")]
        public async Task<IActionResult> Index()
        {
            TempData.Clear();
              return View(await _context.SubjectAreas.ToListAsync());
        }

        // GET: SubjectAreas/Details/5
        [Authorize(Roles = "admin, employee, guest")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SubjectAreas == null)
            {
                return NotFound();
            }

            var subjectArea = await _context.SubjectAreas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subjectArea == null)
            {
                return NotFound();
            }

            return View(subjectArea);
        }

        // GET: SubjectAreas/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create(bool? fromSoftwareTechnicalDetails)
        {
            if (fromSoftwareTechnicalDetails == true)
                ViewBag.fromSoftwareTechnicalDetails = true;
            else
                ViewBag.fromSoftwareTechnicalDetails = false;
            return View();
        }

        // POST: SubjectAreas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name, fromSoftwareTechnicalDetails")] SubjectArea subjectArea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subjectArea);
                await _context.SaveChangesAsync();
                if (subjectArea.fromSoftwareTechnicalDetails == true)
                    return RedirectToAction(nameof(SoftwareTechnicalDetailsAddingController.Index), "SoftwareTechnicalDetailsAdding"); //nameOfQuestion
                else
                    return RedirectToAction(nameof(Index));
           
            }
            return View(subjectArea);
        }

        // GET: SubjectAreas/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SubjectAreas == null)
            {
                return NotFound();
            }

            var subjectArea = await _context.SubjectAreas.FindAsync(id);
            if (subjectArea == null)
            {
                return NotFound();
            }
            return View(subjectArea);
        }

        // POST: SubjectAreas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] SubjectArea subjectArea)
        {
            if (id != subjectArea.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subjectArea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectAreaExists(subjectArea.Id))
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
            return View(subjectArea);
        }

        // GET: SubjectAreas/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SubjectAreas == null)
            {
                return NotFound();
            }

            var subjectArea = await _context.SubjectAreas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subjectArea == null)
            {
                return NotFound();
            }

            return View(subjectArea);
        }

        // POST: SubjectAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SubjectAreas == null)
            {
                return Problem("Entity set 'AppDBContext.SubjectAreas'  is null.");
            }
            var subjectArea = await _context.SubjectAreas.FindAsync(id);
            if (subjectArea != null)
            {
                _context.SubjectAreas.Remove(subjectArea);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectAreaExists(int id)
        {
          return _context.SubjectAreas.Any(e => e.Id == id);
        }
    }
}
