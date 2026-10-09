# Developer 등급 캐릭터 애니메이션 전환 안내

## 변경 목적

플레이어가 Developer 등급에 도달했을 때 캐릭터의 모습도 함께 바뀌도록 했습니다. 개발 스킬이 40 이하인 동안에는 기존 `OldCharacter` 애니메이션을 재생하고, 41 이상이 되면 `NewCharacter` 애니메이션을 재생합니다.

Developer 등급의 시작점을 41로 잡은 이유는 기존 `PlayerTitleController.cs`가 스킬 21~40을 Junior Developer, 41~60을 Developer로 표시하기 때문입니다. 캐릭터 전환 조건을 이 타이틀 기준에 맞췄습니다.

| 개발 스킬 | 표시되는 타이틀 | 재생할 애니메이션 |
| --- | --- | --- |
| 0~20 | Beginner Developer | OldCharacter |
| 21~40 | Junior Developer | OldCharacter |
| 41~60 | Developer | NewCharacter |
| 61~80 | Senior Developer | NewCharacter |
| 81~100 | Expert Developer | NewCharacter |

## 어떤 파일을 변경했나요?

### 1. `Assets/Minjae/Scripts/CharacterTitleAnimation.cs` 추가

캐릭터의 애니메이션을 개발 스킬에 맞춰 전환하는 컴포넌트를 추가했습니다.

- `Awake()`에서 같은 오브젝트에 있는 Animator를 가져옵니다.
- `OnEnable()`과 `Update()`에서 `ResourceManager.Instance.DevelopmentSkill`을 읽습니다. 씬을 처음 열었을 때와 플레이 중 스킬이 바뀌었을 때 모두 현재 스킬에 맞춰 전환합니다.
- 스킬이 41 이상이면 새 Animator Controller를, 40 이하이면 기존 Animator Controller를 선택합니다.
- 현재 컨트롤러와 선택한 컨트롤러가 다를 때만 교체합니다. 매 프레임 애니메이션을 다시 시작하지 않고 계속 재생하도록 하기 위해서입니다.
- ResourceManager가 아직 준비되지 않았다면 다음 갱신을 기다립니다.

Unity에서 스크립트 참조를 유지하도록 `CharacterTitleAnimation.cs.meta`도 함께 추가했습니다.

### 2. `Assets/Minjae/Minjae's Scene.unity` 연결 수정

기존 캐릭터 오브젝트 `OldCharacter_0`에 `CharacterTitleAnimation` 컴포넌트를 붙이고 다음 컨트롤러를 연결했습니다.

| Inspector 필드 | 연결한 에셋 |
| --- | --- |
| Old Character Controller | `Assets/Minjae/Assets/Animation/OldCharacter_0.controller` |
| New Character Controller | `Assets/Minjae/Assets/Animation/UpgradedCharacter_Warm_0.controller` |

새 컨트롤러는 기존에 준비되어 있던 `NewCharacter.anim`을 재생합니다. 이번 변경은 해당 애니메이션을 사용하는 연결과 전환 로직을 추가한 것입니다.

## 왜 기존 캐릭터 오브젝트를 그대로 사용하나요?

### 화면 위치가 밀리는 문제 수정

새 스프라이트의 8개 프레임은 처음에 가운데 피벗(`0.5, 0.5`)으로 설정되어 있었지만, 기존 프레임은 왼쪽 아래 피벗(`0, 0`)이었습니다. 피벗은 이미지에서 오브젝트 위치에 맞출 기준점이므로, Transform이 같아도 서로 다른 피벗을 사용하면 화면 위치가 달라집니다.

`Assets/Minjae/Assets/Room/NewRoom/UpgradedCharacter_Warm.png.meta`의 8개 프레임을 기존과 같은 **Bottom Left** 피벗으로 수정했습니다. 두 이미지의 Pixels Per Unit은 모두 27이며, 캐릭터 Transform은 이동하지 않았습니다.

확인할 때는 현재 Play 모드를 종료하고 Unity가 새 이미지 설정을 다시 임포트하도록 기다린 뒤 재실행하세요. 스킬을 40 → 41 → 40으로 변경하며 캐릭터가 기존 자리에서 바뀌는지 확인합니다. Sprite Editor에서 새 이미지의 모든 프레임 Pivot이 Bottom Left인지도 확인할 수 있습니다. 실제 화면에서의 최종 위치는 이 플레이 테스트로 확인해야 합니다.

씬에는 `OldCharacter_0` 오브젝트가 있고, 새 애니메이션 컨트롤러는 연결되어 있지 않았습니다. 기존 오브젝트의 Animator Controller를 바꾸면 위치, 크기, 렌더링 순서를 유지하면서 캐릭터 스프라이트 애니메이션을 교체할 수 있습니다.

따라서 Developer가 된 후에도 Hierarchy의 오브젝트 이름은 `OldCharacter_0`으로 남습니다. 화면에서는 새 캐릭터 애니메이션이 재생됩니다. 별도의 새 캐릭터 오브젝트를 생성하거나 기존 오브젝트를 삭제하는 방식은 아닙니다.

Senior·Expert가 되어도 새 애니메이션을 유지합니다. 반대로 스킬이 40 이하로 내려가거나 리셋되면 기존 애니메이션으로 돌아옵니다. 현재 구현은 현재 스킬에 따라 모습을 결정하며, 승급 모습을 영구적으로 저장하지는 않습니다.

## Unity 플레이 테스트 방법

### 준비

1. Unity에서 이 프로젝트를 엽니다. 프로젝트에 기록된 에디터 버전은 `6000.3.8f1`입니다.
2. Project 창에서 `Assets/Minjae/Minjae's Scene.unity`를 열고 스크립트 컴파일이 끝날 때까지 기다립니다.
3. Hierarchy에서 `OldCharacter_0`을 검색해 선택합니다.
4. Inspector에서 Animator와 Character Title Animation 컴포넌트가 있는지 확인합니다.
5. Character Title Animation의 두 컨트롤러 필드가 위 표의 에셋으로 연결되어 있는지 확인합니다.
6. Console에 컴파일 오류가 없는지 확인한 뒤 Play 버튼을 누릅니다.

이 기능을 연결한 씬은 Minjae 씬입니다. 다른 씬을 열고 테스트하면 해당 씬의 캐릭터에는 전환 컴포넌트가 없을 수 있습니다.

### 테스트 1: Junior에서 Developer로 전환

1. **Play 모드를 유지한 상태에서** Hierarchy의 `ResourceManager` 오브젝트를 선택합니다. 씬 이동 후에는 `DontDestroyOnLoad` 영역에 있을 수도 있습니다.
2. Inspector의 Resource Manager 컴포넌트에서 `Development Skill`을 **40**으로 설정합니다.
3. Game 창에서 기존 캐릭터 애니메이션이 재생되는지 확인합니다. 타이틀은 `Junior Developer`여야 합니다.
4. `Development Skill`을 **41**로 변경합니다.
5. Game 창에서 새 캐릭터 애니메이션으로 전환되고 타이틀이 `Developer`가 되는지 확인합니다.
6. `OldCharacter_0`을 다시 선택하고 Animator의 Controller가 `UpgradedCharacter_Warm_0`으로 바뀌었는지 확인합니다.
7. 몇 초 동안 관찰해 새 애니메이션이 반복 재생되는지, 캐릭터 위치가 유지되는지 확인합니다.

### 테스트 2: 상위 타이틀에서도 유지

ResourceManager의 `Development Skill`을 차례로 변경하고 다음 결과를 확인합니다.

| 입력값 | 예상 타이틀 | 예상 애니메이션 |
| --- | --- | --- |
| 60 | Developer | NewCharacter |
| 61 | Senior Developer | NewCharacter |
| 80 | Senior Developer | NewCharacter |
| 81 | Expert Developer | NewCharacter |
| 100 | Expert Developer | NewCharacter |

### 테스트 3: 스킬 감소와 리셋

1. 스킬을 41 이상으로 설정해 새 애니메이션이 재생되는 상태를 만듭니다.
2. 스킬을 **40**으로 내립니다. 기존 애니메이션과 Junior Developer 타이틀로 돌아오는지 확인합니다.
3. 스킬을 다시 41 이상으로 올립니다.
4. Resource Manager 컴포넌트의 메뉴에서 **Reset Resources**를 실행합니다.
5. 스킬이 **10**으로 바뀌고 기존 애니메이션과 Beginner Developer 타이틀로 돌아오는지 확인합니다.

Reset Resources는 돈, 팬덤, AI 크레딧, 기술 부채도 함께 초기화합니다. 진행 중인 플레이 상태를 유지해야 한다면 2번의 스킬 변경으로만 확인하세요.

### 테스트 4: 높은 스킬로 씬 시작

1. Play 모드를 종료합니다.
2. 씬의 ResourceManager에서 Development Skill을 **41**로 설정합니다.
3. Play 버튼을 눌러 처음부터 새 캐릭터 애니메이션이 재생되는지 확인합니다.
4. Play 모드를 종료하고 씬의 Development Skill을 원래 값인 **10**으로 복원합니다.

Play 모드 밖에서 바꾼 값은 씬에 저장될 수 있으므로 반드시 복원하세요. 일반적인 Play 모드 중 Inspector 변경은 종료 시 되돌아갑니다.

## 전환이 안 될 때 확인할 것

- **스킬 40에서 바뀌지 않음:** 정상입니다. Developer는 41부터 시작합니다.
- **새 모습은 나오지만 움직이지 않음:** Animator가 활성화되어 있는지, 새 컨트롤러가 연결되어 있는지 확인합니다. `NewCharacter.anim`은 반복 재생하도록 설정되어 있습니다.
- **스킬을 바꿔도 반응이 없음:** Play 모드인지, 현재 사용 중인 ResourceManager를 수정했는지 확인합니다. ResourceManager는 씬 사이에 유지되는 싱글턴이므로 이전 씬에서 넘어온 인스턴스가 사용될 수 있습니다.
- **컴포넌트가 없거나 Missing Script로 표시됨:** 올바른 Minjae 씬을 열었는지 확인하고 Console의 컴파일 오류를 먼저 해결합니다.
- **새 캐릭터가 너무 크거나 작게 보임:** 오브젝트의 Transform은 유지됩니다. 새 스프라이트의 크기와 Pixels Per Unit 설정은 Unity 화면에서 확인해야 합니다.

## 현재 검증 상태

씬의 컴포넌트 참조가 한 번만 연결되어 있는지, 새 컨트롤러가 NewCharacter 애니메이션을 참조하는지, 애니메이션의 스프라이트 GUID가 새 이미지 에셋과 일치하는지 확인했습니다. 새 애니메이션의 반복 재생 설정도 확인했습니다.

Unity Play 모드에서의 실제 화면, 전환 시점, 캐릭터 크기는 아직 직접 검증하지 않았습니다. 위 절차로 확인하면 됩니다.
