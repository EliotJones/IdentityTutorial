namespace IdentityTutorial
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Microsoft.AspNet.Identity;

    public partial class CustomUserStore : IUserEmailStore<CustomUser>
    {
        public Task SetEmailAsync(CustomUser user, string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNull(user, nameof(user));
            if (!user.IsExternalUser)
            {
                Guard.ArgumentNotNullOrEmpty(email, nameof(email));
            }

            user.UpdateEmail(email);

            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync(CustomUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNull(user, nameof(user));

            return Task.FromResult(user.EmailAddress);
        }

        public Task<bool> GetEmailConfirmedAsync(CustomUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNull(user, nameof(user));

            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(CustomUser user, bool confirmed, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNull(user, nameof(user));

            //TODO: Add an email confirmed property.

            return Task.CompletedTask;
        }

        public Task<CustomUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNullOrEmpty(normalizedEmail, nameof(normalizedEmail));

            return Task.FromResult(context.GetUserByEmail(normalizedEmail));
        }

        public Task<string> GetNormalizedEmailAsync(CustomUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNull(user, nameof(user));

            return Task.FromResult(user.EmailAddress);
        }

        public Task SetNormalizedEmailAsync(CustomUser user, string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            return SetEmailAsync(user, normalizedEmail, cancellationToken);
        }
    }
}