using System;

namespace _Project_Text_Detective
{

    public abstract class SceneBase : IScene
    {
        public abstract SceneKey Key { get; }

        public virtual void Enter(GameContext context)
        {
            //필요할때만 자식scene에서 override
        }

        public abstract void Render(GameContext context);

        public abstract void HandleInput(GameContext context);

        public virtual void Exit(GameContext context)
        {

        }

        // 화면 이동
        protected static void GoTo(GameContext context, SceneKey nextScene)
        {
            context.Game.ChangeScene(nextScene);
        }

    }

}