using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace UnitTesting.Stubs
{
    class FilesProcessorOriginal
    {
        public static void ProcessFilesFromFolder( string folder )
        {
            foreach ( var file in Directory.GetFiles( folder ) )
            {
                var data = File.ReadAllText( file );
                using ( var db = new Db( "my_connection_string_name" ) )
                {
                    db.SetParameter( "file", file );
                    db.SetParameter( "data", data );
                    db.ExecuteStoredProcedure( "files_pkg.save_file_with_content" );
                }
            }
        }
    }

    #region way much better :)

    class FilesProcessor
    {
        readonly IFilePathsProvider _filePathsProvider;
        readonly IFileContentProvider _fileContentProvider;
        readonly IDbFactory _dbFactory;

        public FilesProcessor( IFilePathsProvider filePathsProvider,
                               IFileContentProvider fileContentProvider,
                               IDbFactory dbFactory )
        {
            _filePathsProvider = filePathsProvider;
            _fileContentProvider = fileContentProvider;
            _dbFactory = dbFactory;
        }

        public void ProcessFilesFromFolder( string folder )
        {
            var files = _filePathsProvider.GetFilePathsFromFolder( folder );
            Parallel.ForEach( files,
                              file =>
                              {
                                  try
                                  {
                                      var data = _fileContentProvider.GetDataFrom( file );
                                      using ( var db = _dbFactory.Create() )
                                      {
                                          db.SetParameter( "file", file );
                                          db.SetParameter( "data", data );
                                          db.ExecuteStoredProcedure( "files_pkg.save_file_with_content" );
                                      }
                                  }
                                  catch ( Exception ex )
                                  {
                                      //log the error
                                      Debug.WriteLine( ex );
                                  }
                              } );
        }
    }

    public interface IDbFactory
    {
        IDb Create();
    }

    public interface IDb
        : IDisposable
    {
        void SetParameter( string name, string value );
        void ExecuteStoredProcedure( string storedProcedureName );
    }

    class Db : IDb
    {
        public Db(string connectionStringName)
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void SetParameter( string name, string value )
        {
            throw new NotImplementedException();
        }

        public void ExecuteStoredProcedure( string storedProcedureName )
        {
            throw new NotImplementedException();
        }
    }

    public interface IFileContentProvider
    {
        string GetDataFrom( string file );
    }

    public interface IFilePathsProvider
    {
        IEnumerable<string> GetFilePathsFromFolder( string folder );
    }

    #endregion
}
