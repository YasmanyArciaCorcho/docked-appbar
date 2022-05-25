using Common.Models;
using SearchBar.UI.Base;
using System;
using System.Collections.Generic;

namespace SearchBar.UI.Handles.Notes
{
    public interface INotesHandler : IControlHandler
    {
        void CreateNewNote();

        bool SaveNote(Note note);

        bool RemoveNote(Note note);

    }

    public interface INotesHandler<T> : INotesHandler, IDashboardHandler<T> where T : IDashboard
    {
        void UpdateNoteZone(Func<Note, bool> filter, string textPattern);

        void UpdateNoteZone();
    }
}
