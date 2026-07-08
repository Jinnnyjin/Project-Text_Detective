using System;

namespace _Project_Text_Detective
{
    public class TitleScene : SceneBase
    {
        public override SceneKey Key => SceneKey.Title;

        public override void Render(GameContext context)
        {
            Console.WriteLine("=== 명탐정 키우기 ===");
            Console.Write("이름을 입력하세요: ");
            
        }

        public override void HandleInput(GameContext context)
        {
            string name = Console.ReadLine();
            context.Player = new Player(name);
            GoTo(context, SceneKey.MainScene);
        }

    }

}