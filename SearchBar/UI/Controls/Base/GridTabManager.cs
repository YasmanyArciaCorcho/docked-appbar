using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SearchBar.UI.Controls.Base
{
    public class GridTabManager
    {
        readonly Grid _gridCursor;

        Panel _selectedTab;

        public GridTabManager()
        {
            _gridCursor = new Grid()
            {
                Background = new SolidColorBrush(Color.FromRgb(33, 150, 243)),
                Margin = new Thickness(0, 10, 0, 0),
                Height = 3
            };
        }

        public void SelectDefaultCategory(Grid rootPanel, UserControl catetogy, Panel newSelectedPanel)
        {
            _selectedTab = newSelectedPanel;
            _selectedTab.Children.Add(_gridCursor);
            UpdatePanelCategory(rootPanel, catetogy, newSelectedPanel);
        }

        public void UpdatePanelCategory(Grid rootPanel, UserControl catetogy, Panel newSelectedPanel)
        {
            _selectedTab.Children.Remove(_gridCursor);

            _selectedTab = newSelectedPanel;
            _selectedTab.Children.Add(_gridCursor);

            if (rootPanel.Children != null && rootPanel.Children.Count > 1)
                rootPanel.Children.RemoveAt(1);

            catetogy.Margin = new Thickness(0, 15, 0, 0);

            Grid.SetRow(catetogy, 2);
            rootPanel.Children.Add(catetogy);
        }
    }
}
