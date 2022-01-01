using System;

namespace CardGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Engine game = new Engine(1280, 720, "CardGames");
            game.DisplayManager.TargetWidth = 1920;
            game.DisplayManager.TargetHeight = 1080;
            game.SceneManager.AddScene("BlackJack", new BlackJack(), true);
            game.Run();
        }
    }
}
