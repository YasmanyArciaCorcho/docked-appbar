using System;
using System.Linq;
using System.Management;

namespace Services.ComputerManagement
{
    public class ScreenLightServices: IScreenLightService
    {
         // pulled this from https://github.com/JeroenvO/screen-brightness/blob/master/BrightnessConsoleJvO/Class1.cs
         
        public void SetBrightness(int iPercent)
        { 
            var bLevels = GetBrightnessLevels(); //array of valid level values
            if (iPercent < 0 || iPercent > bLevels[bLevels.Count() - 1]) return;
            byte level = 100;
            foreach (byte item in bLevels)
            {
                if (item < iPercent) continue;
                level = item;
                break;
            }
            SetBrightness(level);
            //check_brightness();
        }

        
        public int GetBrightness()
        {
            //define scope (namespace)
            try
            {
                var s = new System.Management.ManagementScope("root\\WMI");
                //define query
                var q = new System.Management.SelectQuery("WmiMonitorBrightness");
                //output current brightness
                var mos = new System.Management.ManagementObjectSearcher(s, q);
                var moc = mos.Get();
                //store result
                byte curBrightness = 0;
                foreach (var managementBaseObject in moc)
                {
                    var o = (ManagementObject) managementBaseObject;
                    curBrightness = (byte) o.GetPropertyValue("CurrentBrightness");
                    break; //only work on the first object
                }

                moc.Dispose();
                mos.Dispose();
                return (int) curBrightness;
            }
            catch
            {
                return 0;
            }
        }
        private static byte[] GetBrightnessLevels()
        {
            //define scope (namespace)
            System.Management.ManagementScope s = new System.Management.ManagementScope("root\\WMI");

            //define query
            System.Management.SelectQuery q = new System.Management.SelectQuery("WmiMonitorBrightness");

            //output current brightness
            using ManagementObjectSearcher mos = new ManagementObjectSearcher(s, q);
            byte[] BrightnessLevels = new byte[0];

            try
            {
                System.Management.ManagementObjectCollection moc = mos.Get();
                //store result
                foreach (System.Management.ManagementObject o in moc)
                {
                    BrightnessLevels = (byte[])o.GetPropertyValue("Level");
                    break; //only work on the first object
                }
                moc.Dispose();
                mos.Dispose();

            }
            catch (Exception)
            {
                //Console.WriteLine("Sorry, Your System does not support this brightness control...");
            }

            return BrightnessLevels;
        }
        private static void SetBrightness(byte targetBrightness)
        {
            var s = new System.Management.ManagementScope("root\\WMI");
            var q = new System.Management.SelectQuery("WmiMonitorBrightnessMethods");
            var mos = new System.Management.ManagementObjectSearcher(s, q);
            var moc = mos.Get();
            foreach (System.Management.ManagementObject o in moc)
            {
                o.InvokeMethod("WmiSetBrightness", new object[] { uint.MaxValue, targetBrightness }); //note the reversed order - won't work otherwise!
                break; //only work on the first object
            }

            moc.Dispose();
            mos.Dispose();
        }
    }
}