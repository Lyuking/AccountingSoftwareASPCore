using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AccountingSoftware.Models;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace AccountingSoftware.Controllers
{
    public class SoftwareTechnicalDetailsController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public SoftwareTechnicalDetailsController(AppDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: SoftwareTechnicalDetails
        [Authorize(Roles = "admin, employee,guest")]
        public async Task<IActionResult> Index()
        {
            TempData.Clear();
            var appDBContext = _context.SoftwareTechnicalDetailses.Include(s => s.SubjectArea);
            return View(await appDBContext.ToListAsync());
        }
        [Authorize(Roles = "admin, employee,guest")]
        // GET: SoftwareTechnicalDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SoftwareTechnicalDetailses == null)
            {
                return NotFound();
            }

            var softwareTechnicalDetails = await _context.SoftwareTechnicalDetailses
                .Include(s => s.SubjectArea)
                .Include(s => s.Softwares).ThenInclude(s => s.Computers).ThenInclude(s => s.Audience)
                .Include(s => s.Softwares).ThenInclude(s => s.Licence).ThenInclude(s => s.LicenceDetails)
                .Include(s => s.Softwares).ThenInclude(s => s.Licence).ThenInclude(s => s.LicenceType)
                .Include(s => s.Softwares).ThenInclude(s => s.Licence).ThenInclude(s => s.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (softwareTechnicalDetails == null)
            {
                return NotFound();
            }
            LoadPhotoBytesToViewBag(softwareTechnicalDetails);
            return View(softwareTechnicalDetails);
        }
        [Authorize(Roles = "admin, employee")]
        // GET: SoftwareTechnicalDetails/Create
        [HttpGet]
        public RedirectToActionResult Create()
        {
            return RedirectToAction(nameof(Index), "SoftwareTechnicalDetailsAdding");
            //ViewData["SubjectAreaId"] = new SelectList(_context.SubjectAreas, "Id", "Id");
            //return View();
        }

        // POST: SoftwareTechnicalDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin, employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,SubjectAreaId,Description,RequiredSpace,Photo")] SoftwareTechnicalDetails softwareTechnicalDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(softwareTechnicalDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubjectAreaId"] = new SelectList(_context.SubjectAreas, "Id", "Name", softwareTechnicalDetails.SubjectAreaId);
            return View(softwareTechnicalDetails);
        }
        [Authorize(Roles = "admin, employee")]
        // GET: SoftwareTechnicalDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SoftwareTechnicalDetailses == null)
            {
                return NotFound();
            }

            var softwareTechnicalDetails = await _context.SoftwareTechnicalDetailses.FindAsync(id);
            if (softwareTechnicalDetails == null)
            {
                return NotFound();
            }
            LoadPhotoBytesToViewBag(softwareTechnicalDetails);
            ViewData["SubjectAreaId"] = new SelectList(_context.SubjectAreas, "Id", "Name", softwareTechnicalDetails.SubjectAreaId);
            return View(softwareTechnicalDetails);
        }
        [Authorize(Roles = "admin, employee")]
        private void LoadPhotoBytesToViewBag(SoftwareTechnicalDetails? softwareTechnicalDetails)
        {
            if (softwareTechnicalDetails != null)
            {
                if (!String.IsNullOrEmpty(softwareTechnicalDetails.Photo))
                {
                    byte[] photodata = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + softwareTechnicalDetails.Photo);

                    ViewBag.Photodata = photodata;
                }
            }
            else
            {
                ViewBag.Photodata = null;
            }
        }

        // POST: SoftwareTechnicalDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin, employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SubjectAreaId,Description,RequiredSpace,Photo")] SoftwareTechnicalDetails softwareTechnicalDetails, IFormFile? upload)
        {
            if (id != softwareTechnicalDetails.Id)
            {
                return NotFound();
            }
            SubjectArea subjectArea = null;
            if (softwareTechnicalDetails.SubjectAreaId != null)
            {
                subjectArea = await _context.SubjectAreas.FirstOrDefaultAsync(t => t.Id == softwareTechnicalDetails.SubjectAreaId);
                softwareTechnicalDetails.SubjectArea = subjectArea;
            }

            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    string path = "/Files/" + upload.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await upload.CopyToAsync(fileStream);
                    }
                    if (!string.IsNullOrEmpty(softwareTechnicalDetails.Photo))
                    {
                        System.IO.File.Delete(_appEnvironment.WebRootPath + softwareTechnicalDetails.Photo);
                    }
                    softwareTechnicalDetails.Photo = path;
                }

                try
                {
                    _context.Update(softwareTechnicalDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SoftwareTechnicalDetailsExists(softwareTechnicalDetails.Id))
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
            ViewData["SubjectAreaId"] = new SelectList(_context.SubjectAreas, "Id", "Name", softwareTechnicalDetails.SubjectAreaId);
            return View(softwareTechnicalDetails);
        }

        // GET: SoftwareTechnicalDetails/Delete/5
        [Authorize(Roles = "admin, employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SoftwareTechnicalDetailses == null)
            {
                return NotFound();
            }

            var softwareTechnicalDetails = await _context.SoftwareTechnicalDetailses
                .Include(s => s.SubjectArea)              
                .FirstOrDefaultAsync(m => m.Id == id);
            if (softwareTechnicalDetails == null)
            {
                return NotFound();
            }

            return View(softwareTechnicalDetails);
        }

        // POST: SoftwareTechnicalDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin, employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SoftwareTechnicalDetailses == null)
            {
                return Problem("Entity set 'AppDBContext.SoftwareTechnicalDetailses'  is null.");
            }
            var softwareTechnicalDetails = await _context.SoftwareTechnicalDetailses
                .Include(s => s.Softwares).ThenInclude(s => s.Licence).ThenInclude(s => s.LicenceDetails)
                .FirstOrDefaultAsync(m => m.Id == id); 
            if (softwareTechnicalDetails != null)
            {
                foreach (var software in softwareTechnicalDetails.Softwares)
                {
                    if (software.Licence != null && software.Licence.LicenceDetails != null)
                    {
                        _context.LicenceDetailses.Remove(software.Licence.LicenceDetails);                       
                    }
                    _context.Softwares.Remove(software);
                }
                _context.SoftwareTechnicalDetailses.Remove(softwareTechnicalDetails);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SoftwareTechnicalDetailsExists(int id)
        {
          return _context.SoftwareTechnicalDetailses.Any(e => e.Id == id);
        }
    }
}
