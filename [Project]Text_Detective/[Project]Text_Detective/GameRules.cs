using System;


namespace _Project_Text_Detective
{

    public static class GameRules
    {

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
