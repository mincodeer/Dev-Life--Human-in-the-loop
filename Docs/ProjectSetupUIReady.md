# 연결된 Project Setup UI 위치와 수정 방법

## 현재 상태

`Assets/Minjae/Minjae's Scene.unity`에 실제 UI 오브젝트를 만들고 이름 입력·잠금 힌트·닫기 버튼·해금 알림·잠금 표시를 연결했습니다. Play 전에도 Hierarchy에서 모두 찾을 수 있습니다. 런타임에 새 UI를 만드는 코드는 추가하지 않았습니다.

기존 Project Setup의 남색 배경, 보라색 테두리, PixelifySans 폰트를 사용했습니다. 이름 입력과 Close 버튼에는 기존 ProjectSetup_1 Sprite를 연결했습니다. 안내창은 Image 오브젝트로 테두리와 안쪽 배경을 구성했습니다. LockIcon은 교체 가능한 Image와 LOCK 텍스트로 구성했습니다.

## 정확한 Hierarchy

```text
Canvas
├─ DevelopmentUI
│  └─ ProjectSetupPanel                         [ProjectSetupUI]
│     ├─ TitleText                             [기존 제목, 비활성]
│     ├─ ProjectNameInput                      [Image + TMP_InputField]
│     │  └─ Text Area                          [RectMask2D]
│     │     ├─ Placeholder                     [TMP Text]
│     │     └─ Text                            [TMP Text]
│     ├─ ThemeDropdown
│     │  └─ Template/Viewport/Content/Item
│     │     └─ LockIcon                        [Image]
│     │        └─ Label                        [LOCK 텍스트]
│     ├─ GenreDropdown
│     │  └─ Template/Viewport/Content/Item
│     │     └─ LockIcon                        [Image]
│     │        └─ Label                        [LOCK 텍스트]
│     └─ UnlockRequirementPanel                [Image, 시작 시 숨김]
│        └─ Card                               [Image: 테두리]
│           ├─ Background                     [Image: 안쪽 배경]
│           ├─ Header                         [TMP Text]
│           ├─ RequirementText                [TMP Text]
│           └─ CloseButton                    [Image + Button]
│              └─ Label                       [TMP Text]
├─ UnlockNotificationController                [UnlockNotificationUI, 항상 활성]
└─ UnlockNotificationPanel                     [Image: 테두리, 시작 시 숨김]
   ├─ Background                              [Image: 안쪽 배경]
   └─ Message                                 [TMP Text]
```

## 이미지 교체

1. Play를 종료합니다. Unity가 외부 씬 변경을 감지해 Reload를 제안하면 새 씬 내용을 불러옵니다. 열린 씬이 이전 내용을 계속 보여주면 변경된 씬을 다시 열어 확인하세요. 저장되지 않은 개인 변경이 있다면 먼저 별도로 보관하세요.
2. 원하는 이미지 파일을 Project 창의 Assets 안에 넣습니다.
3. 이미지 Import Settings에서 **Texture Type = Sprite (2D and UI)**를 선택하고 Apply합니다.
4. 아래 오브젝트를 선택해 **Image → Source Image**에 Sprite를 드래그합니다.

| 바꿀 부분 | 오브젝트 |
| --- | --- |
| 이름 입력창 배경 | ProjectNameInput |
| 힌트 창 배경 | UnlockRequirementPanel/Card/Background |
| 힌트 창 바깥 테두리 | UnlockRequirementPanel/Card |
| 힌트 닫기 버튼 | UnlockRequirementPanel/Card/CloseButton |
| 해금 알림 배경 | Canvas/UnlockNotificationPanel/Background |
| 해금 알림 바깥 테두리 | Canvas/UnlockNotificationPanel |
| 잠금 이미지 | 각 Dropdown의 Template/Viewport/Content/Item/LockIcon |

현재 Background와 테두리 Image는 Color로 색을 입혀 놓았습니다. 새 이미지를 원래 색상으로 표시하려면 **Image → Color를 흰색**으로 바꾸세요. Card/NotificationPanel의 테두리와 Background는 서로 다른 Image입니다. 완성된 배경 Sprite에 테두리가 포함되어 있다면 Background를 원하는 크기로 맞추거나 바깥 Image를 투명하게 할 수 있습니다. 기능이 연결된 Panel 오브젝트 자체는 삭제하지 마세요.

이미지 교체는 컴포넌트 참조를 바꾸지 않으므로 이름 입력·닫기·해금 기능 연결은 유지됩니다. 버튼과 Input Field 컴포넌트를 삭제하거나 교체하지 말고 Image의 Source Image만 바꾸세요.

LockIcon에 자물쇠 Sprite를 넣은 뒤에는 자식 Label을 비활성화하면 LOCK 글자가 사라집니다. LockIcon의 크기·위치는 Rect Transform에서 조절합니다. Template은 편집할 때만 활성화하고 완료 후 다시 숨깁니다.

## 안내문 수정

| 문구 | Inspector에서 수정할 곳 |
| --- | --- |
| 입력 전 이름 안내 | ProjectNameInput/Text Area/Placeholder → TMP Text의 Text |
| 잠긴 항목의 해금 조건 안내 | ThemeDropdown 또는 GenreDropdown → Option Access → 해당 항목의 Requirement |
| 힌트 창 제목 | UnlockRequirementPanel/Card/Header → TMP Text의 Text |
| 닫기 버튼 글자 | UnlockRequirementPanel/Card/CloseButton/Label → TMP Text의 Text |
| SciFi·Horror·Action·Simulation 해금 알림 | Canvas/UnlockNotificationController → Unlock Notification UI의 각 Message 필드 |
| 해금 알림 표시 시간 | 같은 컴포넌트의 Display Seconds |

RequirementText와 Message는 플레이 중 코드가 해당 문구로 갱신합니다. 이 두 Text의 Text 값을 직접 바꾸는 대신 위의 Requirement/Message 필드에서 바꾸세요. 폰트·색상·크기·정렬은 각 Text 컴포넌트에서 변경합니다.

## 연결된 기능

- 이름 입력 중 프로젝트 이름을 저장하고, 편집 종료 시 앞뒤 공백을 정리합니다.
- 잠긴 항목 클릭 시 힌트 창을 열고 기존 선택을 유지합니다. Close 버튼은 코드가 리스너를 연결합니다.
- Fantasy와 RPG는 기본 해금입니다. SciFi는 게임 2개 완성, Horror는 서로 다른 방 업그레이드 2개 구매, Action은 잠자기 완료 신호, Simulation은 스킬 41 이상으로 해금합니다.
- 해금 알림은 5초씩 순서대로 표시합니다. 잠자기 기능 자체는 없으며 RecordSleep() 연결 함수와 테스트 메뉴만 유지합니다.
- 해금 알림 패널은 초기 비활성이지만 UnlockNotificationController는 활성입니다. 컨트롤러를 숨김 패널 안으로 이동하지 마세요.

## 편집 화면에서 확인하기

Play 전 ProjectSetupPanel을 잠시 활성화하면 입력창을 볼 수 있습니다. UnlockRequirementPanel을 함께 활성화하면 힌트 디자인을 확인할 수 있습니다. 해금 알림은 Canvas의 UnlockNotificationPanel을 활성화해 확인합니다. 편집을 마치면 ProjectSetupPanel·UnlockRequirementPanel·UnlockNotificationPanel을 다시 비활성으로 저장하세요.

## 짧은 플레이 테스트

1. Play 후 컴퓨터를 열고 이름 입력·수정·닫았다 다시 열기를 확인합니다.
2. SciFi를 눌러 힌트가 뜨는지 확인하고 Close를 누릅니다.
3. ResourceManager의 Development Skill을 40 → 41로 변경해 Simulation 알림을 확인합니다.
4. ResourceManager의 Project Unlock Progression 메뉴에서 TEST - Record Sleep Completed를 실행해 Action 알림을 확인합니다.
5. 드롭다운을 다시 열어 해금된 항목을 선택할 수 있는지 확인합니다.

씬의 모든 새 로컬 참조와 ProjectSetupUI/UnlockNotificationUI 연결, 두 드롭다운 Item의 잠금 표시 참조를 확인했습니다. 런타임 C# 컴파일은 통과했습니다. Unity 연결 도구가 응답하지 않아 실제 Play 화면 검증은 수행하지 못했습니다.
