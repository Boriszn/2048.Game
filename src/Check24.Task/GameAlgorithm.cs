using System;
using System.Collections.Generic;

namespace Check24.Task
{
    public class GameAlgorithm : IGameAlgorithm
    {
        /// <summary>
        /// Updates the specified board.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="score">The score.</param>
        /// <returns></returns>
        public bool UpdateBoard(ulong[,] board, Direction direction, out ulong score)
        {
            score = 0;
            bool isUpdated = false;
            int rowsCount = board.GetLength(0);
            int columnsCount = board.GetLength(1);

            bool isAlongRow = direction == Direction.Left || direction == Direction.Right;
            bool isIncreasing = direction == Direction.Left || direction == Direction.Up;

            int outterCount = isAlongRow ? rowsCount : columnsCount;
            int innerCount = isAlongRow ? columnsCount : rowsCount;
            int innerStart = isIncreasing ? 0 : innerCount - 1;
            int innerEnd = isIncreasing ? innerCount - 1 : 0;

            /*
             * Local functions for score and board mnipulation
             */
            var drop = 
                isIncreasing
                ? (innerIndex => innerIndex - 1)
                : new Func<int, int>(innerIndex => innerIndex + 1);

            var reverseDrop = 
                isIncreasing
                ? (innerIndex => innerIndex + 1)
                : new Func<int, int>(innerIndex => innerIndex - 1);

            var getScoreValue = 
                isAlongRow
                ? ((boardParam, outterItem, innerItem) => boardParam[outterItem, innerItem])
                : new Func<ulong[,], int, int, ulong>((boardParam, outterItem, innerItem) => boardParam[innerItem, outterItem]);

            var setScoreValue = 
                isAlongRow
                ? ((boardParam, outterItem, newInnerItem, newValue) => boardParam[outterItem, newInnerItem] = newValue)
                : new Action<ulong[,], int, int, ulong>((boardParam, outterItem, newInnerItem, newValue) => boardParam[newInnerItem, outterItem] = newValue);

            /*
            *  Starts the board/score processing
            */
            for (int outterItem = 0; outterItem < outterCount; outterItem++)
            {
                for (int innerItem = innerStart; this.IsInnerCondition(innerItem, innerStart, innerEnd); innerItem = reverseDrop(innerItem))
                {
                    // 1. Validate
                    if (EnsureValueOfOutterAndInnerItemNotZero(board, getScoreValue, outterItem, innerItem))
                    {
                        continue;
                    }

                    // 2. Get new board item 
                    var newInnerItem = CalculateNewItem(board, innerItem, drop, innerStart, innerEnd, getScoreValue, outterItem);

                    bool isInnerCondition = IsInnerCondition(newInnerItem, innerStart, innerEnd);

                    if (
                        isInnerCondition && 
                        getScoreValue(board, outterItem, newInnerItem) == getScoreValue(board, outterItem, innerItem)
                       )
                    {
                        // Merge nodes and save the score
                        ulong newValue = getScoreValue(board, outterItem, newInnerItem) * 2;
                        setScoreValue(board, outterItem, newInnerItem, newValue);
                        setScoreValue(board, outterItem, innerItem, 0);

                        isUpdated = true;
                        score += newValue;
                    }
                    else
                    {
                        // Reverse node and update score then set hasUpdated flag
                        newInnerItem = reverseDrop(newInnerItem); 
                        if (newInnerItem != innerItem)
                        {
                            isUpdated = true;
                        }

                        ulong scoreValue = getScoreValue(board, outterItem, innerItem);
                        setScoreValue(board, outterItem, innerItem, 0);
                        setScoreValue(board, outterItem, newInnerItem, scoreValue);
                    }
                }
            }

            return isUpdated;
        }

        /// <summary>
        /// Determines whether [is game finished] [the specified board].
        /// </summary>
        /// <param name="board">The board.</param>
        /// <returns>
        ///   <c>true</c> if [is game finished] [the specified board]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsGameFinished(ulong[,] board)
        {
            var dirrections = new[] { Direction.Down, Direction.Up, Direction.Left, Direction.Right };
            foreach (var direction in dirrections)
            {
                ulong[,] clonedBoard = (ulong[,])board.Clone();
                ulong score;
                if (this.UpdateBoard(clonedBoard, direction, out score))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Adds the new value to board.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="rowsCount">The rows count.</param>
        /// <param name="columnsCount">The columns count.</param>
        public void AddNewValueToBoard(ulong[,] board, int rowsCount, int columnsCount)
        {
            var random = new Random();
            ulong randomValueOf95Percent = 2;
            ulong randomValueOfTheRest = 4;
            int random95PercentCheck = 95;

            // Find all empty slots
            var emptySlots = new List<Tuple<int, int>>();
            for (int row = 0; row < rowsCount; row++)
            {
                for (int column = 0; column < columnsCount; column++)
                {
                    if (board[row, column] == 0)
                    {
                        emptySlots.Add(new Tuple<int, int>(row, column));
                    }
                }
            }

            // Creates the default random slot
            int randomSlot = random.Next(0, emptySlots.Count);

            // Calculates radom slot value
            ulong value = random.Next(0, 100) < random95PercentCheck ? randomValueOf95Percent : randomValueOfTheRest;

            // Updates the board with calculated random value
            board[ 
                   emptySlots[randomSlot].Item1, 
                   emptySlots[randomSlot].Item2
                 ] = value;
        }

        private int CalculateNewItem(ulong[,] board, int innerItem,
            Func<int, int> drop,
            int innerStart,
            int innerEnd,
            Func<ulong[,], int, int, ulong> getValue,
            int outterItem)
        {
            int newInnerItem = innerItem;
            do
            {
                newInnerItem = drop(newInnerItem);
            }
            // Retrying until it will not reach the end of the boundary
            while
            (
                IsInnerCondition(newInnerItem, innerStart, innerEnd) &&
                getValue(board, outterItem, newInnerItem) == 0
            );
            return newInnerItem;
        }

        private bool IsInnerCondition(int index, int innerStart, int innerEnd)
        {
            int minIndex = Math.Min(innerStart, innerEnd);
            int maxIndex = Math.Max(innerStart, innerEnd);

            return minIndex <= index && index <= maxIndex;
        }

        private static bool EnsureValueOfOutterAndInnerItemNotZero(ulong[,] board, Func<ulong[,], int, int, ulong> getValue, int outterItem, int innerItem)
        {
            if (getValue(board, outterItem, innerItem) == 0)
            {
                return true;
            }

            return false;
        }
    }
}
