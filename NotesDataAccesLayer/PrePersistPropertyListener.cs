using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using NotesDomain;
using DataEntityState = System.Data.Entity.EntityState;

namespace NotesDataAccesLayer
{
    internal class PrePersistPropertyListener
    {
        private readonly IDbContext _dbContext;

        public PrePersistPropertyListener(IDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentException("dbContext");
            }

            _dbContext = dbContext;
        }

        public void SetCommonProperties()
        {
            var entities = ((DbContext)_dbContext).ChangeTracker.Entries();

            if (entities != null)
            {
                foreach (var dbEntityEntry in entities)
                {
                    SetCommonProperties(dbEntityEntry);
                }
            }
            //test
            //test
            //some code
            ///cwecwevwvwev

        }

        private static void SetCommonProperties(DbEntityEntry dbEntityEntry)
        {
            if (dbEntityEntry.State == DataEntityState.Unchanged)
            {
                return;
            }

            if (dbEntityEntry.State == DataEntityState.Detached)
            {
                throw new InvalidDataException("The entity has maked as Detached from the context. " +
                                               "Unabled perform the operations. Entity name: ");
            }

            //test
            var entity = (BaseEntity)dbEntityEntry.Entity;
            entity.DateCreated = DateTime.Now;
            entity.DateModified = DateTime.Now;
            entity.UserId = new Guid("{E9149837-A487-4F87-A7F4-9C4BF9FF5B8E}"); //TODO: Add the current user id    
            entity.IsActive = true;
        }
    }


}