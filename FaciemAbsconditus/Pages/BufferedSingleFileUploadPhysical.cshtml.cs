using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using FaciemAbsconditus.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using SampleApp.Utilities;

namespace SampleApp.Pages
{
    public class BufferedSingleFileUploadPhysicalModel : PageModel
    {
        private readonly long _fileSizeLimit;
        private readonly string[] _permittedExtensions = { ".txt", ".jpg" };
        private readonly IFileService _fileService;
        private readonly IFaceAnonymizationService _faceAnonymizationService;

        [BindProperty]
        public BufferedSingleFileUploadPhysical FileUpload { get; set; }

        public string Result { get; private set; }

        public BufferedSingleFileUploadPhysicalModel(IConfiguration config, IFaceAnonymizationService faceAnonymizationService, IFileService fileService)
        {
            _fileSizeLimit = config.GetValue<long>("FileSizeLimit");
            _faceAnonymizationService = faceAnonymizationService;
            _fileService = fileService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostUploadAsync()
        {
            if (!ModelState.IsValid)
            {
                Result = "Please correct the form.";

                return Page();
            }

            var formFileContent =
                await FileHelpers.ProcessFormFile<BufferedSingleFileUploadPhysical>(
                    FileUpload.FormFile, ModelState, _permittedExtensions,
                    _fileSizeLimit);

            if (!ModelState.IsValid)
            {
                Result = "Please correct the form.";

                return Page();
            }

            // For the file name of the uploaded file stored
            // server-side, use Path.GetRandomFileName to generate a safe
            // random file name.
            var extension = ".jpg"; // Find a way to get the original extension of the file.
            var trustedFileNameForStorage = Path.GetRandomFileName().Replace(".", "") + extension;

            // **WARNING!**
            // In the following example, the file is saved without
            // scanning the file's contents. In most production
            // scenarios, an anti-virus/anti-malware scanner API
            // is used on the file before making the file available
            // for download or for use by other systems. 
            // For more information, see the topic that accompanies 
            // this sample.

            await _fileService.CreateAsync(trustedFileNameForStorage, formFileContent);

            try
            {
                _faceAnonymizationService.AnonymizeFace(trustedFileNameForStorage, AnonymizationMethods.simple);
            }
            catch (System.Exception ex)
            {
                // Log!
                throw;
            }

            return Page();
        }
    }

    public class BufferedSingleFileUploadPhysical
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }

        [Display(Name = "Note")]
        [StringLength(50, MinimumLength = 0)]
        public string Note { get; set; }
    }
}
