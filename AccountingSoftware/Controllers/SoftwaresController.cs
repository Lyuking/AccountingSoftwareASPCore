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
    public class SoftwaresController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        public SoftwaresController(AppDBContext context,IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: Softwares
        [Authorize(Roles = "admin, employee, guest")]

        public async Task<IActionResult> Index()
        {
            TempData.Clear();
            var appDBContext = _context.Softwares.Include(s => s.Licence).Include(s => s.SoftwareTechnicalDetails).Include(s => s.Licence.LicenceDetails).Include(s => s.Licence.LicenceType).Include(s => s.Licence.Employee);
            return View(await appDBContext.ToListAsync());
        }

        // GET: Softwares/Details/5
        [Authorize(Roles = "admin, employee, guest")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Softwares == null)
            {
                return NotFound();
            }

            var software = await _context.Softwares
                .Include(s => s.Licence)
                .Include(s => s.SoftwareTechnicalDetails)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (software == null)
            {
                return NotFound();
            }

            return View(software);
        }
        [Authorize(Roles = "admin, employee")]

        // GET: Softwares/Create
        public IActionResult Create()
        {
            //ViewData["LicenceId"] = new SelectList(_context.Licences, "Id", "Key");
            //ViewData["SoftwareTechnicalDetailsId"] = new SelectList(_context.SoftwareTechnicalDetailses, "Id", "Key");
            return RedirectToAction("SelectSoftware", "LicenceAdding");
        }
        [Authorize(Roles = "admin, employee")]
        // POST: Softwares/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SoftwareTechnicalDetailsId,LicenceId")] Software software)
        {
            if (ModelState.IsValid)
            {
                _context.Add(software);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LicenceId"] = new SelectList(_context.Licences, "Id", "Id", software.LicenceId);
            ViewData["SoftwareTechnicalDetailsId"] = new SelectList(_context.SoftwareTechnicalDetailses, "Id", "Id", software.SoftwareTechnicalDetailsId);
            return View(software);
        }
        [Authorize(Roles = "admin, employee")]

        // GET: Softwares/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Softwares == null)
            {
                return NotFound();
            }

            var software = await _context.Softwares.FindAsync(id);
            if (software == null)
            {
                return NotFound();
            }
            ViewData["LicenceId"] = new SelectList(_context.Licences, "Id", "Id", software.LicenceId);
            ViewData["SoftwareTechnicalDetailsId"] = new SelectList(_context.SoftwareTechnicalDetailses, "Id", "Id", software.SoftwareTechnicalDetailsId);
            return View(software);
        }

        // POST: Softwares/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, employee")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SoftwareTechnicalDetailsId,LicenceId")] Software software)
        {
            if (id != software.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(software);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SoftwareExists(software.Id))
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
            ViewData["LicenceId"] = new SelectList(_context.Licences, "Id", "Id", software.LicenceId);
            ViewData["SoftwareTechnicalDetailsId"] = new SelectList(_context.SoftwareTechnicalDetailses, "Id", "Id", software.SoftwareTechnicalDetailsId);
            return View(software);
        }

        // GET: Softwares/Delete/5
        [Authorize(Roles = "admin, employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Softwares == null)
            {
                return NotFound();
            }

            var software = await _context.Softwares
                .Include(s => s.Licence).ThenInclude(s=>s.LicenceDetails)
                .Include(s => s.SoftwareTechnicalDetails)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (software == null)
            {
                return NotFound();
            }

            return View(software);
        }

        // POST: Softwares/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin, employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Softwares == null)
            {
                return Problem("Entity set 'AppDBContext.Softwares'  is null.");
            }
            var software = await _context.Softwares.Include(t=>t.Licence).ThenInclude(t=>t.LicenceDetails).FirstAsync(t => t.Id == id);           
            if (software != null)
            {
                _context.LicenceDetailses.Remove(software.Licence.LicenceDetails);
                _context.Softwares.Remove(software);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SoftwareExists(int id)
        {
          return _context.Softwares.Any(e => e.Id == id);
        }
        public FileResult GetReport()
        {
            
            // Путь к файлу с шаблоном
            string path = "/Reports/software_report_template.xlsx";
            //Путь к файлу с результатом
            string result = "/Reports/software_report.xlsx";
            FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + path);
            FileInfo fr = new FileInfo(_appEnvironment.WebRootPath + result);
            //будем использовть библитотеку не для коммерческого использования
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            //открываем файл с шаблоном
            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                //устанавливаем поля документа
                excelPackage.Workbook.Properties.Author = "Лука Р.Н.";
                excelPackage.Workbook.Properties.Title = "Список установленного программного обеспечения";
                excelPackage.Workbook.Properties.Subject = "Установленное программное обеспечение";
                excelPackage.Workbook.Properties.Created = DateTime.Now;
                //плучаем лист по имени.
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["SoftwaresLicences"];
                //получаем списко пользователей и в цикле заполняем лист данными
                int currentLine = 3;
                int num = 1;
                List<Software> softwares = _context.Softwares.Include(s => s.Licence).Include(s => s.SoftwareTechnicalDetails).
                    Include(s => s.Licence.LicenceDetails).Include(s => s.Licence.LicenceType).Include(s => s.Licence.Employee).Include(s => s.SoftwareTechnicalDetails.SubjectArea).ToList();

                foreach (Software software in softwares)
                {
                    worksheet.Cells[currentLine, 1].Value = num;
                    worksheet.Cells[currentLine, 2].Value = software.SoftwareTechnicalDetails.Name;
                    if (software.SoftwareTechnicalDetails.SubjectArea != null) { worksheet.Cells[currentLine, 3].Value = software.SoftwareTechnicalDetails.SubjectArea.Name; }
                    worksheet.Cells[currentLine, 4].Value = software.Licence.LicenceDetails.Key;
                    if (software.Licence.LicenceType != null)
                        worksheet.Cells[currentLine, 5].Value = software.Licence.LicenceType.Name;
                    worksheet.Cells[currentLine, 6].Value = software.Licence.LicenceDetails.DateStart.ToString("dd-MM-yyyy");
                    worksheet.Cells[currentLine, 7].Value = software.Licence.LicenceDetails.DateEnd.ToString("dd-MM-yyyy");
                    worksheet.Cells[currentLine, 8].Value = software.Licence.LicenceDetails.Price;
                    worksheet.Cells[currentLine, 9].Value = software.Licence.LicenceDetails.Count;
                    if (software.Licence.Employee != null)
                        worksheet.Cells[currentLine, 10].Value = software.Licence.Employee.FullName;
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
            string file_name = "Licences_report.xlsx";
            return File(result, file_type, file_name);
        }
        //Генерация отчета о ПО, лицензия которого истечет через 7 дней
        public FileResult GetExpiredReport()
        {

            // Путь к файлу с шаблоном
            string path = "/Reports/expiring_software_report_template.xlsx";
            //Путь к файлу с результатом
            string result = "/Reports/expiring_software_report.xlsx";
            FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + path);
            FileInfo fr = new FileInfo(_appEnvironment.WebRootPath + result);
            //будем использовть библитотеку не для коммерческого использования
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            //открываем файл с шаблоном
            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                //устанавливаем поля документа
                excelPackage.Workbook.Properties.Author = "Лука Р.Н.";
                excelPackage.Workbook.Properties.Title = "Список установленного программного обеспечения";
                excelPackage.Workbook.Properties.Subject = "Установленное программное обеспечение";
                excelPackage.Workbook.Properties.Created = DateTime.Now;
                //плучаем лист по имени.
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["SoftwaresLicences"];
                //получаем списко пользователей и в цикле заполняем лист данными
                int currentLine = 3;
                int num = 1;
                List<Software> softwares = _context.Softwares.Include(s => s.Licence).Include(s => s.SoftwareTechnicalDetails).
                    Include(s => s.Licence.LicenceDetails).Include(s => s.Licence.LicenceType).Include(s => s.Licence.Employee).Include(s => s.SoftwareTechnicalDetails.SubjectArea)
                    .Where(l => l.Licence.LicenceDetails.DateEnd <= DateTime.Now.AddDays(7) && l.Licence.LicenceDetails.DateEnd > DateTime.Now).ToList();

                foreach (Software software in softwares)
                {
                    worksheet.Cells[currentLine, 1].Value = num;
                    worksheet.Cells[currentLine, 2].Value = software.SoftwareTechnicalDetails.Name;
                    if (software.SoftwareTechnicalDetails.SubjectArea != null) { worksheet.Cells[currentLine, 3].Value = software.SoftwareTechnicalDetails.SubjectArea.Name; }
                    worksheet.Cells[currentLine, 4].Value = software.Licence.LicenceDetails.Key;
                    if (software.Licence.LicenceType != null)
                        worksheet.Cells[currentLine, 5].Value = software.Licence.LicenceType.Name;
                    worksheet.Cells[currentLine, 6].Value = software.Licence.LicenceDetails.DateStart.ToString("dd-MM-yyyy");
                    worksheet.Cells[currentLine, 7].Value = software.Licence.LicenceDetails.DateEnd.ToString("dd-MM-yyyy");
                    worksheet.Cells[currentLine, 8].Value = software.Licence.LicenceDetails.Price;
                    worksheet.Cells[currentLine, 9].Value = software.Licence.LicenceDetails.Count;
                    if (software.Licence.Employee != null)
                        worksheet.Cells[currentLine, 10].Value = software.Licence.Employee.FullName;
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
            string file_name = "Expired_licences_report.xlsx";
            return File(result, file_type, file_name);
        }
        //Генерация отчета о ПО, лицензия которого уже истекла
        public FileResult GetOutDateReport()
        {

            // Путь к файлу с шаблоном
            string path = "/Reports/outdate_software_report_template.xlsx";
            //Путь к файлу с результатом
            string result = "/Reports/outdate_software_report.xlsx";
            FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + path);
            FileInfo fr = new FileInfo(_appEnvironment.WebRootPath + result);
            //будем использовть библитотеку не для коммерческого использования
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            //открываем файл с шаблоном
            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                //устанавливаем поля документа
                excelPackage.Workbook.Properties.Author = "Лука Р.Н.";
                excelPackage.Workbook.Properties.Title = "Список установленного программного обеспечения";
                excelPackage.Workbook.Properties.Subject = "Установленное программное обеспечение";
                excelPackage.Workbook.Properties.Created = DateTime.Now;
                //плучаем лист по имени.
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["SoftwaresLicences"];
                //получаем списко пользователей и в цикле заполняем лист данными
                int currentLine = 3;
                int num = 1;
                List<Software> softwares = _context.Softwares.Include(s => s.Licence).Include(s => s.SoftwareTechnicalDetails).
                    Include(s => s.Licence.LicenceDetails).Include(s => s.Licence.LicenceType).Include(s => s.Licence.Employee).Include(s => s.SoftwareTechnicalDetails.SubjectArea)
                    .Where(l => l.Licence.LicenceDetails.DateEnd <= DateTime.Now).ToList();

                foreach (Software software in softwares)
                {
                    worksheet.Cells[currentLine, 1].Value = num;
                    worksheet.Cells[currentLine, 2].Value = software.SoftwareTechnicalDetails.Name;
                    if (software.SoftwareTechnicalDetails.SubjectArea != null) { worksheet.Cells[currentLine, 3].Value = software.SoftwareTechnicalDetails.SubjectArea.Name; }
                    worksheet.Cells[currentLine, 4].Value = software.Licence.LicenceDetails.Key;
                    if (software.Licence.LicenceType != null)
                        worksheet.Cells[currentLine, 5].Value = software.Licence.LicenceType.Name;
                    worksheet.Cells[currentLine, 6].Value = software.Licence.LicenceDetails.DateStart.ToString("dd-MM-yyyy");
                    worksheet.Cells[currentLine, 7].Value = software.Licence.LicenceDetails.DateEnd.ToString("dd-MM-yyyy");
                    worksheet.Cells[currentLine, 8].Value = software.Licence.LicenceDetails.Price;
                    worksheet.Cells[currentLine, 9].Value = software.Licence.LicenceDetails.Count;
                    if (software.Licence.Employee != null)
                        worksheet.Cells[currentLine, 10].Value = software.Licence.Employee.FullName;
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
            string file_name = "Outdate_licences_report.xlsx";
            return File(result, file_type, file_name);
        }
    }
}
