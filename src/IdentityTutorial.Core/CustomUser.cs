namespace IdentityTutorial.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CustomUser
    {
        private DateTime passwordLastUpdated;

        public Guid Id { get; private set; }

        public string UserName { get; private set; }

        public string EmailAddress { get; private set; }

        public string PasswordHash { get; private set; }

        private IList<CustomLogin> customLogins;

        public IEnumerable<CustomLogin> CustomLogins => customLogins.AsEnumerable().Skip(0);

        public bool IsExternalUser { get; private set; }

        public CustomUser(Guid id, string emailAddress, string userName, string passwordHash)
        {
            Id = id;
            EmailAddress = emailAddress;
            UserName = userName;
            PasswordHash = passwordHash;
            passwordLastUpdated = DateTime.UtcNow;
            customLogins = new List<CustomLogin>();
        }

        public CustomUser(string emailAddress)
        {
            Id = Guid.NewGuid();
            EmailAddress = emailAddress;
            UserName = emailAddress;
            customLogins = new List<CustomLogin>();
        }

        private CustomUser(bool isExternalUser, CustomLogin customLogin, string userName)
        {
            Id = Guid.NewGuid();
            customLogins = new List<CustomLogin> { customLogin };
            IsExternalUser = isExternalUser;
            UserName = userName;
        }

        public void UpdateName(string userName)
        {
            Guard.ArgumentNotNull(userName, nameof(userName));

            UserName = userName;
        }

        public void UpdateEmail(string emailAddress)
        {
            if (!IsExternalUser)
            {
                Guard.ArgumentNotNullOrEmpty(emailAddress, nameof(emailAddress));
                UserName = emailAddress;
            }

            EmailAddress = emailAddress;
        }

        public void UpdatePasswordHash(string passwordHash)
        {
            if (!IsExternalUser)
            {
                Guard.ArgumentNotNullOrEmpty(passwordHash, nameof(passwordHash));
            }

            PasswordHash = passwordHash;

            passwordLastUpdated = DateTime.UtcNow;
        }

        public void AddLogin(CustomLogin login)
        {
            login.UserId = this.Id;
            customLogins.Add(login);
        }

        public void RemoveLogin(string provider, string key)
        {
            var index = customLogins.IndexOf(
                customLogins.Single(cl => cl.ProviderDisplayName.Equals(provider, StringComparison.OrdinalIgnoreCase)
                                          && cl.ProviderKey.Equals(key, StringComparison.OrdinalIgnoreCase)));

            customLogins.RemoveAt(index);
        }

        public static CustomUser CreateFromExternalSource(CustomLogin customLogin, string userName)
        {
            return new CustomUser(true, customLogin, userName);
        }
    }
}