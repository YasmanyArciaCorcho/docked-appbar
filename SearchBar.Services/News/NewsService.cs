using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using Common;
using Common.Logger;
using Common.ExtensionMethods;

namespace Services.News
{
    public class NewsService : INewsService
    {
        const string _newsApiUrl = "http://newsapi-prod.us-east-1.elasticbeanstalk.com/api/news?c=top-stories";
        readonly Queue<INewsFormat> _newsRead;

        public NewsService()
        {
            _newsRead = new Queue<INewsFormat>();
        }

        public List<INewsFormat> GetNews(int desiredQuantity)
        {
            List<INewsFormat> result = new List<INewsFormat>(desiredQuantity);

            if (desiredQuantity > _newsRead.Count)
            {
                _newsRead.Clear();

                try
                {
                    string queryResult = HTTPRequestHelper.DoQuery(_newsApiUrl);

                    NewsModelAPI newNews = JsonConvert.DeserializeObject<NewsModelAPI>(queryResult);

                    foreach (var news in newNews.FeedItems)
                    {
                        string newsTitle = news.t.ShortString(25) + " ...";
                        string newsBody = news.B.ShortString(50) + " ...";

                        _newsRead.Enqueue(new News()
                        {
                            Title = newsTitle,
                            Url = news.cu,
                            ImagePath = news.im,
                            Body = newsBody,
                            Publisher = news.fn
                        });
                    }

                    StaticLogger.Logger.Info("News service - get news.");
                }
                catch (Exception e)
                {
                    StaticLogger.Logger.Error(e);
                }
            }

            while (desiredQuantity > 0 && _newsRead.Count > 0)
            {
                result.Add(_newsRead.Dequeue());
                desiredQuantity--;
            }

            return result;
        }
    }
}
