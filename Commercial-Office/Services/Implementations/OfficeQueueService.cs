using Commercial_Office.Model;
using Commercial_Office.Services.Interfaces;
using System.Collections.Concurrent;

namespace Commercial_Office.Services.Implementations
{
    public class OfficeQueueService : IOfficeQueueService
    {

        private Dictionary<string, ConcurrentQueue<TimedQueueItem<string>>> _Queues =
            new Dictionary<string, ConcurrentQueue<TimedQueueItem<string>>>();

        public ConcurrentQueue<TimedQueueItem<string>> GetQueueOrCreateQueue(string officeId)
        {
            if (!_Queues.ContainsKey(officeId))
            {
                _Queues[officeId] = new ConcurrentQueue<TimedQueueItem<string>>();
            }
            return _Queues[officeId];
        }
    }
}
