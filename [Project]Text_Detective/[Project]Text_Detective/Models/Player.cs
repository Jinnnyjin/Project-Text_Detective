using System;

namespace _Project_Text_Detective
{
    public enum Location
    {
        Home, Cafe, Library, Gym
    }

    public class Player : Character
    {
        public int Hp { get; set; }
        public float MaxHp { get; set; }
        public Location Location { get; set; }
        public List<Clue> Clues { get; set; }
        public int TurnCount { get; set; }
        public int Day { get; set; }
        
        public int JudegeAbility { get; set; } // 판단력
        public int ObserveAbility { get; set; } // 관찰력
        public float DeductAbility { get; set; } // 추리력


        public Player(string name) : base(name, CharacterType.Player)
        {
            MaxHp = 15;
            Hp = (int)MaxHp;
            this.Location = Location.Home;
            TurnCount = 0;
            Day = 1;

            JudegeAbility = 1;
            ObserveAbility = 1;
            DeductAbility = 1;
            Clues = new List<Clue>();
        }

    }
}






