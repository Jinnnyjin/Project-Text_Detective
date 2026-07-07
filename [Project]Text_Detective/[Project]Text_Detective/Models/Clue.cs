using System;

namespace _Project_Text_Detective
{
    public enum ClueImportance
    {
        Critical, Minor
    }

    public class Clue
    {

        public string Name { get; set; }
        public ClueImportance Importance {  get; set; }
        public string Description { get; set; }

        public Location Located;

    }
}
