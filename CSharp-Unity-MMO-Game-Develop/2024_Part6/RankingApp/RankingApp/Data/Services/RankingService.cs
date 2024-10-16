using Newtonsoft.Json;
using SharedData.Models;
using System.Text;

namespace RankingApp.Data.Services
{
    // [C ↔ S] ↔ [S] - DB
    public class RankingService
	{
        HttpClient _httpClient;

        public RankingService(HttpClient client)
        {
            _httpClient = client;
        }

        // CREATE
        public async Task<GameResult> AddGameResult(GameResult gameResult)
        {
            // 게임 결과를 JSON 문자열로 변환
            string jsonStr = JsonConvert.SerializeObject(gameResult);
            var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync("api/ranking", content);

            if (result.IsSuccessStatusCode == false)
            {
                throw new Exception("AddGameResult Failed");
            }

            // 응답 내용을 문자열로 읽는다.
            var resultContent = await result.Content.ReadAsStringAsync();

            // 응답 문자열을 GameResult 객체로 변환
            GameResult resGameResult = JsonConvert.DeserializeObject<GameResult>(resultContent);   
            return resGameResult;
        }

        // READ
        public async Task<List<GameResult>> GetGameResultsAsync()
        {
            var result = await _httpClient.GetAsync("api/ranking");

            var resultContent = await result.Content.ReadAsStringAsync();
            List<GameResult> gameResults = JsonConvert.DeserializeObject<List<GameResult>>(resultContent);
            return gameResults;
        }

        // UPDATE
        public async Task<bool> UpdateGameResult(GameResult gameResult)
        {
            // 게임 결과를 JSON 문자열로 변환
            string jsonStr = JsonConvert.SerializeObject(gameResult);
            var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            var result = await _httpClient.PutAsync("api/ranking", content);

            if (result.IsSuccessStatusCode == false)
            {
                throw new Exception("UpdateGameResult Failed");
            }

            return true;
        }

        // DELETE
        public async Task<bool> DeleteGameResult(GameResult gameResult)
        {
            var result = await _httpClient.DeleteAsync($"api/ranking/{gameResult.Id}");

            if (result.IsSuccessStatusCode == false)
            {
                throw new Exception("DeleteGameResult Failed");
            }

            return true;
        }
    }
}
