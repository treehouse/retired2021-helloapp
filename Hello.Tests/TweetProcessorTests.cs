using System.Collections.Generic;
using System.Linq;
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
            mockSettings
                .Setup(settings => settings.GetString("ConnectionString"))
                .Returns("Data Source=localhost\\SQL2008;Initial Catalog=helloapp;User Id=hello_bot;Password=asdf;");
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
            var hiFivingUser = repo.Users.Where(u => u.Username == "thatismatt").FirstOrDefault();

            //Act
            processor.ProcessTweet(hiFivingUser,hiFive);
            repo.SubmitChanges();

            HiFive storedHiFive = repo.HiFives.Where(hf => hf.HiFiver == hiFivingUser.Username &&
                                                       hf.HiFivee == "benadderson").FirstOrDefault();
            Point pointsAwarded = repo.Points.Where(p => p.Username == "benadderson").FirstOrDefault();

            //Assert
            Assert.NotNull(storedHiFive);
            Assert.NotNull(pointsAwarded);
            Assert.True(pointsAwarded.Details == "Hi5: thatismatt");
        }

        [Fact]
        [AutoRollback]
        public void CantHiFiveYourself()
        {
            //Arrange
            TestHelperMethods.SetupTestEvent(repo);

            var hiFive = new HiFiveTweet("benadderson");
            var hiFivingUser = repo.Users.Where(u => u.Username == "benadderson").FirstOrDefault();

            //Act
            processor.ProcessTweet(hiFivingUser, hiFive);
            repo.SubmitChanges();

            HiFive storedHiFive = repo.HiFives.Where(hf => hf.HiFiver == hiFivingUser.Username &&
                                                                 hf.HiFivee == "benadderson").FirstOrDefault();
            Point pointsAwarded = repo.Points.Where(p => p.Username == "benadderson").FirstOrDefault();

            //Assert
            Assert.Null(storedHiFive);
            Assert.Null(pointsAwarded);
        }

        [Fact]
        [AutoRollback]
        public void CantHiFiveOutsideAnEvent()
        {
            //Arrange
            var hiFive = new HiFiveTweet("benadderson");
            var hiFivingUser = repo.Users.Where(u => u.Username == "thatismatt").FirstOrDefault();

            //Act
            processor.ProcessTweet(hiFivingUser, hiFive);
            repo.SubmitChanges();

            HiFive storedHiFive = repo.HiFives.Where(hf => hf.HiFiver == hiFivingUser.Username &&
                                                                 hf.HiFivee == "benadderson").FirstOrDefault();
            Point pointsAwarded = repo.Points.Where(p => p.Username == "benadderson").FirstOrDefault();

            //Assert
            Assert.Null(storedHiFive);
            Assert.Null(pointsAwarded);
        }

        [Fact]
        [AutoRollback]
        public void CantExceedTheEventHiFiveLimit()
        {
            //Arrange
            TestHelperMethods.SetupTestEvent(repo);
            var hiFivingUser = repo.Users.Where(u => u.Username == "thatismatt").FirstOrDefault();

            for (int x = 1; x < 6; x++ )
            {
                string userName = "newUser" + x;
                TestHelperMethods.CreateTestUser(repo, userName);
                var hiFive = new HiFiveTweet(userName);
                processor.ProcessTweet(hiFivingUser, hiFive);
                repo.SubmitChanges();
            }

            //Act
            var oneMoreHiFive = new HiFiveTweet("benadderson");
            processor.ProcessTweet(hiFivingUser, oneMoreHiFive);

            var hiFives = repo.HiFives.Where(hf => hf.HiFiver == "thatismatt").Select(hf => hf.HiFivee).AsEnumerable();

            //Assert
            Assert.Contains("newUser1", hiFives);
            Assert.Contains("newUser2", hiFives);
            Assert.Contains("newUser3", hiFives);
            Assert.Contains("newUser4", hiFives);
            Assert.Contains("newUser5", hiFives);
            Assert.DoesNotContain("benadderson", hiFives);
        }

        [Fact]
        [AutoRollback]
        public void ProcessClaimTweet()
        {
            //Arrange
            repo.Campaigns.InsertOnSubmit(new Campaign {CampaignID = 1, Name = "testCampaign", Value = 1});
            repo.SubmitChanges();
            int campaignId = repo.Campaigns.Where(c => c.Name == "testCampaign").FirstOrDefault().CampaignID;
            repo.Tokens.InsertOnSubmit(new Token {AllowedRedemptions = 1, CampaignID = campaignId, Code = "testToken"});
            repo.SubmitChanges();

            var claim = new ClaimTweet();
            claim.Token = "testToken";
            var benadderson = repo.Users.Where(u => u.Username == "benadderson").FirstOrDefault();

            //Act

            processor.ProcessTweet(benadderson, claim);
            repo.SubmitChanges();

            var storedRedemption = repo.Redemptions.Where(r => r.Username == "benadderson").FirstOrDefault();

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
            var helloUser = repo.Users.Where(u => u.Username == "thatismatt").FirstOrDefault();
            var oldTags = repo.Tags.Where(t => t.Username == "thatismatt").Select(t => t.Name).AsEnumerable();

            //Act
            processor.ProcessTweet(helloUser, hello);
            repo.SubmitChanges();

            //Assert
            var newTags = repo.Tags.Where(t => t.Username == "thatismatt").Select(t => t.Name).AsEnumerable();
            var storedUser = repo.Users.Where(u => u.Username == "thatismatt").FirstOrDefault();
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
            var helloUser = TestHelperMethods.CreateTestUser(repo, "newUser");
            
            //Act
            processor.ProcessTweet(helloUser, hello);
            repo.SubmitChanges();

            //Assert
            var storedTags = repo.Tags.Where(t => t.Username == "newUser").Select(t => t.Name).AsEnumerable();
            var storedUser = repo.Users.Where(u => u.Username == "newUser").FirstOrDefault();
            Assert.True(storedUser.UserType.Name == "Biz");
            Assert.Contains("c#", storedTags);
            Assert.Contains("sql", storedTags);
            Assert.Contains("vb", storedTags);
        }


        [Fact]
        [AutoRollback]
        public void ProcessMetTweet()
        {
            //Arrange
            TestHelperMethods.SetupTestEvent(repo);

            var met = new MetTweet{Friends = {"benadderson"}};
            var metUser = repo.Users.Where(u => u.Username == "thatismatt").FirstOrDefault();

            //Act
            processor.ProcessTweet(metUser, met);
            repo.SubmitChanges();

            Friendship storedFriendship = repo.Friendships.Where(f => f.Befriender == metUser.Username &&
                                                       f.Befriendee == "benadderson").FirstOrDefault();
           
            //Assert
            Assert.NotNull(storedFriendship);
        }

        [Fact]
        [AutoRollback]
        public void CantMeetYourself()
        {
            //Arrange
            TestHelperMethods.SetupTestEvent(repo);

            var met = new MetTweet {Friends = {"benadderson"}};
            var metUser = repo.Users.Where(u => u.Username == "benadderson").FirstOrDefault();

            //Act
            processor.ProcessTweet(metUser, met);
            repo.SubmitChanges();

            Friendship storedFriendship = repo.Friendships.Where(f => f.Befriender == metUser.Username &&
                                                                 f.Befriendee == metUser.Username).FirstOrDefault();
           
            //Assert
            Assert.Null(storedFriendship);
        }

        [Fact]
        [AutoRollback]
        public void CantMeetOutsideAnEvent()
        {
            //Arrange
            var met = new MetTweet {Friends = {"benadderson"}};
            var metUser = repo.Users.Where(u => u.Username == "thatismatt").First();

            //Act
            processor.ProcessTweet(metUser, met);
            repo.SubmitChanges();

            Friendship storedFriendship = repo.Friendships.Where(f => f.Befriender == metUser.Username &&
                                                                 f.Befriendee == "benadderson").FirstOrDefault();
            
            //Assert
            Assert.Null(storedFriendship);
        }

        [Fact]
        [AutoRollback]
        public void ProcessMeetingTweetsForPoints()
        {
            //Arrange
            TestHelperMethods.SetupTestEvent(repo);

            var met1 = new MetTweet { Friends = { "benadderson" } };
            var met2 = new MetTweet { Friends = { "thatismatt" } };
            var metUser1 = repo.Users.Where(u => u.Username == "thatismatt").FirstOrDefault();
            var metUser2 = repo.Users.Where(u => u.Username == "benadderson").FirstOrDefault();

            //Act
            processor.ProcessTweet(metUser1, met1);
            processor.ProcessTweet(metUser2, met2);
            repo.SubmitChanges();

            Friendship storedFriendship1 = repo.Friendships.Where(f => f.Befriender == metUser1.Username &&
                                                       f.Befriendee == "benadderson").FirstOrDefault();
            Friendship storedFriendship2 = repo.Friendships.Where(f => f.Befriender == metUser2.Username &&
                                                       f.Befriendee == "thatismatt").FirstOrDefault();
            Point pointsAwarded1 = repo.Points.Where(p => p.Username == metUser1.Username).FirstOrDefault();
            Point pointsAwarded2 = repo.Points.Where(p => p.Username == metUser2.Username).FirstOrDefault();

            //Assert
            Assert.NotNull(storedFriendship1);
            Assert.NotNull(storedFriendship2);
            Assert.NotNull(pointsAwarded1);
            Assert.True(pointsAwarded1.Details == "Mutual meeting");
            Assert.True(pointsAwarded1.Amount == 10);
            Assert.True(pointsAwarded1.Username == "thatismatt");
            Assert.NotNull(pointsAwarded2);
            Assert.True(pointsAwarded2.Details == "Mutual meeting");
            Assert.True(pointsAwarded2.Amount == 10);
            Assert.True(pointsAwarded2.Username == "benadderson");
        }

        [Fact]
        [AutoRollback]
        public void ProcessMessageTweet()
        {
            //Arrange
            var message = new MessageTweet {Message = "testMessage"};
            var messageUser = repo.Users.Where(u => u.Username == "benadderson").FirstOrDefault();

            //Act
            processor.ProcessTweet(messageUser, message);
            repo.SubmitChanges();

            var storedMessage = repo.Messages.Where(m => m.Username == "benadderson").FirstOrDefault();

            //Assert
            Assert.NotNull(storedMessage);
            Assert.True(storedMessage.Text == "testMessage");
        }

        [Fact]
        [AutoRollback]
        public void ProcessSatTweet()
        {
            //Arrange
            int eventId;
            TestHelperMethods.SetupTestEvent(repo, out eventId);
            TestHelperMethods.SetupTestSession(repo, eventId);
            TestHelperMethods.SetupTestSeat(repo, eventId, 1, 1, "ABCDE");

            var sat = new SatTweet { SeatCode = "ABCDE" };
            var satUser = repo.Users.Where(u => u.Username == "thatismatt").FirstOrDefault();

            //Act
            processor.ProcessTweet(satUser, sat);
            repo.SubmitChanges();

            var storedSat = repo.Sats.Where(s => s.Username == "thatismatt").FirstOrDefault();
            var pointsAwarded = repo.Points.Where(p => p.Username == "thatismatt").FirstOrDefault();

            //Assert
            Assert.NotNull(storedSat);
            Assert.True(storedSat.Seat.Code == "ABCDE");
            Assert.NotNull(pointsAwarded);
            Assert.True(pointsAwarded.Details.StartsWith("Sat in seat: "));
            Assert.True(pointsAwarded.Amount == 10);
        }

        [Fact]
        [AutoRollback]
        public void TakeSomebodyElsesSeat()
        {
            //Arrange
            int eventId;
            TestHelperMethods.SetupTestEvent(repo, out eventId);
            TestHelperMethods.SetupTestSession(repo, eventId);
            TestHelperMethods.SetupTestSeat(repo, eventId, 1, 1, "ABCDE");

            var sat = new SatTweet { SeatCode = "ABCDE" };
            var satUser = repo.Users.Where(u => u.Username == "thatismatt").FirstOrDefault();
            processor.ProcessTweet(satUser, sat);
            repo.SubmitChanges();

            //Act
            var sat2 = new SatTweet { SeatCode = "ABCDE" };
            var satUser2 = repo.Users.Where(u => u.Username == "benadderson").FirstOrDefault();
            processor.ProcessTweet(satUser2, sat2);
            repo.SubmitChanges();

            var storedSats = repo.Sats.Where(s => s.Seat.Code == "ABCDE").Select(s => s.Username).AsEnumerable();

            //Assert
            Assert.NotNull(storedSats);
            Assert.Contains("benadderson", storedSats);
            Assert.DoesNotContain("thatismatt", storedSats);
        }

        [Fact]
        [AutoRollback]
        public void MoveToSitSomewhereElse()
        {
            //Arrange
            int eventId;
            TestHelperMethods.SetupTestEvent(repo, out eventId);
            TestHelperMethods.SetupTestSession(repo, eventId);
            TestHelperMethods.SetupTestSeat(repo, eventId, 1, 1, "ABCDE");
            TestHelperMethods.SetupTestSeat(repo, eventId, 1, 2, "FGHIJ");

            var sat = new SatTweet { SeatCode = "ABCDE" };
            var satUser = repo.Users.Where(u => u.Username == "thatismatt").FirstOrDefault();
            processor.ProcessTweet(satUser, sat);
            repo.SubmitChanges();

            //Act
            var sat2 = new SatTweet { SeatCode = "FGHIJ" };
            var satUser2 = repo.Users.Where(u => u.Username == "thatismatt").FirstOrDefault();
            processor.ProcessTweet(satUser2, sat2);
            repo.SubmitChanges();

            var storedSats = repo.Sats.Where(s => s.Username == "thatismatt").Select(s => s.Seat.Code).AsEnumerable();
            var pointsAwarded =
                repo.Points.Where(p => p.Username == "thatismatt" && p.Details.StartsWith("Sat in seat: ")).AsEnumerable
                    ();

            //Assert
            Assert.NotNull(storedSats);
            Assert.Contains("FGHIJ", storedSats);
            Assert.DoesNotContain("ABCDE", storedSats);
            Assert.NotNull(pointsAwarded);
            Assert.True(pointsAwarded.Count() == 1);
        }

        [Fact]
        [AutoRollback]
        public void CantSitOutsideASession()
        {
            //Arrange
            
            //Act
            var sat = new SatTweet { SeatCode = "ABCDE" };
            var satUser = repo.Users.Where(u => u.Username == "thatismatt").FirstOrDefault();
            processor.ProcessTweet(satUser, sat);
            repo.SubmitChanges();

            var storedSat = repo.Sats.Where(s => s.Username == "thatismatt").FirstOrDefault();

            //Assert
			Assert.Null(storedSat);
        }
    }
}
