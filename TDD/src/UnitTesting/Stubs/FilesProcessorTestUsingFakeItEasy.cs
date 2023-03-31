using System;
using FakeItEasy;
using NUnit.Framework;

namespace UnitTesting.Stubs
{
    //[TestFixture]
    public class FilesProcessorTestUsingFakeItEasy
    {
        FilesProcessor _out;
        IFilePathsProvider _filePathsProvider;
        IFileContentProvider _fileContentProvider;
        IDbFactory _dbFactory;
        string _folder;
        string[] _filePaths;
        string _content1;
        string _content3;
        IDb _db;

        [OneTimeSetUp]
        public void ArrangeAndAct()
        {
            _folder = "vgn6to834nchgiyoewrngcyfueiwrgcnytr4";
            _filePaths = new[]
                         {
                             "vngtyicnhyucikw",
                             "ny6ft5874386ngdtyrugw22dfr",
                             "bhj765ghyegft43"
                         };


            _filePathsProvider = A.Fake<IFilePathsProvider>();
            A.CallTo( () => _filePathsProvider.GetFilePathsFromFolder( _folder ) ).Returns( _filePaths );

            _fileContentProvider = A.Fake<IFileContentProvider>();
            _content1 = "n7g9865yn3879gchyruicgchu542";
            A.CallTo( () => _fileContentProvider.GetDataFrom( _filePaths[ 0 ] ) ).Returns( _content1 );
            A.CallTo( () => _fileContentProvider.GetDataFrom( _filePaths[ 1 ] ) ).Throws<Exception>();
            _content3 = "vhy67538ongt87rw2nfgrycngfyr42";
            A.CallTo( () => _fileContentProvider.GetDataFrom( _filePaths[ 2 ] ) ).Returns( _content3 );

            _dbFactory = A.Fake<IDbFactory>();
            _db = A.Fake<IDb>();
            A.CallTo( () => _dbFactory.Create() ).Returns( _db );

            _out = new FilesProcessor( _filePathsProvider, _fileContentProvider, _dbFactory );

            //act
            _out.ProcessFilesFromFolder( _folder );
        }

        [Test]
        public void ShouldCallFilePathsProvider()
        {
            A.CallTo( () => _filePathsProvider.GetFilePathsFromFolder( _folder ) ).MustHaveHappened( 1, Times.Exactly);
        }

        [Test]
        public void ShouldCallFileContentProviderAppropriateNumberOfTimes()
        {
            A.CallTo( () => _fileContentProvider.GetDataFrom( string.Empty ) ).WithAnyArguments().MustHaveHappened(3, Times.Exactly);

            A.CallTo( () => _fileContentProvider.GetDataFrom( _filePaths[ 0 ] ) ).MustHaveHappened();
            A.CallTo( () => _fileContentProvider.GetDataFrom( _filePaths[ 1 ] ) ).MustHaveHappened();
            A.CallTo( () => _fileContentProvider.GetDataFrom( _filePaths[ 2 ] ) ).MustHaveHappened();
        }

        [Test]
        public void ShouldCallDatabaseTwice()
        {
            A.CallTo( () => _dbFactory.Create() ).MustHaveHappened(2, Times.Exactly);
            A.CallTo( () => _db.SetParameter( null, null ) ).WithAnyArguments().MustHaveHappened(4, Times.Exactly);
            A.CallTo( () => _db.ExecuteStoredProcedure( "files_pkg.save_file_with_content" ) ).MustHaveHappened(2, Times.Exactly);
            A.CallTo( () => _db.Dispose() ).MustHaveHappened(2, Times.Exactly);
        }

        [Test]
        public void ShouldCorrectlySaveDataForFirstFile()
        {
            A.CallTo( () => _db.SetParameter( "file", _filePaths[ 0 ] ) ).MustHaveHappened();
            A.CallTo( () => _db.SetParameter( "data", _content1 ) ).MustHaveHappened();
        }

        [Test]
        public void ShouldNotSaveToDbSecondFileDueToException()
        {
            A.CallTo( () => _db.SetParameter( "file", _filePaths[ 1 ] ) ).MustNotHaveHappened();
        }

        [Test]
        public void ShouldCorrectlySaveDataForThirdFile()
        {
            A.CallTo( () => _db.SetParameter( "file", _filePaths[ 2 ] ) ).MustHaveHappened();
            A.CallTo( () => _db.SetParameter( "data", _content3 ) ).MustHaveHappened();
        }
    }
}