using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ConsumerModule.GitLab.Data;
using ConsumerModule.GitLab.Domain;
using ConsumerModule.GitLab.XMLConverter;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace ConsumerModule.GitLab.Controllers
{
    [ApiController]
    [Route("data")]
    public class ExcelController : Controller
    {
        private readonly IGitLabHandler _gitLabHandler;
        private readonly IExcelManager _excelManager;

        public ExcelController(IGitLabHandler gitLabHandler)
        {
            _gitLabHandler = gitLabHandler;
        }
        
        [HttpGet]
        [Route("download")]
        public async Task<IActionResult> GetTrial()
        {
            // Generate the file
            var myBus = new List<string> {"id", "Busname", "Buscode"};
            // above code loads the data using LINQ with EF (query of table), you can substitute this with any data source.
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(myBus, true);
                package.Save();
            }
            stream.Position = 0;

            string excelName = $"AwesomeExcelName.xlsx"; 
            // above I define the name of the file using the current datetime.

            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = excelName
            }; 
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> PostSomeData()
        {
            await _excelManager.PostExcelData();

            return StatusCode(200);
        }
    }
}