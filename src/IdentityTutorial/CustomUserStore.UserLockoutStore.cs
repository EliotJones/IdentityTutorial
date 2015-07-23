namespace IdentityTutorial
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using JetBrains.Annotations;
    using Microsoft.AspNet.Identity;

    public partial class CustomUserStore : IUserLockoutStore<CustomUser>
    {
        public Task<int> GetAccessFailedCountAsync([NotNull]CustomUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync([NotNull]CustomUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync([NotNull]CustomUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult((DateTimeOffset?)user.LockoutEndDate);
        }

        public Task<int> IncrementAccessFailedCountAsync([NotNull]CustomUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            user.SetAccessFailedCount(user.AccessFailedCount + 1);

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync([NotNull]CustomUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.SetAccessFailedCount(0);

            return Task.CompletedTask;
        }

        public Task SetLockoutEnabledAsync([NotNull]CustomUser user, bool enabled, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.SetLockoutEnabled(enabled);

            return Task.CompletedTask;
        }

        public Task SetLockoutEndDateAsync([NotNull]CustomUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.SetLockoutEndDate(lockoutEnd);

            return Task.CompletedTask;
        }
    }
}