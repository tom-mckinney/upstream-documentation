using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Upstream.Documentation.Models;
using Upstream.Testing;
using Xunit;

namespace Upstream.Documentation.Test
{
    public class XmlDocumentationParserTests : TestBase<IXmlDocumentationParser>
    {
        Mock<IFileSystem> _fileSystemMock;

        public XmlDocumentationParserTests()
        {
            _fileSystemMock = MockRepository.Create<IFileSystem>();
        }

        protected override IXmlDocumentationParser CreateTestClass()
        {
            return new XmlDocumentationParser(_fileSystemMock.Object);
        }

        [Theory]
        [MemberData(nameof(GetDocumentationSelectors))]
        public async Task GetDocumentationGroupsAsync_returns_all_members_grouped_by_selector(GroupSelector[] selectors)
        {
            var testFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "Samples", "Sample.Core.xml"));

            var parser = CreateTestClass();

            var output = await parser.GetDocumentationGroupsAsync(testFile, selectors);

            Assert.Equal(2, output.Count());

            var foo = output.ElementAt(0);
            Assert.Equal("Foo", foo.Name);
            Assert.Equal("The best foo in all the land", foo.Summary);

            var bar = output.ElementAt(1);
            Assert.Equal("Bar", bar.Name);
            Assert.Equal("The place where you grab a pint", bar.Summary);
        }

        public static IEnumerable<object[]> GetDocumentationSelectors => new List<object[]>
        {
            new object[]
            {
                new[]
                {
                    new GroupSelector
                    {
                        Name = "User",
                        Match = new Regex("User")
                    }
                },
                //new[]
                //{
                //    new GroupSelector
                //    {
                //        Name = "User",
                //        Type = typeof(User)
                //    }
                //},
            }
        };
    }
}
