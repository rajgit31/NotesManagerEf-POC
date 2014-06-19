using System;
using System.Collections.Generic;
using System.Linq;
using NotesManager.Convertions;
using NotesManager.ViewModels;
using NotesManagerTransferEntities;
using NotesServiceLayer;
using Omu.ValueInjecter;

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
            //var noteToSaveViewModel = _view.NoteToAdd;
            
            ////var noteDto = noteToSaveViewModel.ToDTO(); 
            //var noteDTO = new NoteDTO();
            //noteDTO.InjectFrom(noteToSaveViewModel);

            //noteDTO.EntityState = EntityStateDTO.Added;
            //noteDTO.NoteVersions = new List<NoteVersionDTO>()
            //{
            //    new NoteVersionDTO
            //    {
            //        Version = 1, 
            //        Name = "v1.0",
            //        EntityState = EntityStateDTO.Added,
            //        NoteSection = new List<NoteSectionDTO>
            //        {
            //            new NoteSectionDTO()
            //            {
            //                SectionColor = "Blue",
            //                SectionName = "Ship",
            //                EntityState = EntityStateDTO.Added
            //            }
            //        }
            //    }
            //};

            //_notesService.Save(noteDTO);
            //RaiseNoteSaved(noteToSaveViewModel);
            TrackableTransaction();
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

        public void TrackableTransaction()
        {
            //Add new note
            var noteDto = new NoteDTO()
            {
                EntityState = EntityStateDTO.Added,
                Title = "TrackableNote1",
                NoteVersions = new List<NoteVersionDTO>
                {
                    new NoteVersionDTO()
                    {
                        EntityState = EntityStateDTO.Added,
                        Name = "vT", Version = 99,
                         NoteSection = new List<NoteSectionDTO>
                        {
                            new NoteSectionDTO()
                            {
                                EntityState = EntityStateDTO.Added,
                                SectionColor = "Blue",
                                SectionName = "Ship",
                            }
                        }
                    }
                }
            };

            _notesService.Save(noteDto);

            //Get added note to get the id
            var note = _notesService.FindByTitle(noteDto.Title);


            //Set id and update the existing note
            noteDto.Title = "TrackableNote2";
            noteDto.EntityState = EntityStateDTO.Modified;
            noteDto.Id = note.Id;
            //noteDto.Id = newId;

            _notesService.Update(noteDto);

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
