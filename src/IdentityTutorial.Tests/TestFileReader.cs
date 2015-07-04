namespace XunitTests
{
    using IdentityTutorial.Store;

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
		<id>" + ContextTests.FirstUserId + @"</id>
		<email>" + ContextTests.FirstUserEmail + @"</email>
        <password>" + ContextTests.FirstUserPassword + @"</password>
        <name>" + ContextTests.FirstUserPassword + @"</name>
	</user>
    <user>
		<id>" + ContextTests.SecondUserId + @"</id>
		<email>" + ContextTests.SecondUserEmail + @"</email>
        <password>" + ContextTests.SecondUserPassword + @"</password>
        <name>" + ContextTests.SecondUserPassword + @"</name>
        <logins>
            <login>
                <provider>Trout</provider>
                <key>" + ContextTests.SecondUserLoginKey + @"</key>
                <displayname>" + ContextTests.SecondUserLoginProvider + @"</displayname>
            </login>
        </logins>
	</user>
</users>";
        }
    }
}