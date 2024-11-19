using System.ComponentModel;

public class GameLogic 
{
    private Maze maze;
    private Player player;
    private List<Enemy> enemies;
    private Vault vault;
    private List<Collectible> collectibles;
    private InputHandler inputHandler;
    private bool gameWon = false;
    private CancellationTokenSource cts;
    private int difficulty;

    public GameLogic(int rows, int cols , int difficulty)
    {
        this.difficulty = difficulty;
        maze = new Maze(rows,cols);
        InitializePlayer();
        InitializeVault();
        InitializeEnemies(difficulty);
        InitializeCollectibles(10,9);
        InitializeInputHandler();
    }

    public void DrawMaze()
    {
        maze.DrawMaze();
    }

    private void InitializePlayer()
    {
        (int row, int col) = maze.GetRandomStartingPosition();
        player = new Player(row,col);
        maze.UpdateCell(player.Row, player.Col, '@');
    }
    private void InitializeVault()
    {
        vault = new Vault (7,13,18,34);        
    }

    private void InitializeEnemies(int difficulty)
    {
        enemies = new List<Enemy>();
        int enemyCount = difficulty * 2;
        Random rand = new Random();
        for (int i = 0; i < enemyCount; i ++)
        {
            int row,col;
            do
            {
                row = rand.Next(maze.Rows);
                col = rand.Next(maze.Columns);                
            } while (maze.Grid[row, col] != ' ');
            enemies.Add(new Enemy(row, col));
            maze.UpdateCell(row, col, '%');
        }
    }

    private void InitializeCollectibles(int numCoins, int numVaultTreasures = 1)
    {
        collectibles = new List<Collectible>();
        Random rand = new Random();

        for (int i = 0; i < numCoins; i++)
        {
            int row,col;
            do
            {
                row = rand.Next(maze.Rows);
                col = rand.Next(maze.Columns);
            }while (maze.Grid[row,col] != ' ' || vault.IsInVault(row, col));
            collectibles.Add(new Collectible(row, col, '^'));
            maze.UpdateCell(row, col, '^');
        }
        for (int i = 0; i< numVaultTreasures; i ++)
        {
            int row, col;
            do
            {
                row = rand.Next(vault.TopRow, vault.BottomRow + 1);
                col = rand.Next(vault.LeftCol, vault.RightCol + 1);                
            } while (maze.Grid[row, col] != ' ');
            collectibles.Add(new Collectible(row,col, '$'));
            maze.UpdateCell(row,col,'$');
        }
    }

    private void InitializeInputHandler()
    {
        inputHandler = new InputHandler
        {
            OnUp = () => MovePlayer(player.Row - 1, player.Col),
            OnDown = () => MovePlayer(player.Row + 1, player.Col),
            OnLeft = () => MovePlayer(player.Row, player.Col - 1),
            OnRight = () => MovePlayer(player.Row, player.Col + 1)
        };
    }

    public void RunGameLoop()
    {
        cts = new CancellationTokenSource();
        Task.Run(() => MoveEnemies(cts.Token));

        while (!gameWon)
        {
            ConsoleKey key = Console.ReadKey(true).Key;
            inputHandler.HandleInput(key);
        }

        cts.Cancel();
        Console.CursorVisible = true;
        Console.SetCursorPosition(0, maze.Rows + 2);
        Console.WriteLine("Congratulations, you've won the game!".PadRight(20));
    }

    private void MovePlayer(int newRow, int newCol)
    {
        if(!IsValidMove(newRow, newCol)) return;

        if (maze.Grid[newRow, newCol] == '#')
        {
            gameWon = true;
            maze.UpdateCell(player.Row, player.Col, ' ');
            player.Move(newRow, newCol);
            maze.UpdateCell(player.Row, player.Col, '@');
            return;
        }

        maze.UpdateCell(player.Row, player.Col, ' ');

        var collectible = collectibles.Find(c => c.Row == newRow && c.Col == newCol);
        if (collectible != null)
        {
            if (collectible.Symbol == '$')
            {
                player.CollectTreasure();
            }
            else if (collectible.Symbol == '^')
            {
                player.CollectCoin();
            }
            
            collectibles.Remove(collectible);
            DisplayScore();
        }

        player.Move(newRow, newCol);
        maze.UpdateCell(player.Row, player.Col, '@');
       
        CheckForVault();
        CheckForWin();
    }

    private void DisplayScore()
    {
        Console.SetCursorPosition(0, maze.Rows + 1);
        Console.Write($"Score: {player.Score}".PadRight(20));
    }
    private bool IsValidMove(int row, int col)
    {
        return  row >= 0 && row < maze.Rows &&
                col >= 0 && col < maze.Columns &&
                maze.Grid[row, col] != '*' && 
                maze.Grid[row, col] != '|';
    }

    private void CheckForVault()
    {
        if(player.Score >= 1000 && !vault.IsUnlocked)
        {
            vault.Unlock();
            UnlockVaultInMaze();
        }
    }

    private void UnlockVaultInMaze()
    {
        for (int i = vault.TopRow; i <= vault.BottomRow; i++)
        {
            for (int j = vault.LeftCol; j <= vault.RightCol; j++)
            {
                if (maze.Grid[i,j] == '|')
                {
                    maze.UpdateCell(i, j, ' ');
                }
            }
        }
    }

    private void CheckForWin()
    {
        if (maze.Grid[player.Row, player.Col] == '#')
        {
            gameWon = true;
        }
    }

    private async Task MoveEnemies(CancellationToken token)
    {
        Random rand = new Random();
        int enemyDelay = 500/difficulty; 
        while (!token.IsCancellationRequested)
        {
            foreach (var enemy in enemies)
            {
                maze.UpdateCell(enemy.Row, enemy.Col, ' ');
                enemy.MoveTowardsPlayer(player.Row, player.Col, maze.Grid, rand);
                maze.UpdateCell(enemy.Row, enemy.Col, '%');

                if (enemy.Row == player.Row && enemy.Col == player.Col)
                {
                    GameOver();
                    return;
                }
            }
            await Task.Delay(enemyDelay);
        }
    }

    private void GameOver()
    {
        Console.CursorVisible = true;
        Console.WriteLine("Game Over! You were caught by an enemy.");
        Environment.Exit(0);
    }

}