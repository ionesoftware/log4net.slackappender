using System.IO;
using System.Net;
using System.Net.Http;
using log4net.Appender;
using log4net.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IONESoftware.log4net.SlackAppender
{
    public class SlackAppender : AppenderSkeleton
    {
        private readonly Formatting _jsonFormatting = Formatting.None;

        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public string WebhookUrl { get; set; }

        protected override void Append(LoggingEvent loggingEvent)
        {
            var request = WebRequest.CreateHttp(WebhookUrl);
            request.Method = HttpMethod.Post.ToString();
            using (var requestStream = request.GetRequestStream())
            {
                using (var streamWriter = new StreamWriter(requestStream))
                {
                    var payload = BuildPayloadForLogginEvent(loggingEvent);
                    streamWriter.Write(JsonConvert.SerializeObject(payload, _jsonFormatting, _jsonSerializerSettings));
                }
            }

            using (request.GetResponse())
            {
                // just send request
            }
        }

        private SlackMessagePayload BuildPayloadForLogginEvent(LoggingEvent loggingEvent)
        {
            return new SlackMessagePayload
            {
                Text = RenderLoggingEvent(loggingEvent)
            };
        }
    }
}