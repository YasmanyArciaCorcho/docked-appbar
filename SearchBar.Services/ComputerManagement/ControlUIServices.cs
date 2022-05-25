using System;
using System.Diagnostics;

namespace Services.ComputerManagement
{
    public class ControlUIServices: ISystemControlUiOpener
    {
        private const string ControlPanel = @"c:\windows\system32\control.exe";

        public void OpenSettings()
        {
            Process.Start(ControlPanel);
        }
        private static void OpenComputerSettings(string ui)
        {
            Process.Start(ControlPanel, ui);
        }
        
        public void OpenUserAccount()
        {
            const string userAccounts = @"/name Microsoft.UserAccounts";
            OpenComputerSettings(userAccounts);
        }

        public void OpenSoundDevices()
        {
            const string soundSettings = @"/name Microsoft.AudioDevicesAndSoundThemes";
            OpenComputerSettings(soundSettings);
        }

        public void OpenBlueToothSettings()
        {
            const string blueTooth = @"bthprops.cpl";
            OpenComputerSettings(blueTooth);
        }
        public void OpenWifiSettings()
        {
            const string networkSettings = @"/name Microsoft.NetworkAndSharingCenter";
            OpenComputerSettings(networkSettings);;
        }
    }
}