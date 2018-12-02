using System;

namespace Check24.Task
{
    public class ColorOutput : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorOutput" /> class.
        /// </summary>
        /// <param name="foregroundColor">Color of the foreground.</param>
        /// <param name="backgroundColor">Color of the console background.</param>
        public ColorOutput(ConsoleColor foregroundColor, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Console.ResetColor();
        }
    }
}