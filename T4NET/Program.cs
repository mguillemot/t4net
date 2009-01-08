using System;

namespace T4NET
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (T4Net game = new T4Net())
            {
                game.Run();
            }
        }
    }
}

