using AccountingSoftware.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Numerics;

namespace AccountingSoftware.Controllers
{
    public class SoftwareTechnicalDetailsAddingController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public SoftwareTechnicalDetailsAddingController(AppDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment= appEnvironment;
        }
        //[HttpGet] 
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.SubjectAreas.ToListAsync());           
        //}
        [HttpGet]
        [Authorize(Roles = "admin, employee")]

        public async Task<IActionResult> Index(bool fromSelectSoftware)
        {
            if (fromSelectSoftware == true || (bool?)TempData.Peek("fromSelectSoftware") == true)
                TempData["fromSelectSoftware"] = true;
            else
                TempData["fromSelectSoftware"] = false;
            return View(await _context.SubjectAreas.ToListAsync());
        }
        [HttpPost]
        [Authorize(Roles = "admin, employee")]
        public async Task<IActionResult> Index(int subjectAreaId)
        {
            SubjectArea? subjectArea = await _context.SubjectAreas.FindAsync(subjectAreaId);
            if (subjectArea == null)
                return NotFound();            
            ViewBag.SubjectAreaId = subjectArea.Id;
            ViewBag.SubjectArea = subjectArea;
            await _context.SaveChangesAsync();
            return View("SoftwareInfo"); 
        }
        [HttpPost]
        [Authorize(Roles = "admin, employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? subjectAreaId, string name, string description, string requiredSpace, IFormFile upload)
        {
            SoftwareTechnicalDetails softwareTechnicalDetails = new SoftwareTechnicalDetails();
            if (upload != null)
            {
                string path = "/Files/" + upload.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await upload.CopyToAsync(fileStream);
                }
                softwareTechnicalDetails.Photo = path;
            }
            SubjectArea subjectArea = await _context.SubjectAreas.FindAsync(subjectAreaId);
            softwareTechnicalDetails.SubjectAreaId = subjectAreaId;
            softwareTechnicalDetails.Name = name;
            softwareTechnicalDetails.Description = description;
            softwareTechnicalDetails.RequiredSpace = requiredSpace;           
            //if (ModelState.IsValid)
            //{
            _context.Add(softwareTechnicalDetails);
            await _context.SaveChangesAsync();
            bool? checkFromSelectSoftware = (bool?)TempData["fromSelectSoftware"];
            if (checkFromSelectSoftware == true)
                return RedirectToAction("SelectSoftware", "LicenceAdding");           
            else
                return RedirectToAction(nameof(Index), "SoftwareTechnicalDetails");

            //}
            //ViewData["AudienceId"] = new SelectList(_context.Audiences, "Id", "Id", computer.AudienceId);
            //return View(computer);
        }
    }
}
