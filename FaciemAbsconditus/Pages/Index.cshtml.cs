using FaciemAbsconditus.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.FileProviders;
using System.Net.Mime;
using System.Threading.Tasks;

namespace FaciemAbsconditus.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IFileService _fileService;

        public IndexModel(IFileService fileService)
        {
            _fileService = fileService;
        }

        public IDirectoryContents PhysicalFiles { get; private set; }

        public async Task OnGetAsync()
        {
            PhysicalFiles = _fileService.GetDirectoryContents(string.Empty);
        }

        public IActionResult OnGetDownloadPhysical(string fileName)
        {
            var downloadFile = _fileService.GetFileInfo(fileName);

            return PhysicalFile(downloadFile.PhysicalPath, MediaTypeNames.Application.Octet, fileName);
        }
    }
}
