using RESTQueue.lib.datascience;

namespace RESTQueue.ProcessorApp
{
    public class MessageProcessor
    {
        private readonly DSManager _dsManager;

        public bool Running { get; private set; }

        public MessageProcessor(DSManager dsmanager)
        {
            _dsManager = dsmanager;
        }

        public bool IsMalicious(byte[] data)
        {
            Running = true;

            var result = _dsManager.IsMalicious(data);

            Running = false;

            return result;
        }
    }
}