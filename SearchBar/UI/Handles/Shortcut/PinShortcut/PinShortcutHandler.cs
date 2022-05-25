using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Common.Settings.Chromium;
using Common.Shortcut;
using SearchBar.UI.Base.Shortcut;
using SearchBar.UI.Builders.Image;
using SearchBar.UI.Controls.Dashboad;
using SearchBar.UI.Controls.Shortcut;
using SearchBar.UI.Controls.ShortCut;
using SearchBar.UI.Handles.Widgets;
using SearchBar.UI.WebBar;
using Common.ChromiumSettings;
using Stores.Providers.PinShortcut;
using System.Threading;
using System.Windows.Threading;

namespace SearchBar.UI.Handles.Shortcut.PinShortcut
{
    public class PinShortcutHandler : PinShortcutHandleBase<MainWindow>
    {
        private readonly Dictionary<string, ShortcutControl> _pinnedApplications;

        readonly Dictionary<string, ShortcutControl> _shortcutControls;
        readonly IImageSourceBuilder _imageSourceBuilder;

        public PinShortcutHandler(
            IPinShortcutProvider pinShortcutProvider,
            IWidgetsPresentationHandler widgetHandle,
            IChromiumSettingsService settingsService,
            IImageSourceBuilder imageSourceBuilder)
            : base(pinShortcutProvider, widgetHandle, settingsService)
        {
            _imageSourceBuilder = imageSourceBuilder;
            _shortcutControls = new Dictionary<string, ShortcutControl>();
            _pinnedApplications = new Dictionary<string, ShortcutControl>();
            widgetHandle.PinShortcutHandle = this;
        }

        public override void PintoBar(string shortcutName, string url, bool isFolder, Action<object, RoutedEventArgs> leftClick)
        {
            ShortcutControl newShortcut = ShortcutBuilder.BuildShortcut(shortcutName, url, isFolder, leftClick, null);

            PinToTaskBar(shortcutName, url, newShortcut);
        }

        public override void PintoBar(string shortcutName, string url, bool isFolder, ICommand leftClickCommand, string leftclickCommandParameter)
        {
            ShortcutControl newShortcut = ShortcutBuilder.BuildShortcut(shortcutName, url, isFolder, leftClickCommand, leftclickCommandParameter, null);

            PinToTaskBar(shortcutName, url, newShortcut);
        }

        public override void PinApplicationtoBar(string widgetName)
        {
            PinApplicationtoBar(widgetName, true);
        }

        private void PinApplicationtoBar(string widgetName, bool addRightClickAction = true)
        {
            if (!_pinnedApplications.ContainsKey(widgetName))
            {
                ShortcutControl newAppShortCut = ShortcutBuilder.BuildShortcutWithName(widgetName, _widgetsHandle.GetWidgetImageLogoPath(widgetName), _imageSourceBuilder);

                _widgetsHandle.ChangeAppPinStatus(widgetName, true);

                newAppShortCut.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) =>
                {
                    ShortcutContainer.ToggleDashboard(_widgetsHandle.GetWidgetDashboard(widgetName));
                };

                if (addRightClickAction)
                    newAppShortCut.MouseRightButtonDown += (object sender, MouseButtonEventArgs e) =>
                    {
                        if (_pinnedApplications.ContainsKey(widgetName))
                            ShowUnPinMenu(widgetName, UnPinApplicationtoBar);
                    };

                _pinnedApplications.Add(widgetName, newAppShortCut);
                ShortcutContainer.ShortcutZone.AddShortcut(newAppShortCut);
                _pinShortcutProvider.AddShortCut(widgetName, isApp: true);
            }
        }

        public override async void UnPintoBar(string shortcutName)
        {
            if (_shortcutControls.ContainsKey(shortcutName))
            {
                await Task.Yield();

                ShortcutContainer.ShortcutZone.RemoveShortcut(_shortcutControls[shortcutName]);
                _shortcutControls.Remove(shortcutName);
                _pinShortcutProvider.RemoveShortCut(shortcutName);
            }
        }

        public override void UnPinApplicationtoBar(string widgetName)
        {
            if (_pinnedApplications.ContainsKey(widgetName))
            {
                _widgetsHandle.ChangeAppPinStatus(widgetName, false);

                ShortcutContainer.ShortcutZone.RemoveShortcut(_pinnedApplications[widgetName]);
                _pinnedApplications.Remove(widgetName);
                _pinShortcutProvider.RemoveShortCut(widgetName);
            }
        }

        public override void ShowUnPinMenu(string shortcutUIElement, Action<string> unPinAction)
        {
            ContextMenu unPinMenu = new ContextMenu
            {
                FontSize = 14
            };
            MenuItem unPinItem = new MenuItem
            {
                Header = "Unpin from search bar ..."
            };
            unPinItem.Click += (object sender, RoutedEventArgs e) =>
            {
                unPinAction(shortcutUIElement);
            };
            unPinMenu.Items.Add(unPinItem);

            if (_shortcutControls.ContainsKey(shortcutUIElement))
            {
                unPinMenu.PlacementTarget = _shortcutControls[shortcutUIElement];
            }
            else if (_pinnedApplications.ContainsKey(shortcutUIElement))
            {
                unPinMenu.PlacementTarget = _pinnedApplications[shortcutUIElement];
            }

            unPinMenu.IsEnabled = true;
            unPinMenu.Placement = PlacementMode.Bottom;
            unPinMenu.IsOpen = true;
        }

        public override void RefreshContainer()
        {
            if (_widgetsHandle is IWidgetsPresentationHandler<WidgetsPresentationDashborad> appDash)
            {
                if (appDash.Dashboard == null)
                {
                    appDash.Dashboard = new WidgetsPresentationDashborad(appDash, ShortcutContainer.WebBarViewModel);
                    ShortcutContainer.ApplicationDaskboard = appDash.Dashboard;
                }
            }

            AddDefaultPinnedApplications();

            Thread thre = new Thread(new ThreadStart(() =>
            {
                var shortcuts = _pinShortcutProvider.Shortcuts;

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var shortcut in shortcuts)
                    {
                        if (!shortcut.IsApp)
                        {
                            PintoBar(shortcut.Name, shortcut.Url, shortcut.IsForder, ShortcutContainer.WebBarViewModel.OpenBrowser, shortcut.Url);
                        }
                        else
                        {
                            if (_widgetsHandle.ContainsApp(shortcut.Name))
                            {
                                PinApplicationtoBar(shortcut.Name);
                            }
                        }
                    }
                },
                 DispatcherPriority.Background);
                Dispatcher.Run();
            }));
            thre.SetApartmentState(ApartmentState.MTA);
            thre.IsBackground = true;
            thre.Start();


        }

        private void PinToTaskBar(string ShortcutControlName, string url, ShortcutControl ShortcutControl)
        {
            if (!_shortcutControls.ContainsKey(ShortcutControlName))
            {
                _shortcutControls.Add(ShortcutControlName, ShortcutControl);

                ShortcutControl.MouseRightButtonDown += (object sender, MouseButtonEventArgs e) =>
                {
                    if (_shortcutControls.ContainsKey(ShortcutControlName))
                        ShowUnPinMenu(ShortcutControlName, UnPintoBar);
                };

                ShortcutContainer.ShortcutZone.AddShortcut(ShortcutControl);
                _pinShortcutProvider.AddShortCut(ShortcutControlName, url);
            }
        }

        private void AddDefaultPinnedApplications()
        {
            string defaultApplication = _settingsService.GetImplementationId();
            if (defaultApplication != null && _widgetsHandle.ContainsApp(defaultApplication))
            {
                PinApplicationtoBar(defaultApplication, false);
                _pinnedApplications[defaultApplication].MouseRightButtonDown += (object sender, MouseButtonEventArgs e) =>
                  {
                      ShortcutContainer.ToggleDashboard(_widgetsHandle.GetWidgetDashboard(defaultApplication));
                  };
            }
            else
            {
                if (_settingsService.FirstRun)
                {
                    Thread thre = new Thread(new ThreadStart(() =>
                   {
                       var applications = _widgetsHandle.GetAllWidgetDashboard();

                       System.Windows.Application.Current.Dispatcher.Invoke(() =>
                       {
                           foreach (var application in applications)
                           {
                               PinApplicationtoBar(application, true);
                           }
                       },
                        DispatcherPriority.Background);
                       Dispatcher.Run();
                   }));
                    thre.SetApartmentState(ApartmentState.MTA);
                    thre.IsBackground = true;
                    thre.Start();
                }
            }
        }
    }
}
