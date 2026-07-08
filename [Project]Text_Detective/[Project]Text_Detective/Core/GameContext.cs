using System;

namespace _Project_Text_Detective
{

    public class GameContext
    {
    private const int MaxLogCount = 5;
        public GameContext(GameManager game)
        {
            Game = game;
        }

        public Random Random { get; } = new Random();

        public GameManager Game { get; }

        public Player Player { get; set; }
        public bool IsRunning { get; set; } = true;


        // 로그
        public List<string> Logs { get; } = new List<string>();


        public void AddLog(string message)
        {
            Logs.Add($"[{DateTime.Now:HH:mm:ss}] {message}");

            while (Logs.Count > MaxLogCount)
            {
                Logs.RemoveAt(0);
            }
        }
    }

}