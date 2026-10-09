# Theme·Genre·프로젝트 이름 UI 직접 만들기

> **현재 Minjae 씬은 세팅 완료 상태입니다.** 아래 생성 절차를 다시 수행할 필요 없이 [연결된 UI 위치와 수정 방법](ProjectSetupUIReady.md)을 확인하세요. 이 문서는 다른 씬에서 같은 UI를 직접 구성할 때 참고할 수 있습니다.

## 이번 변경과 적용 범위

Theme·Genre와 프로젝트 이름 입력에 한정해 코드로 UI를 생성하던 부분을 제거했습니다. Dashboard, 리뷰 등 다른 UI 코드는 변경하지 않았습니다. 플레이어 타이틀 표시와 캐릭터 애니메이션 전환도 유지합니다.

| 컴포넌트 | 변경 내용 |
| --- | --- |
| ProjectSetupUI | 이름 입력창·잠금 조건 패널·Close 버튼 생성 제거. 직접 만든 UI를 Inspector에 연결 |
| ProjectSetupDropdown | 코드로 자물쇠 도형 생성, 글자 색상·여백 변경 제거 |
| LockedOptionToggle | 직접 만든 Locked Visual을 잠금 상태에 따라 표시 |
| ProjectUnlockProgression | 해금 상태와 알림 대기열만 담당. Canvas·Image·Text 생성 제거 |
| UnlockNotificationUI | 직접 만든 알림 패널을 표시. 메시지와 표시 시간은 Inspector에서 변경 |

새 UI 오브젝트는 자동으로 만들어주지 않습니다. 아래 작업이 완료되기 전에는 이름 입력·힌트·해금 알림이 표시되지 않습니다. 기존 Theme·Genre 드롭다운 참조와 해금 규칙은 유지했습니다. 누락된 UI 참조는 오류 대신 경고로 안내합니다.

Unity의 기본 TMP Dropdown은 펼칠 때 사용자가 편집한 Template을 복제합니다. 이것은 기존 드롭다운의 기본 동작이며 유지합니다. 별도의 하드코딩된 UI 디자인을 생성하는 코드는 제거했습니다.

## 시작하기

1. Play 모드를 종료하고 `Assets/Minjae/Minjae's Scene.unity`를 엽니다.
2. 컴파일이 끝난 뒤 Hierarchy에서 `ProjectSetupPanel`을 찾습니다.
3. 비활성 상태라면 편집을 위해 잠시 활성화합니다. 연결을 끝내면 기존 초기 상태로 돌려놓습니다.
4. 아래의 이름은 추천 이름입니다. 코드가 오브젝트 이름으로 검색하지 않으므로 원하는 이름을 사용해도 됩니다.

권장 구성은 다음과 같습니다. 이미 존재하는 Canvas·ProjectSetupPanel·드롭다운을 사용하세요.

```text
기존 Canvas
├─ ProjectSetupPanel                 [ProjectSetupUI]
│  ├─ ProjectNameInput               [TMP_InputField + Image]
│  │  └─ Text Area
│  │     ├─ Placeholder              [TMP Text]
│  │     └─ Text                     [TMP Text]
│  ├─ ThemeDropdown                  [기존 ProjectSetupDropdown]
│  ├─ GenreDropdown                  [기존 ProjectSetupDropdown]
│  └─ UnlockRequirementPanel         [Image, 초기 비활성]
│     └─ Card                        [Image]
│        ├─ RequirementText          [TMP Text]
│        └─ CloseButton              [Button]
│           └─ Label                 [TMP Text]
├─ UnlockNotificationController      [UnlockNotificationUI, 항상 활성]
└─ UnlockNotificationPanel           [Image, 초기 비활성]
   └─ Message                       [TMP Text]
```

## 1. 이름 입력창

1. 기존 `TitleText`의 위치를 참고합니다. 이 오브젝트는 이제 코드가 자동으로 숨기지 않습니다. 입력창과 겹치면 직접 비활성화하거나, 별도의 제목 라벨로 이동하세요.
2. 제목이 있던 UI 부모 아래에서 우클릭 → **UI → Input Field - TextMeshPro**를 선택합니다.
3. 이름을 `ProjectNameInput`으로 정하고 Rect Transform에서 위치·크기를 맞춥니다. 기존 TitleText의 Rect Transform을 참고해도 됩니다.
4. Image의 **Source Image**에 원하는 Sprite를 넣습니다. 테두리 있는 이미지는 Sprite Editor에서 Border를 설정하고 Image Type을 Sliced로 선택할 수 있습니다. 색상은 Image의 Color에서 정합니다.
5. TMP Input Field에서 **Line Type = Single Line**, **Rich Text 비활성**을 권장합니다. Text·Placeholder의 폰트, 크기, 정렬, 색상은 직접 정합니다. 한글 입력을 쓰려면 한글 글리프를 지원하는 폰트 또는 fallback이 필요합니다.
6. Placeholder의 Text에 `Untitled Project` 등 원하는 안내를 입력합니다.
7. `ProjectSetupPanel`의 Project Setup UI에서 **Name Input**에 새 입력창을 드래그합니다.

**On Value Changed와 On End Edit에 직접 이벤트를 추가할 필요는 없습니다.** ProjectSetupUI가 연결된 입력창에 리스너를 등록합니다. 입력 중 이름을 저장하고 편집 종료 시 앞뒤 공백을 정리합니다. 빈 입력은 데이터상 Untitled Project로 처리합니다.

기존 `Title Text`, `Name Input Background` 필드는 제거했습니다. 입력창 배경은 새 입력창 자체의 Image에서 수정합니다.

## 2. 잠긴 항목의 힌트 창

1. ProjectSetupPanel 아래에서 **UI → Panel**을 만들고 `UnlockRequirementPanel`로 이름을 정합니다. 원하는 배경과 크기를 설정합니다.
2. 그 안에 Card용 Panel을 만듭니다. Card 아래에 **UI → Text - TextMeshPro**와 **UI → Button - TextMeshPro**를 만듭니다.
3. 텍스트를 `RequirementText`, 버튼을 `CloseButton`으로 정합니다. 버튼 자식 텍스트에는 `Close` 또는 원하는 문구를 적습니다.
4. ProjectSetupPanel의 Project Setup UI에 연결합니다.

| Inspector 필드 | 연결할 오브젝트/컴포넌트 |
| --- | --- |
| Requirement Panel | UnlockRequirementPanel GameObject |
| Requirement Text | RequirementText의 TMP Text |
| Requirement Close Button | CloseButton의 Button |
| Theme Dropdown | 기존 ThemeDropdown의 ProjectSetupDropdown |
| Genre Dropdown | 기존 GenreDropdown의 ProjectSetupDropdown |

5. RequirementPanel은 ProjectSetupPanel 아래에 두되, 코드 컴포넌트가 붙은 ProjectSetupPanel 자체를 연결하지 마세요. 패널을 닫으며 컨트롤러까지 비활성화하면 이벤트 처리가 중단됩니다.
6. 힌트 창이 다른 UI 앞에 보이도록 Hierarchy의 형제 순서를 직접 배치합니다. 보통 부모의 마지막 자식으로 두면 됩니다.
7. UnlockRequirementPanel을 비활성화한 상태로 저장합니다. 코드도 시작 시 숨깁니다. Close Button의 On Click은 수동 연결하지 않아도 됩니다.

힌트 문구는 ThemeDropdown·GenreDropdown의 **Option Access → Requirement**에서 바꿉니다. 안내 텍스트는 선택한 항목 이름과 Requirement 문구를 표시합니다. Requirement를 바꿔도 실제 조건은 바뀌지 않고, **Unlock Rule**이 실제 조건입니다.

## 3. 드롭다운 자물쇠 표시

Theme와 Genre 드롭다운 각각 설정합니다.

1. Hierarchy에서 기존 Dropdown의 **Template → Viewport → Content → Item**을 찾습니다. 실제 구조가 다르면 Inspector의 Template 참조를 따라갑니다.
2. 필요하면 편집을 위해 Template을 잠시 활성화합니다.
3. Item 아래에 **UI → Image**를 만들고 원하는 자물쇠 Sprite와 위치·크기·색상을 정합니다.
4. 자물쇠 Image의 **Raycast Target**을 끕니다. 클릭은 Item의 Toggle이 처리합니다.
5. Item의 **Locked Option Toggle → Locked Visual**에 자물쇠 오브젝트를 연결합니다. Item 자체를 연결하면 잠금 해제 시 선택 행 전체가 숨겨지므로 자물쇠만 연결하세요.
6. LockedOptionToggle은 이미 연결되어 있습니다. 일반 Toggle로 교체하지 마세요. 기존 Target Graphic·Checkmark 연결도 유지합니다.
7. 편집 후 Template을 다시 비활성화합니다.

Locked Visual은 선택 사항입니다. 비워두면 자물쇠만 표시되지 않고 잠긴 항목의 선택 차단과 힌트는 계속 작동합니다. 글자 색상은 이제 코드가 회색으로 바꾸지 않으므로 Template의 Label에서 직접 정합니다.

## 4. 해금 알림

1. ProjectSetupPanel 밖의 **항상 활성인 Canvas** 아래에 빈 UI 오브젝트 `UnlockNotificationController`를 만듭니다.
2. **Add Component → Unlock Notification UI**를 추가합니다.
3. 같은 Canvas 아래에 Panel과 자식 TMP Text를 만들고 각각 `UnlockNotificationPanel`, `Message`로 이름을 정합니다.
4. 패널 위치·크기·Sprite·색상과 텍스트 폰트·정렬을 Inspector에서 정합니다. 알림이 클릭을 막지 않게 패널 Image와 Message의 **Raycast Target**을 끕니다.
5. Unlock Notification UI의 필드를 연결합니다.

| 필드 | 용도 |
| --- | --- |
| Notification Panel | 직접 만든 UnlockNotificationPanel |
| Notification Text | Message의 TMP Text |
| Display Seconds | 알림 표시 시간, 기본 5초 |
| Sci Fi Message | SciFi 해금 문구 |
| Horror Message | Horror 해금 문구 |
| Action Message | Action 해금 문구 |
| Simulation Message | Simulation 해금 문구 |

6. NotificationPanel은 초기 비활성으로 저장하고 **컨트롤러는 활성 상태로 유지**하세요. 컨트롤러를 NotificationPanel 안에 넣거나 같은 오브젝트에 붙이지 마세요. 패널이 숨겨지면 Update도 멈추기 때문입니다.
7. 씬마다 알림을 즉시 표시하려면 해당 씬에도 같은 구성을 배치합니다. 한 씬에는 알림 컨트롤러 하나만 두세요. 컨트롤러가 없는 씬에서는 해금은 진행되지만 알림은 대기하고, 연결된 컨트롤러가 있는 씬으로 돌아왔을 때 표시됩니다.

해금 상태와 알림 대기열은 ResourceManager의 ProjectUnlockProgression에서 유지합니다. ResourceManager에는 UI를 만들거나 배치하지 않습니다. 직접 만든 Canvas의 정렬 순서로 표시 우선순위를 조절하세요.

## 5. 유지된 조건과 잠자기 연결

| 종류 | 항목 | 실제 인덱스 | 조건 |
| --- | --- | --- | --- |
| Theme | Fantasy | 1 | 기본 해금 |
| Theme | SciFi | 2 | 게임 2개 이상 완성 |
| Theme | Horror | 3 | 서로 다른 방 업그레이드 2개 이상 구매 |
| Genre | RPG | 1 | 기본 해금 |
| Genre | Action | 2 | 잠자기 완료 |
| Genre | Simulation | 3 | Developer 달성, 스킬 41 이상 |

두 드롭다운의 인덱스 0은 Select Theme/Select Genre 안내입니다. 실제 장르/테마가 아니므로 개발 시작 조건을 충족하지 않습니다.

잠자기 동작은 추가하지 않았습니다. 완료 시 다음을 호출합니다.

```csharp
if (ResourceManager.Instance != null)
{
    var progression = ResourceManager.Instance.GetComponent<ProjectUnlockProgression>();
    if (progression != null) progression.RecordSleep();
}
```

## 6. 연결 후 플레이 테스트

1. 씬을 저장하고 Play합니다. Console에서 ProjectSetupUI나 UnlockNotificationUI의 참조 누락 경고가 없는지 확인합니다.
2. 컴퓨터에서 Project Setup을 열고 이름을 입력합니다. 닫았다 다시 열어도 이름이 유지되고, Enter가 개발을 시작하지 않는지 확인합니다.
3. Fantasy·RPG는 선택 가능해야 합니다. SciFi·Horror·Action·Simulation을 클릭하면 각 조건이 힌트 창에 표시되고 이전 선택은 유지되어야 합니다. Close로 닫히는지도 확인합니다.
4. 드롭다운을 펼쳤을 때 잠긴 항목에 직접 만든 자물쇠가 보이고 열린 항목에는 숨겨지는지 확인합니다.
5. Play 중 ResourceManager의 Development Skill을 40 → 41로 바꿉니다. Simulation 알림이 Inspector에서 지정한 문구·시간으로 표시되고 드롭다운을 다시 열면 선택 가능해야 합니다.
6. ResourceManager의 Project Unlock Progression 컴포넌트 메뉴에서 **TEST - Record Sleep Completed**를 실행합니다. Action이 해금되고 알림이 표시되어야 합니다. 반복 호출해도 알림이 중복되지 않아야 합니다.
7. 서로 다른 방 업그레이드 2개를 구매해 Horror를 확인하고, 실제 게임 2개를 완성해 SciFi를 확인합니다.
8. Play 중 Hierarchy에 추가 입력창·Canvas·자물쇠가 새로 생성되지 않는지 확인합니다. TMP Dropdown의 기본 Template 복제는 정상입니다.

Play 중 바꾼 디자인 값은 종료 후 일반적으로 돌아갑니다. 디자인과 참조 연결은 Play 전에 저장하세요. 실제 Unity Play 화면 검증은 아직 수행하지 않았습니다.
