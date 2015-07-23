namespace IdentityTutorial.Core
{
    using JetBrains.Annotations;

    public class CustomUserClaim
    {
        public string Type { get; private set; }

        public string Value { get; private set; }

        public CustomUserClaim([NotNull]string type, [NotNull]string value)
        {
            Type = type;
            Value = value;
        }
    }
}
