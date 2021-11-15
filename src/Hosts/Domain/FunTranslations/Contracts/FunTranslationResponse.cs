namespace Hosts.Domain.FunTranslations.Contracts
{
    public class FunTranslationResponse
    {
        public Contents Contents { get; set; }
    }

    public class Contents
    {
        public string Translated { get; set; }
    }
}
