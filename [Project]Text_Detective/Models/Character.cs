namespace _Project_Text_Detective
{
    public enum CharacterType
    { Player, Suspect, Criminal, Victim }
    public class Character
    {
        public string Name { get; private set; }
        public CharacterType Type { get; set; }

        public Character(string name, CharacterType type)
        {
            Name = name;
            Type = type;
        }
    }
}
