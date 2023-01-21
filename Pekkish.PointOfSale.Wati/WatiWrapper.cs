using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Nancy.ModelBinding;
using Pekkish.PointOfSale.Wati.Models.Dtos;

namespace Pekkish.PointOfSale.Wati
{
    public class WatiWrapper
    {
        private readonly string _apiKey;
        private readonly string _apiServerAddress;
        private readonly HttpClient _client;

        public WatiWrapper(string apiServerAddress, string apiKey )
        {
            _apiServerAddress = apiServerAddress;
            _apiKey = apiKey;
            _client = new HttpClient();

            // Set the API key headers
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _apiKey);
        }

        #region Wait.io API Call
        private async Task<dynamic> DataGet(string endpoint)
        {            
            // Make a GET request to the specified endpoint
            var response = await _client.GetAsync(_apiServerAddress + endpoint);

            // Parse the JSON response
            var json = await response.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject(json);
        }

        private async Task<HttpResponseMessage> DataPost(string endpoint, object? data)
        {
            try
            {
                var response = new HttpResponseMessage();
               
                if (data != null)
                {
                    // Convert the data to a JSON string
                    var json = JsonConvert.SerializeObject(data);

                    // Create a StringContent object with the JSON string
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Make a POST request to the specified endpoint with the JSON content
                    response = await _client.PostAsync(_apiServerAddress + endpoint, content);
                }
                else
                {
                    // Make a POST request to the specified endpoint
                    response = await _client.PostAsync(_apiServerAddress + endpoint, null);
                }
                
                return response;
            }
            catch(Exception ex) 
            { 
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        public async Task<HttpResponseMessage> InteractiveListMessageSend(string whatsAppNumber, InteractiveListMessageDto dto)
        {
            var serializer = new JavaScriptSerializer();
            string endpoint = $"sendInteractiveListMessage?whatsappNumber=%2B{whatsAppNumber}";
                        
            var result = await DataPost(endpoint, dto);

            return result;
        }

        public async Task<HttpResponseMessage> SessionMessageSend(string whatsAppNumber, string messageText)
        {
            var serializer = new JavaScriptSerializer();
            string endpoint = $"sendSessionMessage/{whatsAppNumber}?messageText={messageText}";

            var result = await DataPost(endpoint, null);

            return result;
        }
        public async Task<HttpResponseMessage> SessionAssignOperator(string whatsAppNumber, string operatorEmail)
        {            
            string endpoint = $"assignOperator?email={operatorEmail}&whatsappNumber={whatsAppNumber}?";

            var result = await DataPost(endpoint, null);

            return result;
        }
    }
}