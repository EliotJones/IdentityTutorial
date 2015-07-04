namespace IdentityTutorial.Core
{
    using System;

    public class CustomLogin
    {
        public string LoginProvider { get; set; }
        
        public string ProviderKey { get; set; }
        
        public string ProviderDisplayName { get; set; }
        
        public Guid UserId { get; set; }
    }
}