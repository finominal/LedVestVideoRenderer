namespace LedArrayVideoRenderer.Domain
{
    public interface ILedManager
    {
        void FactorLeds(int videoWidth, int videoHeight, int ledWidth, int ledHeight);
    }
}