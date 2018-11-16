using Rabbit.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExceptionLess.WebHook.Abstractions
{
    public interface IWebHookProvider : ISingletonDependency
    {
        string Name { get; }

        Task ProcessAsync(ExceptionLessEventModel model, IDictionary<string, string> parameters);
    }
}