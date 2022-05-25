using Common;
using Common.Logger;
using Microsoft.Win32;
using SearchBar.UI.Base;
using SearchBar.UI.Controls.Base;
using SearchBar.UI.Controls.Dashboad;
using Services.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SearchBar.UI.Handles.News
{
    public class NewsHandler : INewsHandler<MainDashBoard>
    {
        static double _defaultWidht = 185;
        static Thickness _newsMargin = new Thickness(15, 0, 0, 0);

        public INewsService NewsService { get; set; }

        public MainDashBoard Dashboard { get; set; }

        public double NewsZoneWidth { get; private set; } = 0;

        readonly Stack<NewsFormatControl> _newsSet;

        public NewsHandler(INewsService newsService)
        {
            NewsService = newsService;
            _newsSet = new Stack<NewsFormatControl>();
        }

        public void AddNewsZone(int amount)
        {
            Thread thre = new Thread(new ThreadStart(() =>
            {
                var newsList = NewsService.GetNews(amount);
                Dictionary<INewsFormat, byte[]> images = new Dictionary<INewsFormat, byte[]>();
                foreach (var site in newsList)
                    images.Add(site, HTTPRequestHelper.GetFileFromURL(site.ImagePath));

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    if (newsList.Count > 0)
                    {
                        foreach (var currentNews in newsList)
                        {
                            NewsFormatControl currentNewsControl = new NewsFormatControl
                            {
                                Width = _defaultWidht
                            };

                            if (_newsSet.Count > 0)
                                currentNewsControl.Margin = _newsMargin;

                            currentNewsControl.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                            currentNewsControl.DataContext = currentNews;

                            currentNewsControl.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) =>
                            {
                                Dashboard.WebBarViewModel.OpenDirectUrlBrowser(currentNews.Url, "news");
                            };

                            try
                            {
                                currentNewsControl.Image.Source = ImageHandler.BytesToImage(images[currentNews]);
                            }
                            catch (Exception e)
                            {
                                StaticLogger.Logger.Error(e);
                            }

                            Dashboard.NewsZoneStackPanel.Children.Add(currentNewsControl);

                            _newsSet.Push(currentNewsControl);
                        }
                    }

                    StaticLogger.Logger.Info("NewsHandle - added news zone.");
                },
                    DispatcherPriority.Background);
                Dispatcher.Run();
            }));
            thre.SetApartmentState(ApartmentState.MTA);
            thre.IsBackground = true;
            thre.Start();
        }

        public void UpdateNewsZone()
        {
            List<NewsFormatControl> auxNewsControlStack = _newsSet.ToList();

            var currentsNews = NewsService.GetNews(_newsSet.Count);
            for (int i = 0; i < currentsNews.Count; i++)
                auxNewsControlStack[i].DataContext = currentsNews[i];

            auxNewsControlStack.Reverse();

            _newsSet.Clear();

            foreach (var news in auxNewsControlStack)
                _newsSet.Push(news);

            StaticLogger.Logger.Info("NewsHandle - updared news set.");
        }

        public void Initialize()
        {
            double screenWidth = AppBarHandler.ScreenWidth();
            double _availableWidth = screenWidth - Dashboard.LeftGridColumn.Margin.Left - Dashboard.LeftGridColumn.Width - Dashboard.WindowsGrid.Width - Dashboard.WindowsGrid.Margin.Right;
            double newsSize = _defaultWidht + _newsMargin.Left;

            Dashboard.BrowserGrid.Width = _availableWidth;
            int maxNumberofNews = 0;

            while (NewsZoneWidth + newsSize < _availableWidth)
            {
                maxNumberofNews++;
                NewsZoneWidth += _defaultWidht + _newsMargin.Left;
            }
            AddNewsZone(maxNumberofNews);

            StaticLogger.Logger.Info("NewsHandle - loaded initials news.");
        }
    }
}
