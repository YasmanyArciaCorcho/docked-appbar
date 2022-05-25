using Common.Models;
using System;
using System.Collections.Generic;
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

namespace SearchBar.UI.Controls.Notes
{
    /// <summary>
    /// Interaction logic for NoteControl.xaml
    /// </summary>
    public partial class ReadOnlyNoteControl : UserControl
    {
        readonly Note _note;



        public Action<Note> MouseLeftButtonClick;
        public Func<Note, bool> RemoveButtonFunction;

        public ReadOnlyNoteControl(Note note)
        {
            InitializeComponent();
            MouseEnter += NoteControl_MouseEnter;
            MouseLeave += NoteControl_MouseLeave;

            _note = note;
            DataContext = _note;

            RemoveButton.Click += (object sender, RoutedEventArgs e) => { RemoveButtonFunction(_note); };
            MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { MouseLeftButtonClick(_note); };
            NoteTextBlock.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { MouseLeftButtonClick(_note); };

            UpdateNoteText(note.Text, note.UpdateDate);
        }

        public void UpdateNoteText(string text, string date)
        {
            NoteTextBlock.Text = text;
            UpdateDateTextBlock.Text = UpdateNoteDate(date);
        }

        private string UpdateNoteDate(string date)
        {
            if (!string.IsNullOrEmpty(_note?.UpdateDate))
            {
                DateTime dateTime = Convert.ToDateTime(date);
                return dateTime.ToShortDateString();
            }
            return string.Empty;
        }

        private void NoteControl_MouseLeave(object sender, MouseEventArgs e)
        {
            RemoveButton.Visibility = Visibility.Collapsed;
            UpdateDateTextBlock.Visibility = Visibility.Visible;
        }

        private void NoteControl_MouseEnter(object sender, MouseEventArgs e)
        {
            RemoveButton.Visibility = Visibility.Visible;
            UpdateDateTextBlock.Visibility = Visibility.Collapsed;
        }
    }
}
