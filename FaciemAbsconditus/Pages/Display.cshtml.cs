using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FaciemAbsconditus.Pages
{
    public class DisplayModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public string FileName { get; private set; }

        public DisplayModel(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult OnGet(string fileName)
        {
            var filePath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "SavedFiles", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return StatusCode(500);
            }

            FileName = fileName;

            return Page();
        }
    }
}