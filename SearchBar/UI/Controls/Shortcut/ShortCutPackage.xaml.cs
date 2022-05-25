using SearchBar.UI.Controls.Shortcut;
using SearchBar.UI.Handles.Shortcut;
using Services.ShortcutPackage;
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

namespace SearchBar.UI.Controls.ShortCut
{
    /// <summary>
    /// Interaction logic for ShortCutPackage.xaml
    /// </summary>
    public partial class ShortCutPackage : UserControl
    {
        public List<IShortcutPackage> Items;
        public Dictionary<string, List<ShortcutControl>> ShortcutControlLoaded;
        public Func<string, string, Task<ShortcutControl>> BuildShortcut;

        public ShortCutPackage()
        {
            InitializeComponent();

            Items = new List<IShortcutPackage>();
            ShortcutControlLoaded = new Dictionary<string, List<ShortcutControl>>();
        }

        //private async Task UpdateShortcutsPackagePanel(IShortcutPackage package)
        //{
        //    if (!ShortcutControlLoaded.ContainsKey(package.Name))
        //    {
        //        await Task.Yield();

        //        ShortcutControlLoaded.Add(package.Name, new List<ShortcutControl>());
        //        foreach (var shortcut in package.Shortcuts)
        //        {
        //            ShortcutControlLoaded[package.Name].Add(await BuildShortcut(shortcut.Name, shortcut.Url));
        //        }
        //    }

        //    for (int i = 0; i < ShortcutControlLoaded[package.Name].Count; i++)
        //    {
        //        shortcutsPackagePanel.Children.Add(ShortcutControlLoaded[package.Name][i]);
        //    }
        //}
    }
}
