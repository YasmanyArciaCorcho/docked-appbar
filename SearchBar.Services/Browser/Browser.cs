using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Browser
{
    public class Browser : IBrowser
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public byte[] LargeIcon { get; set; }

        public byte[] SmallIcon { get; set; }

        public Browser(string name, string path)
        {
            Name = name;
            Path = path;
        }

        public Browser(string name, string path, byte[] largeIcon, byte[] smallIcon) : this(name, path)
        {
            LargeIcon = largeIcon;
            SmallIcon = smallIcon;
        }
    }
}
