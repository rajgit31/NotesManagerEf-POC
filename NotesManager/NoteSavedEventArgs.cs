using System;
using NotesManager.ViewModels;

namespace NotesManager
{
    public class NoteSavedEventArgs : EventArgs
    {
        private readonly NoteViewModel _noteViewModel;

        public NoteSavedEventArgs(NoteViewModel noteViewModel)
        {
            _noteViewModel = noteViewModel;
        }

        public NoteViewModel Note
        {
            get { return _noteViewModel; }
        }
    }

    public class NoteEditedEventArgs : EventArgs
    {
        private readonly NoteViewModel _noteViewModel;

        public NoteEditedEventArgs(NoteViewModel noteViewModel)
        {
            _noteViewModel = noteViewModel;
        }

        public NoteViewModel Note
        {
            get { return _noteViewModel; }
        }
    }

    public class NoteDeletedEventArgs : EventArgs
    {
        private readonly NoteViewModel _noteViewModel;

        public NoteDeletedEventArgs(NoteViewModel noteViewModel)
        {
            _noteViewModel = noteViewModel;
        }

        public NoteViewModel Note
        {
            get { return _noteViewModel; }
        }
    }


}