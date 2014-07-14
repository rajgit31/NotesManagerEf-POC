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

        public void SaveFillerForm()
        {
            var mapDto = new QuestionAnswerMappingDTO()
            {
                EntityStateDTO = EntityStateDTO.Added,
                Client = "Client",
                NoteDTO = new NoteDTO { Id = 1, Title = "TrackableNote2" }, //existing id
                AcpAnswerDTO = new AcpAnswerDTO()
                {
                    EntityStateDTO = EntityStateDTO.Added,
                    AnswerText = "NewAnswer",
                    AcpQuestionListItemDTO = new AcpQuestionListItemDTO()
                    {
                        Name = "NewAcpQuestionListItem"
                    }
                }
                
            };

            var fillerForm = new FillerFormDTO()
            {
                EntityStateDTO = EntityStateDTO.Added,
                QuestionAnswerMappings = new List<QuestionAnswerMappingDTO>
                {
                    mapDto
                },
                Name = "Filler",
            };

            _notesService.SaveFiller(fillerForm);
        }

        public void LoadNote(int i)
        {
            var note = _notesService.FindById(i);
        }

        public void EditFillerForm()
        {
            var mapDto = new QuestionAnswerMappingDTO()
            {
                Client = "ClientEdited",
                NoteDTO = new NoteDTO { Id = 1, Title = "Bar" } //existing id
            };

            var fillerForm = new FillerFormDTO()
            {
                QuestionAnswerMappings = new List<QuestionAnswerMappingDTO>
                {
                    mapDto
                },
                Name = "Filler",

            };

            _notesService.EditFiller(fillerForm);


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
            noteDTO.EntityStateDTO = EntityStateDTO.Modified;
            noteDTO.NoteVersions = new List<NoteVersionDTO>()
            {
                new NoteVersionDTO
                {
                    Version = 1, 
                    Name = "v1.0",
                    EntityStateDTO = EntityStateDTO.Modified
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
                EntityStateDTO = EntityStateDTO.Added,
                Title = "TrackableNote1",
                NoteVersions = new List<NoteVersionDTO>
                {
                    new NoteVersionDTO()
                    {
                        EntityStateDTO = EntityStateDTO.Added,
                        Name = "vT", Version = 99,
                         NoteSection = new List<NoteSectionDTO>
                        {
                            new NoteSectionDTO()
                            {
                                EntityStateDTO = EntityStateDTO.Added,
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
            noteDto.EntityStateDTO = EntityStateDTO.Modified;
            noteDto.Id = note.Id;
            //noteDto.Id = newId;

            _notesService.Update(noteDto);

        }

        public void Delete()
        {
            var noteToDeleteViewModel = _view.NoteToDelete;
            var noteDTO = noteToDeleteViewModel.ToDTO();
            noteDTO.EntityStateDTO = EntityStateDTO.Deleted;
            _notesService.Delete(noteDTO);
            RaiseNoteDeleted(noteToDeleteViewModel);
        }
    }
}
