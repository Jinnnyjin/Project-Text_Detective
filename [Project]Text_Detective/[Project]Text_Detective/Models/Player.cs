using System;

namespace _Project_Text_Detective
{
    public class Player : Character
    {
        public int Hp { get; set; }

        public float MaxHp { get; set; }
        public int JudegeAbility { get; set; }
        public int ObserveAbility { get; set; }
        public int DeductAbility { get; set; }

        public Player(string name) : base(name, CharacterType.Player)
        {
            MaxHp = 15;
            Hp = (int)MaxHp;
            JudegeAbility = 10;
            ObserveAbility = 10;
            DeductAbility = 10;
        }

    }
}






