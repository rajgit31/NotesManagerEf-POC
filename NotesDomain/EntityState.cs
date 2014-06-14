namespace NotesDomain
{
    /// <summary>
    /// Enum EntityState
    /// </summary>
    public enum EntityState
    {
        /// <summary>
        /// The entity is being tracked by the context and exists in the database, and its property values have not changed from the values in the database
        /// </summary>
        Unchanged,
        /// <summary>
        ///  The entity is being tracked by the context but does not yet exist in the database
        /// </summary>
        Added,
        /// <summary>
        /// The entity is being tracked by the context and exists in the database, and some or all of its property values have been modified
        /// </summary>
        Modified,
        /// <summary>
        /// The entity is being tracked by the context and exists in the database, but has been marked for deletion from the database the next time SaveChanges is called
        /// </summary>
        Deleted,
        DeAttached,
    }
}