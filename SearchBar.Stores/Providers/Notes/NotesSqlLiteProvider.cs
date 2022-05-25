using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.IComparer;
using Common.Logger;
using Common.Models;
using Dapper;

namespace Stores.Providers.Notes
{
    public class NotesSqlLiteProvider : INotesProvider
    {
        static string _connectionString = "Data Source={0};Version=3;";
        private readonly object _notesDbLock = new object();

        public NotesSqlLiteProvider()
        {
            string noteDbpath = DirectoryInfoHelper.GetNotesFilePath();
            _connectionString = string.Format(_connectionString, noteDbpath);

            if (!File.Exists(noteDbpath))
                CreateNotesDatabase(noteDbpath);
        }

        public bool SaveNote(Note note)
        {
            try
            {
                lock (_notesDbLock)
                {
                    using IDbConnection cnn = new SQLiteConnection(_connectionString);

                    string sqlStatement = "";
                    note.UpdateDate = DateTime.Now.Date.ToString();

                    if (note.Id == 0)
                    {
                        sqlStatement = $"insert into notes(Text, UpdateDate) values('{note.Text}', '{note.UpdateDate}')";
                        cnn.Execute(sqlStatement);
                        sqlStatement = "SELECT Id FROM notes WHERE Id = (SELECT MAX(Id)  FROM notes);";
                        note.Id = cnn.Query<int>(sqlStatement, new DynamicParameters()).Single();
                    }
                    else
                    {
                        sqlStatement = @"
                                        UPDATE notes 
                                        SET  Text = @Text
                                        ,UpdateDate = @UpdateDate
                                        WHERE Id = @Id";
                        cnn.Execute(sqlStatement, note);
                    }
                }
                return true;
            }
            catch (Exception ex) 
            {
                StaticLogger.Logger.Error(ex);
            }
            return false;
        }

        public IEnumerable<Note> GetNotes()
        {
            List<Note> notes = new List<Note>();

            try
            {
                lock (_notesDbLock)
                {
                    using IDbConnection cnn = new SQLiteConnection(_connectionString);

                    string sqlStatement = "SELECT * FROM notes;";
                    notes = cnn.Query<Note>(sqlStatement, new DynamicParameters()).ToList();
                }
            }
            catch (Exception ex)
            {
                StaticLogger.Logger.Error(ex);
            }

            notes.Sort(new DateNoteDescComparer());
            return notes;
        }

        public IEnumerable<Note> GetNotes(Func<Note, bool> filter)
        {
            foreach (var note in GetNotes())
            {
                if (filter(note))
                    yield return note;
            }
        }

        public bool RemoveNote(Note note)
        {
            try
            {
                lock (_notesDbLock)
                {
                    using IDbConnection cnn = new SQLiteConnection(_connectionString);

                    string sqlStatement = $"DELETE FROM notes WHERE Id == {note.Id};";
                    cnn.Execute(sqlStatement);
                    return true;
                }
            }
            catch (Exception ex) 
            {
                StaticLogger.Logger.Error(ex);
            }
            return false;
        }

        private void CreateNotesDatabase(string dbPath)
        {
            StaticLogger.Logger.Info("Favicon database does not exists, Creating a new Database.");

            SQLiteConnection.CreateFile(dbPath);

            using IDbConnection cnn = new SQLiteConnection(_connectionString);
            string query = "BEGIN TRANSACTION; " +
                            "CREATE TABLE IF NOT EXISTS Notes" +
                            "(" +
                            "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                            "Text TEXT," +
                            "UpdateDate TEXT NOT NULL" +
                            "); " +
                            "COMMIT;";

            try
            {
                cnn.Execute(query);
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
            }
        }
    }
}
