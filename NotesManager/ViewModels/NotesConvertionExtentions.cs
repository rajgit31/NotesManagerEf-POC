
using NotesManagerTransferEntities;

namespace NotesManager.ViewModels
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