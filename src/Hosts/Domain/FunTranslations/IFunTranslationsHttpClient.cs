using System.Threading.Tasks;

namespace Hosts.Domain.FunTranslations
{
    public interface IFunTranslationsHttpClient
    {
        Task<string> Translate(string text, string language);
    }
}
