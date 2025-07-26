using WebApi.Application.Localization;

namespace WebApi.Implementation.Localization
{
    public class JsonTranslator : ITranslator
    {
        public string Translate(string key)
        {
            return key;
        }
    }
}
