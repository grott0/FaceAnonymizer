using Microsoft.Extensions.FileProviders;
using System.Threading.Tasks;

namespace FaciemAbsconditus.Services
{
    public interface IFileService
    {
        /// <summary>
        /// Creates a file in the storage directory.
        /// </summary>
        /// <param name="subpath">Relative path that identifies the file.</param>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        Task CreateAsync(string subpath, byte[] fileContent);

        /// <summary>
        /// Deletes a file from the storage directory
        /// </summary>
        /// <param name="subpath">Relative path that identifies the file.</param>
        /// <returns></returns>
        /// <exception cref="System.IO.FileNotFoundException">Thrown when trying to delete a non-existant file.</exception>
        Task DeleteAsync(string subpath);
 
        /// <summary>
        /// Enumerate a directory at the given path, if any.
        /// </summary>
        /// <param name="subpath">Relative path that identifies the directory.</param>
        /// <returns>Returns the contents of the directory.</returns>
        IDirectoryContents GetDirectoryContents(string subpath);

        /// <summary>
        /// Locate a file at the given path.
        /// </summary>
        /// <param name="subpath">Relative path that identifies the file.</param>
        /// <returns>The file information. Caller must check Exists property.</returns>
        IFileInfo GetFileInfo(string subpath);

    }
}
