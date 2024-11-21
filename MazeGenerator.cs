public class MazeGenerator
{
    public char[,] GenerateMaze(int rows, int cols)
    {
        if (rows < 5 || cols < 5) throw new ArgumentException("Maze size must be at least 5x5.");

        char[,] maze = new char[rows, cols];

        // Step 1: Fill the perimeter with walls
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (i == 0 || i == rows - 1 || j == 0 || j == cols - 1) 
                {
                
                }
            }
        }

        // Step 2: Generate a random vault
        PlaceVault(maze, rows, cols);

        // Step 3: Add random walls for vertical symmetry
        Random rand = new Random();
        for (int i = 1; i < rows / 2; i++) // Half rows for vertical symmetry
        {
            for (int j = 1; j < cols - 1; j++)
            {
                maze[i, j] = (rand.NextDouble() < 0.2) ? '*' : ' ';
            }
        }

        // Step 4: Mirror the top half to the bottom half for symmetry
        for (int i = 1; i < rows / 2; i++)
        {
            for (int j = 1; j < cols - 1; j++)
            {
                maze[rows - i - 1, j] = maze[i, j];
            }
        }

        return maze;
    }

    private void PlaceVault(char[,] maze, int rows, int cols)
    {
        Random rand = new Random();

        // Determine vault dimensions
        int vaultWidth = rand.Next(5, Math.Min(10, cols - 10)); // Random width
        int vaultHeight = rand.Next(3, Math.Min(6, rows / 3));  // Random height

        // Random position for the vault
        int vaultTop = rows / 4;
        int vaultLeft = cols / 4;

        // Build the vault
        for (int i = 0; i < vaultHeight; i++)
        {
            for (int j = 0; j < vaultWidth; j++)
            {
                if (i == 0 || i == vaultHeight - 1 || j == 0 || j == vaultWidth - 1)
                {
                    maze[vaultTop + i, vaultLeft + j] = (j == vaultWidth - 1 && i == vaultHeight / 2) ? '#' : '|'; // Door on the side
                }
                else
                {
                    maze[vaultTop + i, vaultLeft + j] = ' ';
                }
            }
        }
    }

    public void PrintMaze(char[,] maze)
    {
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                Console.Write(maze[i, j]);
            }
            Console.WriteLine();
        }
    }
}
