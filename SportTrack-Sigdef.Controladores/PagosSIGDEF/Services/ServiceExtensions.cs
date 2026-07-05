using Microsoft.Extensions.DependencyInjection;
using SportTrack_Sigdef.Controladores.PagosSIGDEF;
using SportTrack_Sigdef.Controladores.PagosSIGDEF.Services;

namespace SportTrack_Sigdef.Controladores.PagosSIGDEF.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddMercadoPagoServices(this IServiceCollection services)
        {
            services.AddScoped<MercadoPagoService>();
            services.AddScoped<PaymentService>();
            return services;
        }
    }
}

