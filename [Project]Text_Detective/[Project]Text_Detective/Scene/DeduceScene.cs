using System;
using System.Numerics;

namespace _Project_Text_Detective
{

    public class DeduceScene : SceneBase
    {
        public override SceneKey Key => SceneKey.Deduce;

        public override void Render(GameContext context)
        {
            Console.Clear();
            Program.DisplayStatus(context.Player);
            Program.ShowDiary(context.Player);

   
            // 용의자 입력받기
            Console.WriteLine("범인으로 추정되는 용의자를 지목하세요");

        }

        public override void HandleInput(GameContext context)
        {
            Program.StartDeduce(context.Player);
            //다시 메인으로 복귀
            GoTo(context, SceneKey.MainScene);
        }
    }

}