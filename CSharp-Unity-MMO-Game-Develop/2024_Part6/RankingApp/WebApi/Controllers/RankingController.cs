using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedData.Models;
using WebApi.Data;

namespace WebApi.Controllers
{
    // REST (Representational State Transfer)
    // 공식 효준 스펙은 아니다.
    // 원래 있던 HTTP 통신에서 기능을 '재사용'해서 데이터 송수신 규칙을 만든 것

    // CRUD

    // Create
    // POST
    // /api/ranking
    // → 아이템 생성 요청 (Body에 실제 정보 담아서 보냄)

    // Read
    // GET      
    // /api/ranking
    // → 모든 아이템 목록 요청
    // /api/ranking/1
    // → id 1번 아이템 요청

    // Update
    // PUT
    // /api/ranking (보안 문제로 웹에서는 사용하지 않음)
    // 아이템 갱신 요청 (Body에 실제 정보 담아서 보냄)

    // Delete
    // DELETE
    // /api/ranking/1 (보안 문제로 웹에서는 사용하지 않음)
    // → id 1번 아이템 삭제 요청

    // ApiController 특징
    // 1. 그냥 C# 객체를 반환해도 된다.
    // null을 반환하면 클라이언트에 204 Response (No Content)를 보낸다.
    // string → text/plain
    // 나머지(int, bool) → application/json

    [Route("api/[controller]")]
    [ApiController]
    public class RankingController : ControllerBase
    {
        ApplicationDbContext _context;

        public RankingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create
        [HttpPost]
        public GameResult AddGameResult([FromBody]GameResult gameResult)
        {
            _context.GameResults.Add(gameResult);
            _context.SaveChanges();

            return gameResult;
        }

        // Read
        [HttpGet]
        public List<GameResult> GetGameResults()
        {
            List<GameResult> results = _context.GameResults
                .OrderByDescending(item => item.Score)
                .ToList();

            return results;
        }

        [HttpGet("{id}")]
        public GameResult GetGameResults(int id)
        {
            GameResult result = _context.GameResults
                .Where(item => item.Id == id)
                .FirstOrDefault();

            return result;
        }

        // Update
        [HttpPut]
        public bool UpdateGameResult([FromBody] GameResult gameResult)
        {
            GameResult findResult = _context.GameResults
                .Where(x => x.Id == gameResult.Id)
                .FirstOrDefault();

            if (findResult == null)
            {
                return false;
            }

            findResult.UserName = gameResult.UserName;
            findResult.Score = gameResult.Score;
            _context.SaveChanges();

            return true;
        }

        // Delete
        [HttpDelete("{id}")]
        public bool DeleteGameResult(int id)
        {
            GameResult findResult = _context.GameResults
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (findResult == null)
            {
                return false;
            }

            _context.GameResults.Remove(findResult);
            _context.SaveChanges();

            return true;
        }
    }
}
