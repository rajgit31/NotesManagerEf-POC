using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotesDomain;
using NotesDomain.Entities;

namespace NotesManager.ViewModels
{
    public class NoteViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    //Can replace by automapper
    public static class NotesConvertionExtentions
    {
        public static Note ToDomain(this NoteViewModel noteViewModel)
        {
            return new Note
            {
                Id = noteViewModel.Id,
                Title = noteViewModel.Title,
                Description = noteViewModel.Description
            };
        }
    }
}
