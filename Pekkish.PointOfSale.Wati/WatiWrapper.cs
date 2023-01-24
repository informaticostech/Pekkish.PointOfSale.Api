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

            dto = await FormatInteractiveListMessageDto(dto);

            var result = await DataPost(endpoint, dto);

            return result;
        }

        public async Task<HttpResponseMessage> InteractiveButtonsMessageMediaSend(string whatsAppNumber, InteractiveButtonsMessageMediaDto dto)
        {
            var serializer = new JavaScriptSerializer();
            string endpoint = $"sendInteractiveButtonsMessage?whatsappNumber=%2B{whatsAppNumber}";

            dto = await FormatInteractiveButtonsMessageMediaDto(dto);

            var result = await DataPost(endpoint, dto);

            return result;
        }
        public async Task<HttpResponseMessage> InteractiveButtonsMessageTextSend(string whatsAppNumber, InteractiveButtonsMessageTextDto dto)
        {
            var serializer = new JavaScriptSerializer();
            string endpoint = $"sendInteractiveButtonsMessage?whatsappNumber=%2B{whatsAppNumber}";

            dto = await FormatInteractiveButtonsMessageTextDto(dto);

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

        #region Format Dto for trunactions 
        //Wati.io does not error on text size validation issues
        private async Task<InteractiveListMessageDto> FormatInteractiveListMessageDto(InteractiveListMessageDto dto)
        {
            return await Task.Run(() =>
            {
                dto.Header = (dto.Header.Length > 60) ? dto.Header.Substring(0, 60) : dto.Header;
                dto.Body = (dto.Body.Length > 1024) ? dto.Body.Substring(0, 1024) : dto.Body;
                dto.Footer = (dto.Footer.Length > 60) ? dto.Footer.Substring(0, 60) : dto.Footer;

                foreach (var section in dto.Sections)
                {
                    section.Title = (section.Title.Length > 24) ? section.Title.Substring(0, 24) : section.Title;

                    Parallel.ForEach(section.Rows, row =>
                    {
                        row.Title = (row.Title.Length > 24) ? row.Title.Substring(0, 24) : row.Title;
                        row.Description = (row.Description == null) ? "" : (row.Description.Length > 72) ? row.Description.Substring(0, 72) : row.Description;
                    });
                }

                return dto;
            });
        }
        private async Task<InteractiveButtonsMessageMediaDto> FormatInteractiveButtonsMessageMediaDto(InteractiveButtonsMessageMediaDto dto)
        {
            return await Task.Run(() =>
            {
                dto.Header.Text = (dto.Header.Text.Length > 60) ? dto.Header.Text.Substring(0, 60) : dto.Header.Text;
                dto.Body = (dto.Body.Length > 1024) ? dto.Body.Substring(0, 1024) : dto.Body;
                dto.Footer = (dto.Footer.Length > 60) ? dto.Footer.Substring(0, 60) : dto.Footer;

                foreach (var button in dto.Buttons)
                {
                    button.Text = (button.Text.Length > 20) ? button.Text.Substring(0, 20) : button.Text;
                }

                return dto;
            });
        }
        private async Task<InteractiveButtonsMessageTextDto> FormatInteractiveButtonsMessageTextDto(InteractiveButtonsMessageTextDto dto)
        {
            return await Task.Run(() =>
            {
                dto.Header.Text = (dto.Header.Text.Length > 60) ? dto.Header.Text.Substring(0, 60) : dto.Header.Text;
                dto.Body = (dto.Body.Length > 1024) ? dto.Body.Substring(0, 1024) : dto.Body;
                dto.Footer = (dto.Footer.Length > 60) ? dto.Footer.Substring(0, 60) : dto.Footer;

                foreach (var button in dto.Buttons)
                {
                    button.Text = (button.Text.Length > 20) ? button.Text.Substring(0, 20) : button.Text;
                }

                return dto;
            });
        }
        #endregion
    }
}