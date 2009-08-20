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
            //repo.Events.InsertOnSubmit(new Event{Start = DateTime.Now, 
            //                                     End = DateTime.Now.AddDays(1),
            //                                     HiFiveLimit = 5,
            //                                     Slug = "testEvent",
            //                                     Name = "testEvent"});
            //repo.SubmitChanges();
            
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
            repo.Events.InsertOnSubmit(new Event
                                           {
                                               Start = DateTime.Now,
                                               End = DateTime.Now.AddDays(1),
                                               HiFiveLimit = 5,
                                               Slug = "testEvent",
                                               Name = "testEvent"
                                           });
            repo.SubmitChanges();

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
        public void ProcessHelloTweet()
        {
            //Arrange
            throw new NotImplementedException();

            //Act


            //Assert
						 
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
