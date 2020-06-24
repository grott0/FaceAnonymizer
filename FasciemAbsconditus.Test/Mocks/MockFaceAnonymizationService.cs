using FaciemAbsconditus.Services;
using System;
using System.Diagnostics;
using System.IO;

namespace FasciemAbsconditus.Test.Mocks
{
    public class MockFaceAnonymizationService : IFaceAnonymizationService
    {
        public string AnonymizeFaces(string imagePath, AnonymizationMethods anonymizationMethod, int blocks = 20, double confidence = 0.5)
        {
            var anonymizationMethodArgument = Enum.GetName(typeof(AnonymizationMethods), anonymizationMethod);

            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException();
            }

            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                // Move to config file.
                FileName = @"C:\Users\Georgi Simeonov\AppData\Local\Microsoft\WindowsApps\PythonSoftwareFoundation.Python.3.8_qbz5n2kfra8p0\python.exe",
                UseShellExecute = false,
                Arguments = $".\\Python\\test_detection.py --image \"{imagePath}\" --face .\\Python\\face_detector --method {anonymizationMethodArgument} --blocks {blocks} --confidence {confidence}",
                RedirectStandardError = true
            };

            using (var process = Process.Start(processStartInfo))
            {
                process.WaitForExit();

                using (StreamReader errorStream = process.StandardError)
                {
                    var standardError = errorStream.ReadToEnd();

                    if (process.ExitCode == 0)
                    {
                        return "Y";
                    }
                    else if (process.ExitCode == 738)
                    {
                        return "N";
                    }
                    else
                    {
                        throw new System.Exception(standardError);
                    }
                }
            }
        }
    }
}
