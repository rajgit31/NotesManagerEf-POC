using System;
using System.Windows.Forms;
using NotesManager.Presenters;
using NotesManager.ViewModels;
using NotesServiceLayer;

namespace NotesManager
{
    public partial class FrmNotes : Form, INotesManagerView
    {
        private readonly NotesManagerPresenter _presenter;

        public FrmNotes(INotesManagerService notesService)
        {
            InitializeComponent();
            _presenter = new NotesManagerPresenter(this, notesService);
            LoadAllNotes();
            RegisterViewEvents();
        }

        private void LoadAllNotes()
        {
            var notes = _presenter.LoadNotes();
            listView1.Clear();
            foreach (var note in notes)
            {
                AddNoteToList(note);
            }
        }

        private void RegisterViewEvents()
        {
            _presenter.NoteSaved += noteViewModel_NoteSaved;
            _presenter.NoteEdited += noteViewModel_NoteEdited;
            _presenter.NoteDeleted += noteViewModel_NoteDeleted;
        }

        public NoteViewModel NoteToAdd
        {
            get
            {
                return new NoteViewModel
                {
                    Title = txtTitle.Text,
                    Description = txtDescription.Text
                }; 
            }
        }

        public NoteViewModel NoteToEdit
        {
            get
            {
                return new NoteViewModel
                {
                    Id = int.Parse(txtHiddenId.Text),
                    Title = txtTitle.Text,
                    Description = txtDescription.Text
                };
            }
        }

        public NoteViewModel NoteToDelete
        {
            get
            {
                return new NoteViewModel
                {
                    Id = int.Parse(txtHiddenId.Text),
                    Title = txtTitle.Text,
                    Description = txtDescription.Text
                };
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _presenter.Save();
        }

        private void noteViewModel_NoteSaved(object sender, NoteSavedEventArgs e)
        {
            //AddNoteToList(e.Note);
            //complete refresh from the db so IDs are consitant
            LoadAllNotes();
        }

        private void noteViewModel_NoteEdited(object sender, NoteEditedEventArgs e)
        {
            //No need load all notes as the ids already exist
            EditNoteInList(e.Note);
        }

        private void noteViewModel_NoteDeleted(object sender, NoteEditedEventArgs e)
        {
            LoadAllNotes();
        }

        public void AddNoteToList(NoteViewModel note)
        {
            var listitem = new ListViewItem(note.Title) { ToolTipText = note.Description };
            listitem.Tag = note.Id.ToString();
            listView1.Items.Add(listitem);
        }

        private void EditNoteInList(NoteViewModel note)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                if ((string) item.Tag == note.Id.ToString())
                {
                    item.Text = note.Title;
                    item.ToolTipText = note.Description;
                    txtHiddenId.Text = note.Id.ToString();
                }
            }

            listView1.Refresh();
        }

       

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                txtTitle.Text = e.Item.Text;
                txtDescription.Text = e.Item.ToolTipText;
                txtHiddenId.Text = e.Item.Tag.ToString();
            }
            
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            _presenter.Edit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            _presenter.Delete();
        }
    }
}
