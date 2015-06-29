namespace XunitTests
{
    using System;
    using System.Linq;
    using IdentityTutorial.Core;
    using IdentityTutorial.Store.Mappings;
    using Xunit;

    public class XmlUserMapTests
    {
        private readonly XmlUserMap xmlUserMap = new XmlUserMap();
        private readonly CustomUser customUser = new CustomUser(new Guid("B981CB10-ACF5-4E71-BB89-B1DACA51A825"), "test@test.com", string.Empty);

        [Fact]
        public void ToXml_ReturnsExpectedXml()
        {
            var result = xmlUserMap.Map(customUser);

            Assert.True(result.ToString().StartsWith("<user>"));
            Assert.Equal(customUser.Id.ToString(), result.Descendants("id").Single().Value);
        }
    }
}