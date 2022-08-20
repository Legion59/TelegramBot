using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TelegramBotApp.Model;

namespace TelegramBotApp.Services
{
    public class WeatherApiClient : IWeatherApiClient
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _settings;
        private readonly string _apiKey;


        public WeatherApiClient(HttpClient client, JsonSerializerSettings settings, string apiKey)
        {
            _client = client;
            _settings = settings;
            _apiKey = apiKey;
        }

        public async Task<WeatherResponseModel> GetCurrentWeatherByLocation(string location, CancellationToken cancellationToken = default)
        {
            var response = await _client.GetAsync($"/data/2.5/weather?q={location}&appid={_apiKey}&units=metric", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Waather API returned {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonConvert.DeserializeObject<WeatherResponseModel>(content, _settings);

            return result;
        }

        public async Task<WeatherFiveDaysResponseModel> GetWeatherFiveDaysByLocation(string location, CancellationToken cancellationToken = default)
        {
            var response = await _client.GetAsync($"/data/2.5/forecast?q={location}&appid={_apiKey}&units=metric", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Waather API returned {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonConvert.DeserializeObject<WeatherFiveDaysResponseModel>(content, _settings);

            return result;
        }

        public async Task<bool> CheckResponse(string location, CancellationToken cancellationToken = default)
        {
            var response = await _client.GetAsync($"/data/2.5/forecast?q={location}&appid={_apiKey}&units=metric", cancellationToken);

            return response.IsSuccessStatusCode;
        }

    }
}
