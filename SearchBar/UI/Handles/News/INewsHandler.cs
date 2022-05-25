using SearchBar.UI.Base;
using Services.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SearchBar.UI.Handles.News
{
    public interface INewsHandler : IControlHandler
    {
        INewsService NewsService { get; set; }

        double NewsZoneWidth { get; }

        void UpdateNewsZone();
    }

    public interface INewsHandler<T> : INewsHandler, IDashboardHandler<T> where T : IDashboard
    {

    }
}
