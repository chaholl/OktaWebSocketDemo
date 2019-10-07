using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OktaWebSocketDemo
{
    public interface IGameRepository
    {
        List<Game> Games { get; }

        List<HighScore> HighScores { get; }
    }

    public class GameRepository : IGameRepository
    {
        public GameRepository()
        {
            HighScores = new List<HighScore> { new HighScore { PlayerName = "aaa", Played = 0, Won = 0, Percentage=0 },
            new HighScore { PlayerName = "bbb", Played = 0, Won = 0, Percentage=0 },
            new HighScore { PlayerName = "ccc", Played = 0, Won = 0, Percentage=0 },
            new HighScore { PlayerName = "ddd", Played = 0, Won = 0, Percentage=0 },
            new HighScore { PlayerName = "eee", Played = 0, Won = 0, Percentage=0 },
            new HighScore { PlayerName = "fff", Played = 0, Won = 0, Percentage=0 },
            new HighScore { PlayerName = "ggg", Played = 0, Won = 0, Percentage=0 },
            new HighScore { PlayerName = "hhh", Played = 0, Won = 0, Percentage=0 },
            new HighScore { PlayerName = "iii", Played = 0, Won = 0, Percentage=0 },
            new HighScore { PlayerName = "jjj", Played = 0, Won = 0, Percentage=0 }
            };
        }

        public List<Game> Games { get; } = new List<Game>();

        public List<HighScore> HighScores { get; } = new List<HighScore>();

    }
}
