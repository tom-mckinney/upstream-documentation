using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Upstream.Documentation.Models;

namespace Upstream.Documentation
{
    public interface IXmlDocumentationParser
    {
        ValueTask<IEnumerable<DocumentationGroup>> GetDocumentationGroupsAsync(FileInfo file, IEnumerable<GroupSelector> selectors);
    }

    public class XmlDocumentationParser : IXmlDocumentationParser
    {
        private readonly IFileSystem _fileSystem;

        public XmlDocumentationParser(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public ValueTask<IEnumerable<DocumentationGroup>> GetDocumentationGroupsAsync(FileInfo file, IEnumerable<GroupSelector> selectors)
        {
            var serializer = new XmlSerializer(typeof(MicrosoftXmlDocumentation));

            var documentation = (MicrosoftXmlDocumentation)serializer.Deserialize(file.OpenRead());

            var output = new List<DocumentationGroup>();

            foreach (var member in documentation.Members)
            {
                foreach (var selector in selectors)
                {
                    if (selector.Match.IsMatch(member.Name))
                    {
                        output.Add(new DocumentationGroup
                        {
                            Name = member.Name,
                            Summary = CleanElementString(member.Summary),
                        });
                    }
                }
            }

            return new ValueTask<IEnumerable<DocumentationGroup>>(output);
        }

        public string CleanElementString(string input)
        {
            return input
                .TrimStart('\n')
                .TrimStart()
                .TrimEnd('\n')
                .TrimEnd();
        }
    }
}
