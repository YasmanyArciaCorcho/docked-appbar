using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ShortcutPackage
{
    public class ShortcutPackageModelAPI
    {
#pragma warning disable IDE1006 // Naming Styles

        public string ver { get; set; }
        public bool showWidgetsText { get; set; }
        public bool autosuggest { get; set; }
        public bool search_redirect { get; set; }
        public bool logo_on_left { get; set; }
        public int df_start1 { get; set; }
        public int df_end1 { get; set; }
        public int df_start2 { get; set; }
        public int df_end2 { get; set; }
        public string icon { get; set; }
        public string sprite { get; set; }
        public string widgets { get; set; }

#pragma warning restore IDE1006 // Naming Styles
    }
}
