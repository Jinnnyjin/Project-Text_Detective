# 범인은 누구? (Text Detective)

텍스트 기반 성장형 추리 시뮬레이션 게임. 사건을 해결하고 최고의 탐정이 되어보자

<img width="642" height="615" alt="image" src="https://github.com/user-attachments/assets/0217f0d5-4fe7-4a8f-9cac-fda15be4d737" />

---

## 프로젝트 소개
탐정 스탯을 성장시키고 수집한 단서로 용의자를 심문하여 사건을 해결하는 시뮬레이션 게임입니다. 
단순 반복보다 전략적인 행동 선택과 효율적인 자원(정보/시간) 관리가 핵심입니다.

* **개발 기간:** [2026.07.03 ~ 2026.07.10]
* **개발 인원:** 1인
* **개발 환경:** C#, .NET, Visual Studio

---

## 게임 흐름

[수사 / 성장 / 자원관리]
        ↓
정보 80% 이상 수집 시 '추리하기' 활성화
        ↓
     [법정 배틀]
        ↓
   사건 해결 / 실패

---

##  주요 기능
| 기능 | 설명 |
| :--- | :--- |
| **장소별 행동** | 집·카페·도서관·헬스장 등 장소별 특화 행동 제공 |
| **추리 수첩** | 관찰력 스탯 기반 단서 자동 수집 및 중요도순 정렬 |
| **법정 배틀** | 무작위 변론에 따른 증거 기반의 반박 시스템 |
| **시스템 로그** | 최근 5개 행동을 고정 영역에 출력하여 가독성 확보 |

<img width="792" height="208" alt="image" src="https://github.com/user-attachments/assets/b1650251-4e9a-4849-adb5-9a40153003c5" />

---

##  프로젝트 구조
```
[Project]Text_Detective/
├── Core/       # GameManager, GameContext, IScene, SceneBase, SceneKey
├── Scene/      # TitleScene, MainScene, DeduceScene
├── Models/     # Character, Player, Clue, Evidence, Testimony, Debate
├── Data/       # GameRules, ClueData, CharacterData, DebateData
├── UI/         # GameUI (공통 출력 유틸리티)
└── Program.cs  # 
```

이 프로젝트는 화면(그리기)과 입력 처리 로직을 분리하는 Scene 기반 구조로 설계했습니다.

GameManager:
Scene 등록과 전환, 메인 루프를 담당하는 싱글턴

IScene / SceneBase:
모든 화면이 Render(그리기) / HandleInput(입력 처리) / Enter / Exit를 갖도록 강제하는 공통 규격

---

## 사용한 기술 및 설계 포인트
* **자료구조:** Dictionary: 장소별 행동/이동 목록 매핑 (탐색 성능 $O(1)$ 확보)
    * List: 증거 및 변론 데이터의 무작위 선택 및 정렬 처리
 <img width="785" height="396" alt="image" src="https://github.com/user-attachments/assets/1547c353-bbfe-4c48-ab4d-4bce767ca4c9" />
---

 ## 구현 중 겪은 문제와 해결
**1. 화면 메뉴 번호와 처리 로직 인덱스 불일치**
* **문제:** 장소마다 목록 개수가 달라 화면 번호와 실제 인덱스가 어긋나는 버그 발생
* **해결:** Dictionary로 데이터를 관리하고 Enum과 한국어 배열을 활용하여 화면 출력 리스트와 입력 처리 로직이 항상 동일한 데이터를 참조하도록 리팩토링

**2. 렌더링/로그 표시 지연 문제**
* **문제:** 메시지가 추가되는 시점과 그려지는 시점이 달라 한 사이클 늦게 출력됨
* **해결:** 상태 전환 시점에 로그를 미리 추가하고 렌더링을 호출하도록 순서 조정

---

## 개선점 및 이후 계획
* [ ] 사건(Case) 데이터의 일반화 (여러 사건을 지원하는 구조)
* [ ] 로그창의 화자별 텍스트 색상 구분 (가독성 향상)
* [ ] 추리력을 자원으로 활용하는 심문 시스템 확장
