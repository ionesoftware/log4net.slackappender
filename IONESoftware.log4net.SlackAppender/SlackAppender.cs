using IONESoftware.Utilities.Slack;
using IONESoftware.Utilities.Slack.Payloads;
using log4net.Appender;
using log4net.Core;

namespace IONESoftware.log4net.SlackAppender
{
    public class SlackAppender : AppenderSkeleton
    {
        public string WebhookUrl { get; set; }

        protected override void Append(LoggingEvent loggingEvent)
        {
            SlackWebhook.PostMessage(WebhookUrl, BuildPayloadForLogginEvent(loggingEvent));
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