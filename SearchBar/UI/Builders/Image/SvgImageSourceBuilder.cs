using Common.Logger;
using SearchBar.UI.Handles;
using Stores.Providers.Image;
using Svg2Xaml;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace SearchBar.UI.Builders.Image
{
    public class SvgImageSourceBuilder : ImageSourceBuilder
    {
        public SvgImageSourceBuilder(IImageProvider imageProvider)
            : base(imageProvider)
        {

        }

        public override void SetImageSource(System.Windows.Controls.Image image, string imageNamespace)
        {
            Thread thre = new Thread(new ThreadStart(() =>
            {
                string imagePath = _imageProvider.GetImagePath(imageNamespace);

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    ImageSource source = null;
                    try
                    {
                        using (FileStream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                        {
                            source = SvgReader.Load(stream);
                        }
                    }
                    catch (Exception e)
                    {
                        StaticLogger.Logger.Error($"Fatal error loading image resource: {e}");
                        source = ImageHandler.GetDefaultShortcutImage();
                    }
                    image.Source = source;
                }, DispatcherPriority.Background);
                Dispatcher.Run();
            }));
            thre.SetApartmentState(ApartmentState.MTA);
            thre.IsBackground = true;
            thre.Start();
        }
    }
}
