using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace NotesDomain
{
    /// <summary>
    /// Class BaseEntity.
    /// </summary>
    public class BaseEntity : IEntity
    {
        /// <summary>
        /// Gets or sets the acp template identifier.
        /// </summary>
        /// <value>The acp template identifier.</value>
        [Key]
        [ExcludeTransformation]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="BaseEntity"/> is acive.
        /// </summary>
        /// <value><c>true</c> if acive; otherwise, <c>false</c>.</value>
        [Required]
        [ExcludeTransformation]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the date modified.
        /// </summary>
        /// <value>The date modified.</value>
        [Required]
        [ExcludeTransformation]
        public DateTime DateModified { get; set; }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>The date created.</value>
        [Required]
        [ExcludeTransformation]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [Required]
        [ExcludeTransformation]
        public Guid UserId { get; set; }
        /// <summary>
        /// Gets or sets the state of the entity.
        /// </summary>
        /// <value>The state of the entity.</value>
        //[NotMapped]
        [ExcludeTransformation]
        public EntityState EntityState { get; set; }

        [Required]
        [ExcludeTransformation]
        public bool MarkAsDeleted { get; set; }
    }
}