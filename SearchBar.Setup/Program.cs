using Common.Logger;
using Common.Settings.Chromium;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace Setup
{
    class Program
    {
        static void Main(string[] args)
        {
            CmdParameters parameters = new CmdParameters(args);
            string appName = ProducSettings.ProducName;
            StaticLogger.Logger.Info($"{appName} setup - started");
            if (!string.IsNullOrEmpty(parameters.Action))
            {
                string action = parameters.Action.ToLower();
                StaticLogger.Logger.Info($"{appName} setup - read input args");

                if (action.Equals("install"))
                {
                    new InstallerHelper().InstallProduct(parameters, appName);
                    StaticLogger.Logger.Info($"{appName} setup - finished {action} process");
                }
                else if (action.Equals("uninstall"))
                {
                    new InstallerHelper().UninstallProduct(appName);
                    StaticLogger.Logger.Info($"{appName} uninstall - finished {action} process");
                }
                else if (action.Equals("close"))
                {
                    new InstallerHelper().CloseAppBar();
                }
            }
            else
            {
                try
                {
                    StaticLogger.Logger.Info($"{appName} setup - opening app process.");
                    Process.Start(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + appName + ".exe");
                }
                catch (Exception e)
                {
                    StaticLogger.Logger.Error(e);
                }
            }
        }
    }
}
