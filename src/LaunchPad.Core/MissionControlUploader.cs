using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace LaunchPad.Core;

/// <summary>Sends a flight record to the mission control dashboard.</summary>
public static class MissionControlUploader

    // Mitch - wasn't technically necessary to fix "new WebClient() deprecated" error, but it was the last build warning appearing in the logs 
    //  and part of a Build Engineer's job is to clean up builds and reduce noise (plus, it was quick!)
{
    // public static string Upload(string url, FlightRecord flight)
    // {
    //     var json = JsonConvert.SerializeObject(flight);
    //     using var client = new WebClient();
    //     client.Headers[HttpRequestHeader.ContentType] = "application/json";
    //     return client.UploadString(url, json);
    // }

    private static readonly HttpClient httpClient = new();

    public static string Upload(string url, FlightRecord flight)
    {
        var json = JsonConvert.SerializeObject(flight);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = httpClient.PostAsync(url, content).GetAwaiter().GetResult();

        return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
    }
}
