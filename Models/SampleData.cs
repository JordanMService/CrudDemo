using System;

namespace Granify.Models
{
    public class Item
    {
        public string Id { get; set; }

        public string PhoneNumber { get; set; }

        public string Name { get; set;} 

        public bool IsDeleted { get; set; }

        public DateTime? LastUpdated {get;set;}
    }

    public class ItemStats{
        public int ActiveCount {get;set;}
        public int DeletedCount {get;set;}
    }
}