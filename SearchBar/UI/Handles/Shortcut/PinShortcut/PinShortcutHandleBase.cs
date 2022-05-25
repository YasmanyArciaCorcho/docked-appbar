using Common.Settings.Chromium;
using SearchBar.UI.Handles.Widgets;
using Common.ChromiumSettings;
using Stores.Providers.PinShortcut;
using System;
using System.Windows;
using System.Windows.Input;

namespace SearchBar.UI.Handles.Shortcut.PinShortcut
{
    public abstract class PinShortcutHandleBase<T> : IPinShortcutHandler<T>
    {
        public T ShortcutContainer { get; set; }
        protected IPinShortcutProvider _pinShortcutProvider;
        protected IWidgetsPresentationHandler _widgetsHandle;
        protected IChromiumSettingsService _settingsService;

        public PinShortcutHandleBase(
            IPinShortcutProvider pinShortcutProvider,
            IWidgetsPresentationHandler widgetsHandle,
            IChromiumSettingsService settingsService)
        {
            _pinShortcutProvider = pinShortcutProvider;
            _widgetsHandle = widgetsHandle;
            _settingsService = settingsService;
        }

        public abstract void PintoBar(string shortcutName, string url, bool isFolder, Action<object, RoutedEventArgs> leftClick);

        public abstract void PintoBar(string shortcutName, string url, bool isFolder, ICommand leftClickCommand, string leftclickCommandParameter);

        public abstract void PinApplicationtoBar(string widgetName);

        public abstract void UnPintoBar(string shortcutName);

        public abstract void UnPinApplicationtoBar(string widgetName);

        public abstract void ShowUnPinMenu(string shortcutUIElement, Action<string> unPinAction);

        public abstract void RefreshContainer();

        public void SetShortcutContainer(T container)
        {
            ShortcutContainer = container;
            RefreshContainer();
        }
    }
}
