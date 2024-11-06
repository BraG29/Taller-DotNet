namespace Commercial_Office.Model
{
    public class TimedQueueItem<T>
    {
        public T Item { get; set; }
        public DateTime EnqueuedTime { get; set; }

        public TimedQueueItem(T item)
        {
            Item = item;
            EnqueuedTime = DateTime.Now; // Almacena la hora en que el objeto fue encolado
        }
    }
}
