using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Granify.Models
{
    public class AirTableResponseItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set;}

        [JsonProperty(PropertyName = "fields")]
        public Item Item { get; set;} 
    }

    public class AirTablePostItem{
        
        [JsonProperty(PropertyName = "fields")]
        public Item Fields { get; set;} 
    }

    public class AirTableResponse{
        [JsonProperty(PropertyName = "records")]
        public List<AirTableResponseItem> Records { get; set;}
    }
}