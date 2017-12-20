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

        private const string PendingState = "pending";
        private const string ArchivedState = "archived";
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

        public Uri NotificationUri(string notificationId = null, string status = null, string query = null)
        {
            var builder = new UriBuilder(Client.BaseAddress);
            if (!string.IsNullOrWhiteSpace(notificationId))
            {
                builder.Path += ("/" + notificationId);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                builder.Path += ("/" + status);
            }

            if (!string.IsNullOrWhiteSpace(query))
            {
                builder.Query = ("?" + query);
            }
            return builder.Uri;
        }

        public Task Delete(string notificationId, NotificationState state = NotificationState.Any)
        {
            switch (state)
            {
                case NotificationState.Any:
                    return Client.DeleteAsync(NotificationUri(notificationId));
                case NotificationState.Pending:
                    return Client.DeleteAsync(NotificationUri(notificationId, PendingState));
                case NotificationState.Archived:
                    return Client.DeleteAsync(NotificationUri(notificationId, ArchivedState));
            }
            throw new InvalidOperationException("Unknown notification state");
        }

        public Task<NotificationListResponse> List(NotificationState state)
        {
            return ListRequest(state, string.Empty);
        }

        public Task<NotificationListResponse> List(NotificationState state, int pageSize)
        {
            return ListRequest(state, $"pageSize={pageSize}");
        }

        public Task<NotificationListResponse> List(NotificationState state, string reference)
        {
            return ListRequest(state, $"referenceId={reference}");
        }

        private async Task<NotificationListResponse> ListRequest(NotificationState state, string query)
        {
            Task<HttpResponseMessage> stateTask = null;
            switch (state)
            {
                case NotificationState.Any:
                    stateTask = Client.GetAsync(NotificationUri(string.Empty, string.Empty, query));
                    break;
                case NotificationState.Pending:
                    stateTask = Client.GetAsync(NotificationUri(string.Empty, PendingState, query));
                    break;
                case NotificationState.Archived:
                    stateTask = Client.GetAsync(NotificationUri(string.Empty, ArchivedState, query));
                    break;
            }

            if (stateTask == null)
            {
                throw new InvalidOperationException("unknown notification state");
            }

            var response = await stateTask;
            using(var reader = new JsonTextReader(new StreamReader(await response.Content.ReadAsStreamAsync())))
            {
                return Serializer.Deserialize<NotificationListResponse>(reader);
            }
        }

        public Task Dismiss(string notificationId)
        {
            return Client.DeleteAsync(NotificationUri(notificationId, "dismiss"));
        }
    }
}
