using Microsoft.Win32;
using SearchBar.UI.Handles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SearchBar.UI.Controls.Base
{
    /// <summary>
    /// Interaction logic for WebButtonLink.xaml
    /// </summary>
    public partial class WebButtonLink : UserControl
    {
        public static readonly DependencyProperty MainTextProperty = DependencyProperty.Register("MainText", typeof(string), typeof(WebButtonLink), new PropertyMetadata(""));
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(WebButtonLink), new PropertyMetadata(""));
        static double _minSizeToShowDescription = 1440;

        public string MainText
        {
            get
            {
                return (string)GetValue(MainTextProperty);
            }
            set
            {
                this.SetValue(MainTextProperty, value);
            }
        }

        public string Description
        {
            get
            {
                return (string)GetValue(DescriptionProperty);
            }
            set
            {
                this.SetValue(DescriptionProperty, value);
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == MainTextProperty)
            {
                MainTextBlock.Text = (string)GetValue(e.Property);
            }
            else if (e.Property == DescriptionProperty)
            {
                DescriptionBlock.Text = (string)GetValue(e.Property);
            }
        }

        public WebButtonLink()
        {
            InitializeComponent();
            UpdateDescriptionVisibility();
            SystemEvents.DisplaySettingsChanged += (object sender, EventArgs e) => { UpdateDescriptionVisibility(); };
        }

        private void UpdateDescriptionVisibility()
        {
            if (AppBarHandler.ScreenWidth() <= _minSizeToShowDescription)
            {
                DescriptionBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                DescriptionBlock.Visibility = Visibility.Visible;
            }
        }
    }
}
