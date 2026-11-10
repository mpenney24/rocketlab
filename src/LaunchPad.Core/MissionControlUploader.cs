using System.Net;
using Newtonsoft.Json;

namespace LaunchPad.Core;

/// <summary>Sends a flight record to the mission control dashboard.</summary>
public static class MissionControlUploader
{
    public static string Upload(string url, FlightRecord flight)
    {
        var json = JsonConvert.SerializeObject(flight);
        using var client = new WebClient();
        client.Headers[HttpRequestHeader.ContentType] = "application/json";
        return client.UploadString(url, json);
    }
}
