using MaterialDesignThemes.Wpf;
using SearchBar.UI.Controls.Base;
using SearchBar.UI.Controls.ShortCut;
using SearchBar.UI.Handles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SearchBar.UI.Controls.Shortcut
{
    /// <summary>
    /// Interaction logic for HorizontalShortcutZone.xaml
    /// </summary>
    public partial class HorizontalShortcutZone : UserControl
    {
        public HorizontalShortcutZone()
        {
            InitializeComponent();
        }

        public void AddShortcut(ShortcutControl shortcutControl)
        {
            ShortcutZone.Children.Add(shortcutControl);
        }

        public void RemoveShortcut(ShortcutControl shortcutControl)
        {
            ShortcutZone.Children.Remove(shortcutControl);
        }
    }
}
