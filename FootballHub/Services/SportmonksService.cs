using System.Text.Json;

namespace FootballHub.Services;

public class SportmonksService
{
    private readonly HttpClient _httpClient;
    private readonly string _token;
    private readonly string _base;

    public SportmonksService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _token = config["SportmonksApi:ApiToken"] ?? "";
        _base  = config["SportmonksApi:BaseUrl"] ?? "https://api.sportmonks.com/v3/football";
    }

    // Partite live
    public async Task<JsonElement> GetLivescoresAsync()
    {
        var url = $"{_base}/livescores/inplay?api_token={_token}&include=participants;scores;periods;events;league.country;round";
        var json = await _httpClient.GetStringAsync(url);
        return JsonSerializer.Deserialize<JsonElement>(json);
    }

    // Classifica Serie A (stagione 25533)
    public async Task<JsonElement> GetClassificaAsync()
    {
        var url = $"{_base}/standings/seasons/25533?api_token={_token}&include=participant;rule.type;details.type;form;stage;league;group";
        var json = await _httpClient.GetStringAsync(url);
        return JsonSerializer.Deserialize<JsonElement>(json);
    }

    // Pronostici (round 371883)
    public async Task<JsonElement> GetPronosticiApiAsync()
    {
        var url = $"{_base}/rounds/371883?api_token={_token}&include=fixtures.odds.market;fixtures.odds.bookmaker;fixtures.participants;league.country&filters=markets:1;bookmakers:2";
        var json = await _httpClient.GetStringAsync(url);
        return JsonSerializer.Deserialize<JsonElement>(json);
    }

    // Partite per data
    public async Task<JsonElement> GetPartitePerDataAsync(string data)
    {
        var url = $"{_base}/leagues/date/{data}?api_token={_token}&include=today.scores;today.participants;today.stage;today.group;today.round";
        var json = await _httpClient.GetStringAsync(url);
        return JsonSerializer.Deserialize<JsonElement>(json);
    }
}
