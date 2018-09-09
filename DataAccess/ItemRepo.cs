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

namespace Granify.Api.DataAccess
{
    public class ItemRepo
    {
        private readonly HttpClient _airTableClient; 
        private const string baseUrl = "https://api.airtable.com";
        private const string tableUrl= "/v0/appeetsXPXrTp8SlB/Granify%20Data";
        public ItemRepo(string airTableKey){
            var client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", airTableKey);
            _airTableClient = client;
        }

        public async Task<IEnumerable<AirTableResponseItem>> GetRowsAsync()
        {
            var responseStr = await _airTableClient.GetStringAsync($"{tableUrl}?view=Grid%20view");
            var data = JsonConvert.DeserializeObject<AirTableResponse>(responseStr);
            return data.Records.Where(r=> !r.Item.IsDeleted);
        }
 

        public async Task<AirTableResponseItem> GetAirTableRowByIdAsync(string itemId){
            var item = (await GetRowsAsync()).FirstOrDefault(i => i.Item.Id == itemId && i.Item.IsDeleted == false);
            if(item == null){
                throw new KeyNotFoundException();
            }
            return item;
        }

        public async Task GetItemStatistics(){
            var items = await GetRowsAsync();
            var deletedItems = items.Where(i => i.Item.IsDeleted);
            var activeItems = items.Where(i => !i.Item.IsDeleted);
        }

        public async Task PostItemAsync(Item itemToPost){

            if(String.IsNullOrEmpty(itemToPost.Name)){
                throw new ArgumentException($"Name cannot be empty");          
            }

            var isValidPhone = _TryFormatPhoneNumber(itemToPost.PhoneNumber,out var formattedNumber);
            if(!isValidPhone){
                throw new ArgumentException($"{itemToPost.PhoneNumber} is not a valid phone number");
            }
            itemToPost.PhoneNumber = formattedNumber;

            var Id = _GenerateId();
            itemToPost.Id = Id;

            var airTableItem = new AirTablePostItem{
                Fields = itemToPost
            };

           itemToPost.LastUpdated = DateTime.Now;
            
            var response = await _airTableClient.PostAsync(tableUrl, _ConvertItemToContent(airTableItem));

            if(!response.IsSuccessStatusCode){
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception("Unable to post item");
            }

        }

        public async Task DeleteItemAsync(string itemId){
            var item = await  GetAirTableRowByIdAsync(itemId);
            var airTableItem = new { fields = new {IsDeleted = true, LastUpdated = DateTime.Now}};
            
            var airTablePaylod = JsonConvert.SerializeObject(airTableItem);
            
            var response = await _airTableClient.PatchAsync($"{tableUrl}/{item.Id}",_ConvertItemToContent(airTableItem));

             if(!response.IsSuccessStatusCode){
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception("Unable to post item");
            }

        }

        private string _GenerateId(){
            var outputString = "";
            var random = new Random();    

            while(outputString.Length < 10){
                var nextChar = random.Next(0,9).ToString();
                outputString = $"{outputString}{nextChar}";
            }
            
            return(outputString);
        }

        private bool _TryFormatPhoneNumber(string phoneNumber, out string formattedNumber){
            formattedNumber = null;

            if(String.IsNullOrEmpty(phoneNumber)){
                return false;
            }

            var cleanedStr = Regex.Replace(phoneNumber, @"[^0-9]+","");
            if(String.IsNullOrEmpty(cleanedStr)){
                return false;
            }

            if(cleanedStr.Length != 10){
                return false;
            }

            formattedNumber = $"({cleanedStr.Substring(0,3)}) {cleanedStr.Substring(3,3)}-{cleanedStr.Substring(6)}";
            return true;
            
        }

        private StringContent _ConvertItemToContent(object item){
            var airTablePaylod = JsonConvert.SerializeObject(item);
            return new StringContent(airTablePaylod, System.Text.Encoding.UTF8,"application/json");
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





