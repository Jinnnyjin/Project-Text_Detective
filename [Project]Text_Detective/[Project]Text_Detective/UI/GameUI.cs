using System;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Channels;

namespace _Project_Text_Detective
{
    public static class GameUI
    {
        //===================================
        // 헤드라인
        public static void ShowHeader()
        {

            int lineWidth = 70;
            string title = " 범인은 누구? ";

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('─', lineWidth));
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(GetCentered(lineWidth, title));
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('─', lineWidth));
            Console.ResetColor();
        }

        //===================================
        // 선택 구분창
        public static void DivideSelect()
        {
            int lineWidth = 30;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  ▶ 선택");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('─', lineWidth));
            Console.ResetColor();
        }

        //====================================
        // 중앙
        public static string GetCentered(int lineWidth, string title)
        {
            int padding = (lineWidth - title.Length) / 2;
            string centered = new string(' ', padding) + title;

            return centered;
        }

        //===================================
        // 정수 입력값 받기
        public static int GetIntInput(GameContext context, int min, int max)
        {
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int input))
                {
                    if (input >= min && input <= max)
                    { return input; }
                }
                context.AddLog("\n잘못 입력했습니다. 보기 내 숫자만 입력가능");
            }
        }

        //===================================
        // 함수 - 추리수첩(추리력 기반 일부)
        public static void ShowDiary(Player player)
        {
            int lineWidth = 30;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ▶ 추리 수첩");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('─', lineWidth));
            Console.ResetColor();

            if (player.Clues.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(" ( 비어있음 ) ");
                Console.ResetColor();
            }
            else
            {

                for (int i = 0; i < Math.Min(player.Clues.Count, (int)player.DeductAbility); i++)
                {
                    if (player.JudegeAbility >= 10 && player.Clues[i].Importance == ClueImportance.Critical)
                        Console.WriteLine($"증거[{i + 1}] {player.Clues[i].Name}          ★중요");
                    else Console.WriteLine($"증거[{i + 1}] {player.Clues[i].Name}");

                    Console.WriteLine($" - {player.Clues[i].Description}");
                }
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('─', lineWidth));
            Console.ResetColor();
            Console.WriteLine();
        }

        //===================================
        // 함수 - 상태창
        public static void DisplayStatus(Player player)
        {
            int lineWidth = 30;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  ▶ 내 정보 ({player.Name})");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('─', lineWidth));
            Console.ResetColor();

            Console.WriteLine($"체력: {player.Hp}/{(int)player.MaxHp}      턴: {player.TurnCount}       현위치: {GameRules.LocationKor[(int)player.Location]}" +
                $"\n관찰력:{player.ObserveAbility}     판단력: {player.JudegeAbility}     추리력: {player.DeductAbility}");
            Console.WriteLine();
        }

        //===================================
        // 함수 - 추리수첩 오픈
        public static void OpenDiary(Player player)
        {

            int lineWidth = 50;
            string title = " << 추리 수첩 >> ";
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(new string('─', lineWidth));
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(GetCentered(lineWidth, title));
            Console.ResetColor();

            if (player.Clues.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(" ( 비어있음 ) ");
                Console.ResetColor();
            }

            else
            {

            for (int i = 0; i < player.Clues.Count; i++)
            {
                if (player.JudegeAbility >= 10 && player.Clues[i].Importance == ClueImportance.Critical)
                    Console.WriteLine($"증거[{i + 1}] {player.Clues[i].Name}          ★ 중요");
                else Console.WriteLine($"증거[{i + 1}] {player.Clues[i].Name}");

                Console.WriteLine($" - {player.Clues[i].Description}");
            }
            
            }
            Console.WriteLine();

        }
        //===================================
        // 시스템 정보창
        public static void ShowSystem(GameContext context)
        {
            int lineWidth = 30;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"  ▶ 시스템 창   ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[Day {GameRules.Day}]");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('─', lineWidth));
            Console.ResetColor();

            // 5줄까지만 보이게
            for (int i = 0; i < 5; i++)
            {
                if (i < context.Logs.Count)
                {
                    Console.WriteLine(context.Logs[i]);
                }
                else
                {
                    Console.WriteLine();
                }
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('─', lineWidth));
            Console.ResetColor();

            Console.WriteLine();
        }
    }

}
