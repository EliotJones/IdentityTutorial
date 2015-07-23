namespace IdentityTutorial
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Data;
    using JetBrains.Annotations;
    using Microsoft.AspNet.Identity;

    public partial class CustomUserStore : IUserStore<CustomUser>
    {
        private static readonly Task<IdentityResult> IdentityResultTask = Task.FromResult(IdentityResult.Success);
        private readonly Context context;

        public CustomUserStore(Context context)
        {
            this.context = context;
        }

        public void Dispose()
        {
        }

        public Task<string> GetUserIdAsync([NotNull]CustomUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync([NotNull]CustomUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync([NotNull]CustomUser user, [NotNull]string userName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNullOrEmpty(userName, nameof(userName));

            user.UpdateName(userName);

            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync([NotNull]CustomUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.UserName.ToUpperInvariant());
        }

        public Task SetNormalizedUserNameAsync([NotNull]CustomUser user, [NotNull]string normalizedName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            Guard.ArgumentNotNull(normalizedName, nameof(normalizedName));

            //TODO: do we need this?

            return Task.CompletedTask;
        }

        public Task<IdentityResult> CreateAsync([NotNull]CustomUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            context.AddUser(user);

            return IdentityResultTask;
        }

        public Task<IdentityResult> UpdateAsync([NotNull]CustomUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            context.UpdateUser(user);

            return IdentityResultTask;
        }

        public Task<IdentityResult> DeleteAsync([NotNull]CustomUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            context.DeleteUser(user.Id);

            return IdentityResultTask;
        }

        public Task<CustomUser> FindByIdAsync([NotNull]string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNullOrEmpty(userId, nameof(userId));

            return Task.FromResult(context.GetUserById(new Guid(userId)));
        }

        public Task<CustomUser> FindByNameAsync([NotNull]string normalizedUserName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNullOrEmpty(normalizedUserName, nameof(normalizedUserName));

            return Task.FromResult(context.GetUserByName(normalizedUserName));
        }
    }
}
