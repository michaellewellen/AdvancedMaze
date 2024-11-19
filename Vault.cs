public class Vault 
{
    public int TopRow { get; }
    public int BottomRow { get; }
    public int LeftCol { get; }
    public int RightCol { get; }

    public bool IsUnlocked {get; private set; } = false;

    public Vault (int topRow, int bottomRow, int leftCol, int rightCol)
    {
        TopRow = topRow;
        BottomRow = bottomRow;
        LeftCol = leftCol;
        RightCol = rightCol;
    }

    public bool IsInVault(int row, int col)
    {
        return row >= TopRow && row <= BottomRow && col >= LeftCol && col <= RightCol;
    }

    public void Unlock()
    {
        IsUnlocked = true;
    }
}