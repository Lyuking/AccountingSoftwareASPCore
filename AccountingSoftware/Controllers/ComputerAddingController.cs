using AccountingSoftware.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AccountingSoftware.Controllers
{
    public class ComputerAddingController : Controller
    {
        private readonly AppDBContext _context;
        public ComputerAddingController(AppDBContext context)
        {
            _context = context;
        }
        
        [HttpGet] //post ne rabotaet
        [Authorize(Roles = "admin, employee")]
        public async Task <IActionResult> Index()
        {
            return View(await _context.Audiences.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Index(int? audienceId)
        {
            Audience audience = await _context.Audiences.FindAsync(audienceId);
            if (audience == null)
                return NotFound();

            ViewBag.AudienceId = audience.Id;
            ViewBag.Audience = audience;
            
            return View("ComputerInfo");
        }
        [HttpGet]
        [Authorize(Roles = "admin, employee")]
        public IActionResult WithoutAudience()
        {
            return View("ComputerInfo");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, employee")]
        public async Task<IActionResult> Create(int? AudienceId, string Number, string IpAdress, string Processor, string Videocard, string RAM, string TotalSpace)
        {
            Computer computer = new Computer();
            computer.AudienceId = AudienceId;
            computer.Number = Number;
            computer.IpAdress = IpAdress;
            computer.Processor = Processor;
            computer.Videocard = Videocard;
            computer.TotalSpace = TotalSpace;
            computer.RAM = RAM;
            computer.TotalSpace = TotalSpace;
            //if (ModelState.IsValid)
            //{
            _context.Add(computer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Computers");
            //}
            //ViewData["AudienceId"] = new SelectList(_context.Audiences, "Id", "Id", computer.AudienceId);
            //return View(computer);
        }
        
        //public async Task<IActionResult> Create([Bind("Id,AudienceId,IpAdress,Processor,Videocard,RAM,TotalSpace")] Computer computer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(computer);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index), "Computers");
        //    }
        //    ViewData["AudienceId"] = new SelectList(_context.Audiences, "Id", "Id", computer.AudienceId);
        //    return View(computer);
        //}
    }
}
