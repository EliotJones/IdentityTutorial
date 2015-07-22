namespace IdentityTutorial
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Microsoft.AspNet.Identity;

    public partial class CustomUserStore : IUserLockoutStore<CustomUser>
    {
        public Task<int> GetAccessFailedCountAsync(CustomUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNull(user, nameof(user));

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(CustomUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNull(user, nameof(user));

            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(CustomUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNull(user, nameof(user));

            return Task.FromResult((DateTimeOffset?)user.LockoutEndDate);
        }

        public Task<int> IncrementAccessFailedCountAsync(CustomUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNull(user, nameof(user));
            
            user.SetAccessFailedCount(user.AccessFailedCount + 1);

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(CustomUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNull(user, nameof(user));

            user.SetAccessFailedCount(0);

            return Task.CompletedTask;
        }

        public Task SetLockoutEnabledAsync(CustomUser user, bool enabled, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNull(user, nameof(user));

            user.SetLockoutEnabled(enabled);

            return Task.CompletedTask;
        }

        public Task SetLockoutEndDateAsync(CustomUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNull(user, nameof(user));

            user.SetLockoutEndDate(lockoutEnd);

            return Task.CompletedTask;
        }
    }
}