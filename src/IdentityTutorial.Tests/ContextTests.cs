namespace XunitTests
{
    using System;
    using System.Linq;
    using IdentityTutorial.Core;
    using IdentityTutorial.Data;
    using Xunit;

    public partial class ContextTests
    {
        private const string AnyString = "test";
        private readonly Context context;

        public ContextTests()
        {
            Context.Reset();
            this.context = Context.Get(new TestFileReader());
        }

        [Fact]
        public void GetUsers_ReturnsExpectedUsers()
        {
            var result = context.GetUsers();

            Assert.Equal(2, result.Count);
            Assert.Equal(Constants.FirstUserEmail, result[0].EmailAddress);
        }

        [Fact]
        public void GetUserById_ReturnsExpectedUser()
        {
            var result = context.GetUserById(new Guid(Constants.FirstUserId));

            Assert.Equal(Constants.FirstUserEmail, result.EmailAddress);
        }

        [Fact]
        public void GetUserById_ReturnsNull()
        {
            var result = context.GetUserById(new Guid("3AC3E84C-F9D0-4CDB-A0B8-FCC79D69A65F"));

            Assert.Null(result);
        }

        [Fact]
        public void GetUserByEmail_ReturnsNull()
        {
            var result = context.GetUserByEmail(Constants.FirstUserEmail);

            Assert.Equal(Constants.FirstUserEmail, result.EmailAddress);
            Assert.Equal(new Guid(Constants.FirstUserId), result.Id);
        }

        [Fact]
        public void AddUser_Succeeds()
        {
            var userId = new Guid("56DDA58C-1ACC-4ABF-A3DD-55EDB262159C");

            context.AddUser(new CustomUser(userId, "test3@test.com", AnyString,
                string.Empty, 0, null, true));

            Assert.Contains(userId, context.GetUsers().Select(u => u.Id));
        }

        [Fact]
        public void AddUser_DuplicateUser_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => context.AddUser(new CustomUser(new Guid(Constants.FirstUserId), Constants.SecondUserEmail, AnyString, string.Empty, 0, null, false)));
        }

        [Fact]
        public void AddUser_NullUser_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => context.AddUser(null));
        }

        [Fact]
        public void UpdateUser_NullUser_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => context.UpdateUser(null));
        }

        [Fact]
        public void EditUser_MissingUser_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => context.UpdateUser(new CustomUser(new Guid("5EE37BB8-6381-47E5-A6D2-4B6D035B1D47"), Constants.FirstUserEmail, AnyString, string.Empty, 0, null, true)));
        }

        [Fact]
        public void UpdateUser_ValidEdit_Succeeds()
        {
            var email = "email@email.com";

            context.UpdateUser(new CustomUser(new Guid(Constants.FirstUserId), email, AnyString, string.Empty, 0, null, true));

            Assert.Contains(email, context.GetUsers().Select(u => u.EmailAddress));
        }

        [Fact]
        public void DeleteUser_ValidUser_Succeeds()
        {
            context.DeleteUser(new Guid(Constants.FirstUserId));

            Assert.Null(context.GetUserById(new Guid(Constants.FirstUserId)));
        }

        [Fact]
        public void DeleteUsers_Both_DeletesBoth()
        {
            context.DeleteUser(new Guid(Constants.SecondUserId));
            context.DeleteUser(new Guid(Constants.FirstUserId));

            Assert.Empty(context.GetUsers());
        }

        [Fact]
        public void DeleteUser_Twice_Deletes()
        {
            context.DeleteUser(new Guid(Constants.SecondUserId));
            context.DeleteUser(new Guid(Constants.SecondUserId));

            Assert.Null(context.GetUserById(new Guid(Constants.SecondUserId)));
        }

        [Fact]
        public void Delete_ThenUpdate_Throws()
        {
            context.DeleteUser(new Guid(Constants.SecondUserId));

            Assert.Throws<InvalidOperationException>(
                () => context.UpdateUser(new CustomUser(new Guid(Constants.SecondUserId), "any@email.com", AnyString, string.Empty, 0, null, true)));
        }

        [Fact]
        public void Update_ThenGetByEmail_Returns()
        {
            var email = "any@b.com";

            context.UpdateUser(new CustomUser(new Guid(Constants.SecondUserId), email, AnyString, string.Empty, 0, null, true));

            Assert.Equal(new Guid(Constants.SecondUserId), context.GetUserByEmail(email).Id);
        }

        [Fact]
        public void GetUserWithClaims_ReturnsExpectedNumberOfClaims()
        {
            var user = context.GetUserById(new Guid(Constants.FirstUserId));

            Assert.Equal(2, user.Claims.Count());
        }

        [Fact]
        public void GetUserWithClaims_ReturnsExpectedClaimTypes()
        {
            var user = context.GetUserById(new Guid(Constants.FirstUserId));
            var expectedClaimTypes = new[] {Constants.FirstUserFirstClaimType, Constants.FirstUserSecondClaimType};

            Assert.Equal(expectedClaimTypes, user.Claims.Select(c => c.Type));
        }

        [Fact]
        public void GetUserWithClaims_ReturnsExpectedClaimValues()
        {
            var user = context.GetUserById(new Guid(Constants.FirstUserId));
            var expectedClaimValues = new[] { Constants.FirstUserFirstClaimValue, Constants.FirstUserSecondClaimValue };

            Assert.Equal(expectedClaimValues, user.Claims.Select(c => c.Value));
        }

        [Fact]
        public void GetUserWithNoClaims_ReturnsEmptyList()
        {
            var user = context.GetUserById(new Guid(Constants.SecondUserId));

            Assert.NotNull(user.Claims);
            Assert.Empty(user.Claims);
        }

        [Fact]
        public void GetUsersWithClaim_ReturnsResult()
        {
            var result = context.GetUsersWithClaim(Constants.FirstUserFirstClaimType, Constants.FirstUserFirstClaimValue);

            Assert.Equal(1, result.Count);
        }
    }
}