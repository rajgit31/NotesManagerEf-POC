using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotesDataAccesLayer;
using NotesDomain;
using NotesServiceLayer;

namespace NotesManagerServices.IntegrationTests
{
    [TestClass]
    public class UnitTest1
    {
        const string DB_NAME = "IntegrationTestDb.sdf";
        const string DB_PATH = @".\" + DB_NAME;
        const string CONNECTION_STRING = "data source=" + DB_PATH;

        [TestInitialize]
        public void TestInititialise()
        {
            System.Data.Entity.Database.SetInitializer<NotesDbContext>(new System.Data.Entity.DropCreateDatabaseAlways<NotesDbContext>());

            //DeleteDb();
            //using (var eng = new SqlCeEngine(CONNECTION_STRING))
            //{
            //    eng.CreateDatabase();
            //}
                
            //using (var conn = new SqlCeConnection(CONNECTION_STRING))
            //{
            //    //conn.Open();
            //    //string sql = ReadSQLFromFile(@"C:\Users\Pete\work\Jub\EFTests\Test.sqlce");
            //    //string[] sqlCmds = sql.Split(new string[] { "GO" }, int.MaxValue, StringSplitOptions.RemoveEmptyEntries);
            //    //foreach (string sqlCmd in sqlCmds)
            //    //    try
            //    //    {
            //    //        var cmd = conn.CreateCommand();

            //    //        cmd.CommandText = sqlCmd;
            //    //        cmd.ExecuteNonQuery();
            //    //    }
            //    //    catch (Exception e)
            //    //    {
            //    //        Console.Error.WriteLine("{0}:{1}", e.Message, sqlCmd);
            //    //        throw;
            //    //    }
            //}

        }

        private string ReadSQLFromFile(string sqlFilePath)
        {
            using (TextReader r = new StreamReader(sqlFilePath))
            {
                return r.ReadToEnd();
            }
        }

        public void DeleteDb()
        {
            if (File.Exists(DB_PATH))
                File.Delete(DB_PATH);
        }



        [TestMethod]
        public void TestMethod1()
        {
            using (var conn = new SqlCeConnection(CONNECTION_STRING))
            {
                var dbContext = new NotesDbContext(conn.ConnectionString);
                var target = new NotesManagerService(new EfUnitOfWork(dbContext));
                target.Save(new Note() {Title = "Test"});

                var notes = target.GetNotes().ToList();

                Assert.AreEqual(1, notes.Count());
            }
        }
    }
}
