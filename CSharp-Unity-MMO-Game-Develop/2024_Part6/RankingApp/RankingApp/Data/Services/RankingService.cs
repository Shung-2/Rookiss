using RankingApp.Data.Models;

namespace RankingApp.Data.Services
{
    public class RankingService
	{
		ApplicationDbContext _context;

        public RankingService(ApplicationDbContext context)
        {
            _context = context;
        }

        // CRUD

        // CREATE
        public Task<GameResult> AddGameResult(GameResult gameResult)
        {
            // DB에 데이터를 추가한다.
            _context.GameResults.Add(gameResult);
            _context.SaveChanges();
            return Task.FromResult(gameResult);
        }

        // READ
        public Task<List<GameResult>> GetGameResultsAsync()
		{
			// DB에서 데이터를 가져온다.
			List<GameResult> results = _context.GameResults
									.OrderByDescending(item => item.Score)
									.ToList();
			return Task.FromResult(results);
		}

        // UPDATE
        public Task<bool> UpdateGameResult(GameResult gameResult)
        {
            var findResult = _context.GameResults
                .Where(x => x.Id == gameResult.Id)
                .FirstOrDefault();

            if (findResult == null)
            {
                return Task.FromResult(false);
            }

            findResult.UserName = gameResult.UserName;
            findResult.Score = gameResult.Score;
            // DB 동기화
            _context.SaveChanges();

            // 성공 반환
            return Task.FromResult(true);
        }

        // DELETE
        public Task<bool> DeleteGameResult(GameResult gameResult)
        {
            var findResult = _context.GameResults
                .Where(x => x.Id == gameResult.Id)
                .FirstOrDefault();

            if (findResult == null)
            {
                return Task.FromResult(false);
            }

            _context.GameResults.Remove(gameResult);
            // DB 동기화
            _context.SaveChanges();

            // 성공 반환
            return Task.FromResult(true);
        }
    }
}
