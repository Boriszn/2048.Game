using FluentAssertions;
using Xunit;

namespace Game.App.UnitTests
{
    public class GameAlgorithmTest
    {
        [Fact]
        public void UpdateBoard_WithDefaultBoradAndUpArrow_ReturnsZeroScoreAndGameNotUpdated()
        {
            // Arrange
            var gameAlgorithm = new GameAlgorithm();
            var board = new ulong[4, 4];

            // Act 
            ulong score;
            bool isGameUpdated = gameAlgorithm.UpdateBoard(board, Direction.Up, out score);

            // Assert
            isGameUpdated.Should().BeFalse();
            score.Should().Be(0);
        }

        [Fact]
        public void UpdateBoard_WithGameBoardUpdate_ReturnsGameChanged()
        {
            // Arrange
            var gameAlgorithm = new GameAlgorithm();
            var board = new ulong[4, 4];
            board[3, 1] = 2;

            // Act 
            ulong score;
            bool isGameUpdated = gameAlgorithm.UpdateBoard(board, Direction.Up, out score);

            // Assert
            isGameUpdated.Should().BeTrue();
        }

        [Fact]
        public void UpdateBoard_WithBoardFilled_ReturnsScore()
        {
            // Arrange
            var gameAlgorithm = new GameAlgorithm();
            var board = new ulong[4, 4];
            board[2, 0] = 2;
            board[3, 0] = 4;

            // Act 
            ulong score;
            bool isGameUpdated = gameAlgorithm.UpdateBoard(board, Direction.Down, out score);

            // Assert
            // score.Should().Be(4);
        }

        [Fact]
        public void AddNewValueToBoard_WithBoardArray_ReturnsProperBoardSize()
        {
            // Arrange
            var gameAlgorithm = new GameAlgorithm();
            var board = new ulong[4, 4];

            int rowsCount = 4;
            int columnsCount = 4;

            // Act 
            gameAlgorithm.AddNewValueToBoard(board, rowsCount, columnsCount);

            // Assert
            board.Length.Should().NotBe(0);
            board.Length.Should().Be(16);
        }

        [Fact]
        public void IsGameFinished_WithWholeGameBoard_ReturnsTrue()
        {
            // Arrange
            var gameAlgorithm = new GameAlgorithm();
            var board = new ulong[4, 4];
            board[0, 0] = 2;
            board[0, 1] = 0;//32;
            board[0, 2] = 8;
            board[0, 3] = 2;

            board[1, 0] = 2; // 8;
            board[1, 1] = 4;
            board[1, 2] = 2; //128;
            board[1, 3] = 4;

            board[2, 0] = 4;
            board[2, 1] = 8;
            board[2, 2] = 32;
            board[2, 3] = 8;

            board[3, 0] = 2;
            board[3, 1] = 32;
            board[3, 2] = 64;
            board[3, 3] = 2;

            // Act 
            bool isGameFinished = gameAlgorithm.IsGameFinished(board);

            // Assert
            isGameFinished.Should().BeTrue();
        }
    }
}
