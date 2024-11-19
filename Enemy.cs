using System.Data;
using System.Security.Cryptography.X509Certificates;

public class Enemy
{
    public int Row {get; set;}
    public int Col {get; set;}

    
    public Enemy (int row, int col)
    {
        Row = row;
        Col = col;
    }

    public void MoveTowardsPlayer(int playerRow, int playerCol, char[,] maze, Random rand)
    {
        int newRow = Row;
        int newCol = Col;

        int distance = Math.Abs(playerRow - Row) + Math.Abs(playerCol - Col);

        if (distance <= 10 && HasLineOfSight(playerRow, playerCol, maze))
        {
            if (playerRow < Row) newRow -- ;
            else if (playerRow > Row) newRow ++;

            if (playerCol < Col) newCol --;
            else if (playerCol > Col) newCol ++;
        }
        else
        {
            int direction = rand.Next(4);
            switch(direction)
            {
                case 0: if (Row > 0) newRow --; break;
                case 1: if (Row < maze.GetLength(0) - 1) newRow++; break;
                case 2: if (Col > 0) newCol --; break;
                case 3: if (Col < maze.GetLength(1) -1) newCol ++; break;
            }
        }

        if (maze[newRow, newCol] == ' ' || (newRow == playerRow && newCol == playerCol))
        {
            Row = newRow;
            Col = newCol;
        }
    }
    
    public bool HasLineOfSight(int playerRow, int playerCol, char[,] maze)
    {
        if (Row == playerRow)
        {
            int start = Math.Min(Col, playerCol);
            int end = Math.Max(Col, playerCol);
            for (int col = start + 1; col < end; col ++)
            {
                if (maze[Row, col] != ' ') return false;
            }
            return true;
        }
        if (Col == playerCol)
        {
            int start = Math.Min(Row, playerRow);
            int end = Math.Max(Row, playerRow);
            for (int row = start+1; row < end; row ++)
            {
                if(maze[row, Col] != ' ') return false;
            }
            return true;
        }
        return false;
    }
}