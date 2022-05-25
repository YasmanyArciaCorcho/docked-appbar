using Common;
using Common.Enums;
using Common.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Converter.FileConverter
{
    public class FileConverterAPI : BaseFileConverter
    {
        public FileExtension OriginalFileExtension;

        public FileExtension OutPutFileExtension;
        readonly string _convertApiUrl = "http://v2.convertapi.com/convert/{0}/to/{1}?Secret=Daua4erhW5sNpQlD";

        public override bool TryConvert(string originalFilePath, string destinationFolderPath, out string outputFilePath)
        {
            outputFilePath = string.Empty;
            if (File.Exists(originalFilePath))
            {
                var header = new Dictionary<string, string>()
                {
                    { "accept", "application/octet-stream" }
                };

                byte[] fileResult = HTTPRequestHelper.UploadFile(string.Format(_convertApiUrl, OriginalFileExtension, OutPutFileExtension), originalFilePath, header);

                if (fileResult != null)
                {
                    try
                    {
                        outputFilePath = GetConvertedFileNamePath(originalFilePath, destinationFolderPath, OriginalFileExtension, OutPutFileExtension);
                        File.WriteAllBytes(outputFilePath, fileResult);

                        return true;
                    }
                    catch (Exception e)
                    {
                        StaticLogger.Logger.Trace($"FileConverterAPI - {e}.");
                    }
                }
            }

            return false;
        }
    }
}
