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
            Console.Clear();
            GameUI.ShowHeader();

            DisplayOutline();

            Console.Write("이름을 입력하세요: ");
            
        }

        public override void HandleInput(GameContext context)
        {
            string name = Console.ReadLine();
            context.Player = new Player(name);

            context.Player.OnClueAcquired += (clue) => context.AddLog($"[SYSTEM] {clue.Name}을 획득했습니다.");
            context.Player.OnStatRaised += (stat) =>
            {
                switch (stat)
                {
                    case Stat.ObserveAbility:
                        context.AddLog($"[SYSTEM] 관찰력이 상승했습니다. 현재 관찰력: {context.Player.ObserveAbility}");
                        break;
                    case Stat.JudegeAbility:
                        context.AddLog($"[SYSTEM] 판단력이 상승했습니다. 현재 관찰력: {context.Player.JudegeAbility}");
                        break;
                    case Stat.DeductAbility:
                        context.AddLog($"[SYSTEM] 추리력이 상승했습니다. 현재 관찰력: {context.Player.DeductAbility}");
                        break;
                }


            }; 

            GoTo(context, SceneKey.MainScene);
        }

        public void DisplayOutline()
        {
            int lineWidth = 30;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  ▶ 사건 개요");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('─', lineWidth));
            Console.ResetColor();

            Console.WriteLine("카페 '○○'에서 한 손님이 화장실을 다녀온 사이 ");
            Console.WriteLine("스마트폰 충전기 선이 잘린 채로 발견되었다.");
            Console.WriteLine("");
            Console.WriteLine("현장에 있던 알바생, 나사장, 김단골 중");
            Console.WriteLine("누군가가 이 사건과 관련이 있는 듯하다.");
            Console.WriteLine("");
            Console.WriteLine("이동, 조사, 운동, 공부로 단서를 모으고 능력치를 키우세요.");
            Console.WriteLine("정보를 충분히 모으면 '추리하기'로 범인을 지목할 수 있습니다.");
            Console.WriteLine("몇 턴 만에 사건을 해결할 수 있을지 확인해보세요!\r\n");
            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('─', lineWidth));
            Console.ResetColor();

            Console.WriteLine("");
        }

    }

}