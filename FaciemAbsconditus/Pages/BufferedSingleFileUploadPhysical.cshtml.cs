using FaciemAbsconditus.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using FaciemAbsconditus.Utilities;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace FaciemAbsconditus.Pages
{
    public class BufferedSingleFileUploadPhysicalModel : PageModel
    {
        private readonly long _fileSizeLimit;
        private readonly string[] _permittedExtensions;
        private readonly IFileService _fileService;
        private readonly IFaceAnonymizationService _faceAnonymizationService;

        [BindProperty]
        public BufferedSingleFileUploadPhysical FileUpload { get; set; }

        public string Result { get; private set; }

        public BufferedSingleFileUploadPhysicalModel(IConfiguration config, IFaceAnonymizationService faceAnonymizationService, IFileService fileService)
        {
            _fileSizeLimit = config.GetValue<long>("FileSizeLimit");
            _permittedExtensions = config.GetSection("PermittedExtensions").Get<string[]>();
            _faceAnonymizationService = faceAnonymizationService;
            _fileService = fileService;
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

            var extension = Path.GetExtension(FileUpload.FormFile.FileName);
            var randomFileName = Path.GetRandomFileName();
            var trustedFileNameForStorage = randomFileName.Replace(".", "") + extension;
            var anonymizedFileName = string.Empty;

            await _fileService.CreateAsync(trustedFileNameForStorage, formFileContent);
            try
            {
                anonymizedFileName = _faceAnonymizationService.AnonymizeFaces(trustedFileNameForStorage, AnonymizationMethods.simple);

                // If the anonymization process completed without anonymizing the image (e.g. no face detected),
                // the page should display an appropriate message.
                if (string.IsNullOrEmpty(anonymizedFileName))
                {
                    Result = "Failed to detect a face in the uploaded image. Please try again with a different image.";

                    return Page();
                }
            }
            catch (System.Exception)
            {
                // Log!
                throw;
            }

            return Redirect($"/Display/{anonymizedFileName}");
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
