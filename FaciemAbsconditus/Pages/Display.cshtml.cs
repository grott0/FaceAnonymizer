using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace SampleApp
{
    public class DisplayModel : PageModel
    {
        private readonly string _storedFilesPath;
        public string FileName { get; private set; }

        public DisplayModel(IConfiguration config)
        {
            _storedFilesPath = config.GetValue<string>("StoredFilesPath");
        }

        public IActionResult OnGet(string fileName)
        {
            var filePath = System.IO.Path.Combine(_storedFilesPath, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return StatusCode(500);
            }

            FileName = fileName;

            return Page();
        }
    }
}