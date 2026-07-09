using System;
using System.Numerics;
using System.Security;

//TODO : 보기, 추리수첩, 입력창 수정 
// + 추리씬도 다시 확인하기
namespace _Project_Text_Detective
{

    public enum MainMode
    {
        Default, Moving, ViewDiary
    }

    public class MainScene : SceneBase
    {
        public override SceneKey Key => SceneKey.MainScene;

        public MainMode currentMode = MainMode.Default;

        public override void Render(GameContext context)
        {
            Console.Clear();
            GameUI.ShowHeader();
            GameUI.DisplayStatus(context.Player);
            GameUI.ShowDiary(context.Player);
            GameUI.ShowSystem(context);
            
            switch (currentMode)
            {
                case MainMode.Default:
                    ShowOptions(context.Player);
                    break;
                case MainMode.Moving:
                    MoveOptions(context);
                    break;
                case MainMode.ViewDiary:
                    GameUI.OpenDiary(context.Player);
                    break;
            }
            
        }

        public override void HandleInput(GameContext context)
        {
            switch(currentMode)
            {
                //기본
                case MainMode.Default:
                    List<Behavior> behaviors = GetBehavior(context.Player);
                    int choice = GameUI.GetIntInput(context, 0, behaviors.Count);

                    if (choice == 0)
                    {
                        context.IsRunning = false;
                        context.AddLog("게임을 종료합니다.");
                        return;
                    }

                    switch (behaviors[choice - 1])
                    {
                        case Behavior.Move:
                            currentMode = MainMode.Moving;
                            context.AddLog("이동할 곳을 선택하세요.");
                            return;
                        case Behavior.Investigate:
                            Investigate(context);
                            break;
                        case Behavior.Exercise:
                            Exercise(context);
                            break;
                        case Behavior.Study:
                            Study(context.Player);
                            break;
                        case Behavior.Diary:
                            currentMode = MainMode.ViewDiary;
                            return;
                        case Behavior.Deduce:
                            GoTo(context, SceneKey.Deduce);
                            context.AddLog("범인으로 추정되는 용의자를 지목하세요");
                            break;
                    }
                    break;

                //이동
                case MainMode.Moving:
                    Location[] movableLoca = GameRules.locaToLoca[context.Player.Location];
                    choice = GameUI.GetIntInput(context, 0, movableLoca.Length);
                    if (choice == 0)
                    {
                        currentMode = MainMode.Default;
                        return;
                    }
                    switch (movableLoca[choice - 1])
                    {
                        case Location.Cafe:
                            context.AddLog("카페로 이동합니다.");
                            context.Player.Location = Location.Cafe;
                            context.Player.TurnCount++;
                            context.Player.Hp--;
                            break;
                        case Location.Library:
                            context.AddLog("도서관으로 이동합니다.");
                            context.Player.Location = Location.Library;
                            context.Player.TurnCount++;
                            context.Player.Hp--;
                            break;
                        case Location.Gym:
                            context.AddLog("헬스장으로 이동합니다.");
                            context.Player.Location = Location.Gym;
                            context.Player.TurnCount++;
                            context.Player.Hp--;
                            break;
                        case Location.Home:
                            context.AddLog("집으로 이동합니다.");
                            context.Player.Location = Location.Home;
                            context.Player.TurnCount++;
                            context.Player.Hp--;
                            break;
                    } 
                    break;

                // 추리수첩
                case MainMode.ViewDiary:
                    DiaryOptions(context.Player);
                    choice = GameUI.GetIntInput(context,0, 0);
                    Console.WriteLine();
                    break;
            }
            currentMode = MainMode.Default;
            AddDay(context);

        }

        //===================================
        // 함수 - 날짜 변경
        public static void AddDay(GameContext context)
        {
            if (context.Player.Hp <= 0)
            {
                context.AddLog("체력을 소진하였습니다.");
                GameRules.Day++;
                context.Player.Hp = (int)context.Player.MaxHp;
                if (context.Player.Location != Location.Home)
                {
                    context.Player.TurnCount += 3;
                    context.Player.Location = Location.Home;
                    context.AddLog("집이 아닌 곳에서 체력을 소진하였습니다. [패널티] 턴 3회 추가");
                    context.AddLog("집으로 이동합니다.");

                }
                context.AddLog($"체력을 회복하고 다음날이 됩니다.   Day: {GameRules.Day}");
            }
        }

        //===================================
        // 이동 선택지
        public static void MoveOptions(GameContext context) 
        {
            Location[] movableLoca = GameRules.locaToLoca[context.Player.Location];

            GameUI.DivideSelect();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(" 0) ");
            Console.ResetColor();
            Console.WriteLine("취소");

            for (int i = 0; i < movableLoca.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($" {i + 1}) ");
                Console.ResetColor();
                Console.WriteLine($"{GameRules.LocationKor[(int)movableLoca[i]]}");
            }

        }
        //===================================
        // 함수 - 조사
        public static void Investigate(GameContext context)
        {
            if (context.Player.Location != Location.Cafe)
            {
                context.AddLog("특별히 조사할 것은 없는 것 같다.");
                context.Player.TurnCount++;
                context.Player.Hp--;
                return;
            }

            //모두 모았다면
            if (context.Player.Clues.Count == GameRules.ClueCount)
            {
                context.AddLog("이만하면 다 둘러본 모양이다. 여기서는 더이상 증거를 찾을 수 없을 것 같다.");
                return;
            }

            Random random = new Random();

            // 얻었는지 확인용 변수
            int origin = context.Player.Clues.Count;

            // 증거 등급별 분할
            List<Clue> critical = new List<Clue>();
            List<Clue> minor = new List<Clue>();

            for (int i = 0; i < ClueData.Tutoclues.Count; i++)
            {
                if (ClueData.Tutoclues[i].Importance == ClueImportance.Critical)
                {
                    critical.Add(ClueData.Tutoclues[i]);
                }
                else
                {
                    minor.Add(ClueData.Tutoclues[i]);
                }
            }

            
            int clueNum = 0;
            while (context.Player.Clues.Count == origin)
            {
                int criticalPercentage = random.Next(0, 100);
                // 최대 40%확률로 핵심 증거 획득
                if (criticalPercentage <= 20 + Math.Min(20, context.Player.ObserveAbility))
                {
                    clueNum = random.Next(0, critical.Count);
                    //이미 해당 증거를 가지고 있다면 패스
                    if (context.Player.Clues.Contains(critical[clueNum])) continue;

                    context.Player.AcquireClue(critical[clueNum]);
                }
                //나머지 최소 60%확률로 일반 증거 획득
                else
                {
                    clueNum = random.Next(0, minor.Count);
                    if (context.Player.Clues.Contains(minor[clueNum])) continue;

                    context.Player.AcquireClue(minor[clueNum]);
                }
            }

            context.Player.Hp--;
            context.Player.TurnCount++;
        }
        //===================================
        // 함수 - 운동
        public static void Exercise(GameContext context)
        {
            // 운동 2번 =  hp +1

            //체력이 1이하면 애초에 선택하지 못하게 해두도록 수정
            if (context.Player.Hp >= 1)
            {
                context.Player.Hp--;
                context.Player.TurnCount++;
                context.AddLog("운동을 하고 상쾌해집니다! 체력 증가!");
                context.Player.MaxHp += 0.5f;

            }
            else
            {
                context.AddLog("체력이 부족합니다.");
            }
        }

        //===================================
        // 함수 - 공부
        public static void Study(Player player)
        {
            player.Hp--;
            player.TurnCount++;
            Random random = new Random();
            int num = random.Next(0, 3);

            Stat[] stats = { Stat.ObserveAbility, Stat.DeductAbility, Stat.JudegeAbility };

            player.RaiseStat(stats[num]);
        }
        //===================================
        // 다이어리 메뉴(뒤로가기용)
        public static void DiaryOptions(Player player)
        {
            GameUI.DivideSelect();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($" 0)");
            Console.ResetColor();
            Console.WriteLine($" 뒤로가기");
        }

        //===================================
        // 보기 메뉴 출력
        public static void ShowOptions(Player player)
        {
            List<Behavior> behaviors = GetBehavior(player);

            GameUI.DivideSelect();

            for (int i = 0; i < behaviors.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($" {i+1})");
                Console.ResetColor();
                Console.WriteLine($" {GameRules.BehaveKor[(int)behaviors[i]]}");
            }
        }
        //===================================
        // 함수 - 조건에 만족하는 행동 보기 
        public static List<Behavior> GetBehavior(Player player)
        {
            List<Behavior> newBehaviors = new List<Behavior>(GameRules.locaToBehave[player.Location]);

            // 증거 80% 수집 시 "추리하기" 행동 추가
            bool has80percent = (float)player.Clues.Count / ClueData.Tutoclues.Count >= 0.8;
            bool isAtCafe = (player.Location == Location.Cafe);
            if (has80percent && isAtCafe)
            {
                newBehaviors.Add(Behavior.Deduce);
            }
            return newBehaviors;
        }
    }

}