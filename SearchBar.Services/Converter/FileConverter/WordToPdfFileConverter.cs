using Common.Enums;
using Common.Logger;
using Common.String;
using System;
using System.IO;
using WordToPDF;

namespace Services.Converter.FileConverter
{
    public class WordToPdfFileConverter : BaseFileConverter
    {
        public override bool TryConvert(string originalFilePath, string destinationFolderPath, out string outputFileName)
        {
            outputFileName = string.Empty;

            try
            {
                if (File.Exists(originalFilePath))
                {
                    Word2Pdf objWorPdf = new Word2Pdf();

                    string fileExtension = Path.GetExtension(originalFilePath);
                    if (fileExtension == $"{StringConstants.Dot}{FileExtension.doc}"
                    || fileExtension == $"{ StringConstants.Dot}{ FileExtension.docx}")
                    {
                        Enum.TryParse(fileExtension.Substring(1, fileExtension.Length - 1), out FileExtension inputFileExtension);

                        objWorPdf.InputLocation = originalFilePath;
                        objWorPdf.OutputLocation = GetConvertedFileNamePath(originalFilePath, destinationFolderPath, inputFileExtension, FileExtension.pdf);
                        objWorPdf.Word2PdfCOnversion();

                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Info($"Word To Pdf File Converter - {e}.");
            }
            return false;
        }
    }
}
