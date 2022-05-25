using System.Collections.Generic;

namespace Services.Bookmark.Importer
{
    public interface IBookmarkImporter
    {
        List<Common.Models.Bookmark> ImportBookmarks();
    }
}
