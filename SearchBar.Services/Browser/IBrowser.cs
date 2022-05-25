using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Browser
{
    public interface IBrowser
    {
        string Name { get; }

        string Path { get; }

        byte[] LargeIcon { get; }

        byte[] SmallIcon { get; }
    }
}
