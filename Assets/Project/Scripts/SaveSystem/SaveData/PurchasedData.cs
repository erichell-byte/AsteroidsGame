using System;

namespace SaveLoad
{
    [Serializable]
    public class PurchasedData : ISaveData
    {
        public bool NoAds;
        public long Timestamp { get; set; }
    }
}