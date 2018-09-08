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

            var airTablePaylod = JsonConvert.SerializeObject(airTableItem);
            
            var response = await _airTableClient.PostAsync("",new StringContent(airTablePaylod, System.Text.Encoding.UTF8,"application/json"));

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
    }
}




