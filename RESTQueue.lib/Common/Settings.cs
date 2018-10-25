namespace RESTQueue.lib.Common
{
    public class Settings
    {
        public string DatabaseHostName { get; set; }

        public int DatabasePortNumber { get; set; }

        public string QueueHostName { get; set; }

        public int QueuePortNumber { get; set; }

        public string QueueUsername { get; set; }

        public string QueuePassword { get; set; }

        public bool CacheEnabled { get; set; }

        public int CachePortNumber { get; set; }

        public string CacheHostName { get; set; }
    }
}