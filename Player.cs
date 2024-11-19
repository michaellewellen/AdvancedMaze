public class Player 
{
    public int Row {get; private set;}
    public int Col {get; private set;}
    public int Score {get; private set;} = 0;

    public Player(int startRow, int startCol)
    {
        Row = startRow;
        Col = startCol;
    }
    public void Move(int newRow, int newCol)
    {
        Row = newRow;
        Col = newCol;
    }

    public void CollectCoin()
    {
        Score += 100;
    }

    public void CollectTreasure()
    {
        Score += 1000;
    }

}