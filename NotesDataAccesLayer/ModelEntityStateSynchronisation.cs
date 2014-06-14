using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using NotesDomain;
using DataEntityState = System.Data.Entity.EntityState;
using ModelEntityState = NotesDomain.EntityState;

namespace NotesDataAccesLayer
{
    public static class ModelEntityStateSynchronisation
    {
        public static void SyncEntityStatePreCommit<TEntity>(this IDbContext dbContext) where TEntity : IEntity
        {
            foreach (var dbEntityEntry in ((DbContext)dbContext).ChangeTracker.Entries())
            {
                dbEntityEntry.State = StateHelper.ConvertState(((IEntity)dbEntityEntry.Entity).EntityState);
            }
        }

        public static void SyncEntityStatePostCommit<TEntity>(this IDbContext dbContext) where TEntity : IEntity
        {
            foreach (var dbEntityEntry in ((DbContext)dbContext).ChangeTracker.Entries())
            {
                ((IEntity)dbEntityEntry.Entity).EntityState = StateHelper.ConvertState(dbEntityEntry.State);
            }
        }
    }
}