using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Channels;


/*
TODO :
80% 체크 → 법정 배틀
 */
namespace _Project_Text_Detective
{
    public enum Behavior
    {
        Move, Investigate, Exercise, Study, Diary, Deduce
    }
    internal class Program
    {

        static string[] BehaveKor = { "이동", "조사", "운동", "공부", "추리 수첩 보기", "추리하기" };
        static string[] LocationKor = { "집", "카페", "도서관", "헬스장" };
        static int turnCount = 0;
        public static int ClueCount = 10;

        static void Main(string[] args)
        {
            Console.Write("이름을 입력하세요: ");
            string name = Console.ReadLine();
            Player player = new Player(name);

            bool isRunning = true;
            while (isRunning)
            {
                Console.Clear();
                DisplayStatus(player);
                ShowDiary(player);
                SceneMain(player);
                List<Behavior> behaviors = GetBehavior(player);
                int choice = GetIntInput(0, behaviors.Count);
                if(choice == 0)
                {
                    isRunning = false;
                    Console.WriteLine("게임을 종료합니다.");
                    continue;
                }    
                
                switch (behaviors[choice - 1])
                {
                    case Behavior.Move:
                        Move(player);
                        break;
                    case Behavior.Investigate:
                        Investigate(player);
                        break;
                    case Behavior.Exercise:
                        Exercise(player);
                        break;
                    case Behavior.Study:
                        Study(player);
                        break;
                    case Behavior.Diary:
                        OpenDiary(player);
                        break;
                    case Behavior.Deduce:
                        //구현예정
                        break;
                }
                Console.WriteLine("아무키나 누르세요");
                Console.ReadKey();
            }
        }
        //===================================
        // 함수 - 이동
        public static void Move(Player player)
        {
            Location[] movableLoca = GameRules.locaToLoca[player.Location];
            
            Console.WriteLine("어디로 이동하겠습니까?");
            Console.WriteLine("[0] 취소");

            for (int i = 0; i < movableLoca.Length ; i++)
            {
                Console.WriteLine($"[{i+1}] {LocationKor[(int)movableLoca[i]]}");
            }

            // 0 따로 취소처리
            int choice = GetIntInput(0, movableLoca.Length);
                if (choice == 0) return;

            switch (movableLoca[choice -1])
            {
                case Location.Cafe:
                    Console.WriteLine("카페로 이동합니다.");
                    player.Location = Location.Cafe;
                    turnCount++;
                    player.Hp--;
                    break;
                case Location.Library:
                    Console.WriteLine("도서관으로 이동합니다.");
                    player.Location = Location.Library;
                    turnCount++;
                    player.Hp--;
                    break;
                case Location.Gym:
                    Console.WriteLine("헬스장으로 이동합니다.");
                    player.Location = Location.Gym;
                    turnCount++;
                    player.Hp--;
                    break;
                case Location.Home:
                    Console.WriteLine("집으로 이동합니다.");
                    player.Location = Location.Home;
                    turnCount++;
                    player.Hp--;
                    break;

            }
        }
        //===================================
        // 함수 - 조사
        public static void Investigate(Player player)
        {
            if (player.Clues.Count == ClueCount)
            {
                Console.WriteLine("이만하면 다 둘러본 모양이다. 여기서는 더이상 증거를 찾을 수 없을 것 같다.");
                return;
            }
            
            Random random = new Random();

            int origin = player.Clues.Count;

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
                if (criticalPercentage <= 20 + Math.Min(20,player.ObserveAbility))
                {
                    clueNum = random.Next(0, critical.Count);
                    if (player.Clues.Contains(critical[clueNum])) continue;

                    player.Clues.Add(critical[clueNum]);
                    Console.WriteLine($"{critical[clueNum].Name}을 획득했습니다.");
                }
                else
                {
                    clueNum = random.Next(0, minor.Count);
                    if (player.Clues.Contains(minor[clueNum])) continue;

                    player.Clues.Add(minor[clueNum]);
                    Console.WriteLine($"{minor[clueNum].Name}을 획득했습니다.");
                }
            }

                player.Hp--;
                turnCount++;
        }
        //===================================
        // 함수 - 운동
        public static void Exercise(Player player)
        {
            // 운동 2번 =  hp +1

            //체력이 1이하면 애초에 선택하지 못하게 해두도록 수정
            if (player.Hp > 1)
            {
                player.Hp--;
                turnCount++;
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
            turnCount++;
            Random random = new Random();
            int subject = random.Next(1,4);
            switch(subject)
            {
                case 1:
                    player.DeductAbility += 0.5f;
                    Console.WriteLine($"추리력이 상승했습니다. 현재 추리력: {player.DeductAbility}");
                    break;
                case 2:
                    player.JudegeAbility++;
                    Console.WriteLine($"판단력이 상승했습니다. 현재 판단력: {player.JudegeAbility}");
                    break;
                case 3:
                    player.ObserveAbility++;
                    Console.WriteLine($"관찰력이 상승했습니다. 현재 관찰력: {player.ObserveAbility}");
                    break;
            }
        }
        //===================================
        // 보기 메뉴 출력
        public static void SceneMain(Player player)
        {
            List<Behavior> behaviors = GetBehavior(player);
            for (int i = 0; i < behaviors.Count; i++)
            {
                Console.WriteLine($"[{i+1}] {BehaveKor[(int)behaviors[i]]}");
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

        //===================================
        // 함수 - 상태창
        public static void DisplayStatus(Player player)
        {
            Console.WriteLine("====내 정보====");
            Console.WriteLine($"이름: {player.Name}       체력: {player.Hp}/{(int)player.MaxHp}      턴: {turnCount}" +
                $"\n관찰력:{player.ObserveAbility}     판단력: {player.JudegeAbility}     추리력: {player.DeductAbility}");
        }
        //===================================
        // 함수 - 추리수첩
        public static void ShowDiary(Player player)
        {
            Console.WriteLine("====추리 수첩 일부====");
            for(int i = 0; i < Math.Min(player.Clues.Count, (int)player.DeductAbility); i++)
            {
                if(player.JudegeAbility >= 10 && player.Clues[i].Importance == ClueImportance.Critical)
                    Console.WriteLine($"증거[{i + 1}] {player.Clues[i].Name}          ★중요");
                else Console.WriteLine($"증거[{i+1}] {player.Clues[i].Name}");

                Console.WriteLine($" - {player.Clues[i].Description}");
            }
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
        //===================================
        // 함수 - 정수 입력받기

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


    }
}