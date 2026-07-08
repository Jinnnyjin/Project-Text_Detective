using System;
using System.Numerics;

namespace _Project_Text_Detective
{
    public class MainScene : SceneBase
    {
        public override SceneKey Key => SceneKey.MainScene;

        public override void Render(GameContext context)
        {
            Console.Clear();
            GameUI.DisplayStatus(context.Player);
            GameUI.ShowDiary(context.Player);
            ShowOptions(context.Player);
        }

        public override void HandleInput(GameContext context)
        {
            List<Behavior> behaviors = GetBehavior(context.Player);
            int choice = GameUI.GetIntInput(0, behaviors.Count);

            if (choice == 0)
            {
                context.IsRunning = false;
                Console.WriteLine("게임을 종료합니다.");
                return;
            }

            switch (behaviors[choice - 1])
            {
                case Behavior.Move:
                    Move(context.Player);
                    break;
                case Behavior.Investigate:
                    Investigate(context.Player);
                    break;
                case Behavior.Exercise:
                    Exercise(context.Player);
                    break;
                case Behavior.Study:
                    Study(context.Player);
                    break;
                case Behavior.Diary:
                    GameUI.OpenDiary(context.Player);
                    break;
                case Behavior.Deduce:
                    GoTo(context, SceneKey.Deduce);
                    break;
            }

            AddDay(context.Player);
            Console.WriteLine("아무키나 누르세요");
            Console.ReadKey();

        }

        //===================================
        // 함수 - 날짜 변경
        public static void AddDay(Player player)
        {
            if (player.Hp <= 0)
            {
                Console.WriteLine("체력을 소진하였습니다.");
                player.Day++;
                player.Hp = (int)player.MaxHp;
                if (player.Location != Location.Home)
                {
                    player.TurnCount += 3;
                    player.Location = Location.Home;
                    Console.WriteLine("집이 아닌 곳에서 체력을 소진하였습니다. [패널티] 턴 3회 추가");
                    Console.WriteLine("집으로 이동합니다.");

                }
                Console.WriteLine($"체력을 회복하고 다음날이 됩니다.   Day: {player.Day}");
            }
        }

        //===================================
        // 함수 - 이동
        public static void Move(Player player)
        {
            Location[] movableLoca = GameRules.locaToLoca[player.Location];

            Console.WriteLine("어디로 이동하겠습니까?");
            Console.WriteLine("[0] 취소");

            for (int i = 0; i < movableLoca.Length; i++)
            {
                Console.WriteLine($"[{i + 1}] {GameRules.LocationKor[(int)movableLoca[i]]}");
            }

            // 0 따로 취소처리
            int choice = GameUI.GetIntInput(0, movableLoca.Length);
            if (choice == 0) return;

            switch (movableLoca[choice - 1])
            {
                case Location.Cafe:
                    Console.WriteLine("카페로 이동합니다.");
                    player.Location = Location.Cafe;
                    player.TurnCount++;
                    player.Hp--;
                    break;
                case Location.Library:
                    Console.WriteLine("도서관으로 이동합니다.");
                    player.Location = Location.Library;
                    player.TurnCount++;
                    player.Hp--;
                    break;
                case Location.Gym:
                    Console.WriteLine("헬스장으로 이동합니다.");
                    player.Location = Location.Gym;
                    player.TurnCount++;
                    player.Hp--;
                    break;
                case Location.Home:
                    Console.WriteLine("집으로 이동합니다.");
                    player.Location = Location.Home;
                    player.TurnCount++;
                    player.Hp--;
                    break;

            }
        }
        //===================================
        // 함수 - 조사
        public static void Investigate(Player player)
        {
            if (player.Location != Location.Cafe)
            {
                Console.WriteLine("특별히 조사할 것은 없는 것 같다.");
                player.TurnCount++;
                player.Hp--;
                return;
            }

            //모두 모았다면
            if (player.Clues.Count == GameRules.ClueCount)
            {
                Console.WriteLine("이만하면 다 둘러본 모양이다. 여기서는 더이상 증거를 찾을 수 없을 것 같다.");
                return;
            }

            Random random = new Random();

            // 얻었는지 확인용 변수
            int origin = player.Clues.Count;

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
            while (player.Clues.Count == origin)
            {
                int criticalPercentage = random.Next(0, 100);
                // 최대 40%확률로 핵심 증거 획득
                if (criticalPercentage <= 20 + Math.Min(20, player.ObserveAbility))
                {
                    clueNum = random.Next(0, critical.Count);
                    //이미 해당 증거를 가지고 있다면 패스
                    if (player.Clues.Contains(critical[clueNum])) continue;

                    player.AcquireClue(critical[clueNum]);
                }
                //나머지 최소 60%확률로 일반 증거 획득
                else
                {
                    clueNum = random.Next(0, minor.Count);
                    if (player.Clues.Contains(minor[clueNum])) continue;

                    player.AcquireClue(minor[clueNum]);
                }
            }

            player.Hp--;
            player.TurnCount++;
        }
        //===================================
        // 함수 - 운동
        public static void Exercise(Player player)
        {
            // 운동 2번 =  hp +1

            //체력이 1이하면 애초에 선택하지 못하게 해두도록 수정
            if (player.Hp >= 1)
            {
                player.Hp--;
                player.TurnCount++;
                Console.WriteLine("운동을 하고 상쾌해집니다!");
                player.MaxHp += 0.5f;

            }
            else
            {
                Console.WriteLine("체력이 부족합니다.");
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
        // 보기 메뉴 출력
        public static void ShowOptions(Player player)
        {
            List<Behavior> behaviors = GetBehavior(player);
            for (int i = 0; i < behaviors.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {GameRules.BehaveKor[(int)behaviors[i]]}");
            }
        }
        //===================================
        // 함수 - 조건에 만족하는 행동 보기 
        public static List<Behavior> GetBehavior(Player player)
        {
            List<Behavior> newBehaviors = new List<Behavior>(GameRules.locaToBehave[player.Location]);

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