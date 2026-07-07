using System;

namespace _Project_Text_Detective
{

    public static class CharacterData
    {
        public static List<Character> TutoCharacters = new List<Character>
        {
            new Character("비혜자", CharacterType.Victim),
            new Character("알바생", CharacterType.Criminal),
            new Character("김단골", CharacterType.Suspect),
            new Character("나사장", CharacterType.Suspect)
        };
    }

}
