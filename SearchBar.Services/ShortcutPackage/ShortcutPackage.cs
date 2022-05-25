using Common.Shortcut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ShortcutPackage
{
    public class ShortcutPackage : IShortcutPackage
    {
        public string Name { get; set; }
       
        public string PackageIconUrl { get; set;}

        public List<IShortcut> Shortcuts 
        { get; set; }

        public ShortcutPackage()
        {
            Shortcuts = new List<IShortcut>();
        }
    }
}
