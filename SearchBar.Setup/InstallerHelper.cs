using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.Win32;
using Common.Logger;
using Common.Settings.Chromium;
using Common;
using Common.String;
using Common.ChromiumSettings;
using Common.Settings.UserIp;
using Common.Settings.Chromium.AWSElastic;
using Common.Impressions;
using IWshRuntimeLibrary;

namespace Setup
{
    public class InstallerHelper
    {

        public string GetDestinationPath(string[] args)
        {
            return GetDelimitedString(args, '*', 1);
        }

        public string GetChromiumExeName(string[] args)
        {
            return GetDelimitedString(args, '*', 3);
        }

        public string GetPreInstallUrl(string[] args)
        {
            return GetDelimitedString(args, '*', 5);
        }

        public bool IsProcessRunning(string procName)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Equals(procName))
                    return true;
            }

            return false;
        }

        public IChromiumSettingsService InitializeSettings(CmdParameters parameters, string destinationPath)
        {
            // Creating default browser settings
            string fatherDir = Directory.GetParent(destinationPath).FullName;
            AppSettings setting = new AppSettings()
            {
                BrowserSettings = new BrowserSettings()
                {
                    DefaultPath = destinationPath + "\\" + parameters.ChromiumExePath
                },
                PreInstallUirl = parameters.PreInstallUrl,
                ChromiumProductName = parameters.ChromiumExePath.Substring(0, parameters.ChromiumExePath.Length - 4),
                FirstRun = true,
                TrackingId = parameters.TrakingId
            };

            try
            {
                try
                {
                    using var reader = new StreamReader(fatherDir + "\\User Data\\Last Version");
                    setting.ProductVersion = reader.ReadToEnd();
                }
                catch (Exception e)
                {
                    StaticLogger.Logger.Error(e);
                };

                StaticLogger.Logger.Info($"{ProducSettings.ProducName} setup - default browser path: {setting.BrowserSettings.DefaultPath}");
                string serializedAppSettings = JsonConvert.SerializeObject(setting);
                System.IO.File.WriteAllText($"{destinationPath}\\appsettings.json", serializedAppSettings);
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
            }

            return new ChromiumSettingsService(new AWSElasticChromiumUrlFixed(new IcanhazipResolver()), new AWSElasticAppSettingsInitializer());
        }

        public bool InstallProduct(CmdParameters parameters, string appName)
        {
            try
            {
                string currentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string destinationAppPath = parameters.InstallationPath;
                StaticLogger.Logger.Info($"{ProducSettings.ProducName} setup - install started");
                StaticLogger.Logger.Info($"{ProducSettings.ProducName} setup - destination path: {destinationAppPath}");

                if (!Directory.Exists(destinationAppPath))
                    Directory.CreateDirectory(destinationAppPath);

                StaticImpressions.SettingsService = InitializeSettings(parameters, destinationAppPath);
                StaticImpressions.SendImpression("sbr_install_started");

                if (Directory.Exists(currentPath))
                {
                    // DirectoryInfoHelper.CopyDirectory(currentPath, destinationAppPath);
                    DecompressFiles(currentPath, destinationAppPath);

                    CopySetupFile(currentPath, destinationAppPath);

                    string appExecutable = $"{destinationAppPath}\\{appName}.exe";

                    StaticImpressions.SendImpression("sbr_install_finished");

                    Process.Start(appExecutable);
                }
                return true;
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
                return false;
            }
        }

        public bool UninstallProduct(string appName)
        {
            try
            {
                StaticLogger.Logger.Info($"{ProducSettings.ProducName} setup - uninstall started");
                string appExeName = appName + ".exe";
                StaticLogger.Logger.Info($"{ProducSettings.ProducName} setup - read appsettings");
                string workDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                using (var reader = new StreamReader(workDir + "/appsettings.json"))
                {
                    var appSettings = JsonConvert.DeserializeObject<AppSettings>(reader.ReadToEnd());
                    StaticImpressions.SettingsService = new ChromiumSettingsService(new AWSElasticChromiumUrlFixed(new IcanhazipResolver()), new AWSElasticAppSettingsInitializer());
                }

                CloseAppBar(appName, appExeName);

                StaticLogger.Logger.Info($"{ProducSettings.ProducName} setup - removed search bar windows startup registry");
                StaticImpressions.SendImpression("sbr_uninstall_finished");
                return true;
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
                return false;
            }
        }

        public void CloseAppBar()
        {
            string appName = ProducSettings.ProducName;
            string appExeName = appName + ".exe";

            CloseAppBar(appName, appExeName);
        }

        public void CloseAppBar(string appName, string appExeName)
        {
            if (IsProcessRunning(appName))
            {
                StaticLogger.Logger.Info(Directory.GetCurrentDirectory() + "\\" + appExeName);
                ProcessStartInfo startInfo = new ProcessStartInfo(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + appExeName)
                {
                    Arguments = StringConstants.CloseArgument
                };
                Process.Start(startInfo);
                StaticLogger.Logger.Info($"{ProducSettings.ProducName} setup - closed search bar process");
            }
        }

        public static void CopySetupFile(string appName, string currentPath, string destinationAppPath)
        {
            StaticLogger.Logger.Info($"{ProducSettings.ProducName} setup - started to copy search bar setup file");
            string appSetupName = $"{ProducSettings.InstallerName}.exe";
            string setupFilePath = currentPath + "\\" + appSetupName;
            System.IO.File.Copy(setupFilePath, destinationAppPath + "\\" + appSetupName, true);
            StaticLogger.Logger.Info($"{ProducSettings.ProducName} setup - finished to copy search bar files");

        }

        private static string GetDelimitedString(string[] args, char delimiter, int position)
        {
            string result = "";
            int delimiterCount = 0;

            int i;
            for (i = 0; i < args.Length; i++)
            {
                if (args[i].Contains(delimiter))
                {
                    delimiterCount++;
                    if (delimiterCount == position)
                        break; ;
                }
            }

            i++;
            for (; i < args.Length; i++)
            {
                result += args[i];
                if (args[i + 1].Contains(delimiter))
                {
                    return result;
                }
                else
                {
                    result += " ";
                }
            }

            return result;
        }

        public bool DecompressFiles(string currentDirectory, string destinationPath)
        {
            StaticLogger.Logger.Info($"{ProducSettings.ProducName} setup - started to descrompress files.");

            try
            {
                string sevenZipPath = $"{currentDirectory}\\7za.exe";
                if (System.IO.File.Exists(sevenZipPath))
                {
                    Process cmd = new Process();
                    cmd.StartInfo.FileName = sevenZipPath;
                    cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    cmd.StartInfo.Arguments = $"x -o{destinationPath} -aoa \"{currentDirectory}\\sbr.7z\"";
                    cmd.Start();
                    cmd.WaitForExit();
                }

                StaticLogger.Logger.Info($"{ProducSettings.ProducName} setup - finished to descrompress files.");
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Info($"{ProducSettings.ProducName} setup - falar error unzipping files: {e}");
            }

            return true;
        }

        public static void CopySetupFile(string currentPath, string destinationAppPath)
        {
            StaticLogger.Logger.Info($"{ProducSettings.ProducName} setup - started to copy search bar setup file");
            string appSetupName = $"{ProducSettings.InstallerName}.exe";
            string setupFilePath = currentPath + "\\" + appSetupName;
            System.IO.File.Copy(setupFilePath, destinationAppPath + "\\" + appSetupName, true);
            StaticLogger.Logger.Info($"{ProducSettings.ProducName} setup - finished to copy search bar files");
        }
    }
}
