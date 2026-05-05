using System.Text.Json;

namespace FootballHub.Services;

public class SportmonksService
{
    private readonly HttpClient _httpClient;
    private readonly string _token;
    private readonly string _base;

    private const int SerieALeagueId = 384;
    private const int SerieASeasonId = 23720;

    public SportmonksService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _token = config["SportmonksApi:ApiToken"] ?? "";
        _base = config["SportmonksApi:BaseUrl"] ?? "https://api.sportmonks.com/v3/football";
    }

    public async Task<JsonElement> GetLivescoresAsync()
    {
        var url = $"{_base}/livescores/inplay?api_token={_token}&include=participants;scores;periods;league&filters=fixtureLeagues:{SerieALeagueId}";
        var json = await _httpClient.GetStringAsync(url);
        return JsonSerializer.Deserialize<JsonElement>(json);
    }

    public async Task<JsonElement> GetPartiteOggiAsync()
    {
        var today = DateTime.Now.ToString("yyyy-MM-dd");
        var url = $"{_base}/fixtures/date/{today}?api_token={_token}&include=participants;scores;state;league&filters=fixtureLeagues:{SerieALeagueId}";
        var json = await _httpClient.GetStringAsync(url);
        return JsonSerializer.Deserialize<JsonElement>(json);
    }

    public async Task<JsonElement> GetClassificaAsync()
    {
        var url = $"{_base}/standings/seasons/{SerieASeasonId}?api_token={_token}&include=participant;details.type;form";
        var json = await _httpClient.GetStringAsync(url);
        return JsonSerializer.Deserialize<JsonElement>(json);
    }

    public async Task<JsonElement> GetPartitePerDataAsync(string data)
    {
        var url = $"{_base}/fixtures/date/{data}?api_token={_token}&include=participants;scores;state;league&filters=fixtureLeagues:{SerieALeagueId}";
        var json = await _httpClient.GetStringAsync(url);
        return JsonSerializer.Deserialize<JsonElement>(json);
    }

    public async Task<JsonElement> GetPronosticiApiAsync()
    {
        var url = $"{_base}/fixtures/upcoming/markets/1?api_token={_token}&include=participants;odds.bookmaker&filters=fixtureLeagues:{SerieALeagueId};bookmakers:2&per_page=10";
        var json = await _httpClient.GetStringAsync(url);
        return JsonSerializer.Deserialize<JsonElement>(json);
    }
}