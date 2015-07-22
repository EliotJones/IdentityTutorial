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

        public int AccessFailedCount { get; private set; }

        public DateTimeOffset? LockoutEndDate { get; private set; }

        public bool LockoutEnabled { get; private set; }

        private IList<CustomLogin> customLogins;

        public IEnumerable<CustomLogin> CustomLogins => customLogins.AsEnumerable().Skip(0);

        public bool IsExternalUser { get; private set; }

        public CustomUser(Guid id, string emailAddress, string userName, string passwordHash, int accessFailedCount, DateTimeOffset? lockoutEndDate, bool lockoutEnabled)
        {
            Id = id;
            EmailAddress = emailAddress;
            UserName = userName;
            PasswordHash = passwordHash;
            AccessFailedCount = accessFailedCount;
            LockoutEndDate = lockoutEndDate;
            LockoutEnabled = lockoutEnabled;
            passwordLastUpdated = DateTime.UtcNow;
            customLogins = new List<CustomLogin>();
        }

        public CustomUser(string emailAddress)
        {
            Id = Guid.NewGuid();
            EmailAddress = emailAddress;
            LockoutEnabled = true;
            UserName = emailAddress;
            customLogins = new List<CustomLogin>();
        }

        private CustomUser(bool isExternalUser, CustomLogin customLogin, string userName)
        {
            Id = Guid.NewGuid();
            customLogins = new List<CustomLogin> { customLogin };
            IsExternalUser = isExternalUser;
            UserName = userName;
            LockoutEnabled = true;
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

        public void SetAccessFailedCount(int afc)
        {
            if (afc < 0)
            {
                throw new InvalidOperationException("Cannot set a negative access failed count.");
            }

            AccessFailedCount = afc;
        }

        public void SetLockoutEnabled(bool value)
        {
            LockoutEnabled = value;
        }

        public void SetLockoutEndDate(DateTimeOffset? value)
        {
            if (value.HasValue)
            {
                LockoutEndDate = value.Value;
            }
        }

        public void SetLockoutEndDate(DateTime? value)
        {
            LockoutEndDate = value;
        }

        public static CustomUser CreateFromExternalSource(CustomLogin customLogin, string userName)
        {
            return new CustomUser(true, customLogin, userName);
        }
    }
}