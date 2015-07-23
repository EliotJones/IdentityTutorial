namespace IdentityTutorial
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using JetBrains.Annotations;
    using Microsoft.AspNet.Identity;

    public partial class CustomUserStore : IUserLoginStore<CustomUser>
    {
        public Task AddLoginAsync([NotNull]CustomUser user, [NotNull]UserLoginInfo login, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            user.AddLogin(new CustomLogin
            {
                ProviderKey = login.ProviderKey,
                LoginProvider = login.LoginProvider,
                ProviderDisplayName = login.ProviderDisplayName
            });

            return Task.CompletedTask;
        }

        public Task RemoveLoginAsync([NotNull]CustomUser user, [NotNull]string loginProvider, [NotNull]string providerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNullOrEmpty(loginProvider, nameof(loginProvider));
            Guard.ArgumentNotNullOrEmpty(providerKey, nameof(providerKey));

            user.RemoveLogin(loginProvider, providerKey);

            return Task.CompletedTask;
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync([NotNull]CustomUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.CustomLogins.Select(cl => new UserLoginInfo(cl.LoginProvider, cl.ProviderKey, cl.ProviderDisplayName)).ToArray() as IList<UserLoginInfo>);
        }

        public Task<CustomUser> FindByLoginAsync([NotNull]string loginProvider, [NotNull]string providerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNullOrEmpty(loginProvider, nameof(loginProvider));
            Guard.ArgumentNotNullOrEmpty(providerKey, nameof(providerKey));

            var user = context.GetUsers()
                .SingleOrDefault(
                    u =>
                        u.CustomLogins.Any(
                            cl => cl.ProviderKey.Equals(providerKey, StringComparison.OrdinalIgnoreCase) &&
                                  cl.LoginProvider.Equals(loginProvider, StringComparison.OrdinalIgnoreCase)));

            return
                Task.FromResult(user);
        }
    }
}