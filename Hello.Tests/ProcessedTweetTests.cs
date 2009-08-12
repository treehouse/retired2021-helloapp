using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;
using Hello.Bot;
using Hello.Repo;
using Moq;

namespace Hello.Tests
{
    public class ProcessedTweetTests
    {
        public ProcessedTweetTests()
        {
            var mockSettings = new Mock<ISettingsImpl>();
            mockSettings.Setup(s => s.Get("TwitterBotUsername")).Returns("apphandle");
            mockSettings.Setup(s => s.Get("TwitterHashTag")).Returns("thetag");
            Settings.SettingsImplementation = mockSettings.Object;
        }

        [Fact]
        public void BlankTest()
        {
            var t = TweetProcessor.Process("");

            Assert.Null(t);
        }

        [Theory]
        [InlineData("hello !dev #csharp #dotnet #jquery")]
        [InlineData("@apphandle hello !dev #csharp #dotnet #jquery")]
        [InlineData("#thetag hello !dev #csharp #dotnet #jquery")]
        public void HelloTest(string tweet)
        {
            var t = TweetProcessor.Process(tweet) as HelloTweet;

            Assert.NotNull(t);
            Assert.Equal(t.UserType, "dev");
            Assert.Equal(3, t.Tags.Count);
            Assert.True(t.Tags.Contains("csharp"));
            Assert.True(t.Tags.Contains("dotnet"));
            Assert.True(t.Tags.Contains("jquery"));
        }

        [Fact]
        public void MetTest()
        {
            var t = TweetProcessor.Process("met ryan matt kier") as MetTweet;

            Assert.NotNull(t);
            Assert.Equal(3, t.Friends.Count);
            Assert.True(t.Friends.Contains("ryan"));
            Assert.True(t.Friends.Contains("matt"));
            Assert.True(t.Friends.Contains("kier"));
        }

        [Fact]
        public void MetWithAtsTest()
        {
            var t = TweetProcessor.Process("met @ryan @matt @kier") as MetTweet;

            Assert.NotNull(t);
            Assert.Equal(3, t.Friends.Count);
            Assert.True(t.Friends.Contains("ryan"));
            Assert.True(t.Friends.Contains("matt"));
            Assert.True(t.Friends.Contains("kier"));
        }

        [Fact]
        public void ClaimTest()
        {
            var t = TweetProcessor.Process("claim ASDF1234") as ClaimTweet;

            Assert.NotNull(t);
            Assert.Equal("ASDF1234".ToLower(), t.Token);
        }

        [Fact]
        public void SatTest()
        {
            var t = TweetProcessor.Process("sat ASD12") as SatTweet;

            Assert.NotNull(t);
            Assert.Equal("ASD12".ToLower(), t.SeatCode);
        }

        [Fact]
        public void MessageTest()
        {
            var t = TweetProcessor.Process("This is my shout out to everyone at Carsonified!") as MessageTweet;

            Assert.NotNull(t);
            Assert.Equal(t.Message, "This is my shout out to everyone at Carsonified!");
        }
    }
}
