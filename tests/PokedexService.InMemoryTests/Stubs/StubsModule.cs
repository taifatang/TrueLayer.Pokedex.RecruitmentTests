using Hosts.Domain.FunTranslations;
using Hosts.Domain.PokeApi;

namespace PokedexService.InMemoryTests.Stubs
{
    public class StubsModule
    {
        public readonly PokeApiHttpClientStub PokeApiHttpClientStub;
        public readonly FunTranslationsHttpClientStub FunTranslationsHttpClientStub;

        public StubsModule(IPokeApiHttpClient pokeApiHttpClient, IFunTranslationsHttpClient funTranslationsHttpClient)
        {
            PokeApiHttpClientStub = pokeApiHttpClient as PokeApiHttpClientStub;
            FunTranslationsHttpClientStub = funTranslationsHttpClient as FunTranslationsHttpClientStub;
        }
    }
}
