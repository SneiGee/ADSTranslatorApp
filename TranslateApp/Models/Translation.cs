using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TranslateApp.Models;

public class Translation
{
    [Key]
    public int Id { get; set; }
    [JsonProperty("text")] // Specify the JSON property name
    [Required]
    public string? Text { get; set; }

    [JsonProperty("translated")] // Specify the JSON property name
    [Required]
    public string? Translated { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}
