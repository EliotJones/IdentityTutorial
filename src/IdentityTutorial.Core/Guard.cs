namespace IdentityTutorial.Core
{
    using System;

    public class Guard
    {
        public static void ArgumentNotNull(object argument, string name = "unknown")
        {
            if (argument == null)
            {
                throw new ArgumentNullException($"{name} was null");
            }
        }

        public static void ArgumentNotNullOrEmpty(string argument, string name = "unknown")
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ArgumentException($"{name} was null or empty");
            }
        }
    }
}
