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

        public GameManager Game { get; }

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