using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Converter.FileConverter
{
    public interface IFileConverter
    {
        bool TryConvert(string originalFilePath, string destinationFolderPath, out string outputFilePath);
    }
}
