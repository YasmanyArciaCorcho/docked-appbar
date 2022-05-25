using Common.Enums;
using Common.String;
using System.IO;

namespace Services.Converter.FileConverter
{
    public abstract class BaseFileConverter : IFileConverter
    {
        public abstract bool TryConvert(string originalFilePath, string destinationFolderPath, out string outputFilePath);

        protected string GetConvertedFileNamePath(string originalFilePath, string destinationFolderPath, FileExtension originalFileExtension, FileExtension outputFileExtension)
        {
            string convertedFileName = Path.GetFileName(originalFilePath).Replace($"{StringConstants.Dot}{originalFileExtension}", $"{StringConstants.Dot}{outputFileExtension}");
            return $"{destinationFolderPath}\\{convertedFileName}";
        }
    }
}
