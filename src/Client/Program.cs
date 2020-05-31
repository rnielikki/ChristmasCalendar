using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.Modal;
using Microsoft.Extensions.DependencyInjection;
using joulukalenteri.Client.SharedCode;
//using System.Net.Http;
using joulukalenteri.Shared;

namespace joulukalenteri.Client
{
    public static class Program
    {
        public static async System.Threading.Tasks.Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            ConfigureServices(builder.Services);
            await builder.Build().RunAsync().ConfigureAwait(true); //Blazor has SynchronizationContext
        }
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddBlazoredModal();
            services.AddSingleton<DayReader>();
            services.AddSingleton<ArchiveReader>();
            services.AddSingleton<DaysShuffler>();

            services.AddTransient<Validator>();
            //services.AddTransient<HttpClient>();
            services.AddTransient<IDataReceiver, DataReceiver>();

            services.AddTransient<IDateTime, DefaultDateTime>();
        }
    }
}
