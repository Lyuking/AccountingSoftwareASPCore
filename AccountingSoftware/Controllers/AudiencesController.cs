using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AccountingSoftware.Models;
using AccountingSoftware.Models.ViewModels;
using System.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using OfficeOpenXml;

namespace AccountingSoftware.Controllers
{
    public class AudiencesController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public AudiencesController(AppDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: Audiences
        [Authorize(Roles = "admin, employee, guest")]
        public async Task<IActionResult> Index()
        {
             TempData.Clear();
              return View(await _context.Audiences.ToListAsync());
        }
        [Authorize(Roles = "admin, employee, guest")]
        // GET: Audiences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Audiences == null)
            {
                return NotFound();
            }

            var audience = await _context.Audiences
                .FirstOrDefaultAsync(m => m.Id == id);
            if (audience == null)
            {
                return NotFound();
            }

            AudiencesDetailsViewModel viewModel = new AudiencesDetailsViewModel();
            viewModel.Audience = audience;
            viewModel.Computers = _context.Computers.Where(m => m.AudienceId == audience.Id);
            TempData["fromAudience"] = true;
            return View(viewModel);
        }
        [Authorize(Roles = "admin, employee")]

        // GET: Audiences/Create
        public IActionResult Create(bool? fromComputers)
        {
            if (fromComputers == true)
                ViewBag.fromComputers= true;
            else
                ViewBag.fromComputers = false;
            return View();

        }
        [Authorize(Roles = "admin, employee")]
        // POST: Audiences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name, fromComputers")] Audience audience)
        {
            if (ModelState.IsValid)
            {
                _context.Add(audience);
                await _context.SaveChangesAsync();
                if (audience.fromComputers == true)
                    return RedirectToAction(nameof(ComputerAddingController.Index), "ComputerAdding"); //nameOfQuestion
                else
                    return RedirectToAction(nameof(Index));
            }
            return View(audience);
        }
        [Authorize(Roles = "admin, employee")]
        // GET: Audiences/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Audiences == null)
            {
                return NotFound();
            }

            var audience = await _context.Audiences.FindAsync(id);
            if (audience == null)
            {
                return NotFound();
            }
            return View(audience);
        }
        [Authorize(Roles = "admin, employee")]
        // POST: Audiences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Audience audience)
        {
            if (id != audience.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(audience);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AudienceExists(audience.Id))
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
            return View(audience);
        }
        [Authorize(Roles = "admin, employee")]
        // GET: Audiences/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Audiences == null)
            {
                return NotFound();
            }

            var audience = await _context.Audiences
                .FirstOrDefaultAsync(m => m.Id == id);
            if (audience == null)
            {
                return NotFound();
            }

            return View(audience);
        }
        [Authorize(Roles = "admin, employee")]
        // POST: Audiences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Audiences == null)
            {
                return Problem("Entity set 'AppDBContext.Audiences'  is null.");
            }
            var audience = await _context.Audiences.FindAsync(id);
            if (audience != null)
            {   /*var adv = _context.Audiences.Include(b = b.)  */          
                _context.Audiences.Remove(audience);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public FileResult GetReport()
        {
            // Путь к файлу с шаблоном
            string path = "/Reports/audience_report_template.xlsx";
            //Путь к файлу с результатом
            string result = "/Reports/audience_report.xlsx";
            FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + path);
            FileInfo fr = new FileInfo(_appEnvironment.WebRootPath + result);
            //будем использовть библитотеку не для коммерческого использования
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //открываем файл с шаблоном
            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                //устанавливаем поля документа
                excelPackage.Workbook.Properties.Author = "Лука Р.Н.";
                excelPackage.Workbook.Properties.Title = "Список компьютеров в аудитории";
                excelPackage.Workbook.Properties.Subject = "Компьютеры в аудитории";
                excelPackage.Workbook.Properties.Created = DateTime.Now;
                //плучаем лист по имени.
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["AudiencesComputers"];
                //получаем списко пользователей и в цикле заполняем лист данными
                int currentLine = 3;
                int num = 1;
                List<Audience> audiences = _context.Audiences.Include(c => c.Computers).ToList();
                foreach (Audience audience in audiences)
                {
                    worksheet.Cells[currentLine, 1].Value = num;
                    worksheet.Cells[currentLine, 2].Value = audience.Name;
                    foreach (Computer computer in audience.Computers)
                    {
                        worksheet.Cells[currentLine, 3].Value = computer.Number;
                        worksheet.Cells[currentLine, 4].Value = computer.IpAdress;
                        worksheet.Cells[currentLine, 5].Value = computer.Processor;
                        worksheet.Cells[currentLine, 6].Value = computer.Videocard;
                        worksheet.Cells[currentLine, 7].Value = computer.RAM;
                        worksheet.Cells[currentLine, 8].Value = computer.TotalSpace;
                        currentLine++;
                    }
                    currentLine++;
                    num++;
                }
                //созраняем в новое место
                excelPackage.SaveAs(fr);
            }
            // Тип файла - content-type
            string file_type =
           "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet";
            // Имя файла - необязательно
            string file_name = "Audiences_report.xlsx";
            return File(result, file_type, file_name);
        }
        private bool AudienceExists(int id)
        {
          return _context.Audiences.Any(e => e.Id == id);
        }
    }
}
