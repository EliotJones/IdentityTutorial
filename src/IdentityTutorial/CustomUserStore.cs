namespace IdentityTutorial
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Microsoft.AspNet.Identity;
    using Store;

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

        public Task<string> GetUserIdAsync(CustomUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNull(user, nameof(user));

            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(CustomUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNull(user, nameof(user));

            return Task.FromResult(user.EmailAddress);
        }

        public Task SetUserNameAsync(CustomUser user, string userName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNull(user, nameof(user));
            Guard.ArgumentNotNullOrEmpty(userName, nameof(userName));

            user.UpdateEmail(userName);

            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(CustomUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNull(user, nameof(user));

            return Task.FromResult(user.EmailAddress.ToLowerInvariant());
        }

        public Task SetNormalizedUserNameAsync(CustomUser user, string normalizedName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            Guard.ArgumentNotNull(user, nameof(user));
            Guard.ArgumentNotNullOrEmpty(normalizedName, nameof(normalizedName));

            //TODO: do we need this?

            return Task.CompletedTask;
        }

        public Task<IdentityResult> CreateAsync(CustomUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNull(user, nameof(user));

            context.AddUser(user);

            return IdentityResultTask;
        }

        public Task<IdentityResult> UpdateAsync(CustomUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNull(user, nameof(user));

            context.UpdateUser(user);

            return IdentityResultTask;
        }

        public Task<IdentityResult> DeleteAsync(CustomUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNull(user, nameof(user));

            context.DeleteUser(user.Id);
            
            return IdentityResultTask;
        }

        public Task<CustomUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNullOrEmpty(userId, nameof(userId));

            return Task.FromResult(context.GetUserById(new Guid(userId)));
        }

        public Task<CustomUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guard.ArgumentNotNullOrEmpty(normalizedUserName, nameof(normalizedUserName));

            return Task.FromResult(context.GetUserByEmail(normalizedUserName));
        }
    }
}
