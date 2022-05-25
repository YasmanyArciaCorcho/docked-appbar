using Common.Enums;
using Common.String;
using Microsoft.Win32;
using System.Collections.Generic;

namespace SearchBar.UI.Base
{
    public class FileDialogBox
    {
        public string OpenFileDialogBox(string initialDirectory, List<FileExtension> fileExtensions)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = initialDirectory,
                Filter = GetFileDialogFilter(fileExtensions)
            };

            openFileDialog.ShowDialog();

            if (openFileDialog.FileName is object)
                return openFileDialog.FileName;
            return string.Empty;
        }

        string GetFileDialogFilter(List<FileExtension> fileExtensions)
        {
            string filter = string.Empty;

            static string DefineExtensionFilter(FileExtension fileExtension)
            {
                // file (*.ext)|*.exe
                return $"{fileExtension} {StringConstants.Files} {StringConstants.LeftParenthesis}{StringConstants.Star}{StringConstants.Dot}" +
                    $"{fileExtension}{StringConstants.RightParenthesis}{StringConstants.VerticalBar}{StringConstants.Star}{StringConstants.Dot}{fileExtension}";
            }

            if (fileExtensions.Count > 0)
            {
                filter += DefineExtensionFilter(fileExtensions[0]);
            }

            for (int i = 1; i < fileExtensions.Count; i++)
            {
                filter += $"{StringConstants.VerticalBar}{DefineExtensionFilter(fileExtensions[i])}";
            }

            //"All files (*.*)|*.*"
            filter += $"{StringConstants.VerticalBar}{StringConstants.AllFiles} {StringConstants.LeftParenthesis}{StringConstants.Star}{StringConstants.Dot}" +
                $"{StringConstants.Star}{StringConstants.RightParenthesis}{StringConstants.VerticalBar}{StringConstants.Star}{StringConstants.Dot}{StringConstants.Star}";

            return filter;
        }
    }
}
