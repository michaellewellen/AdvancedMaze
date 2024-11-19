public class InputHandler 
{
    public Action OnUp { get; set; }
    public Action OnDown { get; set; }
    public Action OnLeft { get; set; }
    public Action OnRight { get; set; }

    public void HandleInput(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.UpArrow: OnUp?.Invoke(); break;
            case ConsoleKey.DownArrow: OnDown?.Invoke(); break;
            case ConsoleKey.LeftArrow: OnLeft?.Invoke(); break;
            case ConsoleKey.RightArrow: OnRight?.Invoke(); break;
        }
    }
}