using System;


namespace _Project_Text_Detective
{
    public enum Behavior
    {
        Move, Investigate, Exercise, Study, Diary, Deduce
    }

    public static class GameRules
    {
        public static string[] BehaveKor = { "이동", "조사", "운동", "공부", "추리 수첩 보기", "추리하기" };
        public static string[] LocationKor = { "집", "카페", "도서관", "헬스장" };
        public static int ClueCount = 10;

        // 장소마다 행동 선택지가 다름
        public static Dictionary<Location, Behavior[]> locaToBehave = new Dictionary<Location, Behavior[]>
        {
            { Location.Home, new Behavior[]{ Behavior.Move, Behavior.Investigate, Behavior.Exercise, Behavior.Study, Behavior.Diary } },
            { Location.Cafe, new Behavior[]{ Behavior.Move, Behavior.Investigate, Behavior.Diary } },
            { Location.Library, new Behavior[]{ Behavior.Move, Behavior.Investigate,Behavior.Study, Behavior.Diary } },
            { Location.Gym, new Behavior[]{ Behavior.Move, Behavior.Investigate, Behavior.Exercise, Behavior.Diary } }

        };

        // 장소마다 이동 선택지가 다름
        public static Dictionary<Location, Location[]> locaToLoca = new Dictionary<Location, Location[]>
        {
            { Location.Home, new Location[]{ Location.Cafe, Location.Library, Location.Gym } },
            { Location.Cafe, new Location[]{ Location.Home, Location.Library, Location.Gym } },
            { Location.Library, new Location[]{ Location.Home, Location.Cafe, Location.Gym } },
            { Location.Gym, new Location[]{ Location.Home, Location.Cafe, Location.Library } }
        };
    }
}
