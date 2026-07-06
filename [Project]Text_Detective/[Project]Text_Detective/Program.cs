using System.Reflection.Metadata.Ecma335;

namespace _Project_Text_Detective
{
    
    internal class Program
    {
        // 장소마다 선택지가 다름. 각각 가능한 보기를 dictionary로 구현
        static Dictionary<Location, Behavior[]> _locations = new Dictionary<Location, Behavior[]>
        {
            { Location.Home, new Behavior[]{ Behavior.Move, Behavior.Search, Behavior.Exercise, Behavior.Study } },
            { Location.Cafe, new Behavior[]{ Behavior.Move, Behavior.Search} },
            { Location.Library, new Behavior[]{ Behavior.Move, Behavior.Search,Behavior.Study } },
            { Location.Gym, new Behavior[]{ Behavior.Move, Behavior.Search, Behavior.Exercise } }

        };
        enum Behavior
        {
            Move, Search, Exercise, Study
        }

        static string[] BehaveKor = { "이동", "조사", "운동", "공부" };
        static int turnCount = 0;

        static void Main(string[] args)
        {
            Console.Write("이름을 입력하세요");
            string name = Console.ReadLine();
            Player player = new Player(name);

            bool isRunning = true;
            while (isRunning)
            {
                
                SceneMain(player);
                Behavior[] behaviors = _locations[player.Location];
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
                    case Behavior.Search:                       
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
            // 추후 현위치도 띄우고 현위치는 이동에 안뜨도록 수정필요
            Console.WriteLine("어디로 이동하겠습니까?");
            Console.WriteLine("[0] 취소");
            Console.WriteLine("[1] 카페");
            Console.WriteLine("[2] 도서관");
            Console.WriteLine("[3] 헬스장");
            Console.WriteLine("[4] 집");

            switch (GetIntInput(0,4))
            {
                case 0:
                    break;
                case 1:
                    Console.WriteLine("카페로 이동합니다.");
                    player.Location = Location.Cafe;
                    turnCount++;
                    player.Hp--;
                    break;
                case 2:
                    Console.WriteLine("도서관으로 이동합니다.");
                    player.Location = Location.Library;
                    turnCount++;
                    player.Hp--;
                    break;
                case 3:
                    Console.WriteLine("헬스장으로 이동합니다.");
                    player.Location = Location.Gym;
                    turnCount++;
                    player.Hp--;
                    break;
                case 4:
                    Console.WriteLine("집으로 이동합니다.");
                    player.Location = Location.Home;
                    turnCount++;
                    player.Hp--;
                    break;

            }
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
            Behavior[] behaviors = _locations[player.Location];
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