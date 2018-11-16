using Newtonsoft.Json;

namespace ExceptionLess.WebHook.DingTalk.Messages
{
    public class DingTalkResponseMessage
    {
        [JsonProperty("errmsg")]
        public string Message { get; set; }

        [JsonProperty("errcode")]
        public int ErrorCode { get; set; }
    }
}