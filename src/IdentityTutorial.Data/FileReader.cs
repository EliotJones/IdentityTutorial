namespace IdentityTutorial.Data
{
    using System.IO;
    using System.Text;
    using System.Threading;

    public class FileReader
    {
        private readonly string path;

        public FileReader(string path)
        {
            this.path = path;
        }

        private readonly SemaphoreSlim syncLock = new SemaphoreSlim(1);

        public virtual string ReadFileToString()
        {
            syncLock.Wait();

            StringBuilder sb = new StringBuilder();

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite, 4096, FileOptions.Asynchronous))
            {
                byte[] buffer = new byte[4096];

                int numberRead;

                while ((numberRead = fs.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string text = Encoding.UTF8.GetString(buffer, 0, numberRead);
                    sb.Append(text);
                }
            }
            syncLock.Release();
            return sb.ToString();
        }

        public void WriteStringToFile(string s)
        {
            syncLock.Wait();
            File.WriteAllText(path, s);
            syncLock.Release();
        }
    }
}
