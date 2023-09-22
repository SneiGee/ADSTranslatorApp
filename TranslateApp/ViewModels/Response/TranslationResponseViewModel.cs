using Newtonsoft.Json;

namespace TranslateApp.ViewModels.Response;

public class TranslationResponseViewModel
{
    // [JsonProperty("translated")]
    // public string? Translated { get; set; }

    [JsonProperty("success")]
    public Success? Success { get; set; }
    [JsonProperty("contents")]
    public Contents? Contents { get; set; }
}

public class Success
{
    public int Total { get; set; }
}

public class Contents
{
    [JsonProperty("translated")]
    public string Translated { get; set; } = string.Empty;
    [JsonProperty("text")]
    public string Text { get; set; } = string.Empty;
    [JsonProperty("translation")]
    public string Translation { get; set; } = string.Empty;
}