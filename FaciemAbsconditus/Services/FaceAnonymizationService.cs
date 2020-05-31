using Microsoft.AspNetCore.Hosting;
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

        /// <summary>
        /// Detects and anonymizes faces in an image.
        /// </summary>
        /// <param name="imageName">Path to an image.</param>
        /// <param name="anonymizationMethod">Face anonymization method.</param>
        /// <param name="blocks">Number of blocks for the pixelated anonymization method.</param>
        /// <param name="confidence">Threshold for filtering out weak detections.</param>
        public void AnonymizeFace(string imageName, AnonymizationMethods anonymizationMethod, int blocks = 20, double confidence = 0.5)
        {
            var anonymizationMethodArgument = Enum.GetName(typeof(AnonymizationMethods), anonymizationMethod);
            var pathToImage = Path.Combine(_webHostEnvironment.WebRootPath, "SavedFiles", imageName);

            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                // Move to config file.
                FileName = @"C:\Users\Georgi Simeonov\AppData\Local\Microsoft\WindowsApps\PythonSoftwareFoundation.Python.3.8_qbz5n2kfra8p0\python.exe",
                UseShellExecute = false,
                Arguments = $".\\Python\\blur_face.py --image {pathToImage} --face .\\Python\\face_detector --method {anonymizationMethodArgument} --blocks {blocks} --confidence {confidence}",
                RedirectStandardError = true
            };

            using (var process = Process.Start(processStartInfo))
            {
                process.WaitForExit();

                StreamReader errorStream = process.StandardError;
                var standardError = errorStream.ReadToEnd();

                if (process.ExitCode != 0)
                {
                    throw new System.Exception(standardError);
                }
            }
        }
    }
}
