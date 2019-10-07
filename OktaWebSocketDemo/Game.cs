using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OktaWebSocketDemo
{
    public class Game
    {
        public const string EmptyCell = "white";
        public const string RedCell = "red";
        public const string YellowCell = "yellow";
        public const string HighlightCell = "orange";
        public const int NumberOfRows = 6;
        public const int NumberOfColumns = 7;
        public const int ChainLengthToWin = 4;

        public Game()
        {
            PopulateBoard();
            Player1 = new Player();
            Player2 = new Player();
        }

        public Player GetPlayer(string connectionId)
        {
            if (Player1 != null && Player1.ConnectionId == connectionId)
            {
                return Player1;
            }
            if (Player2 != null && Player2.ConnectionId == connectionId)
            {
                return Player2;
            }
            return null;
        }

        public bool HasPlayer(string connectionId)
        {
            if (Player1 != null && Player1.ConnectionId == connectionId)
            {
                return true;
            }
            if (Player2 != null && Player2.ConnectionId == connectionId)
            {
                return true;
            }
            return false;
        }

        public string Id { get; set; }

        private void PopulateBoard()
        {
            Board = new string[NumberOfRows][];

            for (int x = 0; x < NumberOfRows; x++)
            {
                Board[x] = new string[NumberOfColumns];
                for (int y = 0; y < NumberOfColumns; y++)
                {
                    Board[x][y] = EmptyCell;
                }
            }
        }


        private bool CheckHorizontally(int row, int column, string playerColor)
        {
            var startColumn = column;
            var endColumn = column;

            for (var k = 1; k < ChainLengthToWin; ++k)
            {
                var columnToCheck = column - k;
                if (columnToCheck < 0)
                {
                    break;
                }
                if (Board[row][columnToCheck] != playerColor)
                {
                    break;
                }
                startColumn = columnToCheck;
            }
            for (var k = 1; k < ChainLengthToWin; ++k)
            {
                var columnToCheck = column + k;
                if (columnToCheck >= NumberOfColumns)
                {
                    break;
                }
                if (Board[row][columnToCheck] != playerColor)
                {
                    break;
                }
                endColumn = columnToCheck;
            }

            if (endColumn - startColumn >= ChainLengthToWin - 1)
            {
                for (var k = 0; k < ChainLengthToWin; ++k)
                {
                    Board[row][startColumn + k] = HighlightCell;
                }

                return true;
            }

            return false;
        }

        private bool CheckVertically(int row, int column, string playerColor)
        {
            var startRow = row;
            var endRow = row;

            for (var k = 1; k < ChainLengthToWin; ++k)
            {
                var rowToCheck = row - k;
                if (rowToCheck < 0)
                {
                    break;
                }
                if (Board[rowToCheck][column] != playerColor)
                {
                    break;
                }
                startRow = rowToCheck;
            }
            for (var k = 1; k < ChainLengthToWin; ++k)
            {
                var rowToCheck = row + k;
                if (rowToCheck >= NumberOfRows)
                {
                    break;
                }
                if (Board[rowToCheck][column] != playerColor)
                {
                    break;
                }
                endRow = rowToCheck;
            }

            if (endRow - startRow >= ChainLengthToWin - 1)
            {
                for (var k = 0; k < ChainLengthToWin; ++k)
                {
                    Board[startRow + k][column] = HighlightCell;
                }
                return true;
            }

            return false;
        }

        private bool CheckDiagonally(int row, int column, string playerColor)
        {

            var startRow = row;
            var startColumn = column;
            var endColumn = column;

            // Check diagonal top-left -> bottom-right        
            // count to top-left
            for (var k = 1; k < ChainLengthToWin; ++k)
            {
                var rowToCheck = row - k;
                var columnToCheck = column - k;
                if (rowToCheck < 0 || columnToCheck < 0)
                {
                    break;
                }
                if (Board[rowToCheck][columnToCheck] != playerColor)
                {
                    break;
                }
                startRow = rowToCheck;
                startColumn = columnToCheck;
            }

            // count to bottom-right
            for (var k = 1; k < ChainLengthToWin; ++k)
            {
                var rowToCheck = row + k;
                var columnToCheck = column + k;
                if (rowToCheck >= NumberOfRows || columnToCheck >= NumberOfColumns)
                {
                    break;
                }
                if (Board[rowToCheck][columnToCheck] != playerColor)
                {
                    break;
                }

                endColumn = columnToCheck;
            }

            if (endColumn - startColumn >= ChainLengthToWin - 1)
            {
                for (var k = 0; k < ChainLengthToWin; ++k)
                {
                    Board[startRow + k][startColumn + k] = HighlightCell;
                }

                return true;
            }

            startRow = row;
            startColumn = column;
            endColumn = column;

            // count to bottom-left
            for (var k = 1; k < ChainLengthToWin; ++k)
            {
                var rowToCheck = row + k;
                var columnToCheck = column - k;
                if (rowToCheck >= NumberOfRows || columnToCheck < 0)
                {
                    break;
                }
                if (Board[rowToCheck][columnToCheck] != playerColor)
                {
                    break;
                }
                startRow = rowToCheck;
                startColumn = columnToCheck;
            }

            // count to top-right
            for (var k = 1; k < 4; ++k)
            {
                var rowToCheck = row - k;
                var columnToCheck = column + k;
                if (rowToCheck < 0 || columnToCheck >= NumberOfColumns)
                {
                    break;
                }
                if (Board[rowToCheck][columnToCheck] != playerColor)
                {
                    break;
                }
                endColumn = columnToCheck;
            }




            if (endColumn - startColumn >= ChainLengthToWin - 1)
            {
                for (var k = 0; k < ChainLengthToWin; ++k)
                {
                    Board[startRow - k][startColumn + k] = HighlightCell;
                }

                return true;
            }

            return false;

        }

        public (bool exists, int row) TryGetNextOpenRow(int column)
        {
            int row = -1;
            for (row = Game.NumberOfRows - 1; row >= 0; --row)
            {
                if (Board[row][column] == Game.EmptyCell)
                {
                    Board[row][column] = CurrentPlayer.Color;
                    break;
                }
            }

            if (row != -1)
            {
                return (true, row);
            }

            return (false, 0);


        }

        public void NextPlayer()
        {
            if (CurrentPlayer == Player1)
            {
                CurrentPlayer = Player2;
            }
            else
            {
                CurrentPlayer = Player1;
            }
        }

        public bool CheckVictory(int row, int column)
        {
            var playerColor = Board[row][column];

            if (CheckHorizontally(row, column, playerColor))
            {
                return true;
            }

            if (CheckVertically(row, column, playerColor))
            {
                return true;
            }

            if (CheckDiagonally(row, column, playerColor))
            {
                return true;
            }

            return false;
        }


        public Player Player1 { get; private set; }

        public Player Player2 { get; private set; }

        public Player CurrentPlayer { get; set; }

        public string[][] Board { get; private set; }
        public bool InProgress { get; set; }
    }

    public class Player
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string ConnectionId { get; set; }
        public string Color { get; set; }
    }
}
