using System;

namespace _Project_Text_Detective
{

    public class GameManager
    {
        private readonly Dictionary<SceneKey, IScene> _scenes = new Dictionary<SceneKey, IScene>();
        private IScene? _currentScene;

        public static GameManager instance { get; } = new GameManager();

        private GameManager()
        {
            Context = new GameContext(this);
            RegisterScenes();
        }

        public GameContext Context { get; }

        private void RegisterScenes()
        {
            AddScene(new TitleScene());
            AddScene(new MainScene());
        }

        private void AddScene(IScene scene)
        {
            _scenes[scene.Key] = scene;
        }

        public void ChangeScene(SceneKey key)
        {
            if (!_scenes.TryGetValue(key, out IScene? nextScene))
            {
                Context.AddLog($"등록되지 않은 화면입니다: {key}");
                return;
            }

            _currentScene?.Exit(Context);
            _currentScene = nextScene;
            _currentScene.Enter(Context);
        }

        public void Run()
        {
            ChangeScene(SceneKey.Title);

            while (Context.IsRunning && _currentScene is not null)
            {
                _currentScene.Render(Context);
                _currentScene.HandleInput(Context);
            }
        }
    }

}