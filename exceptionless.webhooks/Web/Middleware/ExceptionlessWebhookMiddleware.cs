﻿using ExceptionLess.WebHook.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Middleware
{
    public class ExceptionLessWebhookMiddleware
    {
        #region Field

        private readonly RequestDelegate _next;
        private readonly IEnumerable<IWebHookProvider> _webHookProviders;
        private readonly ILogger<ExceptionLessWebhookMiddleware> _logger;

        #endregion Field

        #region Constructor

        public ExceptionLessWebhookMiddleware(RequestDelegate next, IEnumerable<IWebHookProvider> webHookProviders, ILogger<ExceptionLessWebhookMiddleware> logger)
        {
            _next = next;
            _webHookProviders = webHookProviders;
            _logger = logger;

            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug($"全部处理程序数量：{_webHookProviders.Count()}");
        }

        #endregion Constructor

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;

            if (request.Method != HttpMethods.Post)
            {
                await _next(context);
                return;
            }

            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("接收到 Webhook 请求。");

            var content = string.Empty;
            try
            {
                using (var reader = new StreamReader(request.Body))
                    content = await reader.ReadToEndAsync();

                if (_logger.IsEnabled(LogLevel.Debug))
                    _logger.LogDebug($"接收到 Webhook 请求：{content}");
               
                var model = JsonConvert.DeserializeObject<ExceptionLessEventModel>(content);
                var providers = GetProviders(request.Query);

                var parameters = new Dictionary<string, string>(request.Query.ToDictionary(i => i.Key, i => i.Value.ToString()), StringComparer.OrdinalIgnoreCase);
                foreach (var provider in providers)
                {
                    try
                    {
                        _logger.LogDebug($"使用{provider.Name}处理中");
                        await provider.ProcessAsync(model, parameters);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(0, e, $"处理 {provider.Name} Webhook 时发生了异常，内容：{content}。");
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, $"处理 Webhook 失败，内容：{content}。");
                throw;
            }
        }

        #region Private Method

        private IEnumerable<IWebHookProvider> GetProviders(IQueryCollection query)
        {
            var providers = _webHookProviders;
            if (!query.TryGetValue("webhooks", out var webhooks))
                return providers;

            var useHooks = webhooks.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            providers = _webHookProviders.Where(
                i => useHooks.Any(z => string.Equals(z, i.Name, StringComparison.Ordinal)));
            return providers;
        }

        #endregion Private Method
    }
}