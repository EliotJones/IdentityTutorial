namespace IdentityTutorial.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Core;
    using Mappings;

    public class Context
    {
        private static volatile Context instance;
        private static readonly object SyncRoot = new object();

        private DateTime lastSync;

        private readonly XDocument document;
        private readonly FileReader fileReader;
        private readonly XmlUserMap xmlUserMap = new XmlUserMap();

        private Context(FileReader fileReader)
        {
            this.fileReader = fileReader;
            var content = fileReader.ReadFileToString();
            document = XDocument.Parse(content);
            lastSync = DateTime.UtcNow;
        }

        public static Context Get(FileReader fileReader)
        {
            if (instance == null)
            {
                lock (SyncRoot)
                {
                    if (instance == null)
                    {
                        instance = new Context(fileReader);
                    }
                }
            }
            return instance;
        }

        public IList<CustomUser> GetUsers()
        {
            return document.Descendants("user").Select(xmlUserMap.Map).ToList();
        }

        public CustomUser GetUserById(Guid id)
        {
            return GetUserElementsById(id)
                .Select(xmlUserMap.Map)
                .SingleOrDefault();
        }

        public CustomUser GetUserByEmail(string emailAddress)
        {
            return GetUserElementsByEmail(emailAddress)
                .Select(xmlUserMap.Map)
                .SingleOrDefault();
        }

        public CustomUser GetUserByName(string userName)
        {
            return GetUserElementsByName(userName).Select(xmlUserMap.Map).SingleOrDefault();
        }

        public void AddUser(CustomUser customUser)
        {
            if (customUser == null)
            {
                throw new ArgumentNullException();
            }

            if (GetUserById(customUser.Id) != null)
            {
                throw new InvalidOperationException("Cannot add a user with the same Id: " + customUser.Id);
            }

            var user = xmlUserMap.Map(customUser);

            GetUsersParentNode().Add(user);

            CheckSync(true);
        }

        public void UpdateUser(CustomUser customUser)
        {
            if (customUser == null)
            {
                throw new ArgumentNullException();
            }

            if (GetUserById(customUser.Id) == null)
            {
                throw new InvalidOperationException("Cannot edit user with id: " + customUser.Id + ". User does not exist.");
            }

            if (GetUserElementsByEmail(customUser.EmailAddress).Count() > 1)
            {
                throw new InvalidOperationException("Cannot edit the email because another user with the same email already exists: " + customUser.EmailAddress);
            }

            var user = xmlUserMap.Map(customUser);

            GetUserElementsById(customUser.Id).Remove();

            GetUsersParentNode().Add(user);

            CheckSync();
        }

        private XElement GetUsersParentNode()
        {
            return document.Descendants("users").Single();
        }

        private IEnumerable<XElement> GetUserElementsByName(string userName)
        {
            return document.Descendants("name").Where(e => userName.Equals(e.Value, StringComparison.OrdinalIgnoreCase))
                .Ancestors("user");
        }

        private IEnumerable<XElement> GetUserElementsByEmail(string emailAddress)
        {
            return document.Descendants("email").Where(e => emailAddress.Equals(e.Value, StringComparison.OrdinalIgnoreCase))
                .Ancestors("user");
        }

        private IEnumerable<XElement> GetUserElementsById(Guid id)
        {
            return document.Descendants("id").Where(e => id.ToString().Equals(e.Value, StringComparison.OrdinalIgnoreCase))
                .Ancestors("user");
        }

        private IEnumerable<XElement> GetUserElementsWithClaim(string type, string value)
        {
            return document.Descendants("claim").Where(e => e.Descendants("type").Single().Value != null
                                                            &&
                                                            e.Descendants("type")
                                                                .Single()
                                                                .Value.Equals(type, StringComparison.OrdinalIgnoreCase)
                                                            && e.Descendants("value").Single().Value != null
                                                            &&
                                                            e.Descendants("value")
                                                                .Single()
                                                                .Value.Equals(value, StringComparison.OrdinalIgnoreCase))
                                                                .Ancestors("user");
        } 

        internal static void Reset()
        {
            instance = null;
        }

        public void DeleteUser(Guid id)
        {
            var user = GetUserElementsById(id).ToArray();
            if (user.Length != 1)
            {
                return;
            }

            user.Remove();
        }

        private void CheckSync(bool forceSave = false)
        {
            if (DateTime.UtcNow - lastSync >= new TimeSpan(0, 0, 10) || forceSave)
            {
                lock (SyncRoot)
                {
                    fileReader.WriteStringToFile(document.ToString(SaveOptions.None));
                }

                lastSync = DateTime.UtcNow;
            }
        }

        public IList<CustomUser> GetUsersWithClaim(string type, string value)
        {
            var elements = GetUserElementsWithClaim(type, value);

            if (elements == null)
            {
                return new CustomUser[] { };
            }

            return elements.Select(xmlUserMap.Map).ToArray();
        }
    }
}