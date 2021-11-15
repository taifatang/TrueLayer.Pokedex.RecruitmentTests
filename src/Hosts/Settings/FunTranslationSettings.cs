using System;
using System.Collections.Generic;

namespace Hosts.Settings
{
    public class FunTranslationSettings
    {
        public static string Position => "FunTranslation";
        public Uri BaseUrl { get; set; }
        public IEnumerable<string> SupportedTranslation { get; set; }
    }
}
