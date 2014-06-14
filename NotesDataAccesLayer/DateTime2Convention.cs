using System;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace NotesDataAccesLayer
{
    /// <summary>
    /// Class DateTime2Convention.
    /// </summary>
    public class DateTime2Convention : Convention
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTime2Convention"/> class.
        /// </summary>
        public DateTime2Convention()
        {
            this.Properties<DateTime>()
                .Configure(c => c.HasColumnType("datetime2"));
        }
    }
}