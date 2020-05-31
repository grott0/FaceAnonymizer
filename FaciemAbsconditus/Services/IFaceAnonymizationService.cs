namespace FaciemAbsconditus.Services
{
    public interface IFaceAnonymizationService
    {
        void AnonymizeFace(string imagePath, AnonymizationMethods anonymizationMethod, int blocks = 20, double confidence = 0.5);
    }
}
