using System;
using System.Collections.Generic;
using System.Data.Sql;
using System.Linq;
using NotesDomain;
using NotesManager.ViewModels;
using NotesServiceLayer;

namespace NotesManager.Presenters
{
    public class NotesManagerPresenter
    {
        private readonly INotesManagerView _view = null;
        private readonly INotesManagerService _notesService;
        public event EventHandler<NoteSavedEventArgs> NoteSaved;
        public event EventHandler<NoteEditedEventArgs> NoteEdited;
        public event EventHandler<NoteEditedEventArgs> NoteDeleted;

        public NotesManagerPresenter(INotesManagerView view, INotesManagerService notesService)
        {
            _notesService = notesService;
            _view = view;
        }

        public void Save()
        {
            var noteToSaveViewModel = _view.NoteToAdd;
            var noteDomain = noteToSaveViewModel.ToDomain(); //Or use an automapper to do this so less coding!
            noteDomain.EntityState = EntityState.Added;
            noteDomain.NoteVersions = new List<NoteVersion>()
            {
                new NoteVersion
                {
                    Version = 1, 
                    Name = "v1.0",
                    EntityState = EntityState.Added
                }
            };

            _notesService.Save(noteDomain);
            RaiseNoteSaved(noteToSaveViewModel);
        }

        public IList<NoteViewModel> LoadNotes()
        {
            var notes = _notesService.GetNotes();
            return notes.Select(note => new NoteViewModel()
            {
                Id = note.Id, Title = note.Title, Description = note.Description
            }).ToList();
        }


        private void RaiseNoteSaved(NoteViewModel noteViewModel)
        {
            if (NoteSaved != null)
            {
                NoteSaved(this, new NoteSavedEventArgs(noteViewModel));
            }
        }

        private void RaiseNoteUpdated(NoteViewModel noteViewModel)
        {
            if (NoteEdited != null)
            {
                NoteEdited(this, new NoteEditedEventArgs(noteViewModel));
            }
        }

        private void RaiseNoteDeleted(NoteViewModel noteToDeleteViewModel)
        {
            if (NoteDeleted != null)
            {
                NoteDeleted(this, new NoteEditedEventArgs(noteToDeleteViewModel));
            }
        }
       
        public void Edit()
        {
            var noteToEditViewModel = _view.NoteToEdit;
            var noteDomain = noteToEditViewModel.ToDomain(); //Or use an automapper to do this so less coding!
            noteDomain.EntityState = EntityState.Modified;
            noteDomain.NoteVersions = new List<NoteVersion>()
            {
                new NoteVersion
                {
                    Version = 1, 
                    Name = "v1.0",
                    EntityState = EntityState.Modified
                }
            };

            _notesService.Update(noteDomain);
            RaiseNoteUpdated(noteToEditViewModel);
        }

        public void Delete()
        {
            var noteToDeleteViewModel = _view.NoteToDelete;
            var noteDomain = noteToDeleteViewModel.ToDomain();
            noteDomain.EntityState = EntityState.Deleted;
            _notesService.Delete(noteDomain);
            RaiseNoteDeleted(noteToDeleteViewModel);
        }
    }
}
