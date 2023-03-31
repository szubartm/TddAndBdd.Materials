using System;
using Moq;
using NUnit.Framework;

namespace UnitTesting.Stubs
{
    [TestFixture]
    public class FilesProcessorTestUsingMoq
    {
        FilesProcessor _out;
        Mock<IFilePathsProvider> _filePathsProvider;
        Mock<IFileContentProvider> _fileContentProvider;
        Mock<IDbFactory> _dbFactory;
        string _folder;
        string[] _filePaths;
        string _content1;
        string _content3;
        Mock<IDb> _db;

        [OneTimeSetUp]
        public void ArrangeAndAct()
        {
            _folder = "jmop gvghfewlinwcvufbewyrqtctoiufbqywckieuwfqbcygyofqewiu";
            _filePaths = new[]
            {
                "vngtyicnhyucikw",
                "ny6ft5874386ngdtyrugw22dfr",
                "bhj765ghyegft43"
            };


            _filePathsProvider = new Mock<IFilePathsProvider>();
            _filePathsProvider.Setup(x => x.GetFilePathsFromFolder(_folder)).Returns(_filePaths);

            _fileContentProvider = new Mock<IFileContentProvider>();
            _content1 = "n7g9865yn3879gchyruicgchu542";
            _fileContentProvider.Setup(x => x.GetDataFrom(_filePaths[0])).Returns(_content1);
            _fileContentProvider.Setup(x => x.GetDataFrom(_filePaths[1])).Throws<Exception>();
            _content3 = "vhy67538ongt87rw2nfgrycngfyr42";
            _fileContentProvider.Setup(x => x.GetDataFrom(_filePaths[2])).Returns(_content3);

            _dbFactory = new Mock<IDbFactory>();
            _db = new Mock<IDb>();
            _dbFactory.Setup(x => x.Create()).Returns(_db.Object);

            _out = new FilesProcessor(_filePathsProvider.Object, _fileContentProvider.Object, _dbFactory.Object);

            //act
            _out.ProcessFilesFromFolder(_folder);
        }

        [Test]
        public void ShouldCallFilePathsProvider()
        {
            _filePathsProvider.Verify(x => x.GetFilePathsFromFolder(_folder), Times.Once);
        }

        [Test]
        public void ShouldCallFileContentProviderAppropriateNumberOfTimes()
        {
            _fileContentProvider.Verify(x => x.GetDataFrom(It.IsAny<string>()), Times.Exactly(3));

            _fileContentProvider.Verify(x => x.GetDataFrom(_filePaths[0]), Times.Once);
            _fileContentProvider.Verify(x => x.GetDataFrom(_filePaths[1]), Times.Once);
            _fileContentProvider.Verify(x => x.GetDataFrom(_filePaths[2]), Times.Once);
        }

        [Test]
        public void ShouldCallDatabaseTwice()
        {
            _dbFactory.Verify(x => x.Create(), Times.Exactly(2));
            _db.Verify(x => x.SetParameter(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(4));
            _db.Verify(x => x.ExecuteStoredProcedure("files_pkg.save_file_with_content"), Times.Exactly(2));
            _db.Verify(x => x.Dispose(), Times.Exactly(2));
        }

        [Test]
        public void ShouldCorrectlySaveDataForFirstFile()
        {
            _db.Verify(x => x.SetParameter("file", _filePaths[0]), Times.Once);
            _db.Verify(x => x.SetParameter("data", _content1), Times.Once);
        }

        [Test]
        public void ShouldNotSaveToDbSecondFileDueToException()
        {
            _db.Verify(x => x.SetParameter("file", _filePaths[1]), Times.Never);
        }

        [Test]
        public void ShouldCorrectlySaveDataForThirdFile()
        {
            _db.Verify(x => x.SetParameter("file", _filePaths[2]), Times.Exactly(1));
            _db.Verify(x => x.SetParameter("data", _content3), Times.Once);
        }
    }
}