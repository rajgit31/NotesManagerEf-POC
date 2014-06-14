using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="BaseEntity"/> is acive.
        /// </summary>
        /// <value><c>true</c> if acive; otherwise, <c>false</c>.</value>
        [Required]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the date modified.
        /// </summary>
        /// <value>The date modified.</value>
        [Required]
        public DateTime DateModified { get; set; }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>The date created.</value>
        [Required]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [Required]
        public Guid UserId { get; set; }
        /// <summary>
        /// Gets or sets the state of the entity.
        /// </summary>
        /// <value>The state of the entity.</value>
        [NotMapped]
        public EntityState EntityState { get; set; }

        [Required]
        public bool MarkAsDeleted { get; set; }
    }
}