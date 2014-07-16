using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using NotesDataAccesLayer.Mappings;
using NotesDataAccesLayer.Migrations;
using NotesDomain.Entities;
using NotesDomainInterfaces;

namespace NotesDataAccesLayer
{
    /// <summary>
    /// Summary description for DalBaseNew. 
    /// </summary>
    public class PersonDbContext : DbContext, IDbContextSimple
    {
        static PersonDbContext()
        {
            Database.SetInitializer<PersonDbContext>(new MigrateDatabaseToLatestVersion<PersonDbContext, Configuration>());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotesDbContext"/> class.
        /// </summary>
        public PersonDbContext()
            : base("name=NotesDb")
        {
            // base.Configuration.LazyLoadingEnabled = false;
        }

        public IDbSet<Person> Persons { get; set; }

        public override int SaveChanges()
        {
            if (HasUnsavedChanges())
            {
                var saveChanges = base.SaveChanges();
                
                return saveChanges;
            }

            return 0;
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            var entities = base.Set<TEntity>();
            return entities;
        }

        public new DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            return base.Entry(entity);
        }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.</remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new PersonMap());
            modelBuilder.Configurations.Add(new PassportMap());
            modelBuilder.Conventions.Add(new DateTime2Convention());
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Creates a Database instance for this context that allows for creation/deletion/existence checks
        /// for the underlying database.
        /// This is accessed by the implementation of <see cref="IUnitOfWork" /> for Transactional purposes. 
        /// </summary>
        /// <value>The database.</value>
        public Database Database { get { return base.Database; } }

        private Boolean HasUnsavedChanges()
        {
            bool hasUnsavedChanges = this.ChangeTracker.Entries().Any(e => e.State == EntityState.Added
                                                                           || e.State == EntityState.Modified
                                                                           || e.State == EntityState.Deleted);
            return hasUnsavedChanges;
        }
    }
}