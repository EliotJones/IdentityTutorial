namespace IdentityTutorial
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Microsoft.AspNet.Identity;

    public partial class CustomUserStore : IUserPasswordStore<CustomUser>
    {
        public Task SetPasswordHashAsync(CustomUser user, string passwordHash, CancellationToken cancellationToken = default(CancellationToken))
        {
			cancellationToken.ThrowIfCancellationRequested();
			Guard.ArgumentNotNull(user, nameof(user));
            if (!user.IsExternalUser)
            {
                Guard.ArgumentNotNullOrEmpty(passwordHash, nameof(passwordHash));
            }

			user.UpdatePasswordHash(passwordHash);

			return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(CustomUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
			cancellationToken.ThrowIfCancellationRequested();
			Guard.ArgumentNotNull(user, nameof(user));

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(CustomUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
			cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(string.IsNullOrWhiteSpace(user.PasswordHash));
        }
    }
}