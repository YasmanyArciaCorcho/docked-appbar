using Common;

namespace Services.News
{
    public class News : INewsFormat
    {
        public string Title 
        { get; set; }
       
        public string ImagePath 
        { get; set; }
      
        public string Body 
        { get; set; }
      
        public string Url 
        { get; set; }

        public string Publisher 
        { get; set; }
    }
}
