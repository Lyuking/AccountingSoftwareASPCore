using AccountingSoftware.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Data;
using System.Numerics;

namespace AccountingSoftware.Controllers
{
    public class LicenceAddingController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        LicenceAdding licenceAdding;
        public LicenceAddingController(AppDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            licenceAdding= new LicenceAdding();
        }
        #region "Выбор ПО"
        [HttpGet]
        [Authorize(Roles = "admin, employee")]

        public async Task<IActionResult> SelectSoftware()
        {
            return View(await _context.SoftwareTechnicalDetailses.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> SelectSoftware(int softwareTechnicalDetailsId)
        {
            SoftwareTechnicalDetails? softwareTechnicalDetails = await _context.SoftwareTechnicalDetailses.FindAsync(softwareTechnicalDetailsId);
            if (softwareTechnicalDetails == null)
                return NotFound();
            licenceAdding.softwareTechnicalDetails= softwareTechnicalDetails;
            TempData.Put("softwareTechnicalDetails", softwareTechnicalDetails);
            return RedirectToAction(nameof(SelectLicenceType));
        }
        #endregion
        #region "Выбор типа лицензии"
        [HttpGet]
        [Authorize(Roles = "admin, employee")]

        public async Task<IActionResult> SelectLicenceType()
        {
            return View(await _context.LicenceType.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> SelectLicenceType(int licenceTypeId)
        {
            LicenceType? licenceType = await _context.LicenceType.FindAsync(licenceTypeId);
            if (licenceType == null)
                return NotFound();

            licenceAdding.licenceType = licenceType;
            TempData.Put("licenceType", licenceType);
            return RedirectToAction(nameof(SelectEmployee));
        }
        #endregion
        #region "Выбор работника"
        [HttpGet]
        [Authorize(Roles = "admin, employee")]

        public async Task<IActionResult> SelectEmployee()
        {
            return View(await _context.Employees.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> SelectEmployee(int employeeId)
        {
            Employee? employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
                return NotFound();

            licenceAdding.employee = employee;
            TempData.Put("employee", employee);
            ViewBag.employee = employee;
            ViewBag.licenceType = TempData.Get<LicenceType>("licenceType");
            ViewBag.softwareTechnicalDetails = TempData.Get<SoftwareTechnicalDetails>("softwareTechnicalDetails");
            return View("LicenceCreating", licenceAdding);
        }
        #endregion
        [HttpPost]
        [Authorize(Roles = "admin, employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int softwareTechnicalDetailsId, int licenceTypeId, int? employeeId, string Key, DateTime DateStart, DateTime DateEnd, float Price, int Count)
        {
            LicenceDetails licenceDetails = new LicenceDetails();
            licenceDetails.Price = Price; licenceDetails.Count = Count; licenceDetails.Key = Key; licenceDetails.DateStart = DateStart; licenceDetails.DateEnd = DateEnd;
            _context.Add(licenceDetails);

            Licence licence = new Licence();
            licence.LicenceType = _context.LicenceType.Find(licenceTypeId);
            licence.Employee = _context.Employees.Find(employeeId);
            licence.LicenceDetails = licenceDetails;
            _context.Add(licence);

            Software software = new Software();
            software.SoftwareTechnicalDetails = _context.SoftwareTechnicalDetailses.Find(softwareTechnicalDetailsId);
            software.Licence = licence;
            _context.Add(software);

            await _context.SaveChangesAsync();
            return  RedirectToAction(nameof(Index), "Softwares");
        }
    }
}
