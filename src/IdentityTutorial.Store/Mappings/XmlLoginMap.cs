namespace IdentityTutorial.Store.Mappings
{
    using System.Xml.Linq;
    using Core;

    public class XmlLoginMap : IMap<XElement, CustomLogin>, IMap<CustomLogin, XElement>
    {
        private static readonly string Provider = "provider";
        private static readonly string DisplayName = "displayname";
        private static readonly string Key = "key";
        

        public CustomLogin Map(XElement source)
        {
            return new CustomLogin
            {
                LoginProvider = source.Element(Provider).Value,
                ProviderDisplayName = source.Element(DisplayName).Value,
                ProviderKey = source.Element(Key).Value
            };
        }

        public XElement Map(CustomLogin login)
        {
            var element = new XElement("login");

            element.Add(new XElement(Provider, login.LoginProvider), 
                new XElement(DisplayName, login.ProviderDisplayName),
                new XElement(Key, login.ProviderKey));

            return element;
        }
    }
}