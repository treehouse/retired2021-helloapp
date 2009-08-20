using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hello.Bot;
using Hello.Repo;
using Hello.Utils;
using Moq;
using Xunit;
using Xunit.Extensions;
using Hello.Bot.TweetTypes;

namespace Hello.Tests
{
    public class TweetProcessorTests
    {
        private HelloRepoDataContext repo;
        private TweetProcessor processor;

        public TweetProcessorTests()
        {
            var mockSettings = new Mock<ISettingsImpl>();
            mockSettings.Setup(settings => settings.GetString("ConnectionString")).Returns(
                "Data Source=localhost\\SQL2008;Initial Catalog=helloapp;User Id=hello_bot;Password=asdf;");
            Settings.SettingsImplementation = mockSettings.Object;

            repo = new HelloRepoDataContext(Settings.ConnectionString);
            processor = new TweetProcessor(repo);
        }

        [Fact]
        [AutoRollback]
        public void ProcessHiFiveTweet()
        {
            //Arrange
            TestHelperMethods.SetupTestEvent(repo);
            
            var hiFive = new HiFiveTweet("benadderson");
            var hiFivingUser = repo.Users.Where(u => u.Username == "thatismatt").First();

            //Act
            processor.ProcessTweet(hiFivingUser,hiFive);
            repo.SubmitChanges();

            HiFive storedTweet; 

            //Assert
            storedTweet = repo.HiFives.Where(hf => hf.HiFiver == hiFivingUser.Username &&
                                                       hf.HiFivee == "benadderson").First();
            Assert.NotNull(storedTweet);
        }

        [Fact]
        [AutoRollback]
        public void CantHiFiveYourself()
        {
            //Arrange
            TestHelperMethods.SetupTestEvent(repo);

            var hiFive = new HiFiveTweet("benadderson");
            var hiFivingUser = repo.Users.Where(u => u.Username == "benadderson").First();

            //Act
            processor.ProcessTweet(hiFivingUser, hiFive);
            repo.SubmitChanges();

            HiFive storedTweet;

            //Assert
            Assert.Throws<InvalidOperationException>(
                    () => storedTweet = repo.HiFives.Where(hf => hf.HiFiver == hiFivingUser.Username &&
                                                                 hf.HiFivee == "benadderson").First());
        }

        [Fact]
        [AutoRollback]
        public void ProcessClaimTweet()
        {
            //Arrange
            repo.Campaigns.InsertOnSubmit(new Campaign {CampaignID = 1, Name = "testCampaign", Value = 1});
            repo.SubmitChanges();
            int campaignId = repo.Campaigns.Where(c => c.Name == "testCampaign").First().CampaignID;
            repo.Tokens.InsertOnSubmit(new Token {AllowedRedemptions = 1, CampaignID = campaignId, Code = "testToken"});
            repo.SubmitChanges();

            var claim = new ClaimTweet();
            claim.Token = "testToken";
            var benadderson = repo.Users.Where(u => u.Username == "benadderson").First();

            //Act

            processor.ProcessTweet(benadderson, claim);
            repo.SubmitChanges();

            var storedRedemption = repo.Redemptions.Where(r => r.Username == "benadderson").First();

            //Assert
            Assert.NotNull(storedRedemption);
            Assert.True(storedRedemption.Token.Campaign.Name == "testCampaign");
            Assert.True(storedRedemption.Token.Code == "testToken");
        }

        [Fact]
        [AutoRollback]
        public void ProcessHelloTweetForExistingUser()
        {
            //Arrange
            TestHelperMethods.SetupTestEvent(repo);
            var tags = new List<string> {"c#", "jquery", "dotnet"};
            var hello = new HelloTweet {Tags = tags, UserType = "dev" };
            var helloUser = repo.Users.Where(u => u.Username == "thatismatt").First();
            var oldTags = repo.Tags.Where(t => t.Username == "thatismatt").Select(t => t.Name).AsEnumerable();

            //Act
            processor.ProcessTweet(helloUser, hello);
            repo.SubmitChanges();

            //Assert
            var newTags = repo.Tags.Where(t => t.Username == "thatismatt").Select(t => t.Name).AsEnumerable();
            var storedUser = repo.Users.Where(u => u.Username == "thatismatt").First();
            Assert.True(storedUser.UserType.Name == "Dev");
            Assert.Contains("c#", newTags);
            Assert.Contains("jquery", newTags);
            Assert.Contains("dotnet", newTags);

            foreach (var tag in oldTags)
            {
                Assert.DoesNotContain(tag, newTags);
            }
        }

        [Fact]
        [AutoRollback]
        public void ProcessHelloTweetForNewUser()
        {
            //Arrange
            TestHelperMethods.SetupTestEvent(repo);
            var tags = new List<string> { "c#", "sql", "vb" };
            var hello = new HelloTweet { Tags = tags, UserType = "biz" };
            var helloUser = new User
                                {
                                    Username = "newUser",
                                    Created = DateTime.Now,
                                    Updated = DateTime.Now,
                                    ImageURL = "URL"
                                };
            repo.Users.InsertOnSubmit(helloUser);
            repo.SubmitChanges();
            
            //Act
            processor.ProcessTweet(helloUser, hello);
            repo.SubmitChanges();

            //Assert
            var Tags = repo.Tags.Where(t => t.Username == "newUser").Select(t => t.Name).AsEnumerable();
            var storedUser = repo.Users.Where(u => u.Username == "newUser").First();
            Assert.True(storedUser.UserType.Name == "Biz");
            Assert.Contains("c#", Tags);
            Assert.Contains("sql", Tags);
            Assert.Contains("vb", Tags);
        }

        [Fact]
        [AutoRollback]
        public void ProcessMessageTweet()
        {
            //Arrange
            throw new NotImplementedException();

            //Act


            //Assert
						 
        }

        [Fact]
        [AutoRollback]
        public void ProcessMetTweet()
        {
            //Arrange
            throw new NotImplementedException();

            //Act


            //Assert
						 
        }

        [Fact]
        [AutoRollback]
        public void ProcessSatTweet()
        {
            //Arrange
            throw new NotImplementedException();

            //Act


            //Assert
						 
        }
    }
}
