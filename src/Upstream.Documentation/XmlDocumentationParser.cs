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
        public ValueTask<IEnumerable<DocumentationGroup>> GetDocumentationGroupsAsync(FileInfo file, IEnumerable<GroupSelector> selectors)
        {
            var serializer = new XmlSerializer(typeof(MicrosoftXmlDocumentation));

            var documentation = (MicrosoftXmlDocumentation)serializer.Deserialize(file.OpenRead());

            var output = new List<DocumentationGroup>();

            foreach (var member in documentation.Members)
            {
                foreach (var selector in selectors)
                {
                    if (TryGetMemberName(member.Name, selector, out string memberName))
                    {
                        output.Add(new DocumentationGroup
                        {
                            Name = memberName,
                            Summary = CleanElementString(member.Summary),
                            Remarks = CleanElementString(member.Remarks),
                        });
                    }
                }
            }

            return new ValueTask<IEnumerable<DocumentationGroup>>(output);
        }

        public bool TryGetMemberName(string name, GroupSelector selector, out string memberName)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var typeName = selector.Type?.FullName ?? 
                selector.TypeName ?? 
                throw new ArgumentException("Selector must include a Type or TypeName");
            var typeNameIndex = name.IndexOf(typeName);

            if (typeNameIndex < 0)
            {
                memberName = null;
                return false;
            }

            memberName = name
                .Substring(typeNameIndex + typeName.Length)
                .TrimStart('.');

            return true;
        }

        public string CleanElementString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            return input
                .TrimStart('\n')
                .TrimStart()
                .TrimEnd('\n')
                .TrimEnd();
        }
    }
}
