using System;

namespace Granify.Models
{
    public class AppSettings
    {
      public Keys Keys { get; set; }
    }

    public class Keys{
        public string AirTable {get; set; }
    }
}