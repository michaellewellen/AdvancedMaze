using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    public static void Main(string[] args)
    {
        Console.CursorVisible = false;

        try
        {
            int difficulty = GetDifficultyLevel();
            GameLogic game = new GameLogic(10, 12, 10, difficulty);

            // Start enemy movement in a separate task
            Task.Run(() => game.MoveEnemiesAsync(difficulty));

            ConsoleKey? currentKey = null; // Track the currently pressed key

            while (true)
            {
                // Check if the game is over
                if (game.IsGameOver())
                {
                    Console.SetCursorPosition(0, game.grid.GetLength(0) + 2); // Move cursor below the grid
                    Console.WriteLine("Game Over! You were caught by an enemy.");
                    break;
                }

                // Handle player input
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Escape) break; // Exit the game
                    currentKey = key; // Update the currently pressed key
                }

                if (currentKey != null)
                {
                    game.MovePlayer(currentKey.Value); // Move player based on the key
                    game.DisplayGrid(); // Update the display
                }

                Thread.Sleep(50); // Control the main loop speed
            }
        }
        finally
        {
            Console.CursorVisible = true; // Restore cursor visibility on game exit
            Console.Clear();
            Console.WriteLine("Thanks for playing!");
        }
      
    }
    

    private static int GetDifficultyLevel()
    {
        int difficulty;
        do
        {
            Console.Clear();
            Console.WriteLine("Select Difficulty: \n1 - Easy\n2 - Normal\n3 - Hard");
            int.TryParse(Console.ReadLine(), out difficulty);
        } while (difficulty < 1 || difficulty > 3);
        return difficulty;
    }
}