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
            Program.DisplayStatus(context.Player);
            Program.ShowDiary(context.Player);
            Program.SceneMain(context.Player);
        }

        public override void HandleInput(GameContext context)
        {
            List<Behavior> behaviors = Program.GetBehavior(context.Player);
            int choice = Program.GetIntInput(0, behaviors.Count);

            if (choice == 0)
            {
                context.IsRunning = false;
                Console.WriteLine("게임을 종료합니다.");
                return;
            }

            switch (behaviors[choice - 1])
            {
                case Behavior.Move:
                    Program.Move(context.Player);
                    break;
                case Behavior.Investigate:
                    Program.Investigate(context.Player);
                    break;
                case Behavior.Exercise:
                    Program.Exercise(context.Player);
                    break;
                case Behavior.Study:
                    Program.Study(context.Player);
                    break;
                case Behavior.Diary:
                    Program.OpenDiary(context.Player);
                    break;
                case Behavior.Deduce:
                    GoTo(context, SceneKey.Deduce);
                    break;
            }

            Program.AddDay(context.Player);
            Console.WriteLine("아무키나 누르세요");
            Console.ReadKey();

        }
    }

}