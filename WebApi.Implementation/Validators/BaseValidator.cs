using FluentValidation;
using WebApi.Application.Localization;

namespace WebApi.Implementation.Validators
{
    public abstract class BaseValidator<TObject> : AbstractValidator<TObject>
    {
        private readonly ITranslator _translator;

        public BaseValidator(ITranslator translator)
        {
            _translator = translator;

            ClassLevelCascadeMode = CascadeMode.Stop;
        }

        protected string T(string messageKey)
        {
            return _translator.Translate(messageKey);
        }

        protected string TF(string messageKey, params object[] values)
        {
            var translatedMessage = T(messageKey);
            var formattedMessage = string.Format(translatedMessage, values);

            return formattedMessage;
        }

        protected string IsRequired()
        {
            return T("isRequired");
        }

        protected string MinLength(int value)
        {
            return TF("minimumLength", value);
        }

        protected string MaxLength(int value)
        {
            return TF("maximumLength", value);
        }
    }
}
