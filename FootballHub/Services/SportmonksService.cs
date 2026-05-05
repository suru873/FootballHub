using System.Text.Json;

namespace HubCalcio.Services
{
    public class SportmonksService
    {
        private readonly HttpClient _httpClient;
        // INSERISCI QUI IL TUO TOKEN PRESO DALLA DASHBOARD SPORTMONKS
        private readonly string _apiKey = "5iHqqufG1neARetN30007TmJFDlf57auO5IflNMyeN1sFkQPfXrcKADc1eVS";

        public SportmonksService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<JsonElement> GetPartitePerDataAsync(string data)
        {
            // URL per la Superliga Danese (ID 271)
            var url = $"https://api.sportmonks.com/v3/football/fixtures/date/{data}?api_token={_apiKey}&include=league;participants;scores&leagues=271";

            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode) return JsonDocument.Parse("[]").RootElement;

                var json = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(json);

                if (doc.RootElement.TryGetProperty("data", out JsonElement dataElement))
                {
                    return dataElement.Clone();
                }
            }
            catch { }

            return JsonDocument.Parse("[]").RootElement;
        }

        public async Task<JsonElement> GetClassificaDanimarcaAsync()
        {
            // Endpoint alternativo più stabile per v3: Standings per League ID
            // Usiamo il filtro "live" o "final" se necessario, ma di base cerchiamo la stagione corrente
            var url = $"https://api.sportmonks.com/v3/football/standings/leagues/271?api_token={_apiKey}&include=details.type;participant";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                // Se continua a dare 404, proviamo l'endpoint generico con filtro per league_id
                var backupUrl = $"https://api.sportmonks.com/v3/football/standings?api_token={_apiKey}&filters=leagueIds:271&include=details.type;participant";
                response = await _httpClient.GetAsync(backupUrl);
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Errore API {response.StatusCode}: Il piano potrebbe non includere la classifica per questo ID.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);

            // In alcuni piani, i dati sono dentro un array "data". Prendiamo il primo elemento.
            var dataProp = doc.RootElement.GetProperty("data");

            if (dataProp.ValueKind == JsonValueKind.Array && dataProp.GetArrayLength() > 0)
            {
                return dataProp; // Restituiamo l'intero array di righe della classifica
            }

            return dataProp;
        }

        private async Task<JsonElement> GetJsonData(string url)
        {
            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Errore 401: Token non valido o scaduto. Controlla la tua dashboard.");
            }

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonDocument.Parse(json).RootElement.GetProperty("data");
        }
    }
}