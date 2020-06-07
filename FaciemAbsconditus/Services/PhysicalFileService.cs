using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Threading.Tasks;

namespace FaciemAbsconditus.Services
{
    public class PhysicalFileService : IFileService
    {
        private readonly IFileProvider _fileProvider;
        private readonly string _storagePath;

        public PhysicalFileService(IWebHostEnvironment webHostEnvironment)
        {
            var storagePath = Path.Combine(webHostEnvironment.WebRootPath, "SavedFiles");

            if (!Directory.Exists(storagePath))
            {
                throw new DirectoryNotFoundException(storagePath);
            }

            _storagePath = storagePath;
            _fileProvider = new PhysicalFileProvider(_storagePath);
        }

        public async Task CreateAsync(string subpath, byte[] fileContent)
        {
            var filePath = Path.Combine(_storagePath, subpath);

            using (var fileStream = System.IO.File.Create(filePath))
            {
                await fileStream.WriteAsync(fileContent);
            }
        }

        public async Task DeleteAsync(string subpath)
        {
            var filePath = Path.Combine(_storagePath, subpath);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            File.Delete(filePath);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return _fileProvider.GetDirectoryContents(subpath);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            return _fileProvider.GetFileInfo(subpath);
        }
    }
}
