using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OktaWebSocketDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly IGameRepository _repository;

        public LeaderboardController(IGameRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<HighScore> Get()
        {
            var scores = _repository.HighScores.OrderByDescending(s => s.Percentage).Take(10);

            return scores.ToArray();
        }
    }
}