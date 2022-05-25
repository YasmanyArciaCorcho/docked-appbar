using Newtonsoft.Json;
using Common.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Common.Shortcut;
using Common;

namespace Stores.Providers.PinShortcut
{
    public class PinShortcutProvider : ProviderBase, IPinShortcutProvider
    {

        public List<IShortcut> Shortcuts { get; set; }

        public PinShortcutProvider()
        {

        }

        public override async Task<bool> Load()
        {
            Shortcuts = new List<IShortcut>();

            try
            {
                string currentShortcutPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + $"\\{DirectoryInfoHelper.GetPinShortcutFilePath()}";
                if (File.Exists(currentShortcutPath))
                {
                    await Task.Yield();

                    List<BaseShortcut> IBookmarks = JsonConvert.DeserializeObject<List<BaseShortcut>>(File.ReadAllText(currentShortcutPath));
                    foreach (var Bookmark in IBookmarks)
                    {
                        Shortcuts.Add(Bookmark);
                    }
                    StaticLogger.Logger.Info($"Pin Shortcut Provider - load pinned Shortcut disk.");
                }
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
                return false;
            }

            return true;
        }

        public override Task<bool> Save()
        {
            return Task.Run(() =>
             {
                 lock (_saveLock)
                 {
                     try
                     {
                         string currentIBookmarkPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + $"\\{DirectoryInfoHelper.GetPinShortcutFilePath()}";
                         if (Shortcuts != null)
                         {
                             string serializedBookmars = JsonConvert.SerializeObject(Shortcuts);
                             File.WriteAllText(currentIBookmarkPath, serializedBookmars);
                         }
                     }
                     catch (Exception e)
                     {
                         StaticLogger.Logger.Error(e);
                         return false;
                     }

                     return true;
                 }
             });
        }

        public void AddShortCut(string name, string url, bool isApp)
        {
            bool isInShortcut = false;
            for (int i = 0; i < Shortcuts.Count; i++)
            {
                if (Shortcuts[i].Name.Equals(name))
                {
                    isInShortcut = true;
                    break;
                }
            }

            if (!isInShortcut)
            {
                Shortcuts.Add(new BaseShortcut(name, url, isApp: isApp));
            }
            Save();
        }

        public void RemoveShortCut(string name)
        {
            for (int i = 0; i < Shortcuts.Count; i++)
            {
                if (Shortcuts[i].Name.Equals(name))
                {
                    Shortcuts.RemoveAt(i);
                    break;
                }
            }
            Save();
        }
    }
}
