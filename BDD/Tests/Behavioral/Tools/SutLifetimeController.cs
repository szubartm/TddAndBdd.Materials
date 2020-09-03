using System;
using System.Diagnostics;

namespace Behavioral.Tools
{
    public class SutLifetimeController : IDisposable
    {
        private Process _sut = null;

        public void StartSut(string filePath)
        {
            try
            {
                _sut = new Process
                {
                    StartInfo =
                    {
                        UseShellExecute = true,
                        FileName =filePath,
                        CreateNoWindow = false
                    }
                };
                _sut.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Dispose()
        {
            _sut.CloseMainWindow();
            _sut?.Dispose();
        }
    }
}