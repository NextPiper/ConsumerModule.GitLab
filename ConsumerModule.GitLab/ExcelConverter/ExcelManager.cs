using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using System.IO.Packaging;
using Microsoft.Extensions.FileProviders;

namespace ConsumerModule.GitLab.XMLConverter
{
    public interface IExcelManager
    {
        //FileResult GetExcelData();
    }
    
    public class ExcelManager : IExcelManager
    {
        /*public FileResult GetExcelData() {
            string wwwrootPath = _hostingEnvironment.WebRootPath;
            string fileName = @"excel.xlsx";
            FileInfo file = new FileInfo(Path.Combine(wwwrootPath, fileName));

            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(wwwrootPath, fileName));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Employee");
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Gender";
                worksheet.Cells[1, 4].Value = "Salary (in $)";

                worksheet.Cells["A2"].Value = 1000;
                worksheet.Cells["B2"].Value = "Jon";
                worksheet.Cells["C2"].Value = "M";
                worksheet.Cells["D2"].Value = 5000;

                worksheet.Cells["A3"].Value = 1001;
                worksheet.Cells["B3"].Value = "Graham";
                worksheet.Cells["C3"].Value = "M";
                worksheet.Cells["D3"].Value = 10000;

                worksheet.Cells["A4"].Value = 1002;
                worksheet.Cells["B4"].Value = "Jenny";
                worksheet.Cells["C4"].Value = "F";
                worksheet.Cells["D4"].Value = 5000;

                package.Save(); 
                downloadFile(wwwrootPath);

            } 
        }

        private void CreateFile()
        {
            
        }

        private FileResult DownloadFile(string filePath, string fileName)
        {
            IFileProvider provider = new PhysicalFileProvider(filePath);
            IFileInfo fileInfo = provider.GetFileInfo(fileName);
            var readStream = fileInfo.CreateReadStream();
            var mimeType = "application/vnd.ms-excel";
            return File(readStream, mimeType, fileName);
        }*/
    }
}