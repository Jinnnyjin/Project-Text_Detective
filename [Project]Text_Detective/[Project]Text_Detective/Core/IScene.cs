using System;

namespace _Project_Text_Detective
{

    public interface IScene
    {
        SceneKey Key { get; }

        // 화면 진입시 1번만 실행
        void Enter(GameContext context);


        // 콘솔에 화면(디자인)
        void Render(GameContext context);

        // 사용자 입력, 처리
        void HandleInput(GameContext context);

        // 다른 화면으로 나가기 직전에 실행
        void Exit(GameContext context);

        

    }

}