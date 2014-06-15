﻿using System;
using System.Collections.Generic;
using System.Linq;
using NotesManager.ViewModels;
using NotesManagerTransferEntities;
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
            var noteDto = noteToSaveViewModel.ToDTO(); //Or use an automapper to do this so less coding!
            noteDto.EntityState = EntityStateDTO.Added;
            noteDto.NoteVersions = new List<NoteVersionDTO>()
            {
                new NoteVersionDTO
                {
                    Version = 1, 
                    Name = "v1.0",
                    EntityState = EntityStateDTO.Added,
                    NoteSection = new List<NoteSectionDTO>
                    {
                        new NoteSectionDTO()
                        {
                            SectionColor = "Blue",
                            SectionName = "Ship",
                            EntityState = EntityStateDTO.Added
                        }
                    }
                }
            };

            _notesService.Save(noteDto);
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
            var noteDTO = noteToEditViewModel.ToDTO(); //Or use an automapper to do this so less coding!
            noteDTO.EntityState = EntityStateDTO.Modified;
            noteDTO.NoteVersions = new List<NoteVersionDTO>()
            {
                new NoteVersionDTO
                {
                    Version = 1, 
                    Name = "v1.0",
                    EntityState = EntityStateDTO.Modified
                }
            };

            _notesService.Update(noteDTO);
            RaiseNoteUpdated(noteToEditViewModel);
        }

        public void Delete()
        {
            var noteToDeleteViewModel = _view.NoteToDelete;
            var noteDTO = noteToDeleteViewModel.ToDTO();
            noteDTO.EntityState = EntityStateDTO.Deleted;
            _notesService.Delete(noteDTO);
            RaiseNoteDeleted(noteToDeleteViewModel);
        }
    }
}
