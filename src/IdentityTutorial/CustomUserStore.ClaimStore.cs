namespace IdentityTutorial
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using JetBrains.Annotations;
    using Microsoft.AspNet.Identity;

    public partial class CustomUserStore : IUserClaimStore<CustomUser>
    {
        public Task<IList<Claim>> GetClaimsAsync([NotNull]CustomUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.Claims.Select(c => new Claim(c.Type, c.Value)).ToArray() as IList<Claim>);
        }

        public Task AddClaimsAsync([NotNull]CustomUser user, [NotNull]IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (var claim in claims)
            {
                user.AddClaim(new CustomUserClaim(claim.Type, claim.Value));
            }

            return Task.CompletedTask;
        }

        public Task ReplaceClaimAsync([NotNull]CustomUser user, [NotNull]Claim claim, [NotNull]Claim newClaim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.RemoveClaim(new CustomUserClaim(claim.Type, claim.Value));
            user.AddClaim(new CustomUserClaim(newClaim.Type, newClaim.Value));

            return Task.CompletedTask;
        }

        public Task RemoveClaimsAsync([NotNull]CustomUser user, [NotNull]IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (var claim in claims)
            {
                user.RemoveClaim(new CustomUserClaim(claim.Type, claim.Value));
            }

            return Task.CompletedTask;
        }

        public Task<IList<CustomUser>> GetUsersForClaimAsync([NotNull]Claim claim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(context.GetUsersWithClaim(claim.Type, claim.Value));
        }
    }
}