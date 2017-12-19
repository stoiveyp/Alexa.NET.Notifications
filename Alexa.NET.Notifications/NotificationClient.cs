using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Notifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Alexa.NET
{
    public class NotificationClient
    {
        public const string NorthAmericaEndpoint = "https://api.amazonalexa.com/v2/notifications";
        public const string EuropeEndpoint = "https://api.eu.amazonalexa.com/v2/notifications";

        private static readonly JsonSerializer Serializer = JsonSerializer.CreateDefault();
        public HttpClient Client { get; set; }

        public NotificationClient(string endpointBase, string accessToken) : this(endpointBase, accessToken, new HttpClient())
        {

        }

        public NotificationClient(string endpointBase, string accessToken, HttpClient client)
        {
            client = client ?? new HttpClient();
            if (client.BaseAddress == null)
            {
                client.BaseAddress = new Uri(endpointBase, UriKind.Absolute);
            }

            if (client.DefaultRequestHeaders.Authorization == null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            Client = client;
        }

        public NotificationClient(HttpClient client)
        {
            Client = client;
        }

        public Task<NotificationInfo> Create(DisplayInfo display, SpokenInfo spoken, string reference, DateTime expiry)
        {
            return Create(new CreateUpdateRequest { DisplayInfo = display, SpokenInfo = spoken, ReferenceId = reference, ExpiryTime = expiry });
        }

        public async Task<NotificationInfo> Create(CreateUpdateRequest request)
        {
            var response = await Client.PostAsync(Client.BaseAddress, new StringContent(JObject.FromObject(request).ToString()));
            using (var reader = new JsonTextReader(new StreamReader(await response.Content.ReadAsStreamAsync())))
            {
                return Serializer.Deserialize<NotificationInfo>(reader);
            }
        }

        public Task Update(string notificationId, DisplayInfo display, SpokenInfo spoken, string reference, DateTime expiry)
        {
            return Update(notificationId, new CreateUpdateRequest { DisplayInfo = display, SpokenInfo = spoken, ReferenceId = reference, ExpiryTime = expiry });
        }

        public Task Update(string notificationId, CreateUpdateRequest request)
        {
            return Client.PutAsync(
               NotificationUri(notificationId),
               new StringContent(JObject.FromObject(request).ToString()));
        }

        public Uri NotificationUri(string notificationId, string status = null)
        {
            return new Uri(Client.BaseAddress, Client.BaseAddress.AbsolutePath + "/" + notificationId + (!string.IsNullOrWhiteSpace(status) ? "/" : string.Empty) + status);
        }

        public Task Delete(string notificationId)
        {
            return Client.DeleteAsync(NotificationUri(notificationId));
        }

        public Task DeletePending(string notificationId)
        {
            return Client.DeleteAsync(NotificationUri(notificationId,"pending"));
        }

        public Task DeleteArchived(string notificationId)
        {
            return Client.DeleteAsync(NotificationUri(notificationId, "archived"));
        }
    }
}
