using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AccountingSoftware.Models;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using OfficeOpenXml;

namespace AccountingSoftware.Controllers
{
    public class InstalledSoftwareController : Controller
    {
        private readonly AppDBContext _context;
        private const string _softwareTechnicalDetailsId = "softwareTechnicalDetailsId";
        private const string _computerId = "computerId";
        private const string _audienceId = "audienceId";
       
        public InstalledSoftwareController(AppDBContext context)
        {
            _context = context;
           
        }
        [Authorize(Roles = "admin, employee, guest")]
        public async Task<IActionResult> Index()
        {
            TempData.Clear();
            var S = _context.Computers.Include(c => c.Audience).Include(c => c.Softwares)
                .Include(c=>c.Softwares).ThenInclude( t=> t.SoftwareTechnicalDetails)
                .Include(c => c.Softwares).ThenInclude(t=> t.Licence).ThenInclude(l => l.LicenceType)
                .Include(c => c.Softwares).ThenInclude(t => t.Licence).ThenInclude(l => l.LicenceDetails)
                .Include(c => c.Softwares).ThenInclude(t => t.Licence).ThenInclude(l => l.Employee);
            return View(await S.ToListAsync());
        }
        [Authorize(Roles = "admin, employee")]
        [HttpGet]
        public async Task<IActionResult> SelectComputer()
        {
            return View(await _context.Computers.Include(a => a.Audience).ToListAsync());
        }
        [Authorize(Roles = "admin, employee")]
        [HttpPost]
        public async Task<IActionResult> SelectComputer(int computerId) //add Async
        {
            TempData[_computerId] = computerId;
             return RedirectToAction(nameof(SelectSoftware));
        }
        [Authorize(Roles = "admin, employee")]
        [HttpGet]
        public async Task<IActionResult> SelectSoftware(int? computerId) //null?
        {
            if (computerId != null)
            {
                TempData[_computerId] = computerId;
                TempData["fromComputerDetails"] = true;
            }
            /* var tmp = _context.Softwares.Include(c => c.SoftwareTechnicalDetails).ToList()*/
            return View(_context.Softwares.Include(c => c.SoftwareTechnicalDetails).ToList());
        }
        [Authorize(Roles = "admin, employee")]
        [HttpPost]
        public async Task<IActionResult> SelectSoftware(int softwareTechnicalDetailsId)
        {
            TempData[_softwareTechnicalDetailsId] = softwareTechnicalDetailsId;
            return RedirectToAction(nameof(SelectLicence));
        }
        [Authorize(Roles = "admin, employee")]
        [HttpGet]
        public async Task<IActionResult> SelectLicence()
        {
            List<Software> result;
            if (TempData.Peek(_audienceId) == null) // вернуть только лицензии, отсутствующие на текущей машине
            {
                /*&& t.Computers.All(t => t.Id != (int)TempData.Peek(_computerId))*/
                result = await _context.Softwares.Where(t => t.SoftwareTechnicalDetailsId == (int)TempData[_softwareTechnicalDetailsId])
                    .Include(c => c.Licence).ThenInclude(c => c.LicenceType)
                    .Include(c => c.Licence).ThenInclude(c => c.LicenceDetails)
                    .Include(c => c.Licence).ThenInclude(c => c.Employee)
                    .Include(c => c.Computers)
                    .Where(c => !c.Computers.Select(t => t.Id).Contains((int)TempData.Peek(_computerId)))
                    .ToListAsync();
            }
            else //вернуть все лицензии для пакетного добавления
            {
                result = await _context.Softwares.Where(t => t.SoftwareTechnicalDetailsId == (int)TempData[_softwareTechnicalDetailsId])
                    .Include(c => c.Licence).ThenInclude(c => c.LicenceType)
                    .Include(c => c.Licence).ThenInclude(c => c.LicenceDetails)
                    .Include(c => c.Licence).ThenInclude(c => c.Employee).ToListAsync();
            }
            //var secondProcess = firstProcess.Where(t => t.Computers.Select(c => c.Id == TempData.Peek(_computerId)) != )
                         
            return View(result);
        }
        [Authorize(Roles = "admin, employee")]
        public async Task<IActionResult> Add(int softwareId, int licenceDetailsId)
        {
            var computer = _context.Computers.Find((int)TempData[_computerId]);
            
            if (computer != null)
            {
                var software = _context.Softwares.Find(softwareId);
                var licence = _context.LicenceDetailses.Find(licenceDetailsId);

                if (software != null && licence.Count > 0)
                {
                    licence.Count--;
                    _context.Update(licence);
                    await _context.SaveChangesAsync();

                    computer.Softwares.Add(software);
                    _context.Update(computer);
                    await _context.SaveChangesAsync();
                    bool? fromComputerDetails = (bool?)(TempData["fromComputerDetails"]);
                    if (fromComputerDetails.HasValue && fromComputerDetails.Value == true)
                        return RedirectToAction("Details", "Computers", new {id = computer.Id});
                    return RedirectToAction(nameof(Index));
                }
                return View("Error"); 
            }
            return NotFound();
        }
        public async Task<IActionResult> AddToAudience(int softwareId, int licenceDetailsId)
        {
            List<Computer> computers = await _context.Computers.Where(s => s.AudienceId.HasValue && s.AudienceId.Value == (int)TempData[_audienceId])
                .Include(s => s.Softwares).ThenInclude(s => s.Licence).ThenInclude(s => s.LicenceDetails)
                .ToListAsync();

            var software = _context.Softwares.Find(softwareId);
            var licence = _context.LicenceDetailses.Find(licenceDetailsId);
            int count = 0;
            int totalCount = computers.Count;
            if (software != null)
            {
                foreach(Computer computer in computers)
                {
                    if(licence.Count > 0 && !computer.Softwares.Contains(software))
                    {
                        licence.Count--;
                        _context.Update(licence);

                        computer.Softwares.Add(software);
                        _context.Update(computer);
                        await _context.SaveChangesAsync();
                        count++;
                    }                                      
                }
                ViewBag.Count = count;
                ViewBag.TotalCount = totalCount - count;
                ViewBag.AvailableCount = licence.Count;
                return View("InfoAboutAddedSoftware");
            }
            return NotFound();
        }
        [Authorize(Roles = "admin, employee")]
        public async Task<IActionResult> UnistallSoftware(int computerId, int softwareId, bool? fromComputerDetails)
        {
            var computer = _context.Computers.Include(t => t.Softwares).First(t => t.Id == computerId);
            var software = _context.Softwares.Find(softwareId);
            var licence = _context.Licences.Find(software.LicenceId);
            var licenceDetails = _context.LicenceDetailses.Find(licence.LicenceDetailsId);
                //.Include(a => a.Licence).ThenInclude(b => b.LicenceDetails);
            if (computer != null && software != null) {
                licenceDetails.Count++;
                _context.Update(licenceDetails);
                computer.Softwares = computer.Softwares.Where(c => c.Id != softwareId).ToList();
                _context.Update(computer);
                await _context.SaveChangesAsync();
                if (fromComputerDetails != null && fromComputerDetails.Value)
                    return RedirectToAction("Details", "Computers", new { id = computerId });
                return RedirectToAction(nameof(Index));
            }            
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin, employee")]
        [HttpGet]
        public async Task<IActionResult> SelectAudience()
        {
            return View(await _context.Audiences.ToListAsync());
        }
        [Authorize(Roles = "admin, employee")]
        [HttpPost]
        public async Task<IActionResult> SelectAudience(int audienceId) //add Async
        {
            TempData[_audienceId] = audienceId;
            return RedirectToAction(nameof(SelectSoftware));
        }
    }
}
