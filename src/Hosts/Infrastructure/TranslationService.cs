using System.Threading.Tasks;
using Hosts.Domain.FunTranslations;

namespace Hosts.Infrastructure
{
    public class TranslationService
    {
        private readonly IFunTranslationsHttpClient _translationsHttpClient;

        public TranslationService(IFunTranslationsHttpClient translationsHttpClient)
        {
            _translationsHttpClient = translationsHttpClient;
        }

        public async Task<string> Translate(string text, string language)
        {
            var translatedText = await SafeTranslate(text, language);

            return !string.IsNullOrWhiteSpace(translatedText) ? translatedText : text;
        }

        private async Task<string> SafeTranslate(string text, string language)
        {
            try
            {
                return await _translationsHttpClient.Translate(text, language);
            }
            catch
            {
                // ignored
            }

            return null;
        }
    }
}
