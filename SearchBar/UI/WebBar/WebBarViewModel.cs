using SearchBar.UI.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Services.Browser;
using SearchBar.UI.SearchBarViewModel;
using SearchBar.UI.Handles.News;
using Services.News;
using System.Windows.Threading;
using Services.Weather;
using SearchBar.UI.Handles;
using Services.Bookmark;
using SearchBar.UI.Controls;
using SearchBar.UI.Controls.Dashboad;
using Common.Settings.Chromium;
using Common;
using Common.ChromiumSettings;
using Common.Impressions;

namespace SearchBar.UI.WebBar
{
    public class WebBarViewModel : ViewModelBase
    {
        public IBrowserService BrowserService;

        public ICommand OpenBrowser { get; private set; }

        public readonly IChromiumSettingsService Settings;

        public string BlurImagePath
        {
            get; set;
        }

        public WebBarViewModel(
            IBrowserService browserService,
            ISearchBarViewModel searchViewModel,
            IChromiumSettingsService settings) : base()
        {
            BrowserService = browserService;
            SearchBarViewModel = searchViewModel;
            Settings = settings;

            SearchBarViewModel.SearchSelectedItemEvent += new ISearchBarViewModel.DoSelectItem(OpenSearchBrowser);
            SearchBarViewModel.OpenDirectlySelectedItemEvent += new ISearchBarViewModel.DoSelectItem(OpenDirectUrlBrowser);

            BlurImagePath = ImageHandler.GetWallPaperImagePath();
            ImageHandler.WallPaperChangedEvent += WallPaperChanged;
        }

        public ISearchBarViewModel SearchBarViewModel
        { get; set; }

        protected override void Initialize()
        {
            this.OpenBrowser = new RelayCommand(new Action<object>(WebBar_OpenDirectUrlBrowser), (object o) => true);
        }

        public void OpenSearchBrowser(string url)
        {
            BrowserService.Open(url);
        }

        public void OpenDirectUrlBrowser(string url)
        {
            OpenDirectUrlBrowser(url, "");
        }

        public void OpenDirectUrlBrowser(string url, string iEvent = "")
        {
            BrowserService.Open(BrowserService.DefaultBrowser, url);

            if (!string.IsNullOrEmpty(iEvent))
            {
                StaticImpressions.SendImpression($"sbr-open-{iEvent}-url");
            }
            else
            {
                StaticImpressions.SendImpression($"sbr-open-url");
            }
        }

        public void WebBar_OpenDirectUrlBrowser(object parameter)
        {
            if (parameter is string shortuCutName)
                OpenDirectUrlBrowser(shortuCutName);
        }

        private void WallPaperChanged(string windowsWallPaperPath)
        {
            BlurImagePath = windowsWallPaperPath;
        }
    }
}
