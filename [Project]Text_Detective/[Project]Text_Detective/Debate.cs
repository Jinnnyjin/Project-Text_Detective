using System;

namespace _Project_Text_Detective
{

    public class Debate
    {
        public string Statement { get; set; }
        public Clue RightClue { get; set; }


        public Debate (string statement, Clue rightClue)
        {
            Statement = statement;
            RightClue = rightClue;
        }
    }

}