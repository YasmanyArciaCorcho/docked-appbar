namespace Common.Shortcut
{
    public class BaseShortcut : IShortcut
    {
        public string Name
        { get; set; }
       
        public string Url
        { get; set; }
     
        public string Caption 
        { get; set; }
      
        public bool IsApp 
        { get; set; }
    
        public bool IsForder
        {
            get; private set;
        }

        public BaseShortcut(string name, string url, string caption = "", bool isApp = false, bool isFolder = false)
        {
            Name = name;
            Url = url;
            Caption = caption;
            IsApp = isApp;
            IsForder = isFolder;
        }
    }
}
