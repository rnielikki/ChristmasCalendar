using Blazored.Modal;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using joulukalenteri.Client.SharedCode;

namespace joulukalenteri.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBlazoredModal();
            services.AddSingleton<Validator>();
            services.AddSingleton<DayReader>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
