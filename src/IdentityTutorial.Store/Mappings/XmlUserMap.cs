namespace IdentityTutorial.Store.Mappings
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using Core;

    public class XmlUserMap : IMap<XElement, CustomUser>,
        IMap<CustomUser, XElement>
    {
        private static readonly string Users = "users";
        private static readonly string User = "user";
        private static readonly string Username = "name";
        private static readonly string Id = "id";
        private static readonly string Email = "email";
        private static readonly string Password = "password";

        private readonly XmlLoginMap loginMap = new XmlLoginMap();

        public CustomUser Map(XElement source)
        {
            if (source == null)
            {
                return null;
            }

            var user = new CustomUser(new Guid(source.Element(Id).Value),
            source.Element(Email).Value,
            source.Element(Username).Value,
            source.Element(Password).Value);

            if (source.Descendants("login").Any())
            {
                var logins = source.Descendants("login").Select(loginMap.Map);

                foreach (var customLogin in logins)
                {
                    user.AddLogin(customLogin);
                }
            }

            return user;
        }

        public XElement Map(CustomUser source)
        {
            var id = new XElement(Id, source.Id);
            var email = new XElement(Email, source.EmailAddress);
            var password = new XElement(Password, source.PasswordHash);
            var username = new XElement(Username, source.UserName);

            var logins = new XElement("logins");

            logins.Add(source.CustomLogins.Select(loginMap.Map).ToArray());

            return new XElement(User, id, email, password, logins, username);
        }
    }
}