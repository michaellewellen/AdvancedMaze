public class Maze
{
    public char[,] Grid { get; private set; }
    public int Rows => Grid.GetLength(0);
    public int Columns => Grid.GetLength(1);
    public Vault Vault { get; private set; } // Expose the Vault for other operations

    public Maze(int rows, int cols)
    {
        if (rows < 5 || cols < 5) throw new ArgumentException("Maze dimensions must be at least 5x5.");
        GenerateMaze(rows, cols); // Generate a random maze
    }

    private void GenerateMaze(int rows, int cols)
    {
        Grid = new char[rows, cols];

        // Step 1: Fill the perimeter with walls
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Grid[i, j] = (i == 0 || i == rows - 1 || j == 0 || j == cols - 1) ? '*' : ' ';
            }
        }

        // Step 2: Generate a random vault
        PlaceVault(rows, cols);

        // Step 3: Add random walls for vertical symmetry
        Random rand = new Random();
        for (int i = 1; i < rows / 2; i++) // Half rows for vertical symmetry
        {
            for (int j = 1; j < cols - 1; j++)
            {
                Grid[i, j] = (rand.NextDouble() < 0.2) ? '*' : ' ';
            }
        }

        // Step 4: Mirror the top half to the bottom half for symmetry
        for (int i = 1; i < rows / 2; i++)
        {
            for (int j = 1; j < cols - 1; j++)
            {
                Grid[rows - i - 1, j] = Grid[i, j];
            }
        }
    }

    private void PlaceVault(int rows, int cols)
    {
        Random rand = new Random();

        // Determine vault dimensions
        int vaultWidth = rand.Next(5, Math.Min(10, cols - 10)); // Random width
        int vaultHeight = rand.Next(3, Math.Min(6, rows / 3));  // Random height

        // Random position for the vault
        int vaultTop = rows / 4;
        int vaultLeft = cols / 4;

        // Create the Vault object
        Vault  = new Vault(vaultTop, vaultTop + vaultHeight - 1, vaultLeft, vaultLeft + vaultWidth - 1);

        // Build the vault in the maze grid
        for (int i = Vault.TopRow; i <= Vault.BottomRow; i++)
        {
            for (int j = Vault.LeftCol; j <= Vault.RightCol; j++)
            {
                if (i == Vault.TopRow || i == Vault.BottomRow || j == Vault.LeftCol || j == Vault.RightCol)
                {
                    Grid[i, j] = (j == Vault.RightCol && i == (Vault.TopRow + Vault.BottomRow) / 2) ? '#' : '|'; // Door on the side
                }
                else
                {
                    Grid[i, j] = ' ';
                }
            }
        }
    }

    public (int, int) GetRandomStartingPosition()
    {
        if (Vault == null)
        {
            throw new InvalidOperationException("Vault is not initialized");
        }
        Random rand = new Random();
        int row, col;

        do
        {
            row = rand.Next(1, Rows - 1);
            col = rand.Next(1, Columns - 1);
        } while (Grid[row, col] != ' ' || Vault.IsInVault(row, col)); // Use Vault's IsInVault method

        return (row, col);
    }

    public void DrawMaze()
    {
        Console.SetCursorPosition(0, 0); // Reset cursor position to the top-left
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                Console.Write(Grid[i, j]);
            }
            Console.WriteLine();
        }
    }

    public void UpdateCell(int row, int col, char value)
    {
        Grid[row, col] = value;
        Console.SetCursorPosition(col, row);
        Console.Write(value);
    }
}
