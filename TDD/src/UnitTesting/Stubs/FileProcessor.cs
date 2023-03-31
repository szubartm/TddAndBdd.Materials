using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using JetBrains.Annotations;

namespace UnitTesting.Stubs
{
    [UsedImplicitly]
    internal class FilesProcessorOriginal
    {
        public static void ProcessFilesFromFolder(string folder)
        {
            foreach (var file in Directory.GetFiles(folder))
            {
                var data = File.ReadAllText(file);

                using var db = new Db("my_connection_string_name");
                db.SetParameter("file", file);
                db.SetParameter("data", data);
                db.ExecuteStoredProcedure("files_pkg.save_file_with_content");
            }
        }
    }

    #region way much better approach 

    internal record FilesProcessor(IFilePathsProvider FilePathsProvider, 
        IFileContentProvider FileContentProvider, IDbFactory DbFactory)
    {
        public void ProcessFilesFromFolder(string folder)
        {
            var files = FilePathsProvider.GetFilePathsFromFolder(folder);
            foreach (var file in files)
            {
                try
                {
                    var data = FileContentProvider.GetDataFrom(file);

                    using var db = DbFactory.Create();
                    db.SetParameter("file", file);
                    db.SetParameter("data", data);
                    db.ExecuteStoredProcedure("files_pkg.save_file_with_content");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex); /*logging*/
                }
            }
        }
    }

    public interface IDbFactory
    {
        IDb Create();
    }

    public interface IDb
        : IDisposable
    {
        void SetParameter(string name, string value);
        void ExecuteStoredProcedure(string storedProcedureName);
    }

    internal class Db : IDb
    {
        // ReSharper disable once UnusedParameter.Local
        public Db(string connectionStringName) { }
        public void Dispose() { }

        public void SetParameter(string name, string value) { }

        public void ExecuteStoredProcedure(string storedProcedureName) { }
    }

    public interface IFileContentProvider
    {
        string GetDataFrom(string file);
    }

    public interface IFilePathsProvider
    {
        IEnumerable<string> GetFilePathsFromFolder(string folder);
    }

    #endregion
}
