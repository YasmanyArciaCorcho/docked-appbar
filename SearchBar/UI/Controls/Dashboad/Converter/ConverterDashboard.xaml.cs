using Common;
using Common.Enums;
using SearchBar.UI.Base;
using SearchBar.UI.Builders.Image;
using SearchBar.UI.Controls.Base;
using SearchBar.UI.WebBar;
using Services.Converter.FileConverter;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace SearchBar.UI.Controls.Dashboad.Converter
{
    /// <summary>
    /// Interaction logic for ConverterDashboard.xaml
    /// </summary>
    public partial class ConverterDashboard : UserControl
    {
        public static string DashboardName = "Converter";
        public static string ImagePath = "converter_logo";

        FileExtension _primaryFileExtension;
        InformationWindow infoWindow;

        public ConverterDashboard(WebBarViewModel webBarViewModel, IImageSourceBuilder imageSourceBuilder)
        {
            InitializeComponent();
            InitializeImages(imageSourceBuilder);

            fileConverterNow.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://pdfconverterguru.com/?source=wbx"); };
            fileConverterNowImage.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://pdfconverterguru.com/?source=wbx"); };
            FileCoverter.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://allfilesconverter.com/convert-to-pdf/"); };
            SmallPdf.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://smallpdf.com/pdf-converter"); };
            CloudConvert.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://cloudconvert.com/"); };
            Mp3.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://cloudconvert.com/mp3-converter"); };
            Flac.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://cloudconvert.com/flac-converter"); };
            FreeRip.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.freerip.com/?utm_source=romb&utm_medium=tile&utm_campaign=hconvertfilestools.com"); };
            Mp4.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://cloudconvert.com/mp4-converter"); };
            Avi.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://cloudconvert.com/avi-converter"); };
            FLV.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.flv.com/flvconverter.html?utm_source=romb&utm_medium=tile&utm_campaign=hconvertfilestools.com"); };

            SetPrimaryFileExtension(FileExtension.doc);

            WordOption.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { SetPrimaryFileExtension(FileExtension.doc); };
            PDFOpion.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { SetPrimaryFileExtension(FileExtension.pdf); };
        }

        private void SetPrimaryFileExtension(FileExtension fileExtension)
        {
            _primaryFileExtension = fileExtension;

            var blueBlush = new SolidColorBrush(Color.FromRgb(33, 150, 243));

            if (_primaryFileExtension == FileExtension.doc)
            {
                WordOptionText.Foreground = blueBlush;
                PDFOptionText.Foreground = Brushes.White;
            }
            else
            {
                WordOptionText.Foreground = Brushes.White;
                PDFOptionText.Foreground = blueBlush;
            }
        }

        private void selectAndConvert_Click(object sender, RoutedEventArgs e)
        {
            Thread sta = new Thread(delegate ()
            {
                IFileConverter converter = null;

                List<FileExtension> fileExtension;

                switch (_primaryFileExtension)
                {
                    case FileExtension.doc:
                        fileExtension = new List<FileExtension>() { FileExtension.doc, FileExtension.docx };
                        converter = new WordToPdfFileConverter();
                        break;
                    default:
                        fileExtension = new List<FileExtension>() { FileExtension.pdf };
                        var converterApi = new FileConverterAPI
                        {
                            OriginalFileExtension = FileExtension.pdf,
                            OutPutFileExtension = FileExtension.pptx
                        };
                        converter = converterApi;
                        break;
                }

                string userDocumentPath = DirectoryInfoHelper.GetMyDocumetsUserPath();

                string filePath = new FileDialogBox().OpenFileDialogBox(userDocumentPath, fileExtension);

                if (!filePath.Equals(string.Empty))
                {
                    string mainSubject;
                    string description;
                    if (converter.TryConvert(filePath, userDocumentPath, out string outputFilePath))
                    {
                        mainSubject = "Your file was successfully converted.";
                        description = $"You can find your file in: '{outputFilePath}'";
                    }
                    else
                    {
                        mainSubject = "Your file could not be converted.";
                        description = $"Please, try converting again in a few minutes.";
                    }

                    infoWindow = new InformationWindow(mainSubject, description);

                    infoWindow.Show();

                    Dispatcher.Run();
                }
            });
            sta.SetApartmentState(ApartmentState.STA);
            sta.IsBackground = true;
            sta.Start();
        }

        private void InitializeImages(IImageSourceBuilder imageSourceBuilder)
        {
            string imageNamespace = "converter_{0}";
            imageSourceBuilder.SetImageSource(logo, string.Format(imageNamespace, "logo"));
            imageSourceBuilder.SetImageSource(filesConverter, string.Format(imageNamespace, "filesConverter"));
            imageSourceBuilder.SetImageSource(SmallPdf.Image, string.Format(imageNamespace, "smallpdf"));
            imageSourceBuilder.SetImageSource(CloudConvert.Image, string.Format(imageNamespace, "cloudconvert"));
            imageSourceBuilder.SetImageSource(Mp3.Image, string.Format(imageNamespace, "mp3"));
            imageSourceBuilder.SetImageSource(Flac.Image, string.Format(imageNamespace, "flac"));
            imageSourceBuilder.SetImageSource(FreeRip.Image, string.Format(imageNamespace, "freerip"));
            imageSourceBuilder.SetImageSource(Mp4.Image, string.Format(imageNamespace, "mp4"));
            imageSourceBuilder.SetImageSource(Avi.Image, string.Format(imageNamespace, "avi"));
            imageSourceBuilder.SetImageSource(FLV.Image, string.Format(imageNamespace, "flv"));
        }
    }
}
