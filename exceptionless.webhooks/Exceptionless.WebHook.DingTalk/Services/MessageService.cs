using ExceptionLess.WebHook.DingTalk.Messages;
using ExceptionLess.WebHook.DingTalk.Utilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Rabbit.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionLess.WebHook.DingTalk.Services
{
    public class MessageService : ISingletonDependency
    {
        private readonly ILogger<MessageService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public MessageService(ILogger<MessageService> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task SendAsync(Uri url, DingTalkRequestMessage message)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug($"准备向 {url} 发送数据");

            var type = StringUtility.ToCamelCase(message.Type.ToString());
            var data = new Dictionary<string, object>
            {
                {"msgtype", type},
                {type, message.Data}
            };

            var json = JsonConvert.SerializeObject(data);

            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug($"send dingTalk json:{json}");

            string responseContent = null;
            try
            {
                var result = await _httpClientFactory.CreateClient().PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
                responseContent = await result.Content.ReadAsStringAsync();

                if (_logger.IsEnabled(LogLevel.Debug))
                    _logger.LogDebug(responseContent);

                var response = JsonConvert.DeserializeObject<DingTalkResponseMessage>(responseContent);
                if (response.ErrorCode > 0)
                    _logger.LogError($"钉钉返回错误消息：{responseContent}");
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, $"向钉钉发送消息时失败，钉钉返回的消息：{responseContent}，发送的数据：{json}。");
            }
        }
    }
}