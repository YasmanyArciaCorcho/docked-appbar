namespace Common
{
    public interface INewsFormat
    {
        string Title
        { get; set; }

        string ImagePath
        { get; set; }

        string Body
        { get; set; }

        string Url
        { get; set; }

        string Publisher
        { get; set; }
    }
}
