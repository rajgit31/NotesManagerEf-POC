using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NotesDomain;
using NotesManager.Presenters;
using NotesManager.ViewModels;
using NotesServiceLayer;
using UnityAutoMoq;

namespace NotesManager.UnitTests
{
    [TestClass]
    public class NotesPresenterUnitTests
    {
        private UnityAutoMoqContainer _autoMoqContainer;
        private NotesManagerPresenter _sut;

        public NotesPresenterUnitTests()
        {
            _autoMoqContainer = new UnityAutoMoqContainer();
        }

        [TestMethod]
        public void Save_WhenThereIsNodeToSave_VerifySaveMethodHasbeenCalledOnce()
        {

            //With standard Moq.Mock you do this..
            //var notesManagerViewStub = new Mock<INotesManagerView>();
            //var notesManagerServiceMock = new Mock<INotesManagerService>();
            //notesManagerViewStub.SetupGet(x => x.NoteToAdd).Returns(new NoteViewModel());
            //_sut = new NotesManagerPresenter(notesManagerViewStub.Object, notesManagerServiceMock.Object);

            //With UnityMoq you don't have to worry about addtional stub. It just works for you!
            var notesManagerServiceMock = _autoMoqContainer.GetMock<INotesManagerService>();
            _sut = _autoMoqContainer.Resolve<NotesManagerPresenter>();

            _sut.Save();

            notesManagerServiceMock.Verify(x => x.Save(It.IsAny<Note>()), Times.Once());
        }
    }
}
