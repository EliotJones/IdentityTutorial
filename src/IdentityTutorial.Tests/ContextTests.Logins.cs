namespace XunitTests
{
    using System;
    using System.Linq;
    using IdentityTutorial.Core;
    using Xunit;

    public partial class ContextTests
    {
        [Fact]
        public void GetUser_EmptyLogins_ReturnsEmptyList()
        {
            var result = context.GetUserById(new Guid(Constants.FirstUserId));

            Assert.Empty(result.CustomLogins);
        }

        [Fact]
        public void GetUser_HasLogin_ReturnsCorrectLogin()
        {
            var result = context.GetUserById(new Guid(Constants.SecondUserId));

            Assert.Equal(1, result.CustomLogins.Count());
            Assert.Equal(Constants.SecondUserLoginKey, result.CustomLogins.Single().ProviderKey);
            Assert.Equal(new Guid(Constants.SecondUserId), result.CustomLogins.Single().UserId);
        }

        [Fact]
        public void AddLogin_UserWithEmptyLogins_SavesToContext()
        {
            var user = context.GetUserById(new Guid(Constants.FirstUserId));
                
            user.AddLogin(new CustomLogin
            {
                LoginProvider = "Toast",
                ProviderDisplayName = "Butter",
                ProviderKey = "Jam"
            });

            context.UpdateUser(user);

            Assert.Equal(1, context.GetUserById(new Guid(Constants.FirstUserId)).CustomLogins.Count());
        }

        [Fact]
        public void RemoveLogin_UserHasLogin_RemovesLogin()
        {
            var user = context.GetUserById(new Guid(Constants.SecondUserId));

            user.RemoveLogin(Constants.SecondUserLoginProvider, Constants.SecondUserLoginKey);

            context.UpdateUser(user);

            Assert.Empty(context.GetUserById(new Guid(Constants.SecondUserId)).CustomLogins);
        }
    }
}