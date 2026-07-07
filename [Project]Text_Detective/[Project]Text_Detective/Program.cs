using System.Reflection.Metadata.Ecma335;

namespace _Project_Text_Detective
{
    public enum Behavior
    {
        Move, Investigate, Exercise, Study
    }
    internal class Program
    {

        static string[] BehaveKor = { "이동", "조사", "운동", "공부" };
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
                
                SceneMain(player);
                Behavior[] behaviors = GameRules.locaToBehave[player.Location];
                int choice = GetIntInput(0, behaviors.Length);
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
                }
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

            while (player.Clues.Count == origin)
            {
                int clueNum = random.Next(0, 10);
                
                if (player.Clues.Contains(ClueData.clues[clueNum])) continue;
                
                player.Clues.Add(ClueData.clues[clueNum]);
                Console.WriteLine($"{ClueData.clues[clueNum].Name}을 획득했습니다.");
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
            if(player.Hp > 1)
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
                    player.DeductAbility++;
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
        // 보기(집)
        public static void SceneMain(Player player)
        {
            Behavior[] behaviors = GameRules.locaToBehave[player.Location];
            for (int i = 0; i < behaviors.Length; i++)
            {
                Console.WriteLine($"[{i+1}] {BehaveKor[(int)behaviors[i]]}");
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