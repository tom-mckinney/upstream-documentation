using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Upstream.Testing;
using Xunit;

namespace Upstream.Documentation.Test
{
    public class FileSystemTests : TestBase<IFileSystem>
    {
        protected override IFileSystem CreateTestClass()
        {
            return new FileSystem();
        }

        [Fact]
        public void GetDocumentationFiles_returns_matching_files()
        {
            var fileSystem = CreateTestClass();

            var assemblyNames = new[]
            {
                "Sample.Core"
            };

            var configFiles = fileSystem.GetDocumentationFiles(assemblyNames, Directory.GetCurrentDirectory());

            var file = Assert.Single(configFiles);
            Assert.Equal("Sample.Core.xml", file.Name);
        }

        [Fact]
        public void GetDocumentationFiles_returns_empty_array_if_no_match()
        {
            var fileSystem = CreateTestClass();

            var assemblyNames = new[]
            {
                "Fake.Web"
            };

            var configFiles = fileSystem.GetDocumentationFiles(assemblyNames, Directory.GetCurrentDirectory());

            Assert.Empty(configFiles);
        }
    }
}
