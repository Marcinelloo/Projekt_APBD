using System;
using System.Collections.Generic;

namespace APBD.Shared
{
    public class WatchList
    {
        public List<WatchItem> WatchItems { get; set; }
    }


    public class WatchItem
    {
        public string Logo { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Sector { get; set; }
        public string Country { get; set; }
        public string CEO { get; set; }
    }
}

