using WebApi.Application.ApplicationUsers;
using WebApi.Application.Localization;

namespace WebApi.Implementation.Localization
{
    public class JsonTranslator : ITranslator
    {
        private readonly IApplicationUser _user;

        public JsonTranslator(
            IApplicationUser user
        )
        {
            _user = user;
        }

        public string Translate(string key)
        {
            return key;
        }
    }
}
