
using NotesManager.ViewModels;
using NotesManagerTransferEntities;

namespace NotesManager.Convertions
{
    public static class NotesConvertionExtentions
    {
        public static NoteDTO ToDTO(this NoteViewModel noteViewModel)
        {
            return new NoteDTO
            {
                Id = noteViewModel.Id,
                Title = noteViewModel.Title,
                Description = noteViewModel.Description,
            };
        }
    }
}