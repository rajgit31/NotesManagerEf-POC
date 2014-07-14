using System.Data.Entity;
using System.Linq;
using NotesDataAccesLayer.Repositories;
using NotesDomain.Entities;
using NotesDomainInterfaces;

namespace NotesDataAccesLayer
{
    /// <summary>
    /// Class DateTime2Convention.
    /// </summary>
    public static class RepoExt
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTime2Convention"/> class.
        /// </summary>
        public static FillerForm GetFillerMap(this IRepository<FillerForm> fillerRepository, int id)
        {
            var internalSet = ((EfGenericRepository<FillerForm>)fillerRepository).InternalSet;

            var items = internalSet.Include("QuestionAnswerMappings");

            var singleOrDefault = items.SingleOrDefault(x => x.Id == id);

            return singleOrDefault;
        }
    }
}