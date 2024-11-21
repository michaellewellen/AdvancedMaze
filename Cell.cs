using System.Security.Cryptography.X509Certificates;

public class Cell 
{
    public int TopLeftX { get;}
    public int TopLeftY { get;}
    public bool NorthWall {get; set;} = true;
    public bool SouthWall {get; set;} = true;
    public bool EastWall {get; set;} = true;
    public bool WestWall {get; set;} = true;

    public bool HasDoor {get; set;} = false;

    public Cell (int x, int y)
    {
        TopLeftX = x;
        TopLeftY = y;
    }

      public void DisplayCell(Cell[,] mazeGrid, int mazeWidth, int mazeHeight)
    {
        char symbol = '*';
        if (HasDoor)
            symbol = '|';
        // Draw the top wall only if it exists
        if(NorthWall)
        {
            Console.SetCursorPosition(TopLeftX, TopLeftY);
            Console.Write("******");
        }
        else 
        {
            Console.SetCursorPosition(TopLeftX + 1, TopLeftY);
            Console.Write("    ");            
        }
        if(EastWall)
        {
            Console.SetCursorPosition(TopLeftX + 5, TopLeftY );
            Console.Write("*");
            Console.SetCursorPosition(TopLeftX + 5, TopLeftY + 1);
            Console.Write(symbol);
            Console.SetCursorPosition(TopLeftX + 5, TopLeftY + 2);
            Console.Write(symbol);
            Console.SetCursorPosition(TopLeftX + 5, TopLeftY + 3);
            Console.Write("*");
        }
        else
        {
            Console.SetCursorPosition(TopLeftX + 5, TopLeftY + 1);
            Console.Write(" ");
            Console.SetCursorPosition(TopLeftX + 5, TopLeftY + 2);
            Console.Write(" ");
        }
        if(WestWall)
        {
            Console.SetCursorPosition(TopLeftX, TopLeftY);
            Console.Write("*");
            Console.SetCursorPosition(TopLeftX, TopLeftY + 1);
            Console.Write(symbol);
            Console.SetCursorPosition(TopLeftX, TopLeftY + 2);
            Console.Write(symbol);
            Console.SetCursorPosition(TopLeftX, TopLeftY + 3);
            Console.Write("*");
        }
        else
        {
            Console.SetCursorPosition(TopLeftX, TopLeftY + 1);
            Console.Write(" ");
            Console.SetCursorPosition(TopLeftX, TopLeftY + 2);
            Console.Write(" ");
        }
        if(SouthWall)
        {
            Console.SetCursorPosition(TopLeftX, TopLeftY + 3);
            Console.Write("******");
        }
        else
        {
            Console.SetCursorPosition(TopLeftX + 1, TopLeftY + 3);
            Console.Write("    ");
        }

        
    }


        
   
}