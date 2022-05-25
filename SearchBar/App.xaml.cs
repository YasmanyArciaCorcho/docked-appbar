using Autofac;
using SearchBar.UI.Handles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using Common.String;
using Common.Logger;
using Stores.Providers.FaviconStore;
using Common.ChromiumSettings;
using Common.Impressions;
using SearchBar.DC;

namespace SearchBar
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// 
    /// White background test: White code - #FFFFFF. Gray chome button color 666A6D
    /// Gray background test: appbackground=#E7EAED appforeground=#666A6D 
    /// </summary>
    public partial class App : Application, ISingleInstanceApp
    {
        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            if (args.Count > 1)
            {
                if (args[1].Equals(StringConstants.CloseArgument))
                {
                    try
                    {
                        MainWindow.Close();
                    }
                    catch 
                    {
                    }
                }
            }
            return true;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
       {
            if (SingleInstance<App>.InitializeAsFirstInstance(StringConstants.UniqueSearchBarId))
            {
                try
                {
                    var scope = AppContainer.GetCurrentScope();

                    CmdStringRead cmdParameters = new CmdStringRead(e.Args);

                    IChromiumSettingsService settingsService = scope.Resolve<IChromiumSettingsService>();
                    StaticImpressions.SettingsService = settingsService;
                    ImageHandler.FaviconStore = scope.Resolve<FaviconSqlLiteStore>();

                    var app = scope.Resolve<MainWindow>();

                    app.Show();

                    //if (settingsService.FirstRun && !settingsService.GetImplementationId().Equals(""))
                    //{
                    //    app.ShowDefaultDashboard();
                    //}

                    #region Test AB search bar colors

                    //if (cmdParameters.HasKey(StringConstants.AppBackground))
                    //{
                    //    app.SetDefaultBackground((Brush)(new BrushConverter().ConvertFrom(cmdParameters.GetValue(StringConstants.AppBackground))));
                    //}
                    //if (cmdParameters.HasKey(StringConstants.AppForeground))
                    //{
                    //    app.SetDefaultButtonColor((Brush)(new BrushConverter().ConvertFrom(cmdParameters.GetValue(StringConstants.AppForeground))));
                    //}

                    #endregion

                    StaticImpressions.SendImpression("sbr-started");
                }
                catch (Exception ex)
                {
                    StaticLogger.Logger.Error(ex);
                    throw ex;
                }
            }
            else
            {
                Process.GetCurrentProcess().Kill();
            }
        }
    }
}
