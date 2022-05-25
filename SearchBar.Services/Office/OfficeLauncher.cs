using System;
using System.Collections.Generic;
using System.Linq;
using NetOffice.OfficeApi.Enums;
using NetOffice.OutlookApi.Enums;
using NetOffice.PowerPointApi.Enums;
using Word = NetOffice.WordApi;
using PowerPoint = NetOffice.PowerPointApi;
using Excel = NetOffice.ExcelApi;
using Outlook = NetOffice.OutlookApi;
using Visio = NetOffice.VisioApi;
using NetOffice.OfficeApi.Tools.Contribution;

namespace Services.Office
{
    public class OfficeLauncher : IOfficeService
    {
        public void StartOfficeApp(OfficeLaunchApp appToStart)
        {
            try
            {
                switch (appToStart)
                {
                    case OfficeLaunchApp.Visio:
                        var visioApplication = new Visio.Application();
                        visioApplication.Documents.Add("test");
                        visioApplication.Visible = true;
                        visioApplication.Dispose();
                        break;
                    case OfficeLaunchApp.Word:
                        {
                            var wordApplication = new Word.Application(true);
                            using var utils = new CommonUtils(wordApplication);
                            wordApplication.Visible = true;
                            wordApplication.Documents.Add();
                            wordApplication.Dispose();
                        }
                        break;
                    case OfficeLaunchApp.Powerpoint:
                        var powerPoint = new PowerPoint.Application();
                        var presentation = powerPoint.Presentations.Add(MsoTriState.msoTrue);
                        presentation.Slides.Add(1, PpSlideLayout.ppLayoutBlank);
                        powerPoint.Dispose();
                        break;
                    case OfficeLaunchApp.Excel:
                        var excelApplication = new Excel.Application();
                        excelApplication.Workbooks.Add();
                        excelApplication.Visible = true;
                        excelApplication.Dispose();
                        break;
                    case OfficeLaunchApp.Outlook:
                        var outlookApplication = new Outlook.Application();
                        var outlookNS = outlookApplication.GetNamespace("MAPI");
                        var inboxFolder = outlookNS.GetDefaultFolder(OlDefaultFolders.olFolderInbox);
                        inboxFolder.Display();
                        outlookApplication.Dispose();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(appToStart), appToStart, null);
                }
            }
            catch(Exception e)
            {
                throw new NetOffice.NetOfficeException(e);
            }
        }
    }
}