namespace FaciemAbsconditus.Services
{
    public interface IFaceAnonymizationService
    {
        /// <summary>
        /// Detects and anonymizes faces in an image.
        /// </summary>
        /// <param name="imageName">Path to an image.</param>
        /// <param name="anonymizationMethod">Face anonymization method.</param>
        /// <param name="blocks">Number of blocks for the pixelated anonymization method.</param>
        /// <param name="confidence">Threshold for filtering out weak detections.</param>
        /// <exception cref="System.IO.FileNotFoundException">Thrown when trying to anonymize a non-existant image.</exception>
        string AnonymizeFaces(string imagePath, AnonymizationMethods anonymizationMethod, int blocks = 20, double confidence = 0.5);
    }
}
