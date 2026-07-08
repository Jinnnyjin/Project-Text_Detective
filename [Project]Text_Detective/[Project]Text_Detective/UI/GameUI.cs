using System;

namespace _Project_Text_Detective
{
    public static class GameUI
    {
        // 정수 입력값 받기
        public static int GetIntInput(int min, int max)
        {
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int input))
                {
                    if (input >= min && input <= max)
                    { return input; }
                }
                Console.WriteLine("\n잘못 입력했습니다. 보기 내 숫자만 입력가능");
            }
        }

        //===================================
        // 함수 - 추리수첩(추리력 기반 일부)
        public static void ShowDiary(Player player)
        {
            Console.WriteLine("====추리 수첩 일부====");
            for (int i = 0; i < Math.Min(player.Clues.Count, (int)player.DeductAbility); i++)
            {
                if (player.JudegeAbility >= 10 && player.Clues[i].Importance == ClueImportance.Critical)
                    Console.WriteLine($"증거[{i + 1}] {player.Clues[i].Name}          ★중요");
                else Console.WriteLine($"증거[{i + 1}] {player.Clues[i].Name}");

                Console.WriteLine($" - {player.Clues[i].Description}");
            }
        }

        //===================================
        // 함수 - 상태창
        public static void DisplayStatus(Player player)
        {
            Console.WriteLine("====내 정보====");
            Console.WriteLine($"이름: {player.Name}       체력: {player.Hp}/{(int)player.MaxHp}      턴: {player.TurnCount}         Day: {player.Day}" +
                $"\n관찰력:{player.ObserveAbility}     판단력: {player.JudegeAbility}     추리력: {player.DeductAbility}");
        }

        //===================================
        // 함수 - 추리수첩 오픈
        public static void OpenDiary(Player player)
        {
            Console.WriteLine("====추리 수첩====");
            for (int i = 0; i < player.Clues.Count; i++)
            {
                if (player.JudegeAbility >= 10 && player.Clues[i].Importance == ClueImportance.Critical)
                    Console.WriteLine($"증거[{i + 1}] {player.Clues[i].Name}          ★ 중요");
                else Console.WriteLine($"증거[{i + 1}] {player.Clues[i].Name}");

                Console.WriteLine($" - {player.Clues[i].Description}");
            }

        }

    }

}
