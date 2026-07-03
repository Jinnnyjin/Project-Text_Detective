namespace _Project_Text_Detective
{
    internal class Program
    {
        static void Main(string[] args)
        {

            bool isRunning = true;
            while (isRunning)
            {
                SceneMain();
                GetIntInput(1, BehaveKor.Length);
            }
        }

        enum Behavior
        {
            Move, Search, Exercise, Study
        }

        static string[] BehaveKor = { "이동", "조사", "운동", "공부" };
        //===================================
        // 보기(집)
        public static void SceneMain()
        {
            // 추후 집/집이외의 장소마다 선택지 다르게 부여
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine($" [{i+1}] {BehaveKor[i]}");
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