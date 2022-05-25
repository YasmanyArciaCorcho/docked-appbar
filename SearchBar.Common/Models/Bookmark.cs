using Common.Shortcut;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Common.Models
{
    public class Bookmark : BaseShortcut, IShortcut
    {
        public List<Bookmark> Children
        { get; private set; }

        [JsonIgnore]
        public Bookmark Parent 
        { get; set; }

        public Bookmark(Bookmark parent, string name, string url, bool isForder)
            : base(name, url, isFolder: isForder)
        {
            Parent = parent;
            Children = new List<Bookmark>();
        }
    }
}
