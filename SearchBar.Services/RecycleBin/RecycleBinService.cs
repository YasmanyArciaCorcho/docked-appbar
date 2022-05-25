using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.RecycleBin
{
    public class RecycleBinService : IRecycleBinService
    {
        public void OpenRecycleBin()
        {
            System.Diagnostics.Process.Start("explorer.exe", "shell:RecycleBinFolder");
        }
    }
}
