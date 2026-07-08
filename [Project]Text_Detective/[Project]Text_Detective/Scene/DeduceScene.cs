using System;
using System.Numerics;

namespace _Project_Text_Detective
{

    public class DeduceScene : SceneBase
    {
        public override SceneKey Key => SceneKey.Deduce;

        public override void Render(GameContext context)
        {
            Console.Clear();
            GameUI.DisplayStatus(context.Player);
            GameUI.ShowDiary(context.Player);

   
            // 용의자 입력받기
            Console.WriteLine("범인으로 추정되는 용의자를 지목하세요");

        }

        public override void HandleInput(GameContext context)
        {
            StartDeduce(context.Player);
            //다시 메인으로 복귀
            GoTo(context, SceneKey.MainScene);
        }

        //===================================
        // 함수 - 추리
        public static void StartDeduce(Player player)
        {
            // 캐릭터 중 용의자 추리기 ( 피해자 1명 빼기 )
            List<Character> suspects = new List<Character>();
            for (int i = 0; i < CharacterData.TutoCharacters.Count; i++)
            {
                if (CharacterData.TutoCharacters[i].Type != CharacterType.Victim)
                { suspects.Add(CharacterData.TutoCharacters[i]); }
            }


            // 용의자 입력받기
            for (int i = 0; i < suspects.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {suspects[i].Name}");
            }

            int choice = GameUI.GetIntInput(1, suspects.Count);

            // 범인 선택 시 추리 시작
            if (suspects[choice - 1].Type == CharacterType.Criminal) Deduce(player);
            // 범인 선택 실패
            else FailCatch("범인", player);
        }
        //===================================
        // 함수 - 용의자 검거 성공 시 추리
        public static void Deduce(Player player)
        {
            Random random = new Random();
            List<Debate> debate = new List<Debate>();

            // 5가지 중 진술 3가지 중복없이 선택
            while (debate.Count <= 2)
            {
                int debateNum = random.Next(0, DebateData.TutoDebate.Count);
                if (debate.Contains(DebateData.TutoDebate[debateNum])) continue;
                debate.Add(DebateData.TutoDebate[debateNum]);
            }

            // 진술에 대한 반박 성공여부
            for (int i = 0; i < debate.Count; i++)
            {
                Console.WriteLine(debate[i].Statement);
                GameUI.OpenDiary(player);
                Console.WriteLine("해당 진술에 대해 반박할 만한 증거를 선택하세요");
                int choice = GameUI.GetIntInput(1, player.Clues.Count);

                if (player.Clues[choice - 1] == debate[i].RightClue)
                {
                    Console.WriteLine("제대로 반박한것 같다.");
                }
                else
                {
                    FailCatch("알맞은 반박", player);
                    return;
                }
            }

            // 핵심 증거 수집 확인
            List<Clue> criticalClues = new List<Clue>();

            // 핵심 증거 분류
            for (int i = 0; i < ClueData.Tutoclues.Count; i++)
            {
                if (ClueData.Tutoclues[i].Importance == ClueImportance.Critical)
                {
                    criticalClues.Add(ClueData.Tutoclues[i]);
                }
            }

            for (int i = 0; i < criticalClues.Count; i++)
            {
                if (!player.Clues.Contains(criticalClues[i]))
                {
                    Console.WriteLine("중요한 증거를 아직 다 못 찾은 듯 하다.. 조금만 더 둘러보자");
                    Console.WriteLine("아무키나 누르세요");
                    Console.ReadKey();
                    return;
                }

            }
            Console.WriteLine("범인을 찾았다! 이번 사건도 해결 완료!");

        }
        //===================================
        // 함수 - 용의자 검거 실패
        public static void FailCatch(string reason, Player player)
        {

            Console.WriteLine($"{reason}이 아닌 것 같다..");
            Console.WriteLine("다시 생각해보자");
            Console.WriteLine("아무키나 누르세요");
            Console.ReadKey();

            // 실패 패널티 턴+5
            player.TurnCount += 5;
        }
    }

}