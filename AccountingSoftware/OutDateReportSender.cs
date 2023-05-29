using AccountingSoftware.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Net.Mail;
using System.Net;

using Quartz;

namespace AccountingSoftware.Jobs
{
    public class OutDateReportSender : IJob
    {
        const string file_path_template = "/Reports/outdate_software_report_template.xlsx";
        const string file_path_report = "/Reports/outdate_software_report.xlsx";
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        public OutDateReportSender(AppDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }
        int count = 0;
        async Task PrepareReportAsync()
        {
            // Путь к файлу с шаблоном
            //Путь к файлу с результатом
            FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + file_path_template);
            FileInfo fr = new FileInfo(_appEnvironment.WebRootPath + file_path_report);
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

                List<Software> expiredSoftwares = softwares.Where(l => l.Licence.LicenceDetails.DateEnd <= DateTime.Now).ToList();
                count = expiredSoftwares.Count;
                foreach (Software software in expiredSoftwares)
                {
                    worksheet.Cells[currentLine, 1].Value = num;
                    worksheet.Cells[currentLine, 2].Value = software.SoftwareTechnicalDetails.Name;
                    if(software.SoftwareTechnicalDetails.SubjectArea != null) { worksheet.Cells[currentLine, 3].Value = software.SoftwareTechnicalDetails.SubjectArea.Name; }                   
                    worksheet.Cells[currentLine, 4].Value = software.Licence.LicenceDetails.Key;
                    if(software.Licence.LicenceType!=null)
                        worksheet.Cells[currentLine, 5].Value = software.Licence.LicenceType.Name;
                    worksheet.Cells[currentLine, 6].Value = software.Licence.LicenceDetails.DateStart.ToString("dd-MM-yyyy");
                    worksheet.Cells[currentLine, 7].Value = software.Licence.LicenceDetails.DateEnd.ToString("dd-MM-yyyy");
                    worksheet.Cells[currentLine, 8].Value = software.Licence.LicenceDetails.Price;
                    worksheet.Cells[currentLine, 9].Value = software.Licence.LicenceDetails.Count;
                    if(software.Licence.Employee!= null) 
                        worksheet.Cells[currentLine, 10].Value = software.Licence.Employee.FullName;
                    currentLine++;
                    num++;
                }
                //созраняем в новое место
                await excelPackage.SaveAsAsync(fr);
                excelPackage.Dispose();
            }
            //// Тип файла - content-type
            //string file_type ="application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet";
            //// Имя файла - необязательно
            //string file_name = "Licences_report.xlsx";
            ////созраняем в новое место
            //worksheet.SaveAs(file_path_report);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                if (File.Exists(_appEnvironment.WebRootPath + file_path_report))
                    File.Delete(_appEnvironment.WebRootPath + file_path_report);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            await PrepareReportAsync();
            if (count > 0)
            {
                // отправитель - устанавливаем адрес и отображаемое в письме имя
                MailAddress from = new MailAddress("lukarus22@yandex.ru", "Система автоматической отчетности");
                // кому отправляем
                MailAddress to = new MailAddress("lukarus22@yandex.ru");
                // создаем объект сообщения
                MailMessage m = new MailMessage(from, to);
                // тема письма
                m.Subject = "!!!Отчет об истекших лицензиях!!!";
                // текст письма
                m.Body = "<h2>!!!Системой был сформирован и отправлен отчет об истекших лицензиях!!!</h2>";
                // письмо представляет код html
                m.IsBodyHtml = true;
                using(SmtpClient smtpClient = new SmtpClient("smtp.yandex.ru", 587))
                { // адрес smtp-сервера и порт, с которого будем отправлять письмо
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                    // логин и пароль
                    smtpClient.Credentials = new NetworkCredential("lukarus22@yandex.ru",
                    "uyjpezpjxnsgrooa"); //eEaJVhzFh1K0jewPp66Q
                    smtpClient.EnableSsl = true;
                    // вкладываем файл в письмо
                    m.Attachments.Add(new Attachment(_appEnvironment.WebRootPath + file_path_report));
                    // отправляем асинхронно
                    smtpClient.DeliveryFormat = SmtpDeliveryFormat.SevenBit;
                    smtpClient.Timeout = 500;
                    await smtpClient.SendMailAsync(m);
                }               
                count = 0;
            }
        }
    }
}
