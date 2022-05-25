namespace Services.ComputerManagement
{
    public interface IScreenLightService
    {
        void SetBrightness(int iPercent);

        int GetBrightness();
    }
}