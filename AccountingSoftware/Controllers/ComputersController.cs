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
using OfficeOpenXml;

namespace AccountingSoftware.Controllers
{
    public class ComputersController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;


        public ComputersController(AppDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: Computers
        [Authorize(Roles = "admin, employee, guest")]
        public async Task<IActionResult> Index()
        {
            TempData.Clear();
            var appDBContext = _context.Computers.Include(c => c.Audience);
            return View(await appDBContext.ToListAsync());
        }

        // GET: Computers/Details/5
        [Authorize(Roles = "admin, employee, guest")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Computers == null)
            {
                return NotFound();
            }

            var computer = await _context.Computers
                .Include(c => c.Softwares).ThenInclude(b => b.SoftwareTechnicalDetails)
                .Include(c => c.Softwares).ThenInclude(b => b.Licence).ThenInclude(v => v.LicenceDetails)
                .Include(c => c.Softwares).ThenInclude(b => b.Licence).ThenInclude(v => v.LicenceType)
                .Include(c => c.Audience)
                .FirstOrDefaultAsync(m => m.Id == id);               
            
            if (computer == null)
            {
                return NotFound();
            }

            return View(computer);
        }

        // GET: Computers/Create
        [Authorize(Roles = "admin, employee")]
        public RedirectToActionResult Create()
        {
            return RedirectToAction(nameof(Index), "ComputerAdding");
            //ViewData["AudienceId"] = new SelectList(_context.Audiences, "Id", "Name");
            //return View();
        }
        [Authorize(Roles = "admin, employee")]
        // POST: Computers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AudienceId,IpAdress,Processor,Videocard,RAM,TotalSpace")] Computer computer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(computer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AudienceId"] = new SelectList(_context.Audiences, "Id", "Name", computer.AudienceId);
            return View(computer);
        }
        [Authorize(Roles = "admin, employee")]
        // GET: Computers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Computers == null)
            {
                return NotFound();
            }

            var computer = await _context.Computers.FindAsync(id);
            if (computer == null)
            {
                return NotFound();
            }
            ViewData["AudienceId"] = new SelectList(_context.Audiences, "Id", "Name", computer.AudienceId);
            return View(computer);
        }
        [Authorize(Roles = "admin, employee")]
        // POST: Computers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AudienceId,Number,IpAdress,Processor,Videocard,RAM,TotalSpace")] Computer computer)
        {
            if (id != computer.Id)
            {
                return NotFound();
            }


            

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(computer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComputerExists(computer.Id))
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
            ViewData["AudienceId"] = new SelectList(_context.Audiences, "Id", "Name", computer.AudienceId);
            return View(computer);
        }
        [Authorize(Roles = "admin, employee")]
        // GET: Computers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Computers == null)
            {
                return NotFound();
            }

            var computer = await _context.Computers
                .Include(c => c.Audience)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (computer == null)
            {
                return NotFound();
            }

            return View(computer);
        }
        [Authorize(Roles = "admin, employee")]
        // POST: Computers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Computers == null)
            {
                return Problem("Entity set 'AppDBContext.Computers'  is null.");
            }
            var computer =  await _context.Computers.Include(t => t.Softwares).ThenInclude(t=>t.Licence).ThenInclude(t=>t.LicenceDetails).FirstAsync(t => t.Id == id);
            computer.Softwares.Where(t=> t.Licence != null && t.Licence.LicenceDetails != null).ToList().ForEach(t => t.Licence.LicenceDetails.Count += 1);
            if (computer != null)
            {
                _context.Computers.Remove(computer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public FileResult GetReport()
        {
            // Путь к файлу с шаблоном
            string path = "/Reports/computer_report_template.xlsx";
            //Путь к файлу с результатом
            string result = "/Reports/computer_report.xlsx";
            FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + path);
            FileInfo fr = new FileInfo(_appEnvironment.WebRootPath + result);
            //будем использовть библитотеку не для коммерческого использования
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //открываем файл с шаблоном
            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                //устанавливаем поля документа
                excelPackage.Workbook.Properties.Author = "Лука Р.Н.";
                excelPackage.Workbook.Properties.Title = "Список установленного программного обеспечения";
                excelPackage.Workbook.Properties.Subject = "Установленное программное обеспечение";
                excelPackage.Workbook.Properties.Created = DateTime.Now;
                //плучаем лист по имени.
                ExcelWorksheet worksheet =excelPackage.Workbook.Worksheets["InstalledSoftware"];
                //получаем списко пользователей и в цикле заполняем лист данными
                int currentLine = 3;
                int num = 1;
                List<Computer> computers = _context.Computers.Include(c => c.Audience).Include(c => c.Softwares)
                .Include(c => c.Softwares).ThenInclude(t => t.SoftwareTechnicalDetails)
                .Include(c => c.Softwares).ThenInclude(t => t.Licence).ThenInclude(l => l.LicenceType)
                .Include(c => c.Softwares).ThenInclude(t => t.Licence).ThenInclude(l => l.LicenceDetails)
                .Include(c => c.Softwares).ThenInclude(t => t.Licence).ThenInclude(l => l.Employee).ToList();
                foreach (Computer computer in computers)
                {
                    worksheet.Cells[currentLine, 1].Value = num;
                    worksheet.Cells[currentLine, 2].Value = computer.Number;
                    if (!(computer.Audience is null))
                        worksheet.Cells[currentLine, 3].Value = computer.Audience.Name;
                    foreach(Software software in computer.Softwares)
                    {
                        worksheet.Cells[currentLine, 4].Value = software.SoftwareTechnicalDetails.Name;

                        if(!(software.Licence.LicenceType is null))
                            worksheet.Cells[currentLine, 5].Value = software.Licence.LicenceType.Name;
                        if(!(software.Licence.LicenceDetails.Key is null))
                            worksheet.Cells[currentLine, 6].Value = software.Licence.LicenceDetails.Key;

                        worksheet.Cells[currentLine, 7].Value = software.Licence.LicenceDetails.DateStart.Date.ToString("dd-MM-yyyy");
                        worksheet.Cells[currentLine, 8].Value = software.Licence.LicenceDetails.DateEnd.Date.ToString("dd-MM-yyyy");
                        //worksheet.Cells[currentLine, 9].Value = software.Licence.LicenceDetails.Price;
                        //worksheet.Cells[currentLine, 10].Value = software.Licence.LicenceDetails.Count;
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
            string file_name = "Computer_report.xlsx";
            return File(result, file_type, file_name);
        }
        private bool ComputerExists(int id)
        {
          return _context.Computers.Any(e => e.Id == id);
        }
    }
}
