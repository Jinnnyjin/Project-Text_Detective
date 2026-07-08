using System;
using System.Numerics;
using System.Threading.Channels;

namespace _Project_Text_Detective
{
    public class TitleScene : SceneBase
    {
        public override SceneKey Key => SceneKey.Title;

        public override void Render(GameContext context)
        {
            GameUI.ShowHeader();

            Console.Write("이름을 입력하세요: ");
            
        }

        public override void HandleInput(GameContext context)
        {
            string name = Console.ReadLine();
            context.Player = new Player(name);

            context.Player.OnClueAcquired += (clue) => Console.WriteLine($"{clue.Name}을 획득했습니다.");
            context.Player.OnStatRaised += (stat) =>
            {
                switch (stat)
                {
                    case Stat.ObserveAbility:
                        Console.WriteLine($"관찰력이 상승했습니다. 현재 관찰력: {context.Player.ObserveAbility}");
                        break;
                    case Stat.JudegeAbility:
                        Console.WriteLine($"판단력이 상승했습니다. 현재 관찰력: {context.Player.JudegeAbility}");
                        break;
                    case Stat.DeductAbility:
                        Console.WriteLine($"추리력이 상승했습니다. 현재 관찰력: {context.Player.DeductAbility}");
                        break;
                }


            }; 

            GoTo(context, SceneKey.MainScene);
        }

    }

}