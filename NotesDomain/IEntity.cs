using System.Collections.Generic;
using NotesDomain;

namespace NotesDomain
{
    /// <summary>
    /// Interface IEntity
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets the state of the entity.
        /// </summary>
        /// <value>The state of the entity.</value>
        EntityState EntityState { get; set; }
    }

}
