public class Maze
{
    private int Rows { get; }
    private int Columns { get; }
    public Cell[,] Cells { get; }

    public Maze(int rows, int cols)
    {
        Rows = rows;
        Columns = cols;
        Cells = new Cell[Rows, Columns];

        // Initialize cells with their top-left positions
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                int x = j * 5; 
                int y = i * 3; 
                Cells[i, j] = new Cell(x, y);
            }
        }

        InsertVault();
        GeneratePaths();
        ToGrid();
    }

    // Display the entire maze
    public void DisplayMaze()
    {
        Console.Clear();

        // Iterate through all cells and display them
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                Cells[row, col].DisplayCell(Cells, Columns, Rows);
            }
        }
    }

    public void InsertVault()
    {
        Cells[0,0].EastWall = false;
        Cells[0,0].SouthWall = false;
        Cells[0,1].WestWall = false;
        Cells[0,1].SouthWall = false;
        Cells[1,0].NorthWall = false;
        Cells[1,0].EastWall = false;
        Cells[1,1].NorthWall = false;
        Cells[1,1].WestWall = false;
        Cells[1,1].HasDoor = true;
        Cells[1,2].HasDoor = true;
    }

    public void GeneratePaths()
    {
        Random rand = new Random();
        HashSet<(int, int)> visited = new HashSet<(int, int)>();
        Stack<(int,int)> stack = new Stack<(int, int)>();

        // start in the bottom right corner
        stack.Push((Rows-1,Columns-1));
        visited.Add((Rows-1,Columns-1));
        Console.WriteLine($"Starting at ({Rows - 1}, {Columns - 1})");

        while (stack.Count > 0)
        {
            var (currentRow,currentCol) = stack.Peek();
            List<(int,int)> neighbors = GetUnvisitedNeighbors(currentRow, currentCol, visited);
            Console.WriteLine($"Current Cell: ({currentRow}, {currentCol}), Neighbors: {neighbors.Count}");

            if (neighbors.Count > 0)
            {
                var (nextRow, nextCol) = neighbors[rand.Next(neighbors.Count)];

                Console.WriteLine($"Moving to ({nextRow}, {nextCol})");
                RemoveWallBetween(currentRow, currentCol, nextRow, nextCol);

                visited.Add((nextRow, nextCol));
                stack.Push((nextRow, nextCol));
            }
            else
            {
                Console.WriteLine($"Backtracking from ({currentRow}, {currentCol})");
                stack.Pop();
            }
        }
    }

    private List<(int, int)> GetUnvisitedNeighbors(int row, int col, HashSet<(int,int)> visited)
    {
        var neighbors = new List<(int,int)>();
        // Check north neighbor
        if (row > 0 && !visited.Contains((row-1,col)) && !IsVaultCell(row-1,col))
        {
            neighbors.Add((row-1,col));
            Console.WriteLine("Cell has a northern neighbor");
        }
        // Check south neighbor
        if (row < Rows -1 && !visited.Contains((row+1,col)) && !IsVaultCell(row+1,col))
        {
            neighbors.Add((row+1,col));
            Console.WriteLine("Cell has a southern neighbor");
        }
        // Check west neighbor
        if (col > 0 && !visited.Contains((row,col-1)) && !IsVaultCell(row,col-1))
        {
            neighbors.Add((row,col-1));
            Console.WriteLine("Cell has an Western Neighbor");
        }
        // Check east neighbor
        if (col < Columns-1 && !visited.Contains((row,col+1)) && !IsVaultCell(row,col+1))
        {
            neighbors.Add ((row, col+1));
            Console.WriteLine("Cell has an Eastern neighbor");
        }

        neighbors = neighbors.OrderBy(_ => Guid.NewGuid()).ToList();
        return neighbors;
    }

    private bool IsVaultCell(int row, int col)
    {
        return (row == 0 && col == 0) || (row == 0 && col ==1) || (row == 1 && col == 0) || (row == 1 && col == 1);
    }

    private void RemoveWallBetween(int row1, int col1, int row2, int col2)
    {
        if (row1 == row2) // same row
        {
            if(col1 < col2) 
            {
                Cells[row1, col1].EastWall = false;
                Cells[row2, col2].WestWall = false;
            }
            else
            {
                Cells[row1, col1].WestWall = false;
                Cells[row2, col2].EastWall = false;
            }
        }
         else if (col1 == col2) // Same column
        {
            if (row1 < row2) // Cell2 is to the south of Cell1
            {
                Cells[row1, col1].SouthWall = false;
                Cells[row2, col2].NorthWall = false;
            }
            else // Cell2 is to the north of Cell1
            {
                Cells[row1, col1].NorthWall = false;
                Cells[row2, col2].SouthWall = false;
            }
        }
    }
   
    public char[,] ToGrid()
    {
        int gridRows = Rows * 3 + 1; // Account for the bottom wall of the last row
        int gridCols = Columns * 5 + 1; // Account for the right wall of the last column
        char[,] grid = new char[gridRows, gridCols];

        // Fill the grid with spaces initially
        for (int i = 0; i < gridRows; i++)
        {
            for (int j = 0; j < gridCols; j++)
            {
                grid[i, j] = ' ';
            }
        }

        for (int row = 0; row < Rows; row ++)
        {
            for (int col = 0; col < Columns; col ++)
            {
                Cells[row, col].WriteCellToGrid(grid);
            }
        }
        return grid;
    }    
}
