using SearchBar.UI.Base;
using SearchBar.UI.Controls.ShortCut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SearchBar.UI.Handles.Shortcut.PinShortcut
{
    public interface IPinShortcutHandler
    {
        void PintoBar(string shortcutName, string url,bool isFolder, Action<object, RoutedEventArgs> leftClick);

        void PintoBar(string shortcutName, string url,bool isFolder, ICommand leftClickCommand, string leftclickCommandParameter);

        void PinApplicationtoBar(string widgetName);

        void UnPintoBar(string shortcutUIElement);

        void UnPinApplicationtoBar(string widgetName);

        void ShowUnPinMenu(string shortcutName, Action<string> unPinAction);
    }

    public interface IPinShortcutHandler<T> : IPinShortcutHandler
    {
        T ShortcutContainer { get; set; }

        void SetShortcutContainer(T container);
    }

    public interface IMWPinShortcutHandle<K> : IPinShortcutHandler<MainWindow>
    {
    }
}
