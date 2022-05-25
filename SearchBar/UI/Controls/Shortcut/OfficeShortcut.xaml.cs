using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using Services.Office;
using SearchBar.UI.Controls.Base;
using Common.Settings.Chromium;

namespace SearchBar.UI.Controls.Shortcut
{
    /// <summary>
    /// Interaction logic for OfficeShortcut.xaml
    /// </summary>
    public partial class OfficeShortcut : UserControl
    {
        public bool IsOfficeInstalled
        { get; private set; } = false;

        private PackIcon GetOfficeIcon(OfficeLaunchApp app)
        {
            var stockIcon = new PackIcon()
            {
                Kind = PackIconKind.MicrosoftOffice,
                Foreground = Brushes.White,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                Height = 20,
                Width = 20,
            };
            switch (app)
            {
                case OfficeLaunchApp.Word:
                    stockIcon.Kind = PackIconKind.MicrosoftWord;
                    break;
                case OfficeLaunchApp.Powerpoint:
                    stockIcon.Kind = PackIconKind.MicrosoftExcel;
                    break;
                case OfficeLaunchApp.Excel:
                    stockIcon.Kind = PackIconKind.MicrosoftPowerpoint;
                    break;
                case OfficeLaunchApp.Outlook:
                    stockIcon.Kind = PackIconKind.MicrosoftOutlook;
                    break;
                case OfficeLaunchApp.Visio:
                    stockIcon.Kind = PackIconKind.Drawing;
                    break;
            }
            return stockIcon;
        }

        public OfficeShortcut()
        {
            InitializeComponent();

            var officeService = new OfficeLauncher();

            foreach (var shortcutDefinition in Enum.GetValues(typeof(OfficeLaunchApp)).Cast<OfficeLaunchApp>())
            {
                var officeIcon = GetOfficeIcon(shortcutDefinition);
                var officeContainer = new StackPanel()
                {
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center,
                    Orientation = Orientation.Horizontal,
                    Height = 40,
                };
                var officeName = new Label()
                {
                    HorizontalContentAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center,
                    Content = shortcutDefinition.ToString()
                };

                officeContainer.Children.Add(officeIcon);
                officeContainer.Children.Add(officeName);

                var newSortCut = new CustomButton()
                {
                    Padding = new Thickness(15, 0, 0, 0),
                    SnapsToDevicePixels = true,
                    Content = officeContainer,
                    ToolTip = $"Open {shortcutDefinition}",
                    HorizontalAlignment = HorizontalAlignment.Right
                    
                };
                newSortCut.PreviewMouseLeftButtonDown += (sender, e) =>
                {
                    Task.Run(() =>
                    {
                        try
                        {
                            officeService.StartOfficeApp(shortcutDefinition);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show($"Sorry, some errors were found trying to open {shortcutDefinition}. Consider installing office on your computer.",
                                $"{ProducSettings.ProducName} - Office application not found",
                                MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                        }
                    });
                };

                shortcutZone.Children.Add(newSortCut);
            }
        }
    }
}
