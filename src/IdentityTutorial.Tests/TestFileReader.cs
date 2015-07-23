namespace XunitTests
{
    using IdentityTutorial.Data;

    public class TestFileReader : FileReader
    {
        public TestFileReader() : base("C:\\git\\test.txt")
        {
        }

        public override string ReadFileToString()
        {
            return @"<?xml version='1.0'?>
<users>
	<user>
		<id>" + Constants.FirstUserId + @"</id>
		<email>" + Constants.FirstUserEmail + @"</email>
        <password>" + Constants.FirstUserPassword + @"</password>
        <name>" + Constants.FirstUserPassword + @"</name>
        <accessFailedCount>0</accessFailedCount>
        <lockoutEndDate/>
        <lockoutEnabled>true</lockoutEnabled>
        <claims>
            <claim>
                <type>" + Constants.FirstUserFirstClaimType + @"</type>
                <value>" + Constants.FirstUserFirstClaimValue + @"</value>
            </claim>
            <claim>
                <type>" + Constants.FirstUserSecondClaimType + @"</type>
                <value>" + Constants.FirstUserSecondClaimValue + @"</value>
            </claim>
        </claims>
	</user>
    <user>
		<id>" + Constants.SecondUserId + @"</id>
		<email>" + Constants.SecondUserEmail + @"</email>
        <password>" + Constants.SecondUserPassword + @"</password>
        <name>" + Constants.SecondUserPassword + @"</name>
        <accessFailedCount>0</accessFailedCount>
        <lockoutEndDate/>
        <lockoutEnabled>true</lockoutEnabled>
        <logins>
            <login>
                <provider>Trout</provider>
                <key>" + Constants.SecondUserLoginKey + @"</key>
                <displayname>" + Constants.SecondUserLoginProvider + @"</displayname>
            </login>
        </logins>
	</user>
</users>";
        }
    }
}