using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using AdventCalendar.Services;
using Microsoft.Extensions.Configuration;
using AdventCalendar.Settings;
using AdventCalendar.Models;

namespace AdventCalendar
{
    internal static class Program
    {
        private static async System.Threading.Tasks.Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.Services.AddScoped(sp => new System.Net.Http.HttpClient { BaseAddress = new System.Uri(builder.HostEnvironment.BaseAddress) });
            builder.RootComponents.Add<App>("#app");
            ConfigureServices(builder.Services);
            await builder.Build().RunAsync().ConfigureAwait(true); //Blazor has SynchronizationContext
        }
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAppSettings>(
                service => new AppSettings(service.GetService<IConfiguration>())
            );
            services.AddSingleton<DayReader>();
            services.AddSingleton<ArchiveReader>();
            services.AddSingleton<DaysShuffler>();

            services.AddTransient<Validator>();
            services.AddTransient<IDataReceiver, DataReceiver>(sp => new DataReceiver(sp.GetService<IAppSettings>(), sp.GetService<Microsoft.AspNetCore.Components.NavigationManager>()));

            services.AddTransient<IDateTime, DefaultDateTime>();
        }
    }
}
