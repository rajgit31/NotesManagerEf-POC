using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotesDomain;

namespace NotesDataAccesLayer.Mappings
{
    /// <summary>
    /// Define the base mapping base entity.
    /// </summary>
    public class EntityMap<T> : EntityTypeConfiguration<T> where T : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMap{T}"/> class.
        /// </summary>
        public EntityMap()
        {
            //Primary Key of any entity
            this.HasKey(t => t.Id);

            this.Property(t => t.IsActive);
            this.Property(t => t.DateModified);
            this.Property(t => t.DateCreated);
            this.Property(t => t.UserId);
            this.Property(t => t.MarkAsDeleted);

            this.Ignore(x => x.EntityState);
        }
    }
}
