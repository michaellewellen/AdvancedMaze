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
            GameLogic game = new GameLogic(15, 15, 10, difficulty);
            game.DisplayInitialGrid();

            Task.Run(() => game.MoveEnemiesAsync(difficulty));
        

            while (true)
            {
                if (game.IsGameOver())
                {
                    Console.SetCursorPosition(0, 15*3 + 3); // Move cursor below the score
                    Console.WriteLine("Game Over! You were caught by an enemy.");
                    break;
                }

                ConsoleKey key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Escape) break;

                game.MovePlayer(key); // Handle player movement
               
            }
        }

        finally
        {
            Console.CursorVisible = true;
            Console.Clear();
            Console.WriteLine("Thanks for playing");
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