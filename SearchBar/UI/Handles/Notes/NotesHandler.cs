using Common.DataStructures.KMP;
using Common.Logger;
using Common.Models;
using SearchBar.UI.Controls.Dashboad.Notes;
using SearchBar.UI.Controls.Notes;
using Stores.Providers.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SearchBar.UI.Handles.Notes
{
    public class NotesHandler : INotesHandler<NotesDashboard>
    {
        public NotesDashboard Dashboard
        { get; set; }
        readonly INotesProvider _notesProvider;
        readonly SortedList<int, int> _editingNotes = new SortedList<int, int>();

        public NotesHandler(INotesProvider notesProvider)
        {
            _notesProvider = notesProvider;
            _editingNotes = new SortedList<int, int>();
        }

        public void CreateNewNote()
        {
            Note newNote = new Note
            {
                UpdateDate = DateTime.Now.Date.ToString()
            };
            AddEditNoteControl(newNote, (string s, string d) => { });
        }

        public bool SaveNote(Note note)
        {
            bool isNewNote = note.Id == 0;

            if (_notesProvider.SaveNote(note))
            {
                if (isNewNote)
                    AddReadOnlyNoteController(CreateReadOnlyNoteController(note));
                return true;

            }

            return false;
        }

        public bool RemoveNote(Note note)
        {
            try
            {
                return _notesProvider.RemoveNote(note);
            }
            catch (Exception ex)
            {
                StaticLogger.Logger.Error(ex);
            }

            return false;
        }

        public void Initialize()
        {
            Task.Run(() =>
           {
               Thread thre = new Thread(new ThreadStart(() =>
               {
                   System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.DataBind,
                       new NotesArgDelegate(UpdateNoteZone));
                   Dispatcher.Run();
               }));
               thre.SetApartmentState(ApartmentState.STA);
               thre.IsBackground = true;
               thre.Start();
           });
        }

        private delegate void NotesArgDelegate();

        private bool AddEditNoteControl(Note note, Action<string, string> updateNoteControl)
        {
            if (note.Id == 0 || !_editingNotes.ContainsKey(note.Id))
            {
                EditNoteControl editNoteControl = new EditNoteControl(note)
                {
                    Margin = new Thickness(5, 5, 0, 0)
                };
                Dashboard.OpensNotesPanel.Children.Add(editNoteControl);

                if (note.Id != 0)
                    _editingNotes.Add(note.Id, Dashboard.OpensNotesPanel.Children.Count - 1);
                editNoteControl.RemoveButtonFunction = (Note note) =>
                {
                    Dashboard.OpensNotesPanel.Children.Remove(editNoteControl);
                    if (note.Id != 0)
                        _editingNotes.Remove(note.Id);
                };

                editNoteControl.SaveFunction = (Note note, string text) =>
                {
                    note.Text = text;
                    if (SaveNote(note))
                    {
                        updateNoteControl(text, note.UpdateDate);

                        Dashboard.OpensNotesPanel.Children.Remove(editNoteControl);
                        if (note.Id != 0)
                            _editingNotes.Remove(note.Id);
                    }
                };
            }

            return false;
        }

        private void AddReadOnlyNoteController(ReadOnlyNoteControl readOnlyNoteControl)
        {
            Dashboard.NoteListZonePanel.Children.Add(readOnlyNoteControl);
            readOnlyNoteControl.RemoveButtonFunction = (Note note) =>
            {
                if (RemoveNote(note))
                {
                    Dashboard.NoteListZonePanel.Children.Remove(readOnlyNoteControl);
                    return true;
                }
                return false;
            };

        }

        private ReadOnlyNoteControl CreateReadOnlyNoteController(Note note)
        {
            ReadOnlyNoteControl readOnlyNoteController = new ReadOnlyNoteControl(note)
            {
                Margin = new Thickness(5)
            };

            readOnlyNoteController.MouseLeftButtonClick = (Note note) =>
            {
                AddEditNoteControl(note, readOnlyNoteController.UpdateNoteText);
            };

            return readOnlyNoteController;
        }

        public void UpdateNoteZone(Func<Note, bool> filter, string textPattern)
        {
            ClearNoteDashboard();

            Thread thre = new Thread(new ThreadStart(() =>
           {
               var notes = _notesProvider.GetNotes();

               System.Windows.Application.Current.Dispatcher.Invoke(() =>
               {
                   foreach (var note in notes)
                   {
                       if (filter(note))
                       {
                           if (!string.IsNullOrEmpty(textPattern) && !string.IsNullOrWhiteSpace(textPattern))
                           {
                               KMPSearch kmp = new KMPSearch();
                               if (!kmp.Contains(textPattern.ToLower(), note.Text.ToLower()))
                                   continue;
                           }

                           AddReadOnlyNoteController(CreateReadOnlyNoteController(note));
                       }
                   }
               },
                   DispatcherPriority.Background);
               Dispatcher.Run();
           }));
            thre.SetApartmentState(ApartmentState.MTA);
            thre.IsBackground = true;
            thre.Start();
        }

        public void UpdateNoteZone()
        {
            UpdateNoteZone((Note) => true, string.Empty);
        }

        private void ClearNoteDashboard()
        {
            Dashboard.NoteListZonePanel.Children.Clear();
            Dashboard.OpensNotesPanel.Children.Clear();
            _editingNotes.Clear();
        }
    }
}
