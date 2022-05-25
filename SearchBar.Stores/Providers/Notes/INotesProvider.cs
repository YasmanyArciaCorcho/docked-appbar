using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stores.Providers.Notes
{
    public interface INotesProvider
    {
        IEnumerable<Note> GetNotes();

        IEnumerable<Note> GetNotes(Func<Note, bool> filter);

        public bool SaveNote(Note note);

        bool RemoveNote(Note note);
    }
}
