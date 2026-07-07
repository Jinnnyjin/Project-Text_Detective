using System;

namespace _Project_Text_Detective
{

    public static class ClueData
    {
        public static List<Clue> Tutoclues = new List<Clue>
        {
            new Evidence {Name = "카페 내부 CCTV", Description = "매장 내부에 설치된 CCTV이다. 그 날 고장나있었기 때문에 녹화된 것이 없다. ", Importance = ClueImportance.Minor },
            new Evidence {Name = "충전기에 찍혀있는 지문", Description = " 피해자 지문 외의 [나사장]과 [알바생]의 지문도 추가검출 되었다.", Importance = ClueImportance.Minor },
            new Evidence {Name = "김단골의 스마트폰", Description = "배터리가 부족하다. 당장이라도 충전을 해야할 것 같다. ", Importance = ClueImportance.Minor },
            new Evidence {Name = "케이블 홀더", Description = "[알바생]의 가방에서 피해자 충전기에 꽂혀있던 케이블 홀더가 발견되었다.", Importance = ClueImportance.Critical },
            new Evidence {Name = "멕가이버 칼", Description = "[김단골]의 소지품. 평소에도 소지하고 있다고한다. ", Importance = ClueImportance.Minor },
            new Evidence {Name = "커터칼", Description = "매장 카운터에 있는 커터칼. 매장에서 근무한다면 누구나 사용할 수 있다. ", Importance = ClueImportance.Critical },

            new Testimony {Name = "증언 1", Description = "[알바생]이 피해자와 이전에 말다툼을 한적이 있어요. ", Importance = ClueImportance.Critical },
            new Testimony {Name = "증언 2", Description = "[나사장]은 피해자가 이 카페에 오는걸 원하지 않았어요.", Importance = ClueImportance.Minor },
            new Testimony {Name = "증언 3", Description = "[알바생], [나사장], [김단골]이 그 테이블 주변에 왔다갔다 하는걸 봤어요. ", Importance = ClueImportance.Minor },
            new Testimony {Name = "증언 4", Description = "[김단골]이 휴대폰 배터리가 없다며 카페 카운터 충전기를 빌려달라고 했어요. ", Importance = ClueImportance.Minor }
        };



        
    }

}