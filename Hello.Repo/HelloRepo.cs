using System;
using System.Collections.Generic;
using System.Linq;

namespace Hello.Repo
{
    partial class User
    {
        public bool HasTag(string tag)
        {
            return this.Tags.Where(t => t.Name.Equals(tag, StringComparison.InvariantCultureIgnoreCase)).Count() > 0;
        }

        public IEnumerable<Friendship> Friends
        {
            get
            {
                return this
                    .Befriendees
                    .Where(befriendee => this
                        .Befrienders
                        .Any(befriender => befriender.Befriendee == befriendee.Befriender));
            }
        }
    }
}
