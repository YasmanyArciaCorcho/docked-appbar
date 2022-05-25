using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Browser
{
    public interface IBrowserService
    {
        /// <summary>
        /// Browser selected by default to make queries.
        /// </summary>
        IBrowser DefaultBrowser { get; set; }

        /// <summary>
        /// Open the url in the default browser.
        /// </summary>
        /// <param name="url"></param>
        void Open(string url);

        /// <summary>
        /// Open the url in the selected browser.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="url"></param>
        void Open(IBrowser browser, string url);
    }
}
