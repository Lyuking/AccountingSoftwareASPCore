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
    public class EmployeesController : Controller
    {
        private readonly AppDBContext _context;

        public EmployeesController(AppDBContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "admin, employee, guest")]
        // GET: Employees
        public async Task<IActionResult> Index()
        {
            TempData.Clear();
              return View(await _context.Employees.ToListAsync());
        }
        [Authorize(Roles = "admin, employee, guest")]
        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(s => s.Licences).ThenInclude(s => s.LicenceDetails)
                .Include(s => s.Licences).ThenInclude(s => s.LicenceType)
                .Include(s => s.Licences).ThenInclude(s => s.Softwares).ThenInclude(s=>s.SoftwareTechnicalDetails)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }
        [Authorize(Roles = "admin, employee")]
        // GET: Employees/Create
        [HttpGet]
        public IActionResult Create(bool fromSelectEmployee)
        {
            if (fromSelectEmployee)
                TempData["fromSelectEmployee"] = true;
            return View();
        }
        
        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, employee")]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Patronymic")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                if ((bool?)TempData["fromSelectEmployee"] == true)
                    return RedirectToAction("SelectEmployee", "LicenceAdding");
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }
        [Authorize(Roles = "admin, employee")]
        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin, employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Patronymic")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            return View(employee);
        }
        [Authorize(Roles = "admin, employee")]
        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }
        [Authorize(Roles = "admin, employee")]
        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'AppDBContext.Employees'  is null.");
            }
            var employee = await _context.Employees.Include(t => t.Licences).FirstOrDefaultAsync(m => m.Id == id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return _context.Employees.Any(e => e.Id == id);
        }
    }
}
