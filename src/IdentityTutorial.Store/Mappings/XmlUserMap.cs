namespace IdentityTutorial.Store.Mappings
{
    using System;
    using System.Xml.Linq;
    using Core;

    public class XmlUserMap : IMap<XElement, CustomUser>,
        IMap<CustomUser, XElement>
    {
        private static readonly string Users = "users";
        private static readonly string User = "user";
        private static readonly string Id = "id";
        private static readonly string Email = "email";
        private static readonly string Password = "password";

        public CustomUser Map(XElement source)
        {
            if (source == null)
            {
                return null;
            }

            return new CustomUser(new Guid(source.Element(Id).Value),
            source.Element(Email).Value, source.Element(Password).Value);
        }

        public XElement Map(CustomUser source)
        {
            var id = new XElement(Id, source.Id);
            var email = new XElement(Email, source.EmailAddress);
            var password = new XElement(Password, source.PasswordHash);
            
            return new XElement(User, id, email, password);
        }
    }
}