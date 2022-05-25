using Common.Logger;
using SearchBar.UI.Builders.Image;
using SearchBar.UI.Controls;
using SearchBar.UI.Controls.Base;
using SearchBar.UI.Controls.Shortcut;
using SearchBar.UI.Controls.ShortCut;
using SearchBar.UI.Handles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SearchBar.UI.Base.Shortcut
{
    static class ShortcutBuilder
    {
        public async static Task<ShortcutControl> BuildShortcutAsync(string shortcutName, string urlIcon, bool isFolder, ICommand leftClickCommand, string leftclickCommandParameter)
        => await BuildShortcutAsync(shortcutName, urlIcon, isFolder, leftClickCommand, leftclickCommandParameter, (object o, MouseButtonEventArgs e) => { });

        public static ShortcutControl BuildShortcut(string shortcutName, string imagePath)
        {
            return BuildShortcut(shortcutName, imagePath, (Image image, string imagePath) => { image.Source = ImageHandler.GetBitmapFromLocalSource(imagePath); });
        }

        public static ShortcutControl BuildShortcut(string shortcutName, string imagePath, Action<Image, string> setImageBuilderFunc)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                ShortcutControl shortcutControl = new ShortcutControl();
                if (shortcutControl.Content is CustomButton button)
                {
                    button.ToolTip = shortcutName;
                    setImageBuilderFunc(shortcutControl.imageIcon, imagePath);
                }
                return shortcutControl;
            }
            return null;
        }

        public static ShortcutControl BuildShortcutWithName(string shortcutName, string imagePath, IImageSourceBuilder imageSourceBuilder)
        {
            ShortcutControl result = BuildShortcut(shortcutName, imagePath, imageSourceBuilder.SetImageSource);

            SetShortcutName(result, shortcutName);

            return result;
        }

        public static ShortcutControl BuildShortcutWithName(string shortcutName, string imagePath)
        {
            ShortcutControl result = BuildShortcut(shortcutName, imagePath);

            SetShortcutName(result, shortcutName);

            return result;
        }

        public static ShortcutControl BuildShortcut(string shortcutName, string uriIcon, bool isFolder,
            ICommand leftClickCommand, string leftclickCommandParameter,
            Action<object, MouseButtonEventArgs> rightButtonDownAction)
        {
            ShortcutControl shortcutControl = GetNewShortcut(shortcutName, leftClickCommand, leftclickCommandParameter, rightButtonDownAction);

            UpdateShortcutControlImage(shortcutControl, uriIcon, isFolder);

            if (isFolder)
                SetShortcutName(shortcutControl, shortcutName);

            return shortcutControl;
        }

        public static ShortcutControl BuildShortcut(string shortcutName, string urlIcon, bool isFolder,
            Action<object, MouseButtonEventArgs> leftButtonDownAction,
            Action<object, MouseButtonEventArgs> rightButtonDownAction)
        {
            ShortcutControl shortcutControl = GetNewShortcut(shortcutName, leftButtonDownAction, rightButtonDownAction);

            UpdateShortcutControlImage(shortcutControl, urlIcon, isFolder);

            if (isFolder)
                SetShortcutName(shortcutControl, shortcutName);

            return shortcutControl;
        }

        public static ShortcutControl BuildShortcut(string shortcutName, Image image, ICommand leftClickCommand, string leftclickCommandParameter, Action<object, MouseButtonEventArgs> rightButtonDownAction)
        {
            ShortcutControl shortcutControl = GetNewShortcut(shortcutName, leftClickCommand, leftclickCommandParameter, rightButtonDownAction);

            shortcutControl.imageIcon = image;

            return shortcutControl;
        }

        public static ShortcutControl BuildShortcut(string shortcutName, Image image, Action<object, MouseButtonEventArgs> leftButtonDownAction, Action<object, MouseButtonEventArgs> rightButtonDownAction)
        {
            ShortcutControl shortcutControl = GetNewShortcut(shortcutName, leftButtonDownAction, rightButtonDownAction);

            shortcutControl.imageIcon = image;

            return shortcutControl;
        }

        public static async Task<ShortcutControl> BuildShortcutAsync(string shortcutName, string uriIcon, bool isFolder, ICommand leftClickCommand, string leftclickCommandParameter, Action<object, MouseButtonEventArgs> rightButtonDownAction)
        {
            ShortcutControl shortcutControl = GetNewShortcut(shortcutName, leftClickCommand, leftclickCommandParameter, rightButtonDownAction);

            await UpdateShortcutControlImageAsync(shortcutControl, uriIcon, isFolder);

            if (isFolder)
                SetShortcutName(shortcutControl, shortcutName);

            return shortcutControl;
        }

        public static async Task<ShortcutControl> BuildShortcutAsync(string shortcutName, string urlIcon, bool isFolder, Action<object, MouseButtonEventArgs> leftButtonDownAction, Action<object, MouseButtonEventArgs> rightButtonDownAction)
        {
            ShortcutControl shortcutControl = GetNewShortcut(shortcutName, leftButtonDownAction, rightButtonDownAction);

            await UpdateShortcutControlImageAsync(shortcutControl, urlIcon, isFolder);

            if (isFolder)
                SetShortcutName(shortcutControl, shortcutName);

            return shortcutControl;
        }

        public static ShortcutControl GetNewShortcut(string shortcutName, Action<object, MouseButtonEventArgs> leftButtonDownAction, Action<object, MouseButtonEventArgs> rightButtonDownAction)
        {
            ShortcutControl shortcutControl = new ShortcutControl();
            if (shortcutControl.Content is CustomButton button)
            {
                button.ToolTip = shortcutName;

                if (leftButtonDownAction != null)
                    button.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { leftButtonDownAction(sender, e); };

                if (rightButtonDownAction != null)
                    button.MouseRightButtonDown += (object sender, MouseButtonEventArgs e) => { rightButtonDownAction(sender, e); };
            }
            return shortcutControl;
        }

        public static ShortcutControl GetNewShortcut(string shortcutName, ICommand leftClickCommand, string leftclickCommandParameter, Action<object, MouseButtonEventArgs> rightButtonDownAction)
        {
            ShortcutControl shortcutControl = new ShortcutControl();
            if (shortcutControl.Content is CustomButton button)
            {
                button.ToolTip = shortcutName;

                if (leftClickCommand != null)
                {
                    button.Command = leftClickCommand;
                    button.CommandParameter = leftclickCommandParameter;
                }

                if (rightButtonDownAction != null)
                    button.MouseRightButtonDown += (object sender, MouseButtonEventArgs e) => { rightButtonDownAction(sender, e); };
            }
            return shortcutControl;
        }

        public static async Task UpdateShortcutControlImageAsync(ShortcutControl shortcutControl, string url, bool isFolder)
        {
            if (isFolder)
            {
                shortcutControl.imageIcon.Source = ImageHandler.GetDefaultFolderImage();
            }
            else
            {
                if (!string.IsNullOrEmpty(url))
                {
                    try
                    {
                        shortcutControl.imageIcon.Source = await ImageHandler.GetBitmapFromWebAsync(url);
                    }
                    catch (Exception e)
                    {
                        StaticLogger.Logger.Error(e);
                    }
                }
                if (shortcutControl.imageIcon.Source == null)
                    SetDefaultIcon(shortcutControl);
            }
        }

        public static void UpdateShortcutControlImage(ShortcutControl shortcutControl, string imagePath, bool isFolder)
        {
            if (isFolder)
            {
                shortcutControl.imageIcon.Source = ImageHandler.GetDefaultFolderImage();
            }
            else
            {
                if (!string.IsNullOrEmpty(imagePath))
                {
                    try
                    {
                        shortcutControl.imageIcon.Source = ImageHandler.GetBitmapFromWeb(imagePath);
                    }
                    catch (Exception e)
                    {
                        StaticLogger.Logger.Error(e);
                    }
                }
                if (shortcutControl.imageIcon.Source == null)
                    SetDefaultIcon(shortcutControl);
            }
        }

        public static void SetShortcutName(ShortcutControl shortcutControl, string shortcutName)
        {
            shortcutControl.CustomContent.Margin = new System.Windows.Thickness(12, 5, 12, 5);
            shortcutControl.ShortcutName.Margin = new System.Windows.Thickness(5, 7, 0, 3);
            shortcutControl.ShortcutName.Text = shortcutName;
            shortcutControl.ShortcutName.FontWeight = FontWeights.Bold;
        }

        private static void SetDefaultIcon(ShortcutControl shortcutControl)
        {
            shortcutControl.imageIcon.Source = ImageHandler.GetDefaultShortcutImage();
            shortcutControl.imageIcon.Width = 25;
            shortcutControl.imageIcon.Height = 25;
        }
    }
}
