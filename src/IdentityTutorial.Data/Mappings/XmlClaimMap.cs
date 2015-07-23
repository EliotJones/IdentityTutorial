namespace IdentityTutorial.Data.Mappings
{
    using System.Xml.Linq;
    using Core;

    public class XmlClaimMap : IMap<XElement, CustomUserClaim>, IMap<CustomUserClaim, XElement>
    {
        private static readonly string Type = "type";
        private static readonly string Value = "value";

        public CustomUserClaim Map(XElement source)
        {
            return new CustomUserClaim(source.Element(Type).Value, source.Element(Value).Value);
        }

        public XElement Map(CustomUserClaim source)
        {
            var element = new XElement("claim");

            element.Add(new XElement(Type, source.Type), new XElement(Value, source.Value));

            return element;
        }
    }
}