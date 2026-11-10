using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace LaunchPad.Core;

/// <summary>Sends a flight record to the mission control dashboard.</summary>
public static class MissionControlUploader
{
    // public static string Upload(string url, FlightRecord flight)
    // {
    //     var json = JsonConvert.SerializeObject(flight);
    //     // Mitch - NEED TO CHANGE THIS?
    //     using var client = new WebClient();
    //     client.Headers[HttpRequestHeader.ContentType] = "application/json";
    //     return client.UploadString(url, json);
    // }

    // Mitch - wasn't necessary to fix this, but was the last build warning appearing in my logs and I wanted them clean! (plus, it was quick)

    private static readonly HttpClient httpClient = new();

    public static string Upload(string url, FlightRecord flight)
    {
        var json = JsonConvert.SerializeObject(flight);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = httpClient.PostAsync(url, content).GetAwaiter().GetResult();

        return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
    }
}
