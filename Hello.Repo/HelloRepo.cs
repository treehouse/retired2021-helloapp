using System.Linq;
using System;

namespace Hello.Repo
{
    partial class User
    {
        public bool HasTag(string tag)
        {
            return this.Tags.Where(t => t.Name.Equals(tag, StringComparison.InvariantCultureIgnoreCase)).Count() > 0;
        }
    }
}
