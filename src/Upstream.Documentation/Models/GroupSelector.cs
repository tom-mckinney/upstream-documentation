using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Upstream.Documentation.Models
{
    public class GroupSelector
    {
        public string Name { get; set; }

        public Regex Match { get; set; }
    }
}
