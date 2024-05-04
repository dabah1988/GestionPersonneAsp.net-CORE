using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using ServicesContrat;

namespace CRUD.controllers
{

    public class CountryController : Controller
    {
        private readonly ICountryService _ICountryService  ;
        public CountryController( ICountryService ICountryService)
        {
            _ICountryService = ICountryService;
        }
 
        [HttpGet]
        [Route("UploadFile")]
        public IActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        [Route("UploadFile")]
        public async Task<IActionResult> UploadingFile(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ViewBag.message = "Veuillez choisir votre fichier ";
                return View("~/Views/Country/UploadFile.cshtml");
            }
            if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.message = "Veuillez choisir un fichier  Excel";
                return View("~/Views/Country/UploadFile.cshtml");
            }
            int nombreDeLignesInserees = await _ICountryService.UploadCountryFromExcelFile(excelFile);
            ViewBag.nbreRecords = nombreDeLignesInserees;
            return View("~/Views/Country/UploadFile.cshtml");
        }

    }
}
