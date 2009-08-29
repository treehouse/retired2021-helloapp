using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hello.Repo
{
    public static class RepoExtensions
    {
        public static IEnumerable<Friendship> Friendships(this User user)
        {
            return user
                .Befriendees
                .Where(bee => user
                    .Befrienders
                    .Any(ber => ber.Befriendee == bee.Befriender));
        }
    }
}
