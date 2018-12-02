namespace Game.App
{
    public interface IGameAlgorithm
    {
        /// <summary>
        /// Updates the specified board.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="score">The score.</param>
        /// <returns></returns>
        bool UpdateBoard(ulong[,] board, Direction direction, out ulong score);

        /// <summary>
        /// Determines whether [is game finished] [the specified board].
        /// </summary>
        /// <param name="board">The board.</param>
        /// <returns>
        ///   <c>true</c> if [is game finished] [the specified board]; otherwise, <c>false</c>.
        /// </returns>
        bool IsGameFinished(ulong[,] board);

        /// <summary>
        /// Adds the new value to board.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="rowsCount">The rows count.</param>
        /// <param name="columnsCount">The columns count.</param>
        void AddNewValueToBoard(ulong[,] board, int rowsCount, int columnsCount);
    }
}