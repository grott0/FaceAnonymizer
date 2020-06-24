using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System;
using System.Diagnostics;
using System.IO;

namespace FaciemAbsconditus.Services
{
    public enum AnonymizationMethods
    {
        simple,
        pixelated
    }

    public class FaceAnonymizationService : IFaceAnonymizationService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FaceAnonymizationService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string AnonymizeFaces(string imageName, AnonymizationMethods anonymizationMethod, int blocks = 20, double confidence = 0.5)
        {
            var anonymizationMethodArgument = Enum.GetName(typeof(AnonymizationMethods), anonymizationMethod);
            var pathToImage = Path.Combine(_webHostEnvironment.WebRootPath, "SavedFiles", imageName);

            if (!File.Exists(pathToImage))
            {
                throw new FileNotFoundException();
            }

            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                // Move to config file.
                FileName = @"C:\Users\Georgi Simeonov\AppData\Local\Microsoft\WindowsApps\PythonSoftwareFoundation.Python.3.8_qbz5n2kfra8p0\python.exe",
                UseShellExecute = false,
                Arguments = $".\\Python\\blur_face.py --image \"{pathToImage}\" --face .\\Python\\face_detector --method {anonymizationMethodArgument} --blocks {blocks} --confidence {confidence}",
                RedirectStandardError = true
            };

            using (var process = Process.Start(processStartInfo))
            {
                process.WaitForExit();

                using (StreamReader errorStream = process.StandardError)
                {
                    var standardError = errorStream.ReadToEnd();

                    if (process.ExitCode != 0)
                    {
                        throw new System.Exception(standardError);
                    }
                }
            }

            var imageExtension = Path.GetExtension(pathToImage);
            var imageNameWithoutExtension = Path.GetFileNameWithoutExtension(pathToImage);
            var anonymizedImageName = imageNameWithoutExtension + "-anonymized" + imageExtension;
            var pathToAnonymizedImage = Path.Combine(_webHostEnvironment.WebRootPath, "SavedFiles", anonymizedImageName);

            if (!File.Exists(pathToAnonymizedImage))
            {
                return string.Empty;
            }

            return anonymizedImageName;
        }
    }
}
