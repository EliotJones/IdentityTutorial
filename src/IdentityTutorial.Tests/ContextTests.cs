namespace XunitTests
{
    using System;
    using System.Linq;
    using IdentityTutorial.Core;
    using IdentityTutorial.Store;
    using Xunit;

    public class ContextTests
    {
        public static string FirstUserId = "B2019313-BEEB-4344-BEC7-F4E0EEB0CE62";
        public static string FirstUserEmail = "test@test.com";
        public static string FirstUserPassword = "ABCDEFG";
        public static string SecondUserId = "A0010313-BEEB-4344-BEC7-F4E0EEB0CE62";
        public static string SecondUserEmail = "test2@test.com";
        public static string SecondUserPassword = "HASHEDP";
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
            Assert.Equal(FirstUserEmail, result[0].EmailAddress);
        }

        [Fact]
        public void GetUserById_ReturnsExpectedUser()
        {
            var result = context.GetUserById(new Guid(FirstUserId));

            Assert.Equal(FirstUserEmail, result.EmailAddress);
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
            var result = context.GetUserByEmail(FirstUserEmail);

            Assert.Equal(FirstUserEmail, result.EmailAddress);
            Assert.Equal(new Guid(FirstUserId), result.Id);
        }

        [Fact]
        public void AddUser_Succeeds()
        {
            var userId = new Guid("56DDA58C-1ACC-4ABF-A3DD-55EDB262159C");

            context.AddUser(new CustomUser(userId, "test3@test.com",
                string.Empty));

            Assert.Contains(userId, context.GetUsers().Select(u => u.Id));
        }

        [Fact]
        public void AddUser_DuplicateUser_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => context.AddUser(new CustomUser(new Guid(FirstUserId), SecondUserEmail, string.Empty)));
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
            Assert.Throws<InvalidOperationException>(() => context.UpdateUser(new CustomUser(new Guid("5EE37BB8-6381-47E5-A6D2-4B6D035B1D47"), FirstUserEmail, string.Empty)));
        }

        [Fact]
        public void UpdateUser_ValidEdit_Succeeds()
        {
            var email = "email@email.com";

            context.UpdateUser(new CustomUser(new Guid(FirstUserId), email, string.Empty));

            Assert.Contains(email, context.GetUsers().Select(u => u.EmailAddress));
        }

        [Fact]
        public void DeleteUser_ValidUser_Succeeds()
        {
            context.DeleteUser(new Guid(FirstUserId));

            Assert.Null(context.GetUserById(new Guid(FirstUserId)));
        }

        [Fact]
        public void DeleteUsers_Both_DeletesBoth()
        {
            context.DeleteUser(new Guid(SecondUserId));
            context.DeleteUser(new Guid(FirstUserId));

            Assert.Empty(context.GetUsers());
        }

        [Fact]
        public void DeleteUser_Twice_Deletes()
        {
            context.DeleteUser(new Guid(SecondUserId));
            context.DeleteUser(new Guid(SecondUserId));

            Assert.Null(context.GetUserById(new Guid(SecondUserId)));
        }

        [Fact]
        public void Delete_ThenUpdate_Throws()
        {
            context.DeleteUser(new Guid(SecondUserId));

            Assert.Throws<InvalidOperationException>(
                () => context.UpdateUser(new CustomUser(new Guid(SecondUserId), "any@email.com", string.Empty)));
        }

        [Fact]
        public void Update_ThenGetByEmail_Returns()
        {
            var email = "any@b.com";

            context.UpdateUser(new CustomUser(new Guid(SecondUserId), email, string.Empty));

            Assert.Equal(new Guid(SecondUserId), context.GetUserByEmail(email).Id);
        }
    }
}