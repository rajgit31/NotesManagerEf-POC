﻿using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Text;
using System.Threading.Tasks;
using NotesDomain;

namespace NotesDataAccesLayer
{
    /// <summary>
    /// Interface IDbContext
    /// </summary>
    public interface IDbContext
    {
        //IDbSet<Note> Notes { get; set; }

        /// <summary>
        /// Sets this instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <returns>IDbSet&lt;TEntity&gt;.</returns>
        IDbSet<TEntity> Set<TEntity>() where TEntity : class, IEntity;
        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int SaveChanges();
        /// <summary>
        /// Entries the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>DbEntityEntry&lt;TEntity&gt;.</returns>
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity :  class, IEntity;
     
        /// <summary>
        /// Disposes this instance.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>The database.</value>
        Database Database { get; }
    }
}
