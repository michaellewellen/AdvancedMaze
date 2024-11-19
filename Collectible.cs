public class Collectible 
{
    public int Row { get; }
    public int Col { get; }
    public char Symbol { get; }

    public Collectible (int row, int col, char symbol)
    {
        Row = row;
        Col = col;
        Symbol = symbol;
    }
}