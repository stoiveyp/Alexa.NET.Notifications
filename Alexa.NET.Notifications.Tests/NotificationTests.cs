using Alexa.NET.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Alexa.NET.Response.Ssml;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Alexa.NET.Notifications.Tests
{
    public class NotificationTests
    {
        [Fact]
        public void LinksDeserializeCorrectly()
        {
            var notification = GetNotification();
            var links = notification.Links;
            Assert.Equal(3, links.Count);
            Assert.Equal(
                new Uri("https://api.amazonalexa.com/v2/notifications/ab2cd6a3-72c6-43ad-9ace-47e4f45cba99/dismiss",
                    UriKind.Absolute).ToString(),
                links["dismiss"].ToString());

            Assert.Equal(
                new Uri("https://api.amazonalexa.com/v2/notifications/ab2cd6a3-72c6-43ad-9ace-47e4f45cba99",
                    UriKind.Absolute).ToString(),
                links["delete"].ToString());

            Assert.Equal(
                new Uri("https://api.amazonalexa.com/v2/notifications/ab2cd6a3-72c6-43ad-9ace-47e4f45cba99",
                    UriKind.Absolute).ToString(),
                links["update"].ToString());
        }

        [Fact]
        public void SpokenInfoDeserializesCorrectly()
        {
            var notification = GetNotification();
            var spoken = notification.SpokenInfo;

            var spokenContent = spoken.Content.First();
            Assert.Single(spoken.Content);
            Assert.Equal("en-US", spokenContent.Locale);
            Assert.Equal("You met your daily goal. Total steps that you took today are 12345.", spokenContent.Text);
            Assert.Equal(
                new Speech
                {
                    Elements = new List<ISsml>
                    {
                        new PlainText("You met your daily goal. Total steps that you took today are 12345.")
                    }
                }.ToXml(),
                spokenContent.Ssml);
        }

        [Fact]
        public void DisplayInfoDeserializesCorrectly()
        {
            var notification = GetNotification();
            var display = notification.DisplayInfo;
            Assert.Single(display.Content);

            var displayContent = display.Content.First();
            Assert.Equal("en-US", displayContent.Locale);
            Assert.Equal("You have reached your daily goal", displayContent.Toast.PrimaryText);
            Assert.Equal("You have reached your daily goal of 10K steps", displayContent.Title);
            Assert.Equal("You finished 12345 steps today", displayContent.BodyItems.First().PrimaryText);
        }

        [Fact]
        public void SkillInfoDeserializesCorrectly()
        {
            var notification = GetNotification();
            var skillInfo = notification.SkillInfo;
            var skillContent = skillInfo.Content.First();

            Assert.Equal("mySkill_skillId", skillInfo.Id);
            Assert.Equal("en-US", skillContent.Locale);
            Assert.Equal("MySkill", skillContent.Name);
            Assert.Equal("MySkillIconId", skillContent.Icon.Id);
            Assert.Equal(new Uri("https://example.com/contacts/Bob.jpg").ToString(),
                skillContent.Icon.Sources["x-small"].Url.ToString());
        }

        [Fact]
        public void NotificationProducesCorrectJson()
        {
            var notification = GetNotification();
            Assert.Equal("ab2cd6a3-72c6-43ad-9ace-47e4f45cba99", notification.Id);
            Assert.Equal("Bob", notification.RecipientName);
            Assert.Equal("mySkill_referenceId_to_be_provided", notification.ReferenceId);
        }

        [Fact]
        public async Task CreateGeneratesCorrectRequest()
        {
            var client = GetClient(req =>
            {
                Assert.Equal(HttpMethod.Post, req.Method);
                Assert.Equal(req.RequestUri.ToString(),
                    new Uri(NotificationClient.EuropeEndpoint, UriKind.Absolute).ToString());
                Assert.Equal("xxx", req.Headers.Authorization.Parameter);
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JObject.FromObject(new NotificationInfo()).ToString())
                };
            });
            await client.Create(new DisplayInfo(), new SpokenInfo(), "ref", DateTime.UtcNow);
        }

        [Fact]
        public async Task UpdateGeneratesCorrectRequest()
        {
            NotificationClient client = null;
            client = GetClient(req =>
            {
                Assert.Equal(HttpMethod.Put, req.Method);
                Assert.Equal(req.RequestUri.ToString(), client.NotificationUri("notifyid").ToString());
                Assert.Equal("xxx", req.Headers.Authorization.Parameter);
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JObject.FromObject(new NotificationInfo()).ToString())
                };
            });
            await client.Update("notifyid", new DisplayInfo(), new SpokenInfo(), "ref", DateTime.UtcNow);
        }

        [Fact]
        public async Task DeleteGeneratesCorrectRequest()
        {
            NotificationClient client = null;
            client = GetClient(req =>
            {
                Assert.Equal(HttpMethod.Delete, req.Method);
                Assert.Equal(req.RequestUri.ToString(), client.NotificationUri("notifyid").ToString());
                Assert.Equal("xxx", req.Headers.Authorization.Parameter);
                return new HttpResponseMessage(HttpStatusCode.Accepted);
            });
            await client.Delete("notifyid");
        }

        [Fact]
        public async Task DeletePendingGeneratesCorrectRequest()
        {
            NotificationClient client = null;
            client = GetClient(req =>
            {
                Assert.Equal(HttpMethod.Delete, req.Method);
                Assert.Equal(req.RequestUri.ToString(), client.NotificationUri("notifyid", "pending").ToString());
                Assert.Equal("xxx", req.Headers.Authorization.Parameter);
                return new HttpResponseMessage(HttpStatusCode.Accepted);
            });
            await client.Delete("notifyid", NotificationState.Pending);
        }

        [Fact]
        public async Task DeleteArchivedGeneratesCorrectRequest()
        {
            NotificationClient client = null;
            client = GetClient(req =>
            {
                Assert.Equal(HttpMethod.Delete, req.Method);
                Assert.Equal(req.RequestUri.ToString(), client.NotificationUri("notifyid", "archived").ToString());
                Assert.Equal("xxx", req.Headers.Authorization.Parameter);
                return new HttpResponseMessage(HttpStatusCode.Accepted);
            });
            await client.Delete("notifyid", NotificationState.Archived);
        }

        [Fact]
        public void NotificationListDeserializesCorrectly()
        {
            var listresponse = Utility.GetObjectFromExample<NotificationListResponse>("NotificationList.json");
            Assert.Equal(10,listresponse.TotalCount);
            Assert.Single(listresponse.Notifications);
            Assert.True(listresponse.Links.ContainsKey("next"));
        }

        [Fact]
        public async Task ListGeneratesCorrectRequest()
        {
            NotificationClient client = null;
            client = GetClient(req =>
            {
                Assert.Equal(HttpMethod.Get, req.Method);
                Assert.Equal(req.RequestUri.ToString(), client.NotificationUri(string.Empty).ToString());
                Assert.Equal("xxx", req.Headers.Authorization.Parameter);
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(Utility.GetExampleJson("NotificationList.json")) };
            });
            await client.List(NotificationState.Any);
        }

        [Fact]
        public async Task ListPendingGeneratesCorrectRequest()
        {
            NotificationClient client = null;
            client = GetClient(req =>
            {
                Assert.Equal(HttpMethod.Get, req.Method);
                Assert.Equal(req.RequestUri.ToString(), client.NotificationUri(string.Empty,"pending").ToString());
                Assert.Equal("xxx", req.Headers.Authorization.Parameter);
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(Utility.GetExampleJson("NotificationList.json")) };
            });
            await client.List(NotificationState.Pending);
        }

        [Fact]
        public async Task ListArchivedGeneratesCorrectRequest()
        {
            NotificationClient client = null;
            client = GetClient(req =>
            {
                Assert.Equal(HttpMethod.Get, req.Method);
                Assert.Equal(req.RequestUri.ToString(), client.NotificationUri(string.Empty,"archived").ToString());
                Assert.Equal("xxx", req.Headers.Authorization.Parameter);
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(Utility.GetExampleJson("NotificationList.json")) };
            });
            await client.List(NotificationState.Archived);
        }

        [Fact]
        public async Task ListProducesCorrectResponse()
        {
            NotificationClient client = null;
            client = GetClient(req => new HttpResponseMessage(HttpStatusCode.OK){Content = new StringContent(Utility.GetExampleJson("NotificationList.json"))});
            var response = await client.List(NotificationState.Any);
            Assert.Single(response.Notifications);
        }


        private NotificationClient GetClient(Func<HttpRequestMessage, HttpResponseMessage> action)
        {
            return new NotificationClient(NotificationClient.EuropeEndpoint, "xxx", new HttpClient(new ActionMessageHandler(action)));
        }

        private Notification GetNotification()
        {
            return Utility.GetObjectFromExample<Notification>("notification.Json");
        }
    }
}
