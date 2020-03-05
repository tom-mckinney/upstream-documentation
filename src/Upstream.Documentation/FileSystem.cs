using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Upstream.Documentation
{
    public interface IFileSystem
    {
        IEnumerable<FileInfo> GetDocumentationFiles(IEnumerable<string> assemblyNames, string rootDirectoryPath);

        Stream ReadFile(FileInfo file);
    }

    public class FileSystem : IFileSystem
    {
        public IEnumerable<FileInfo> GetDocumentationFiles(IEnumerable<string> assemblyNames, string rootDirectoryPath)
        {
            var rootDirectory = new DirectoryInfo(rootDirectoryPath);

            return assemblyNames.SelectMany(n => rootDirectory.GetFiles($"{n}.xml", SearchOption.AllDirectories));
        }

        public Stream ReadFile(FileInfo file)
        {
            throw new NotImplementedException();
        }
    }
}
