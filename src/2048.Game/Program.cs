using System;
using Microsoft.Extensions.DependencyInjection;

namespace Game.App
{
    class Program
    {
        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            // Bootstrap IoC configuration
            var serviceProvider = Statup.ConfigureServices();

            // Start/Run the Game
            new Game(serviceProvider).Run();
        }
    }

    /// <summary>
    /// The main Game class 
    /// </summary>
    public class Game
    {
        public ulong Score { get; private set; }
        public ulong[,] Board { get; }

        private readonly int rowsCount;
        private readonly int columnsCount;

        private const string UserArrowConstantMessage = "Use arrow keys to move. Press Ctrl-C to exit.";
        private const string PressAnyKeyToExitMessage = "Press any key to quit...";

        /// <summary>
        /// The game algorithm
        /// </summary>
        private readonly IGameAlgorithm gameAlgorithm;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game" /> class.
        /// </summary>
        public Game(IServiceProvider serviceProvider)
        {
            gameAlgorithm = serviceProvider.GetService<IGameAlgorithm>();
            this.Board = new ulong[4, 4];
            this.rowsCount = this.Board.GetLength(0);
            this.columnsCount = this.Board.GetLength(1);
            this.Score = 0;
        }

        /// <summary>
        /// Runs the game.
        /// </summary>
        public void Run()
        {
            bool isUpdated = true;
            do
            {
                if (isUpdated)
                {
                    this.gameAlgorithm.AddNewValueToBoard(this.Board, rowsCount, columnsCount);
                }

                // Runs the dislaplaying the Game (and Game itself)
                DisplayGrid();

                // Check if the Game was finished then stop and display message
                if (IsGameFinished())
                {
                    break;
                }
                    
                Console.WriteLine(UserArrowConstantMessage);
                ConsoleKeyInfo input = Console.ReadKey(true);
                Console.WriteLine(input.Key.ToString());

                isUpdated = ProcessGame(input, isUpdated);
            }
            while (true);

            Console.WriteLine(PressAnyKeyToExitMessage);
            Console.Read();
        }

        /// <summary>
        /// Processes the game.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="isUpdated">if set to <c>true</c> [is updated].</param>
        /// <returns></returns>
        private bool ProcessGame(ConsoleKeyInfo input, bool isUpdated)
        {
            switch (input.Key)
            {
                case ConsoleKey.UpArrow:
                    isUpdated = UpdateGameBoard(Direction.Up);
                    break;

                case ConsoleKey.DownArrow:
                    isUpdated = UpdateGameBoard(Direction.Down);
                    break;

                case ConsoleKey.LeftArrow:
                    isUpdated = UpdateGameBoard(Direction.Left);
                    break;

                case ConsoleKey.RightArrow:
                    isUpdated = UpdateGameBoard(Direction.Right);
                    break;

                default:
                    isUpdated = false;
                    break;
            }
            return isUpdated;
        }

        /// <summary>
        /// Determines whether [is game finished].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is game finished]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsGameFinished()
        {
            if (this.gameAlgorithm.IsGameFinished(Board))
            {
                using (new ColorOutput(ConsoleColor.Red))
                {
                    Console.WriteLine("The game was finished");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Displays the grid (with collored items) and the score in the console
        /// </summary>
        private void DisplayGrid()
        {
            Console.Clear();
            Console.WriteLine();
            for (int row = 0; row < rowsCount; row++)
            {
                for (int column = 0; column < columnsCount; column++)
                {
                    using (new ColorOutput(ConsoleUtils.GetConsoleColor(Board[row, column])))
                    {
                        Console.Write("{0,6}", Board[row, column]);
                    }
                }

                Console.WriteLine();
                Console.WriteLine();
            }

            Console.WriteLine("Score: {0}", this.Score);
            Console.WriteLine();
        }

        /// <summary>
        /// Updates the specified .
        /// </summary>
        /// <param name="dirrection">The input arrow dirrection.</param>
        /// <returns></returns>
        private bool UpdateGameBoard(Direction dirrection)
        {
            ulong score;
            // gets the is updated and score
            bool isUpdated = this.gameAlgorithm.UpdateBoard(this.Board, dirrection, out score);
            this.Score += score;
            return isUpdated;
        }
    }
}