using ExceptionLess.WebHook.Abstractions;
using ExceptionLess.WebHook.DingTalk.Messages;
using ExceptionLess.WebHook.DingTalk.Services;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExceptionLess.WebHook.DingTalk
{
    public class DingTalkWebHookProvider : IWebHookProvider
    {
        private readonly FileTemplateService _fileTemplateService;
        private readonly MessageService _messageService;
        private readonly IHostingEnvironment _environment;

        public DingTalkWebHookProvider(
            FileTemplateService fileTemplateService,
            MessageService messageService,
            IHostingEnvironment environment)
        {
            _fileTemplateService = fileTemplateService;
            _messageService = messageService;
            _environment = environment;
        }

        #region Implementation of IWebHookProvider

        public string Name { get; } = "DingTake";

        public async Task ProcessAsync(ExceptionLessEventModel model, IDictionary<string, string> parameters)
        {
            parameters.TryGetValue("accessToken", out var accessToken);

            model.Environment = _environment.EnvironmentName;
            var content = await _fileTemplateService.GetContent("markdownTemplate.md", model);

            await _messageService.SendAsync(
                new Uri($"https://oapi.dingtalk.com/robot/send?access_token={accessToken}"),
                new DingTalkRequestMessage
                {
                    Data = new MarkdownDingTalkMessage($"[{_environment.EnvironmentName}]ExceptionLess有新事件", content)
                });
        }

        #endregion Implementation of IWebHookProvider
    }
}