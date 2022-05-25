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
    /// Interaction logic for EditNoteControl.xaml
    /// </summary>
    public partial class EditNoteControl : UserControl
    {
        readonly Note _note;
        public Action<Note> RemoveButtonFunction
        { get; set; }
        public Action<Note, string> SaveFunction;

        public EditNoteControl(Note note)
        {
            InitializeComponent();
            MouseEnter += NoteControl_MouseEnter;
            MouseLeave += NoteControl_MouseLeave;

            _note = note;
            DataContext = _note;
            TextZone.Text = _note.Text;
            RemoveButton.Click += (object sender, RoutedEventArgs e) =>
            {
                RemoveButtonFunction?.Invoke(_note);
            };

            SaveButton.Click += (object sender, RoutedEventArgs e) =>
            {
                SaveFunction?.Invoke(_note, TextZone.Text);
            };
        }

        private void NoteControl_MouseLeave(object sender, MouseEventArgs e)
        {
            GridOption.Visibility = Visibility.Collapsed;
        }

        private void NoteControl_MouseEnter(object sender, MouseEventArgs e)
        {
            GridOption.Visibility = Visibility.Visible;
        }
    }
}
