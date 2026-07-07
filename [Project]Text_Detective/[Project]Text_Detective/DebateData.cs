using System;
using System.Data;

namespace _Project_Text_Detective
{
    public static class DebateData
    {
        public static List<Debate> TutoDebate = new List<Debate>
        {
            new Debate("제가 왜 그런 짓을 했겠어요? 말도 안돼요.", ClueData.Tutoclues[6]),
            new Debate("그 주변에는 얼씬도 안했다구요!", ClueData.Tutoclues[8]),
            new Debate("저는 오늘 직원용 휴게실에서 쉬었을 뿐이예요.", ClueData.Tutoclues[3]),
            new Debate("저는 충전기는 본적도 만진적도 없어요", ClueData.Tutoclues[1]),
            new Debate("그럼 제가 손으로 그걸 끊었다는 말씀이세요?", ClueData.Tutoclues[5]),

        };
    }

}