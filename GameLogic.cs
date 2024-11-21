using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public class GameLogic
{
    public char[,] grid;
    private Player player;
    private List<Collectible> collectibles;
    private List<Enemy> enemies;
    private Maze maze; 

    public GameLogic(int rows, int cols, int numCoins, int difficulty)
    {
        maze = new Maze(rows, cols);
        grid = maze.ToGrid();

        int numEnemies = difficulty * 3;
        InitializePlayer();
        InitializeCollectibles(numCoins);
        InitializeEnemies(numEnemies);   
        InitializeExit();     
    }

    public void DisplayInitialGrid()
    {
        Console.Clear();

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Console.Write(grid[i, j]);
            }
            Console.WriteLine();
        }

        // Move the cursor to the score area
        Console.SetCursorPosition(0, grid.GetLength(0));
        Console.WriteLine($"Score: {player.Score}");
    }

    private void UpdateScoreDisplay()
    {
        Console.SetCursorPosition(0, grid.GetLength(0));
        Console.WriteLine($"Score: {player.Score}        ");
    }

    private void UpdateCellDisplay(int row, int col, char symbol)
    {
        Console.SetCursorPosition(col, row);
        Console.Write(symbol);
    }
    private void InitializeExit()
    {
        int exitRow = 1*3 + 1;
        int exitCol = 1*5 + 2;
        grid[exitRow,exitCol] = '#';
        Console.SetCursorPosition(exitCol,exitRow);
        Console.Write("#");
    }
    private void InitializePlayer()
    {
        var (row,col) = FindRandomEmptyPosition();
        player = new Player(row,col);
        grid[row,col] = '@';
    }

    private void InitializeCollectibles(int numCoins)
    {
        collectibles = new List<Collectible>();

        for (int i = 0; i < numCoins; i++)
        {
            var(row, col) = FindRandomEmptyPosition();
            var coin = new Collectible(row, col, '^');
            collectibles.Add(coin);
            grid[row,col] = '^';
        }
         var (treasureRow, treasureCol) = FindRandomEmptyPosition();
         var treasure = new Collectible(treasureRow, treasureCol, '$');
         collectibles.Add(treasure);
         grid[treasureRow,treasureCol] = '$';
    }

    private void InitializeEnemies(int numEnemies)
    {
        enemies = new List<Enemy>();

        for (int i = 0; i < numEnemies; i++)
        {
            var (row,col) = FindRandomEmptyPosition();
            var enemy = new Enemy(row, col);
            enemies.Add(enemy);
            grid[row,col] = '%';
        }
    }

    private (int,int) FindRandomEmptyPosition()
    {
        Random rand = new Random();
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        while (true)
        {
            int row = rand.Next(rows);
            int col = rand.Next(cols);

            if (grid[row,col] == ' ')
            {
                return (row, col);
            }
        }
    }

    public void DisplayGrid()
    {
        Console.Clear();

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j ++)
            {
                Console.Write(grid[i,j]);
            }
        }


    }

    public async Task MoveEnemiesAsync(int difficulty)
    {
        int delay = 500 / difficulty; // Speed increases with difficulty

        while (true)
        {
            MoveEnemies(); // Move enemies based on their logic
            await Task.Delay(delay); // Pause before moving again
        }
    }


    public void MovePlayer(ConsoleKey direction)
    {
        int newRow = player.Row, newCol = player.Col;

        switch (direction)
        {
            case ConsoleKey.UpArrow:
                if (newRow > 0 && grid[newRow - 1, newCol] != '*') newRow--;
                break;
            case ConsoleKey.DownArrow:
                if (newRow < grid.GetLength(0) - 1 && grid[newRow + 1, newCol] != '*') newRow++;
                break;
            case ConsoleKey.LeftArrow:
                if (newCol > 0 && grid[newRow, newCol - 1] != '*') newCol--;
                break;
            case ConsoleKey.RightArrow:
                if (newCol < grid.GetLength(1) - 1 && grid[newRow, newCol + 1] != '*') newCol++;
                break;
        }

        HandlePlayerMove(newRow, newCol);
    }

    private void HandlePlayerMove(int newRow, int newCol)
    {
        // Check for collectibles
        var collectible = collectibles.FirstOrDefault(c => c.Row == newRow && c.Col == newCol);
        if (collectible != null)
        {
            if (collectible.Symbol == '^')
            {
                player.CollectCoin();
            }
            else if (collectible.Symbol == '$')
            {
                player.CollectTreasure();
            }

            // Remove the collectible
            collectibles.Remove(collectible);
            grid[newRow, newCol] = ' ';

            if(player.Score >= 1000)
            {
                UnlockVault();
            }
            UpdateScoreDisplay();
        }

        
        if (grid[newRow, newCol] == '#')
        {
            Console.Clear();
            Console.WriteLine("Congratulations! You win!");
            Environment.Exit(0);
        }
    
        // Update player position
       UpdateCellDisplay(player.Row, player.Col, ' ');
       player.Move(newRow, newCol);
       UpdateCellDisplay(newRow, newCol, '@');
    }

    private void UnlockVault()
    {
        Console.WriteLine (" Vault unlocked");
        grid[1*3 + 1, 2*5] = ' ';
        grid[1*3 + 1, 1*5 + 4] = ' ';
    }
    public void MoveEnemies()
    {
        Random rand = new Random();    
        
        foreach (var enemy in enemies)
        {
            int oldRow = enemy.Row;
            int oldCol = enemy.Col;

            enemy.MoveTowardsPlayer(player.Row, player.Col, grid, rand);

            if (enemy.Row != oldRow || enemy.Col != oldCol)
            {
                UpdateCellDisplay(oldRow, oldCol, ' ');
                UpdateCellDisplay(enemy.Row, enemy.Col, '%');
            }

        }
    }

    public bool IsGameOver()
    {
        return enemies.Any(e => e.Row == player.Row && e.Col == player.Col); // Enemy catches the player
    }

}