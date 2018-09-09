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
using Granify.Providers;
using System.Net;

namespace Granify.Api.DataAccess
{
    public class ItemRepo
    {
        private readonly AirTableClientProvider _airTableClientProvider; 
      
        private const string tableUrl= "/v0/appeetsXPXrTp8SlB/Granify%20Data";
        public ItemRepo(AirTableClientProvider provider){
          _airTableClientProvider = provider;
        }

        /// <summary>
        /// Get all rows stored in Airtable
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<AirTableResponseItem>> GetRowsAsync()
        {
            var responseStr = await _airTableClientProvider.GetStringAsync($"{tableUrl}?view=Grid%20view");
            var data = JsonConvert.DeserializeObject<AirTableResponse>(responseStr);
            return data.Records.Where(r=> !r.Item.IsDeleted);
        }
 
        /// <summary>
        /// Get Airtable row by assigned Id
        /// </summary>
        /// <param name="itemId">10 character item Id</param>
        /// <returns></returns>
        public async Task<AirTableResponseItem> GetRowById(string itemId){
            try{
                var formula = $"filterByFormula=%7BId%7D%3D%22{itemId}%22";
                
                var responseStr = await _airTableClientProvider.GetStringAsync($"{tableUrl}?{formula}");
                var data = JsonConvert.DeserializeObject<AirTableResponse>(responseStr).Records.Where(r => !r.Item.IsDeleted);
                if(data.Count() == 0){
                    throw new KeyNotFoundException();
                }
                return(data.FirstOrDefault());
            }
            catch(Exception ex)
            {
                var test = ex;
            }

            return null;
           
        }

        /// <summary>
        /// Get hourly statistics
        /// </summary>
        /// <returns></returns>
        public async Task GetItemStatistics(){
            var items = await GetRowsAsync();
            var deletedItems = items.Where(i => i.Item.IsDeleted);
            var activeItems = items.Where(i => !i.Item.IsDeleted);
        }

        /// <summary>
        /// Post Item to Airtable
        /// </summary>
        /// <param name="itemToPost"></param>
        /// <returns></returns>
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
            
            var response = await _airTableClientProvider.PostAsync(tableUrl, _ConvertItemToContent(airTableItem));

            if(!response.IsSuccessStatusCode){
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception("Unable to post item");
            }

        }

        /// <summary>
        /// Set is deleted flag in Airtable
        /// </summary>
        /// <param name="itemId">10 character Item Id</param>
        /// <returns></returns>
        public async Task DeleteItemAsync(string itemId){
            var item = await  GetRowById(itemId);
            var airTableItem = new { fields = new {IsDeleted = true, LastUpdated = DateTime.Now}};
            
            var airTablePaylod = JsonConvert.SerializeObject(airTableItem);
            
            var response = await _airTableClientProvider.PatchAsync($"{tableUrl}/{item.Id}",_ConvertItemToContent(airTableItem));

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
}





