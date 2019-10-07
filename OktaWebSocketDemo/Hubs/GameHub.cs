using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using OktaWebSocketDemo;

namespace OktaWebSocketDemo.Hubs
{
    public interface IGameClient
    {
        Task RenderBoard(string[][] board);
        Task Color(string color);
        Task Turn(string player);
        Task RollCall(Player player1, Player player2);
        Task Concede();
        Task Victory(string player, string[][] board);
    }

    public class GameHub : Hub<IGameClient>
    {
        private IGameRepository _repository;
        private readonly Random _random;

        public GameHub(IGameRepository repository, Random random)
        {
            _repository = repository;
            _random = random;
        }

        public async Task UpdateUser(string email, string name)
        {
            var game = _repository.Games.FirstOrDefault(g => g.HasPlayer(Context.ConnectionId));
            if (game != null)
            {
                var player = game.GetPlayer(Context.ConnectionId);
                player.Email = email;
                player.Name = name;
                await Clients.Group(game.Id).RollCall(game.Player1, game.Player2);
            }
        }

        public async Task ColumnClick(int column)
        {
            var game = _repository.Games.FirstOrDefault(g => g.HasPlayer(Context.ConnectionId));
            if (game is null)
            {
                //Ignore click if no game exists
                return;
            }

            if (Context.ConnectionId != game.CurrentPlayer.ConnectionId)
            {
                //Ignore player clicking if it's not their turn
                return;
            }

            //ignore games that havent started
            if (!game.InProgress) return;

            var result = game.TryGetNextOpenRow(column);

            // find first open spot in the column
            if (!result.exists)
            {
                //ignore clicks on full columns
                return;
            }

            await Clients.Group(game.Id.ToString()).RenderBoard(game.Board);

            // Check victory (only current player can win)
            if (game.CheckVictory(result.row, column))
            {
                if (game.CurrentPlayer == game.Player1)
                {
                    UpdateHighScore(game.Player1, game.Player2);
                }
                else
                {
                    UpdateHighScore(game.Player2, game.Player1);
                }

                await Clients.Group(game.Id).Victory(game.CurrentPlayer.Color, game.Board);
                _repository.Games.Remove(game);
                return;
            }

            game.NextPlayer();

            await Clients.Group(game.Id).Turn(game.CurrentPlayer.Color);
        }

        public override async Task OnConnectedAsync()
        {
            //Find a game or create a new one
            var game = _repository.Games.FirstOrDefault(g => !g.InProgress);
            if (game is null)
            {
                game = new Game();
                game.Id = Guid.NewGuid().ToString();
                game.Player1.ConnectionId = Context.ConnectionId;
                _repository.Games.Add(game);
            }
            else
            {
                game.Player2.ConnectionId = Context.ConnectionId;
                game.InProgress = true;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, game.Id);
            await base.OnConnectedAsync();

            if (game.InProgress)
            {
                CoinToss(game);
                await Clients.Group(game.Id.ToString()).RenderBoard(game.Board);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //If game is complete delete it
            var game = _repository.Games.FirstOrDefault(g => g.Player1.ConnectionId == Context.ConnectionId || g.Player2.ConnectionId == Context.ConnectionId);
            if (!(game is null))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, game.Id);
                await Clients.Group(game.Id).Concede();
                _repository.Games.Remove(game);
            }

            await base.OnDisconnectedAsync(exception);
        }

        private void UpdateHighScore(Player winner, Player loser)
        {
            var winnerScore = _repository.HighScores.FirstOrDefault(s => s.PlayerName == winner.Name);
            if (winnerScore == null)
            {
                winnerScore = new HighScore { PlayerName = winner.Name };
                _repository.HighScores.Add(winnerScore);
            }

            winnerScore.Played++;
            winnerScore.Won++;
            winnerScore.Percentage = Convert.ToInt32((winnerScore.Won / Convert.ToSingle(winnerScore.Played)) * 100);

            var loserScore = _repository.HighScores.FirstOrDefault(s => s.PlayerName == loser.Name);
            if (loserScore == null)
            {
                loserScore = new HighScore { PlayerName = winner.Name };
                _repository.HighScores.Add(loserScore);
            }

            loserScore.Played++;
            loserScore.Percentage = Convert.ToInt32((loserScore.Won / Convert.ToSingle(loserScore.Played)) * 100);
        }

        private async void CoinToss(Game game)
        {
            var result = _random.Next(2);
            if (result == 1)
            {
                game.Player1.Color = Game.RedCell;
                game.Player2.Color = Game.YellowCell;
                game.CurrentPlayer = game.Player1;
            }
            else
            {
                game.Player1.Color = Game.YellowCell;
                game.Player2.Color = Game.RedCell;
                game.CurrentPlayer = game.Player2;
            }

            await Clients.Client(game.Player1.ConnectionId).Color(game.Player1.Color);
            await Clients.Client(game.Player2.ConnectionId).Color(game.Player2.Color);
            await Clients.Group(game.Id).Turn(Game.RedCell);
        }

    }
}