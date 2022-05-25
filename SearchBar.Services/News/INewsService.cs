using Common;
using System.Collections.Generic;

namespace Services.News
{
   public interface INewsService
    {
        List<INewsFormat> GetNews(int desiredQuantity);
    }
}
