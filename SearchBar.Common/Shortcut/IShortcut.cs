using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Shortcut
{
    public interface IShortcut
    {
        string Name
        { get; set; }

        string Url
        { get; set; }

        string Caption
        { get; set; }

        bool IsApp
        { get; set; }

        bool IsForder
        { get; }
    }
}
