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
        private static readonly string AccessFailedCount = "accessFailedCount";
        private static readonly string LockoutEndDate = "lockoutEndDate";
        private static readonly string LockoutEnabled = "lockoutEnabled";

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
            source.Element(Password).Value,
            GetInt(source.Element(AccessFailedCount).Value),
            GetNullableDateTime(source.Element(LockoutEndDate).Value),
            GetBool(source.Element(LockoutEnabled).Value));

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

        private int GetInt(string xmlValue)
        {
            int i;
            int.TryParse(xmlValue, out i);
            return i;
        }

        private bool GetBool(string xmlValue)
        {
            bool b;
            bool.TryParse(xmlValue, out b);
            return b;
        }

        private DateTimeOffset? GetNullableDateTime(string xmlValue)
        {
            DateTimeOffset dt;
            if (DateTimeOffset.TryParse(xmlValue, out dt))
            {
                return dt;
            }
            return null;
        }

        public XElement Map(CustomUser source)
        {
            var id = new XElement(Id, source.Id);
            var email = new XElement(Email, source.EmailAddress);
            var password = new XElement(Password, source.PasswordHash);
            var username = new XElement(Username, source.UserName);
            var accessFailedCount = new XElement(AccessFailedCount, source.AccessFailedCount);
            var lockoutEndDate = new XElement(LockoutEndDate, source.LockoutEndDate);
            var lockoutEnabled = new XElement(LockoutEnabled, source.LockoutEnabled);

            var logins = new XElement("logins");

            logins.Add(source.CustomLogins.Select(loginMap.Map).ToArray());

            return new XElement(User, id, email, password, logins, username, accessFailedCount, lockoutEndDate, lockoutEnabled);
        }
    }
}