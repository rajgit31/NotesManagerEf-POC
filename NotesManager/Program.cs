using System;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using NotesDataAccesLayer;
using NotesDataAccesLayer.Repositories;
using NotesDomain;
using NotesDomainInterfaces;
using NotesServiceLayer;

namespace NotesManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var container = RegsiterContainer();
            var form = container.Resolve<INotesManagerView>();
            Application.Run((FrmNotes)form);
        }

        static IUnityContainer RegsiterContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<INotesManagerView, FrmNotes>();
            container.RegisterType<INotesManagerService, NotesManagerService>();
            

            //Don't need  to have the ContainerControlledLifetimeManager. The factory create repos.
            //Also if the container get disposed before the uof, there will be an exception..
            container.RegisterType<IUnitOfWork, EfUnitOfWork>();

            container.RegisterType<IDbContext, NotesDbContext>();
            container.RegisterType(typeof(IFilterCriteria<>), typeof(QueryFilterCriteria<>));
            container.RegisterType(typeof(IRepository<>), typeof(EfGenericRepository<>));
            
            return container;
        }
    }
}
