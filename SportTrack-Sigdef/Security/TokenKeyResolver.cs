using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace SportTrack_Sigdef.Security;

public static class TokenKeyResolver
{
    public const string DevelopmentFallback =
        "SportTrackSuperSecretKey2026!ForEducationalPurposeOnly_LongEnoughToBeSecure";

    /// <summary>
    /// En Development permite fallback; en el resto exige TokenKey en config/env.
    /// </summary>
    public static string Resolve(IConfiguration config, IHostEnvironment env)
    {
        var key = config["TokenKey"];
        if (!string.IsNullOrWhiteSpace(key))
            return key.Trim();

        if (env.IsDevelopment())
        {
            Console.WriteLine("⚠️ TokenKey no configurado: usando fallback solo para Development.");
            return DevelopmentFallback;
        }

        throw new InvalidOperationException(
            "TokenKey no configurado. Definí la variable de entorno TokenKey en Render (o appsettings) antes de iniciar en producción.");
    }
}
