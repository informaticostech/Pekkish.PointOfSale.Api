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
        }

        #region Wait.io API Call
        private async Task<dynamic> DataGet(string endpoint)
        {
            // Add the API key to the headers
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _apiKey);

            // Make a GET request to the specified endpoint
            var response = await _client.GetAsync(_apiServerAddress + endpoint);

            // Parse the JSON response
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject(json);
        }

        private async Task<HttpResponseMessage> DataPost(string endpoint, object data)
        {
            // Add the API key to the headers
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _apiKey);

            // Convert the data to a JSON string
            var json = JsonConvert.SerializeObject(data);

            // Create a StringContent object with the JSON string
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Make a POST request to the specified endpoint with the JSON content
            var response = await _client.PostAsync(_apiServerAddress + endpoint, content);
            return response;
        }
        #endregion

        public async Task<HttpResponseMessage> InteractiveListMessageSend(string whatsAppNumber, InteractiveListMessageDto dto)
        {
            var serializer = new JavaScriptSerializer();
            string endpoint = $"sendInteractiveListMessage?whatsappNumber=%2B{whatsAppNumber}";
            
            //string postData = serializer.Serialize(new
            //{
            //    Header = dto.Header,
            //    Body = dto.Body,
            //    Footer = dto.Footer,
            //    ButtonText = dto.ButtonText,
            //    Sections = dto.Sections
            //});

            //var result = await DataPost(endpoint, postData);
            var result = await DataPost(endpoint, dto);

            return result;
        }
    }
}