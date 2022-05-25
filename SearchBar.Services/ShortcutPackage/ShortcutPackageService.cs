using Common;
using Common.Logger;
using Common.Settings.Chromium;
using Common.Shortcut;
using Newtonsoft.Json;
using Common.ChromiumSettings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.ExtensionMethods;

namespace Services.ShortcutPackage
{
    public class ShortcutPackageService : IShortcutPackageService
    {
        readonly IChromiumSettingsService _settingsService;
        readonly ICommonVerticalLinksService _commonVerticalLinks;

        public ShortcutPackageService(IChromiumSettingsService settingsService, ICommonVerticalLinksService commonVerticalLinks)
        {
            _settingsService = settingsService;
            _commonVerticalLinks = commonVerticalLinks;
        }

        List<IShortcutPackage> _packages = null;
        public List<IShortcutPackage> Packages
        {
            get
            {
                if (_packages == null)
                    _packages = GetPackages();
                return _packages;
            }
        }

        List<IShortcutPackage> GetPackages()
        {
            _packages = new List<IShortcutPackage>();

            string verticalId = _settingsService.GetImplementationId().ToLower();

            if (verticalId != null && !verticalId.Equals(string.Empty))
            {
                try
                {
                    string queryResult = HTTPRequestHelper.DoQuery(_settingsService.GetShortcutPackageUrl(verticalId));
                    ShortcutPackageModelAPI packageInfo = JsonConvert.DeserializeObject<ShortcutPackageModelAPI>(queryResult);
                    ShortcutPackage shortcutPackage = new ShortcutPackage()
                    {
                        Name = verticalId.UpperCaseFirst(),
                        PackageIconUrl = packageInfo.icon,
                    };

                    var shortcutsInfo = JsonConvert.DeserializeObject<List<List<string>>>(packageInfo.widgets);

                    foreach (var shortcutInfo in shortcutsInfo)
                    {
                        if (shortcutInfo.Count > 3)
                        {
                            BaseShortcut shortcut = new BaseShortcut(shortcutInfo[1], shortcutInfo[3], shortcutInfo[1]);
                            shortcutPackage.Shortcuts.Add(shortcut);
                        }
                    }

                    Packages.Add(shortcutPackage);
                }
                catch (Exception e)
                {
                    StaticLogger.Logger.Error(e);
                    StaticLogger.Logger.Error($"possibly an icon pack was not found for {verticalId}");
                }
            }

            #region Common Links Zone

            if (_commonVerticalLinks != null)
            {
                if (_packages.Count == 0)
                    _packages.Add(new ShortcutPackage());

                _packages[0].Shortcuts.AddRange(_commonVerticalLinks.GetCommonShortcuts());
            }

            #endregion

            return _packages;
        }
    }
}
