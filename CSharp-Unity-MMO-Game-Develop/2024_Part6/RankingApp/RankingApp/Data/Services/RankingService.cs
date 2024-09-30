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

        public Task<List<GameResult>> GetGameResultsAsync()
		{
			// DB에서 데이터를 가져온다.
			List<GameResult> results = _context.GameResults
									.OrderByDescending(item => item.Score)
									.ToList();
			return Task.FromResult(results);
		}
	}
}
