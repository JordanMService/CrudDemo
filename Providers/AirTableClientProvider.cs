using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Granify.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Granify.Providers
{
    public class AirTableClientProvider
    {
        private readonly HttpClient _airTableClient; 
        private const string baseUrl = "https://api.airtable.com";
        private const string tableUrl= "/v0/appeetsXPXrTp8SlB/Granify%20Data";
        public AirTableClientProvider(string airTableKey){
            var client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", airTableKey);
            _airTableClient = client;
        }


        public virtual async Task<string> GetStringAsync(string uri){
            return await _airTableClient.GetStringAsync(uri);
        }
       
        public virtual async Task<HttpResponseMessage> PostAsync(string uri, HttpContent content){
            return await _airTableClient.PostAsync(uri,content);
        }
   
        public virtual async Task<HttpResponseMessage> PatchAsync(string uri, HttpContent content){
            return await _airTableClient.PatchAsync(uri, content);
        }
    }
    public static class HttpClientExtensions{
        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient httpClient, string uriString, HttpContent httpContent){
            var request = new HttpRequestMessage(new HttpMethod("PATCH"),uriString){
                Content = httpContent
            };
            
            return await httpClient.SendAsync(request);
            
        }
    }
}





