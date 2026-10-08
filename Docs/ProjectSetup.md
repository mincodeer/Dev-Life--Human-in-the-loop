# Project Setup 변경 안내

대상 Scene은 `Assets/Minjae/Minjae's Scene.unity`입니다. 이름 입력과 잠금 안내를 연결했고, 기존 Project Setup → Coding → Design → Sound → Debugging → Build → Result 순서는 유지합니다.

이 문서는 Unity를 켜고 하나씩 따라 확인할 수 있는 작업 노트예요. 연결이 헷갈리면 먼저 **Inspector 확인 순서**, 실제 동작을 확인하고 싶으면 **Unity 플레이 테스트 순서**, 오류가 생기면 **디버깅 순서**를 읽어주세요.

앞으로 관련 작업을 마칠 때마다 여기에 수정 이유, 바뀐 기능, 수정 파일, Inspector 연결, 테스트 방법과 결과를 함께 정리하겠습니다. 직접 확인한 결과와 아직 Unity에서 확인해야 할 항목도 구분해서 적을게요.

## 최근 작업에서는 무엇을 정리했나요?

이름 입력과 잠금 UI를 만들면서 저장 기능도 연결했지만, SaveLoadManager는 건드리지 말아 달라는 요청에 따라 그 작업에서 추가한 저장 연결과 이름 복원 보조 코드를 되돌렸어요. 이름 입력·수정과 실행 중 이름 유지는 남아 있으며, 게임을 다시 실행했을 때 이름을 불러오는 기능은 현재 포함하지 않습니다.

반복되던 `LockedOptionToggle` 오류의 원인을 확인했어요. 제가 만든 `.meta` 파일의 GUID가 Unity에서 요구하는 32자리보다 한 자리 긴 **33자리**였습니다. Unity Editor 로그에도 유효하지 않은 GUID 때문에 에셋을 무시한다는 오류가 남아 있었어요. 그래서 `.cs` 파일은 있어도 Unity 컴파일 목록에 들어가지 않았던 것입니다. 앞선 수정에서 GUID 길이까지 확인하지 못했습니다.

이번에는 LockedOptionToggle의 GUID를 올바른 32자리로 고쳤고, Theme와 Genre의 Item이 사용하는 Scene 참조 두 곳도 함께 바꿨어요. ProjectSetupDropdownEditor의 `.meta`에도 같은 길이 문제가 있어 같이 수정했습니다. 이름 입력·잠금 동작의 C# 코드는 그대로 두었으며 SaveLoadManager는 수정하지 않았습니다.

수정 파일은 `LockedOptionToggle.cs.meta`, `ProjectSetupDropdownEditor.cs.meta`, `Minjae's Scene.unity`, 이 문서입니다. GUID는 Unity가 스크립트 에셋을 찾는 식별자라서, `.meta`만 고치고 Scene 참조를 그대로 두면 Missing Script가 생길 수 있어요. 이번에는 양쪽을 함께 맞췄으므로 대상 Scene의 Inspector 연결을 다시 만들 필요는 없습니다.

## 기존 연결과 변경 이유

- `ProjectSetupPanel/TitleText`의 `CREATE NEW GAME`은 TMP 제목 텍스트입니다. 버튼이 아니므로 이 위치에 입력창을 생성합니다. 별도 `StartButton`의 `StartDevelopment()` 연결은 유지합니다.
- Theme와 Genre는 `TMP_Dropdown`이며, dropdown index 1~3을 `DevelopmentFlowManager`가 enum에 대응시켜 `ProjectDataManager.CurrentProject`에 저장합니다.
- `ProjectData`에는 이미 `projectName = "Untitled Project"`가 있습니다. 개발 단계는 이 객체의 작업 방법을 변경하며 이름을 초기화하지 않습니다. Result에서 `ArchiveCurrentProject()`가 JSON으로 별도 복사하므로 이후 이름을 바꿔도 완료된 프로젝트 이름은 유지됩니다. Dashboard/Review는 기존 이름 필드를 사용합니다.
- `StartNewProject()`에서만 새 데이터를 만듭니다. 새 프로젝트는 기본 이름으로 돌아갑니다.
- 요청에 따라 `SaveLoadManager`는 변경 전으로 복원했습니다. 프로젝트 이름 Save/Load 및 불러온 이름 전달 코드는 제거했습니다. 이름 입력과 실행 중 개발·완료 데이터의 이름 유지는 그대로입니다.
- `DevelopmentTutorialPopup`은 튜토리얼 진행 단계, Skip 상태, 최초 표시 플래그와 타이핑 Coroutine을 관리합니다. 잠금 설명에는 별도 작은 패널을 사용하며 이 스크립트는 변경하지 않습니다.
- 저장소의 코드·README에서 Theme/Genre 잠금 규칙 또는 Trello 링크를 찾지 못했습니다. 기본적으로 기존 항목 모두 선택 가능합니다. 게임 수·Fandom·레벨 수치는 추가하지 않았습니다.

## 수정 파일

| 파일 | 역할 |
| --- | --- |
| `Assets/Minjae/Minjae's Scene.unity` | Setup UI 참조 연결, dropdown/Item 확장 컴포넌트 사용, Tutorial 중복 이벤트 제거 |
| `Assets/Minjae/Scripts/Development process/ProjectSetupUI.cs` | 제목 위치의 이름 입력창, 데이터 동기화, 별도 해제 조건 패널 |
| `Assets/Minjae/Scripts/Development process/ProjectSetupDropdown.cs` | 기존 TMP 템플릿 유지, 항목별 잠금 설정, 회색 글씨와 자물쇠 |
| `Assets/Minjae/Scripts/Development process/LockedOptionToggle.cs` | 잠긴 항목의 마우스 클릭과 키보드 Submit 차단 및 안내 호출 |
| `Assets/Minjae/Editor/ProjectSetupDropdownEditor.cs` | TMP Inspector를 유지하면서 Option Access 설정 표시 |
| `Assets/Minjae/Scripts/Development process/ProjectDataManager.cs` | 이름 정규화, 새 프로젝트·초기화 시 UI 변경 알림 |
| `Assets/Minjae/Scripts/Development process/DevelopmentFlowManager.cs` | 유효한 선택 뒤 Tutorial 호출, 잠금 검사, placeholder 선택 시 데이터 None 동기화 |
| `Docs/ProjectSetup.md` | 연결·학습·검증 안내 |

새 스크립트와 Editor 폴더의 `.meta`도 포함됩니다. 기존 `.vscode/settings.json`과 `.slnx`의 사용자 변경은 수정하지 않았습니다. 검증용 임시 파일은 Git에서 제외되는 `Temp` 또는 `.utmp`에 두며 게임 에셋에 포함하지 않습니다. Unity가 `Temp`를 정리하면 이전 검사 로그는 없어질 수 있어요.

## Inspector 확인 순서

대상 Scene에는 연결을 저장해 두었어요. 처음부터 오브젝트를 다시 만들 필요는 없습니다. 우선 Play 모드를 끄고 아래 순서로 연결이 남아 있는지 확인해주세요.

1. `ProjectSetupPanel`의 `ProjectSetupUI`에서 `Title Text = TitleText`, `Theme Dropdown = ThemeDropdown`, `Genre Dropdown = GenreDropdown`을 확인합니다. 입력창과 안내 패널은 Play 시 생성되어 Hierarchy에 표시됩니다. 기존 제목은 Play 시 숨깁니다.
2. 두 dropdown이 `ProjectSetupDropdown`인지 확인합니다. 기존 Template, Caption Text, Item Text, Options는 그대로입니다. Template → Viewport → Content → Item의 Toggle은 `LockedOptionToggle`입니다. Target Graphic과 Checkmark 연결도 유지합니다.
3. Theme dropdown의 On Value Changed에는 동적 int `DevelopmentFlowManager.OnThemeDropdownChanged`를, Genre에는 `OnGenreDropdownChanged`를 유지합니다. Tutorial의 `ThemeSelected`/`GenreSelected`를 Inspector에 다시 추가하지 마세요. 유효한 선택 뒤 코드에서 호출합니다.
4. `StartButton`에는 `DevelopmentFlowManager.StartDevelopment()`가 연결됩니다. `ShowCoding()`을 별도로 추가하지 마세요. Coding 전환 성공 후 기존 `ShowStageTutorial()`이 호출합니다.
5. 잠금 규칙이 정해지면 dropdown의 `Option Access`에 항목을 추가합니다. `Option Index`, `Unlocked`, `Requirement`를 지정합니다. 같은 index는 한 번만 등록합니다. index 0은 "Select …"이므로 잠그지 않습니다.

| index | Theme | Genre |
| --- | --- | --- |
| 0 | Select Theme | Select Genre |
| 1 | Fantasy | RPG |
| 2 | SciFi | Action |
| 3 | Horror | Simulation |

등록하지 않은 항목은 기존처럼 열려 있습니다. `Unlocked = false`일 때 회색 글씨와 작은 자물쇠가 표시됩니다. `Requirement`에는 실제 확정된 조건을 입력하세요. 설명이 비어 있으면 조건이 아직 정의되지 않았다는 안내를 표시합니다.

자물쇠는 UI Image 네 개로 그리는 작은 단색 아이콘으로, 폰트 이모지 지원과 무관합니다. 기존 dropdown의 배경, 폰트, ScrollRect, 체크 표시와 키보드 이동을 유지합니다. 일반 TMP dropdown은 잠금 클릭 전용 이벤트가 없고 Toggle을 비활성화하면 클릭 자체가 사라지므로, Toggle의 클릭/Submit만 확장했습니다.

기존 진행 코드에서 조건을 판정하게 되면 해당 dropdown의 `SetOptionUnlocked(index, true)`를 호출합니다. 현재 이 메서드에 게임 수·Fandom·레벨 규칙을 자동 연결하지 않았으며 잠금 상태 자체를 Save/Load에 추가하지 않았습니다. 필요한 결정은 **잠글 항목, 실제 해제 조건, 조건의 기준 데이터, 잠금 상태를 조건에서 재계산할지 별도 저장할지**입니다. Trello 카드가 있다면 카드의 정의를 우선 사용해야 합니다.

`SaveLoadManager`와 Save/Load 버튼의 기존 연결은 변경하지 않습니다. 이름 저장·복원은 현재 구현 범위에서 제외했습니다.

`LockedOptionToggle.cs`가 디스크에 있는데도 CS0246이 발생하면 `.meta`의 GUID와 Unity 임포트 로그를 함께 확인해주세요. 이번 오류는 GUID가 33자리여서 Unity가 해당 파일을 무시한 경우였습니다. 올바른 32자리 GUID와 Scene 참조로 수정했으니 Unity에서 Assets → Refresh를 실행해주세요. 오류가 남으면 해당 스크립트를 선택해 Reimport하고 컴파일을 기다립니다. 생성된 `.csproj`에 수동으로 추가할 필요는 없습니다.

## 핵심 메서드의 실행 순서

이름 입력:

1. Setup이 처음 활성화되면 `ProjectSetupUI.Awake()`가 `CreateNameInput()`과 `CreateRequirementPanel()`을 실행합니다. 제목의 위치·크기·폰트를 사용합니다.
2. `OnEnable()`이 프로젝트 변경 알림을 구독하고 `RefreshFromProject()`로 데이터의 이름과 선택값을 표시합니다. `SetTextWithoutNotify`/`SetValueWithoutNotify`를 사용하므로 동기화만으로 Tutorial이나 선택 이벤트가 발생하지 않습니다.
3. 입력할 때 `OnNameChanged()` → `ProjectDataManager.SetProjectName()` → `NormalizeProjectName()` 순서로 저장합니다. 앞뒤 공백을 제거하고 빈 값·공백만 있는 값은 `Untitled Project`로 저장합니다. 입력 중에는 필드의 원래 문자열을 유지하여 공백을 입력할 때 커서가 튀지 않습니다.
4. 편집 종료 시 `OnNameEditEnded()`가 정규화한 문자열을 화면에도 반영합니다. 기본 이름이면 필드를 비워 짙은 회색 반투명 placeholder가 다시 보이게 합니다. 이름 입력의 Enter는 편집만 종료하고 게임을 시작하지 않습니다.
5. `StartDevelopment()`는 Theme/Genre 선택 및 잠금 여부를 확인한 뒤 Coding으로 전환합니다. 이름은 같은 `ProjectData`에 남습니다. Result의 기존 archive 복사가 이름도 보관합니다.

잠금 안내:

1. dropdown을 열면 `CreateDropdownList()`가 항목 번호를 초기화합니다.
2. TMP가 `CreateItem()`을 호출할 때 index별 설정을 검사하여 잠긴 글씨를 회색으로 만들고 자물쇠를 붙입니다.
3. 클릭/키보드 Submit → `LockedOptionToggle.TryShowRequirement()`가 현재 잠금을 다시 확인합니다.
4. 잠겼다면 기본 Toggle 선택 코드를 실행하지 않고 조건 안내를 호출합니다. dropdown value, 프로젝트 enum, Tutorial 플래그는 변경하지 않습니다.
5. 안내의 Close를 누르면 원래 dropdown으로 키보드 포커스를 돌립니다. 열린 항목을 선택하면 기존 TMP 이벤트 → FlowManager 선택 저장 → Tutorial 안내 순서로 진행합니다.

**저장 범위:** 이름은 현재 실행 중 `ProjectData`와 완료 프로젝트 archive에 유지됩니다. 프로젝트 이름을 PlayerPrefs에 저장하거나 Load하는 코드는 제거했으므로 게임 재실행 후 이름 복원은 제공하지 않습니다. 기존 Save/Load 동작은 변경 전과 동일합니다.

## Unity 플레이 테스트 순서

먼저 기본 기능을 확인하고, 그다음 잠금 테스트를 하는 순서가 편해요. 한 번에 모두 바꾸면 어떤 설정 때문에 문제가 생겼는지 찾기 어려우니 아래처럼 나눠 확인해주세요.

1. **컴파일부터 확인해요.** Unity에서 Assets → Refresh를 실행하고 컴파일이 끝날 때까지 기다려주세요. Console에 빨간 오류가 남아 있으면 Play 전에 아래 디버깅 순서부터 확인합니다.
2. **기본 화면을 열어요.** 대상 Scene에서 기존 로비·컴퓨터 진입 흐름을 따라 Project Setup을 엽니다. 제목 위치에 짙은 회색 반투명 `Untitled Project`가 보여야 해요. 입력창과 안내 패널은 실행 중 생성되므로 Play 전 Hierarchy에 없어도 괜찮습니다.
3. **이름을 입력하고 수정해요.** `My First Game`을 입력한 뒤 `My Second Game`으로 바꿔보세요. Enter는 편집만 끝내고 개발을 시작하지 않아야 합니다. 빈 값, 공백만 있는 값, `  My Game  `도 확인해주세요. 빈 값은 placeholder로 돌아오고, 앞뒤 공백은 편집 종료 후 정리되어야 해요. 긴 이름은 입력창 안에서 스크롤되는지, 한글은 글자가 제대로 표시되는지도 확인합니다.
4. **시작 조건과 다시 열기를 확인해요.** Theme/Genre를 모두 고르기 전에 Start를 누르면 Coding으로 넘어가지 않아야 합니다. 둘 다 고른 뒤 컴퓨터를 닫았다 다시 열어 이름과 선택값이 유지되는지 확인해주세요.
5. **개발과 결과까지 이어가요.** Start Development를 누르고 Coding → Design → Sound → Debugging → Build → Result를 진행합니다. `ProjectDataManager`의 Current Project와 완료 프로젝트 데이터, Dashboard에 표시되는 이름을 확인해주세요. 다음 프로젝트를 시작하면 기본 이름으로 돌아와야 합니다.
6. **잠금 테스트를 준비해요.** Play를 끈 뒤 Theme dropdown의 Option Access에 index `2`를 한 번만 추가합니다. Unlocked를 끄고 Requirement에 `잠금 안내 테스트`를 입력해주세요. 실제 해제 규칙이 아니라 UI 확인용 임시 설정입니다.
7. **잠긴 항목을 눌러봐요.** 다시 Play해서 Fantasy를 먼저 선택한 뒤 SciFi를 누릅니다. SciFi에는 회색 글씨와 작은 자물쇠가 보이고, 클릭하면 테스트 문구가 있는 안내 패널이 열려야 해요. 선택값은 Fantasy로 남아 있어야 합니다. Close를 누른 뒤 Horror처럼 열린 항목도 선택해보세요. 같은 테스트를 Genre의 RPG → 잠긴 Action으로도 확인합니다.
8. **키보드와 해제 상태도 확인해요.** dropdown에서 방향키로 잠긴 항목으로 이동해 Submit할 때도 안내만 열리는지 확인합니다. Play를 끄고 테스트 항목의 Unlocked를 켠 뒤 다시 Play하면 회색 표시·자물쇠 없이 선택되어야 해요. Tutorial은 잠금 안내 때문에 최초 표시 상태가 바뀌지 않아야 합니다.
9. **테스트 설정을 정리해요.** Play를 끄고 임시 Option Access 항목을 제거합니다. 필요한 Scene 변경만 저장해주세요. 실제 잠금 규칙은 항목과 해제 조건이 정해진 뒤 연결하면 됩니다.

Save/Load는 이름 테스트에 포함하지 않아요. 기존 저장 기능은 이번 UI 작업의 수정 범위에서 제외했으며, 이름이 실행 중 유지되는 것과 게임 재실행 후 복원되는 것은 별개입니다.

## 문제가 생겼을 때 디버깅 순서

Console에서는 먼저 나오는 빨간 오류부터 확인해주세요. 뒤에 나오는 오류들은 첫 오류의 영향일 수도 있어요. 고친 뒤에는 같은 테스트를 다시 실행해 문제가 사라졌는지 확인하면 됩니다.

| 증상 | 먼저 볼 곳 | 확인 순서 |
| --- | --- | --- |
| `LockedOptionToggle`을 찾지 못한다는 CS0246 | 해당 `.cs` 파일과 `.meta` | GUID가 32자리인지, Console/Editor 로그에 invalid GUID 오류가 있는지 확인합니다. 이번에는 33자리 GUID를 수정했어요. Assets → Refresh 후 필요하면 파일 우클릭 Reimport를 실행합니다. |
| `Project Setup dropdown Item needs LockedOptionToggle` | dropdown의 Template 안 Item | Item에 일반 Toggle 대신 LockedOptionToggle이 있는지, Target Graphic과 Checkmark 참조가 남아 있는지 확인합니다. |
| 이름 입력창이 나타나지 않아요 | ProjectSetupPanel의 ProjectSetupUI | Title Text·Theme Dropdown·Genre Dropdown 참조를 확인합니다. Play 중 Panel이 활성화됐는지, `Project Name Input`이 생성됐는지도 살펴보세요. |
| 이름을 입력해도 데이터가 바뀌지 않아요 | ProjectDataManager의 Current Project | 입력 중 `projectName`이 바뀌는지 확인합니다. 활성 ProjectDataManager가 있고 singleton Instance가 연결됐는지 확인해주세요. |
| 한글이 네모로 보여요 | TitleText에서 사용하는 TMP 폰트 | 입력창이 제목 폰트를 사용하므로 한글 글리프 또는 fallback font 지원을 확인합니다. |
| 잠근 항목이 계속 선택돼요 | Option Access와 Item Toggle | index·Unlocked 값을 확인하고 Play를 새로 시작합니다. Item의 LockedOptionToggle 연결과 dropdown 이벤트가 올바른지도 확인해주세요. |
| 잠금 안내가 표시되지 않아요 | ProjectSetupUI의 dropdown 참조 | 실제로 클릭한 dropdown이 연결돼 있는지, 실행 중 `Unlock Requirement Panel`이 생성됐는지 확인합니다. |
| 이름·선택값이 갑자기 초기화돼요 | StartNewProject 호출 시점 | 컴퓨터를 다시 여는 것인지 새 프로젝트를 시작하는 것인지 먼저 확인합니다. 데이터는 StartNewProject에서 초기화되므로 그 호출 위치를 따라가세요. |
| Tutorial이 잠금 클릭에도 진행돼요 | dropdown의 On Value Changed | Inspector에 ThemeSelected·GenreSelected가 별도로 남아 있는지 확인합니다. 유효한 선택 후 FlowManager에서만 호출되도록 연결을 확인해주세요. |

코드 실행 순서를 직접 보고 싶다면 이름 입력에서는 `OnNameChanged()` → `SetProjectName()`, 잠금 클릭에서는 `TryShowRequirement()` → `ShowRequirement()` 순서로 breakpoint를 걸어보세요. 잠긴 항목 클릭 시 FlowManager의 선택 저장 메서드로 넘어가지 않는지 확인하면 선택값이 유지되는 이유를 이해하기 쉬워요.

## 어디까지 확인했나요?

- 이름 Save/Load를 제거한 뒤 전체 런타임 소스와 새 Inspector 코드를 Unity 6000.3.8f1 참조로 컴파일했고 오류가 없었습니다.
- 앞선 버전은 분리된 Unity Play 모드에서 25개 검사를 통과했어요. 그 검사에는 지금 제거한 이름 Save/Load 검사도 포함되어 있으므로, 현재 버전의 Save/Load 검증 결과로 보지는 않습니다.
- 원본 Unity Editor 로그에서 invalid GUID로 LockedOptionToggle 에셋이 무시되는 것을 확인했습니다. 두 새 `.meta`의 33자리 GUID와 Scene의 관련 참조를 수정했습니다.
- 수정 후 추가한 `.meta` 5개의 GUID가 모두 32자리이며 서로 중복되지 않는지 확인했습니다. 대상 Scene의 GUID 형식과 Theme/Genre Toggle 참조 두 곳도 검사했고, 전체 런타임·Inspector 코드를 Unity 참조로 재컴파일해 오류 없이 통과했습니다.
- 이번 수정은 에셋 식별자와 Scene 참조를 바로잡는 작업입니다. 새로운 전체 플레이 테스트는 실행하지 않았으므로, Unity에서 Refresh 후 두 dropdown의 Item에 Missing Script가 없는지 먼저 확인해주세요. 그다음 이름 입력 → 열린 항목 선택 → 잠긴 항목 클릭 → 조건 패널 닫기 → Start Development 순서로 확인하면 됩니다.
