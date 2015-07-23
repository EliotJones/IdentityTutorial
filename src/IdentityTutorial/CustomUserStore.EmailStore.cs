namespace IdentityTutorial
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using JetBrains.Annotations;
    using Microsoft.AspNet.Identity;

    public partial class CustomUserStore : IUserEmailStore<CustomUser>
    {
        public Task SetEmailAsync([NotNull]CustomUser user, string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!user.IsExternalUser)
            {
                Guard.ArgumentNotNullOrEmpty(email, nameof(email));
            }

            user.UpdateEmail(email);

            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync([NotNull]CustomUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.EmailAddress);
        }

        public Task<bool> GetEmailConfirmedAsync([NotNull]CustomUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync([NotNull]CustomUser user, bool confirmed, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            //TODO: Add an email confirmed property.

            return Task.CompletedTask;
        }

        public Task<CustomUser> FindByEmailAsync([NotNull]string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(context.GetUserByEmail(normalizedEmail));
        }

        public Task<string> GetNormalizedEmailAsync([NotNull]CustomUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.EmailAddress);
        }

        public Task SetNormalizedEmailAsync(CustomUser user, string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
    }
}