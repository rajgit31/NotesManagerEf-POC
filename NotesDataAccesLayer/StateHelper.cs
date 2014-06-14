using System;
using System.Data.Entity;

namespace NotesDataAccesLayer
{
    public class StateHelper
    {
        public static EntityState ConvertState(NotesDomain.EntityState state)
        {
            switch (state)
            {
                case NotesDomain.EntityState.Added:
                    return EntityState.Added;
                case NotesDomain.EntityState.Modified:
                    return EntityState.Modified;
                case NotesDomain.EntityState.Deleted:
                    return EntityState.Deleted;
                default:
                    return EntityState.Unchanged;
            }
        }

        public static NotesDomain.EntityState ConvertState(EntityState state)
        {
            switch (state)
            {
                case EntityState.Added:
                    return NotesDomain.EntityState.Added;
                case EntityState.Deleted:
                    return NotesDomain.EntityState.Deleted;
                case EntityState.Modified:
                    return NotesDomain.EntityState.Modified;
                case EntityState.Unchanged:
                    return NotesDomain.EntityState.Unchanged;
                default:
                    throw new ArgumentOutOfRangeException("state");
            }
        }
    }
}