using System.ComponentModel.Design;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Channels;


/*
TODO : deduce 구현
장소마다 조사 다르게 구현
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
        static int Day = 1;

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
                if (choice == 0)
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
                        StartDeduce(player);
                        break;
                }

                AddDay(player);
                Console.WriteLine("아무키나 누르세요");
                Console.ReadKey();
            }
        }
        //===================================
        // 함수 - 날짜 변경
        public static void AddDay(Player player)
        {
            if (player.Hp <= 0)
            {
                Console.WriteLine("체력을 소진하였습니다.");
                Day++;
                player.Hp = (int)player.MaxHp;
                if (player.Location != Location.Home)
                {
                    turnCount += 3;
                    player.Location = Location.Home;
                    Console.WriteLine("집이 아닌 곳에서 체력을 소진하였습니다. [패널티] 턴 3회 추가");
                    Console.WriteLine("집으로 이동합니다.");

                }
                Console.WriteLine($"체력을 회복하고 다음날이 됩니다.   Day: {Day}");
            }
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
            Console.WriteLine("범인으로 추정되는 용의자를 지목하세요");

            for (int i = 0; i < suspects.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {suspects[i].Name}");
            }

            int choice = GetIntInput(1, suspects.Count);

            // 범인 선택 시 추리 시작
            if (suspects[choice - 1].Type == CharacterType.Criminal) Deduce(player);
            // 범인 선택 실패
            else FailCatch("범인");
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
                OpenDiary(player);
                Console.WriteLine("해당 진술에 대해 반박할 만한 증거를 선택하세요");
                int choice = GetIntInput(1, player.Clues.Count);

                if (player.Clues[choice-1] == debate[i].RightClue)
                {
                    Console.WriteLine("제대로 반박한것 같다.");
                }
                else
                {
                    FailCatch("알맞은 반박");
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

            for(int i = 0; i < criticalClues.Count; i++)
            {
                if (!player.Clues.Contains(criticalClues[i]))
                {
                    Console.WriteLine("중요한 증거를 아직 다 못 찾은 듯 하다.. 조금만 더 둘러보자");
                    return;
                }
                
            }
            Console.WriteLine("범인을 찾았다! 이번 사건도 해결 완료!");

        }
        //===================================
        // 함수 - 용의자 검거 실패
        public static void FailCatch(string reason)
        {

            Console.WriteLine($"{reason}이 아닌 것 같다..");
            Console.WriteLine("다시 생각해보자");

            // 실패 패널티 턴+5
            turnCount += 5;
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
                Console.WriteLine($"[{i + 1}] {LocationKor[(int)movableLoca[i]]}");
            }

            // 0 따로 취소처리
            int choice = GetIntInput(0, movableLoca.Length);
            if (choice == 0) return;

            switch (movableLoca[choice - 1])
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
            //모두 모았다면
            if (player.Clues.Count == ClueCount)
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

            // 
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

                    player.Clues.Add(critical[clueNum]);
                    Console.WriteLine($"{critical[clueNum].Name}을 획득했습니다.");
                }
                //나머지 최소 60%확률로 일반 증거 획득
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
            if (player.Hp >= 1)
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
            int subject = random.Next(1, 4);
            switch (subject)
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
                Console.WriteLine($"[{i + 1}] {BehaveKor[(int)behaviors[i]]}");
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
            Console.WriteLine($"이름: {player.Name}       체력: {player.Hp}/{(int)player.MaxHp}      턴: {turnCount}         Day: {Day}" +
                $"\n관찰력:{player.ObserveAbility}     판단력: {player.JudegeAbility}     추리력: {player.DeductAbility}");
        }
        //===================================
        // 함수 - 추리수첩
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