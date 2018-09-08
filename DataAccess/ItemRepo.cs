using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Granify.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Granify.Api.DataAccess
{
    public class ItemRepo
    {
        private readonly HttpClient _airTableClient; 
        private const string baseUrl = "https://api.airtable.com/v0/appeetsXPXrTp8SlB/Granify%20Data";
        public ItemRepo(string airTableKey){
            var client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", airTableKey);
            _airTableClient = client;
        }
        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            var responseStr = await _airTableClient.GetStringAsync("?view=Grid%20view");
            var data = JsonConvert.DeserializeObject<AirTableResponse>(responseStr);
            return data.Records.Select(r => r.Fields);
        }
    }
}




