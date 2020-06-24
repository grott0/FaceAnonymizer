using FaciemAbsconditus.Services;
using System;
using System.IO;
using FasciemAbsconditus.Test.Mocks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace FasciemAbsconditus.Test
{
    public class Program
    {
        private static string _lfwLogFilePath = "lfwLog.txt";
        private static string _elefanteLogFilePath = "elefanteLog.txt";
        private static string _monkeysLogFilePath = "monkeysLog.txt";

        static void Main()
        {
            DetectInLFWDataset();
            DetectInMonkeysDataset();
            DetectInElefanteDataset();
        }

        static void DetectInLFWDataset()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            IFaceAnonymizationService anonymizationService = new MockFaceAnonymizationService();
            var imageDirectories = Directory.EnumerateDirectories(@"..\..\..\Data\lfw");
            var failedDetections = new ConcurrentBag<string>();
            var successfulDetections = new ConcurrentBag<string>();
            var exceptions = new ConcurrentBag<string>();
            var totalImages = 0;

            Parallel.ForEach(imageDirectories, new ParallelOptions() { MaxDegreeOfParallelism = 8 },
                (imageDirectory) =>
                {
                    var imageFiles = Directory.EnumerateFiles(imageDirectory);
                    foreach (var imageFile in imageFiles)
                    {
                        totalImages++;
                        var imageFullPath = Path.GetFullPath(imageFile);
                        try
                        {
                            var success = anonymizationService.AnonymizeFaces(imageFullPath, AnonymizationMethods.pixelated, 20, 0.9);
                            Console.WriteLine(totalImages + " -> " + success);

                            if (success == "N")
                            {
                                failedDetections.Add(imageFullPath);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                });

            stopwatch.Stop();

            File.AppendAllText(_lfwLogFilePath, Environment.NewLine + "Total images: " + totalImages);
            File.AppendAllText(_lfwLogFilePath, Environment.NewLine + "Failed detections: " + failedDetections.Count);
            File.AppendAllLines(_lfwLogFilePath, failedDetections);
            File.AppendAllLines(_lfwLogFilePath, exceptions);
            File.AppendAllText(_lfwLogFilePath, Environment.NewLine + "Running time: " + stopwatch.Elapsed);
        }
        
        static void DetectInMonkeysDataset()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            IFaceAnonymizationService anonymizationService = new MockFaceAnonymizationService();
            var imageDirectories = Directory.EnumerateDirectories(@"..\..\..\Data\monkeys");
            var successfulDetections = new ConcurrentBag<string>();
            var exceptions = new ConcurrentBag<string>();
            var totalImages = 0;

            Parallel.ForEach(imageDirectories, new ParallelOptions() { MaxDegreeOfParallelism = 8 },
                (imageDirectory) =>
                {
                    var imageFiles = Directory.EnumerateFiles(imageDirectory);
                    foreach (var imageFile in imageFiles)
                    {
                        totalImages++;
                        Console.WriteLine(totalImages);
                        var imageFullPath = Path.GetFullPath(imageFile);
                        try
                        {
                            var success = anonymizationService.AnonymizeFaces(imageFullPath, AnonymizationMethods.simple, 20, 0.9);
                            if (success == "Y")
                            {
                                successfulDetections.Add(imageFullPath);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                });

            stopwatch.Stop();

            File.AppendAllText(_monkeysLogFilePath, Environment.NewLine + "Total images: " + totalImages);
            File.AppendAllText(_monkeysLogFilePath, Environment.NewLine + "SuccessfulDetections: " + successfulDetections.Count);
            File.AppendAllLines(_monkeysLogFilePath, successfulDetections);
            File.AppendAllLines(_monkeysLogFilePath, exceptions);
            File.AppendAllText(_monkeysLogFilePath, Environment.NewLine + "Running time: " + stopwatch.Elapsed);
        }

        static void DetectInElefanteDataset()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            IFaceAnonymizationService anonymizationService = new MockFaceAnonymizationService();
            var imageFiles = Directory.EnumerateFiles(@"..\..\..\Data\elefante");
            var successfulDetections = new ConcurrentBag<string>();
            var exceptions = new ConcurrentBag<string>();
            var totalImages = 0;

            Parallel.ForEach(imageFiles, new ParallelOptions() { MaxDegreeOfParallelism = 8 },
                (imageFile) =>
                {

                    totalImages++;
                    Console.WriteLine(totalImages);
                    var imageFullPath = Path.GetFullPath(imageFile);
                    try
                    {
                        var success = anonymizationService.AnonymizeFaces(imageFullPath, AnonymizationMethods.simple, 20, 0.9);
                        if (success == "Y")
                        {
                            successfulDetections.Add(imageFullPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                });

            stopwatch.Stop();

            File.AppendAllText(_elefanteLogFilePath, Environment.NewLine + "Total images: " + totalImages);
            File.AppendAllText(_elefanteLogFilePath, Environment.NewLine + "SuccessfulDetections: " + successfulDetections.Count);
            File.AppendAllLines(_elefanteLogFilePath, successfulDetections);
            File.AppendAllLines(_elefanteLogFilePath, exceptions);
            File.AppendAllText(_elefanteLogFilePath, Environment.NewLine + "Running time: " + stopwatch.Elapsed);
        }
    }
}
