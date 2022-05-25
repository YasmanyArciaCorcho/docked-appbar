using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Settings.Chromium
{
    public interface IAppSettingsInitializer
    {
        AppSettings Initialize(out bool firstRun);

        void UpdateAppSettings(AppSettings appSettings, params object[] parameters);
    }
}
