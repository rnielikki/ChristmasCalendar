using Blazored.Modal;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using joulukalenteri.Client.SharedCode;
using System.Net.Http;
using joulukalenteri.Shared;

namespace joulukalenteri.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBlazoredModal();
            services.AddSingleton<DayReader>();
            services.AddSingleton<ArchiveReader>();
            services.AddSingleton<DaysShuffler>();

            services.AddTransient<Validator>();
            services.AddTransient<HttpClient>();
            services.AddTransient<IDataReceiver, DataReceiver>();

            services.AddTransient<IDateTime, DefaultDateTime>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
