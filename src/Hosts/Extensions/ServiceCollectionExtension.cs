using Hosts.Domain.FunTranslations;
using Hosts.Domain.PokeApi;
using Hosts.Infrastructure;
using Hosts.Infrastructure.FunTranslations;
using Hosts.Infrastructure.PokeApi;
using Hosts.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Hosts.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PokeApiSettings>(configuration.GetSection(PokeApiSettings.Position));
            services.Configure<FunTranslationSettings>(configuration.GetSection(FunTranslationSettings.Position));

            return services;
        }

        public static IServiceCollection RegisterHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<IPokeApiHttpClient, PokeApiHttpClient>((provider, client) =>
            {
                client.BaseAddress = provider.GetRequiredService<IOptions<PokeApiSettings>>().Value.BaseUrl;
            });

            services.AddHttpClient<IFunTranslationsHttpClient, FunTranslationsHttpClient>((provider, client) =>
            {
                client.BaseAddress = provider.GetRequiredService<IOptions<FunTranslationSettings>>().Value.BaseUrl;
            });

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<PokemonService>();
            services.AddTransient<TranslationService>();

            return services;
        }
    }
}
