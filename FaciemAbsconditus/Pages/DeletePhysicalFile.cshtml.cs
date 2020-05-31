using FaciemAbsconditus.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.FileProviders;

namespace SampleApp.Pages
{
    public class DeletePhysicalFileModel : PageModel
    {
        private readonly IFileService _fileService;

        public DeletePhysicalFileModel(IFileService fileService)
        {
            _fileService = fileService;
        }

        public IFileInfo RemoveFile { get; private set; }

        public IActionResult OnGet(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return RedirectToPage("/Index");
            }

            RemoveFile = _fileService.GetFileInfo(fileName);

            if (!RemoveFile.Exists)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public IActionResult OnPost(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return RedirectToPage("/Index");
            }

            RemoveFile = _fileService.GetFileInfo(fileName);

            if (RemoveFile.Exists)
            {
                _fileService.Delete(fileName);
            }

            return RedirectToPage("./Index");
        }
    }
}
