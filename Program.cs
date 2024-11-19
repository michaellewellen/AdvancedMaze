using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        Console.CursorVisible = false;

        int difficulty = GetDifficultyLevel();
        Console.Clear();
        
        GameLogic game = new GameLogic(50,25, difficulty);
        game.DrawMaze();
        

        game.RunGameLoop();
    }

    static int GetDifficultyLevel()
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