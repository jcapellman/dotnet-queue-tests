namespace RESTQueue.lib.Common
{
    public class Settings
    {
        public string DatabaseHostName;

        public int DatabasePortNumber;

        public string QueueHostName;

        public int QueuePortNumber;

        public string QueueUsername;

        public string QueuePassword;

        public string CacheHostName;

        public string CachePortNumber;

        public bool CacheEnabled { get; set; }
    }
}