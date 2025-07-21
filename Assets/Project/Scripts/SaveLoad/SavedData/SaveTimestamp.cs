using System;

namespace SaveLoad
{
    [Serializable]
    public class SaveTimestamp
    {
        public long ticks;
        
        public SaveTimestamp(long ticks)
        {
            this.ticks = ticks;
        }
        
        public SaveTimestamp()
        {
            ticks = DateTime.UtcNow.Ticks;
        }
    }
}