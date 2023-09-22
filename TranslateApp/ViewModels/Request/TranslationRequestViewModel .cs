using Newtonsoft.Json;

namespace TranslateApp.ViewModels.Request;

public class TranslationRequestViewModel 
{
    [JsonProperty("text")]
    public string? Text { get; set; }
}