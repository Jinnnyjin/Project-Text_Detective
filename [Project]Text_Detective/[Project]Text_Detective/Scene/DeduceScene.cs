using System;

namespace _Project_Text_Detective
{

    public enum DeduceMode
    {
        SelectSuspect, Debating, Result
    }

    public class DeduceScene : SceneBase
    {
        public override SceneKey Key => SceneKey.Deduce;

        public DeduceMode currentmode = DeduceMode.SelectSuspect;

        private List<Debate> debates = new List<Debate>();

        private int debateIndex = 0;


        public override void Render(GameContext context)
        {
            Console.Clear();
            GameUI.ShowHeader();
            GameUI.DisplayStatus(context.Player);
            GameUI.ShowDiary(context.Player);
            GameUI.ShowSystem(context);


            switch (currentmode)
            {
                case DeduceMode.SelectSuspect:
                    SuspectOptions();
                    break;

                case DeduceMode.Debating:
                    GameUI.OpenDiary(context.Player);
                    break;

                case DeduceMode.Result:
                    break;
            }
        }

        public override void HandleInput(GameContext context)
        {

            switch (currentmode)
            {
                case DeduceMode.SelectSuspect:
                    StartDeduce(context);
                    break;


                case DeduceMode.Debating:
                    Deduce(context);
                    break;

                case DeduceMode.Result:

                    GoTo(context, SceneKey.MainScene);
                    break;
            }
        }
        //===================================
        // 함수 - 용의자 도출
        public List<Character> GetSuspect()
        {
            List<Character> suspects = new List<Character>();
            for (int i = 0; i < CharacterData.TutoCharacters.Count; i++)
            {
                if (CharacterData.TutoCharacters[i].Type != CharacterType.Victim)
                { suspects.Add(CharacterData.TutoCharacters[i]); }
            }

            return suspects;
        }

        //===================================
        // 용의자 선택창
        public void SuspectOptions()
        {
            List<Character> suspects = GetSuspect();

            GameUI.DivideSelect();

            for (int i = 0; i < suspects.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($" {i + 1})");
                Console.ResetColor();
                Console.WriteLine($" {suspects[i].Name}");
            }
        }
        //===================================
        // 함수 - 추리
        public void StartDeduce(GameContext context)
        {
            // 캐릭터 중 용의자 추리기 ( 피해자 1명 빼기 )
            List<Character> suspects = GetSuspect();

            int choice = GameUI.GetIntInput(context, 1, suspects.Count);

            // 범인 선택 시 추리 시작
            if (suspects[choice - 1].Type == CharacterType.Criminal)
            {
                currentmode = DeduceMode.Debating;


                while (debates.Count <= 2)
                {
                    int Num = context.Random.Next(0, DebateData.TutoDebate.Count);

                    if (debates.Contains(DebateData.TutoDebate[Num])) continue;

                    debates.Add(DebateData.TutoDebate[Num]);
                }
                debateIndex = 0;

                context.AddLog(debates[debateIndex].Statement);
                context.AddLog("해당 진술에 대해 반박할 만한 증거를 선택하세요");

            }
            // 범인 선택 실패
            else
            {
                currentmode = DeduceMode.Result;
                FailCatch("범인", context);
            }
        }
        //===================================
        // 함수 - 용의자 검거 성공 시 추리
        public void Deduce(GameContext context)
        {

            int choice = GameUI.GetIntInput(context, 1, context.Player.Clues.Count);

            if (context.Player.Clues[choice - 1] == debates[debateIndex].RightClue)
            {
                context.AddLog("제대로 반박한것 같다.");
                debateIndex++;

                if (debateIndex == 3)
                {
                    currentmode = DeduceMode.Result;
                    GetResult(context);
                }
                else
                {
                    context.AddLog(debates[debateIndex].Statement);
                    context.AddLog("해당 진술에 대해 반박할 만한 증거를 선택하세요");
                }
            }
            else
            {
                currentmode = DeduceMode.Result;
                FailCatch("알맞은 반박", context);
            }
        }

        public void GetResult(GameContext context)
        {
            

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
                if (!context.Player.Clues.Contains(criticalClues[i]))
                {
                    context.AddLog("중요한 증거를 아직 다 못 찾은 듯 하다.. 조금만 더 둘러보자");
                    return;
                }

            }
            
            context.AddLog("범인을 찾았다! 이번 사건도 해결 완료!");

        }
        
        //===================================
        // 함수 - 용의자 검거 실패
        public void FailCatch(string reason, GameContext context)
        {

            context.AddLog($"{reason}이 아닌 것 같다..다시 생각해보자");
            context.AddLog("실패 패널티 턴 +5");

            // 실패 패널티 턴+5
            context.Player.TurnCount += 5;
        }
    }

}