namespace Services.ComputerManagement
{
    public interface ISystemControlUiOpener
    {
        void OpenSettings();

        void OpenUserAccount();

        void OpenBlueToothSettings();

        void OpenWifiSettings();
    }
}