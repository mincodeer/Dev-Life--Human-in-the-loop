> **이 문서는 이전 구현 기록입니다.** 코드로 UI를 생성하는 방식은 제거했습니다. 현재 Inspector 필드와 직접 만드는 방법은 [ProjectSetupInspectorUI.md](ProjectSetupInspectorUI.md)를 따라주세요. 아래의 자동 생성 UI 및 Name Input Background 안내는 현재 구현에 적용되지 않습니다.

# Project Setup 스프라이트와 해금 조건

## 제목 입력창 이미지

Minjae 씬의 ProjectSetupPanel에서 Project Setup UI 컴포넌트의 **Name Input Background**에 원하는 Sprite를 연결합니다. Play 중 생성되는 Project Name Input의 배경으로 사용합니다. 비워두면 기존의 밝은 배경을 사용합니다. 테두리를 유지하며 늘리려면 Sprite Editor에서 Border를 설정하세요.

## Theme 해금

| 항목 | 조건 |
| --- | --- |
| Fantasy | 처음부터 선택 가능 |
| SciFi | ProjectDataManager에 완료된 게임이 2개 이상 기록됨 |
| Horror | 서로 다른 방 업그레이드 2개 이상 구매 |

방 업그레이드는 PC, 조명, 청소, 창문, 장식 중 구매한 종류를 셉니다. 청소로 침대와 테이블이 함께 바뀌어도 구매 1개입니다. 기존 RoomUpgradeManager는 씬 시작 시 구매 정보를 초기화하는 테스트 모드를 유지하고 있습니다.

## 구현 방식

`ProjectUnlockProgression`을 ResourceManager에 자동 추가합니다. 완료 게임 수, 방 구매 수, 잠자기 기록, 개발 스킬을 확인하고 조건 달성 여부를 유지합니다. 드롭다운의 Option Access에 지정한 Unlock Rule로 선택 가능 여부를 판단합니다. 기존 Manual 규칙도 유지합니다.

해금 시 화면 위쪽에 영어 알림을 5초 동안 표시합니다. 동시에 여러 항목이 해금되면 순서대로 표시하고, 각 조건의 알림은 한 번만 표시합니다. 알림은 입력을 막지 않습니다. 기존 게임 UI 언어에 맞춰 영어를 사용했습니다.

해금 상태는 씬 이동 후에도 유지되고, 스킬이 내려가도 다시 잠기지 않습니다. 게임 종료 후 복원을 위한 Save/Load 연결은 이번 구현에 포함되지 않습니다.

## 테스트

1. Minjae 씬에서 Play하고 컴퓨터의 Project Setup을 엽니다. Fantasy가 선택 가능하고 SciFi와 Horror는 잠겨 있는지 확인합니다.
2. SciFi를 클릭합니다. 완료 게임 2개 조건이 표시되고 현재 선택은 바뀌지 않아야 합니다.
3. 게임을 하나 완성하고 SciFi가 아직 잠겨 있는지 확인합니다. 두 번째 게임을 완성하면 해금 알림이 한 번 표시되고 SciFi를 선택할 수 있어야 합니다. Dashboard의 샘플 기록 추가는 실제 ProjectDataManager의 완료 기록이 아니므로 이 조건을 충족하지 않습니다.
4. 상점에서 서로 다른 방 업그레이드를 구매합니다. 첫 구매에서는 Horror가 잠겨 있고, 두 번째 구매 후 알림과 함께 선택 가능해야 합니다.
5. Project Setup을 닫았다가 다시 열어도 해금이 유지되는지 확인합니다. 다른 씬으로 이동했다가 돌아와도 유지되어야 합니다.
6. Play를 종료한 뒤 Name Input Background에 원하는 Sprite를 넣고 재실행합니다. 제목 입력, 수정, 빈 제목 처리와 배경 표시를 확인합니다.

## Genre 해금과 잠자기 연결

현재 드롭다운의 실제 인덱스는 `0=Select Genre, 1=RPG, 2=Action, 3=Simulation`입니다. Select Genre는 선택 안내이며 개발 시작에 사용할 수 있는 장르가 아닙니다.

| 장르 | 실제 인덱스 | 조건 |
| --- | --- | --- |
| RPG | 1 | 처음부터 선택 가능 |
| Action | 2 | 잠자기 한 번 완료 |
| Simulation | 3 | Developer 달성: 개발 스킬 41 이상 |

잠자기 동작 자체는 추가하지 않았습니다. 추후 잠자기 기능에서 **정상적으로 잠을 잔 후** 다음 함수를 호출하면 Action이 해금됩니다. 잠자기 화면을 열거나 취소했을 때는 호출하지 마세요.

```csharp
if (ResourceManager.Instance != null)
{
    var progression = ResourceManager.Instance.GetComponent<ProjectUnlockProgression>();
    if (progression != null)
        progression.RecordSleep();
}
```

같은 플레이에서 여러 번 호출해도 Action 해금 알림은 한 번만 표시됩니다.

### Genre 플레이 테스트

1. Minjae 씬에서 새로 Play하고 Project Setup을 엽니다. RPG는 선택 가능하고 Action·Simulation은 잠겨 있어야 합니다.
2. Action을 클릭하면 잠자기 조건이 표시되고, 현재 선택은 그대로여야 합니다.
3. Play 중 ResourceManager 오브젝트를 선택합니다. 자동으로 추가된 Project Unlock Progression 컴포넌트의 메뉴에서 **TEST - Record Sleep Completed**를 실행합니다. 이는 잠자기 완료 신호만 보내는 테스트이며 시간이나 날짜를 바꾸지 않습니다.
4. `Genre unlocked: Action!` 알림과 Action 선택 가능 여부를 확인합니다. 같은 메뉴를 다시 실행해도 알림이 추가로 나오지 않아야 합니다.
5. Resource Manager의 Development Skill을 **40**으로 설정합니다. 아직 Developer에 도달한 적이 없다면 Simulation은 잠겨 있어야 합니다.
6. Development Skill을 **41**로 설정합니다. 타이틀이 Developer로 바뀌고 `Genre unlocked: Simulation!` 알림이 표시되어야 합니다. 드롭다운을 다시 열면 Simulation을 선택할 수 있어야 합니다.
7. 스킬을 다시 40으로 내려도 Simulation은 해금 상태를 유지해야 합니다. Action·Simulation을 한 번에 해금하면 알림이 각각 5초씩 순서대로 표시되는지 확인합니다.
8. 새 Play에서는 완료 신호를 보내기 전까지 Action이 잠겨 있어야 합니다. 실제 잠자기 기능을 연결하기 전에는 일반 게임 플레이로 Action을 해금할 수 없습니다.

## 검증 범위

Unity 6000.3.8f1의 어셈블리 참조로 런타임 C# 코드를 컴파일했습니다. 실제 Unity Play 모드에서의 알림 표시와 게임 진행 테스트는 아직 실행하지 않았습니다.
