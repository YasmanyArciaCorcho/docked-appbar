using Common.Models;
using SearchBar.UI.Base;
using SearchBar.UI.Builders.Image;
using SearchBar.UI.Handles.Notes;
using SearchBar.UI.WebBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SearchBar.UI.Controls.Dashboad.Notes
{
    /// <summary>
    /// Interaction logic for NotesDashboard.xaml
    /// </summary>
    public partial class NotesDashboard : UserControl, IDashboard
    {
        public static string DashboardName = "Notes";
        public static string ImagePath = "notes_logo";
        readonly INotesHandler<NotesDashboard> _notesHandler;

        public WebBarViewModel WebBarViewModel { get; set; }

        public NotesDashboard(WebBarViewModel webBarViewModel, INotesHandler<NotesDashboard> notesHandler, IImageSourceBuilder imageSourceBuilder)
        {
            InitializeComponent();
            WebBarViewModel = webBarViewModel;
            _notesHandler = notesHandler;
            _notesHandler.Dashboard = this;
            _notesHandler.Initialize();
            imageSourceBuilder.SetImageSource(logo, ImagePath);
        }

        private void NewNoteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _notesHandler.CreateNewNote();
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            if (SpecificDay.IsChecked == true)
            {
                _notesHandler.UpdateNoteZone((Note note) =>
                {
                    if (NotesCalendar.SelectedDate != null)
                        return Convert.ToDateTime(note.UpdateDate).Date.Equals(NotesCalendar.SelectedDate.Value);
                    return false;

                }, TextPattern.Text);
            }
            else if (BeforeDate.IsChecked == true)
            {
                _notesHandler.UpdateNoteZone((Note note) =>
                {
                    if (NotesCalendar.SelectedDate != null)
                        return Convert.ToDateTime(note.UpdateDate).Date < NotesCalendar.SelectedDate.Value;
                    return false;

                }, TextPattern.Text);
            }
            else if (AfterDate.IsChecked == true)
            {
                _notesHandler.UpdateNoteZone((Note note) =>
                {
                    if (NotesCalendar.SelectedDate != null)
                        return Convert.ToDateTime(note.UpdateDate).Date > NotesCalendar.SelectedDate.Value;
                    return false;

                }, TextPattern.Text);
            }
            else
            {
                _notesHandler.UpdateNoteZone((Note note) => true, TextPattern.Text);
            }
        }
    }
}
