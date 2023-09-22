using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TranslateApp.Helpers;
using TranslateApp.Interfaces;
using TranslateApp.ViewModels;
using TranslateApp.ViewModels.Response;

namespace TranslateApp.Services;

public class TranslationService : ITranslationService
{
    private readonly HttpClient _httpClient;
    private readonly TranslationApiConfig _apiConfig;
    private readonly ILogger<TranslationService> _logger;
    public TranslationService(HttpClient httpClient, IOptions<TranslationApiConfig> apiConfig, ILogger<TranslationService> logger)
    {
        _logger = logger;
        _apiConfig = apiConfig?.Value ?? throw new ArgumentNullException(nameof(apiConfig));
        _httpClient = httpClient;
    }
    public async Task<string> TranslateAsync(string text)
    {
        try
        {
            string apiUrl = _apiConfig.BaseUrl + "yoda.json";
            _logger.LogInformation($"Sending API request for text: {text}");
            var response = await _httpClient.GetAsync($"{apiUrl}?text={Uri.EscapeDataString(text)}");
            _logger.LogInformation($"Received API response: {response}");

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Response: {result}");

                // Parse the JSON result to get the translated text.
                var translationResponse = JsonConvert.DeserializeObject<TranslationResponseViewModel>(result);

                _logger.LogInformation($"Received API response: {translationResponse!.Contents!.Translated}");

                // Extract the translated text from the model and return it.
                return translationResponse!.Contents!.Translated;
            }
            else
            {
                return "API Request Error";
            }
        }
        catch (Exception ex)
        {
            return "API Request Exception: " + ex.Message;
        }
    }
}