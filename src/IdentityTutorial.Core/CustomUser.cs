namespace IdentityTutorial.Core
{
    using System;

    public class CustomUser
    {
        private DateTime passwordLastUpdated;

        public Guid Id { get; private set; }
        
        public string EmailAddress { get; private set; }

        public string PasswordHash { get; private set; }

        public CustomUser(Guid id, string emailAddress, string passwordHash)
        {
            Id = id;
            EmailAddress = emailAddress;
            PasswordHash = passwordHash;
            passwordLastUpdated = DateTime.UtcNow;
        }

        public CustomUser(string emailAddress)
        {
            Id = Guid.NewGuid();
            EmailAddress = emailAddress;
        }

        public void UpdateEmail(string emailAddress)
        {
            Guard.ArgumentNotNullOrEmpty(emailAddress, nameof(emailAddress));

            EmailAddress = emailAddress;
        }

        public void UpdatePasswordHash(string passwordHash)
        {
            Guard.ArgumentNotNullOrEmpty(passwordHash, nameof(passwordHash));

            PasswordHash = passwordHash;

            passwordLastUpdated = DateTime.UtcNow;
        }
    }
}