using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Max.Tools.DomainGenerator
{
    public sealed class CurrentDirectoryScope : IDisposable
    {
        string previousPath;

        public CurrentDirectoryScope(string path)
        {
            // Store original path:
            this.previousPath = Environment.CurrentDirectory;

            // Change directory:
            if (path == null)
            { 
                // Do nothing;
            }
            else if (Directory.Exists(path))
            {
                Environment.CurrentDirectory = path;
            }
            else if (File.Exists(path))
            {
                Environment.CurrentDirectory = new FileInfo(path).Directory.FullName;
            }
            else
            {
                throw new System.IO.FileNotFoundException();
            }
        }

        public void Dispose()
        {
            // Restore original path:
            Environment.CurrentDirectory = this.previousPath;
        }
    }
}
