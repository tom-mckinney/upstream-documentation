using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Upstream.Documentation.Models;
using Upstream.Testing;
using Xunit;

namespace Upstream.Documentation.Test
{
    public class XmlDocumentationParserTests : TestBase<IXmlDocumentationParser>
    {
        protected override IXmlDocumentationParser CreateTestClass()
        {
            return new XmlDocumentationParser();
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
            Assert.Null(foo.Remarks);

            var bar = output.ElementAt(1);
            Assert.Equal("Bar", bar.Name);
            Assert.Equal("The place where you grab a pint", bar.Summary);
            Assert.Equal("$1 beers on Tuesday", bar.Remarks);
        }

        [Fact]
        public void TryGetMemberName_success()
        {

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
                        Type = typeof(Sample.Core.User),
                    }
                },
            },
            new object[]
            {
                new[]
                {
                    new GroupSelector
                    {
                        Name = "User",
                        TypeName = "Sample.Core.User",
                    }
                },
            },
            new object[]
            {
                new[]
                {
                    new GroupSelector
                    {
                        Name = "User",
                        TypeName = "User",
                    }
                },
            }
        };
    }
}
