using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Settings.Chromium.AWSElastic
{
    // These values can be retrieved in the same respose.
    // We are using only: AwConversionId and AwAccountNumber.
    //"adprovider": "appfocus1",
    //"dfn": "Maps Launcher by WebNavigator",
    //"domain": "maplauncher.co",
    //"implementation_id": "wbn-maps",
    //"re_url": null,
    //"source": "s-g-cp2-yahootest-cg7-lp0-obgc-wbn-bb8-ab10-w64-brwsr",
    //"stub": 1,
    //"useragent": "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.89 Safari/537.36",
    //"uc": "20200831",
    //"user_id": "8ec07ee2-01d8-47c0-a736-9103c4472493",
    //"Vertical": "Maps",
    //"AwConversionId": "3WmVCJv2gaIBEPKZ_94C",
    //"AwAccountNumber": "AW-736087282"
    public class AWSElasticCacheModelAPI
    {
        public string AwConversionId { get; set; }

        public string AwAccountNumber { get; set; }
    }
}
