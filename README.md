# Dev Life Human in the Loop

A 2D game development simulation made for AUT COMP602. You run a small indie studio, create games, earn money and fandom, and decide when to work manually or use AI. Manual work builds your skills but adds workload. AI is faster and uses credits, with different effects on quality, bugs and technical debt.

## Team

- Minjae Kim - Product Owner and developer
- Sofia - Scrum Master and developer
- Katie - developer
- Jeremy - developer
- Chenxuan - developer

Individual feature contributions and Scrum evidence are recorded on the team's Trello board.

## Requirements

- Unity **6000.3.8f1** (Unity 6.3 LTS), matching `ProjectSettings/ProjectVersion.txt`.
- Unity Hub and the build support module for your chosen platform. Windows is the development and demonstration platform.
- A mouse and keyboard. A 16:9 Game view is recommended.
- Git to clone the repository, or download and extract its source ZIP.

Unity Package Manager restores the packages listed in `Packages/manifest.json`. These include Universal Render Pipeline 17.3.0, Input System 1.18.0, Unity UI 2.0.0, the 2D animation/sprite/importer packages, and Unity Test Framework 1.6.0. Keep the manifest and package lock file with the project.

The game's AI mode is simulated locally. It does not require an AI account, API key, subscription payment or external AI service. Money, purchases and subscriptions shown in the game are fictional in-game resources.

## Open and run in Unity

1. Clone or download the repository and select the `Release` branch for the assessed final version.
2. In Unity Hub, choose **Add project from disk** and select the folder containing `Assets`, `Packages` and `ProjectSettings`.
3. Open it with Unity 6000.3.8f1. Allow package restoration and asset import to finish.
4. Open **`Assets/Minjae/Minjae's Scene.unity`**. This is the integrated game scene, including its start menu.
5. Press the Unity Editor's Play button, then click **Play** inside the game.
6. Follow the tutorial or use **Skip**. Later tutorials explain the individual development stages and other systems.

Do not start the game from `Assets/Scenes/MainScene.unity`: that scene only contains the camera and lighting. The Dashboard, Shop, Market Trends and Reviews scenes are opened from the integrated room and rely on its managers.

## Controls

| Action | How to use it |
| --- | --- |
| Interact with the room and UI | Left-click objects, icons and buttons. |
| Open development | Click the computer once to zoom in, then click it again to open the current project. |
| Choose a work method | Click **Manual** or **AI**. Coding, Sound and Debugging also accept **M** or **A** at the method selection screen. Use the buttons in Design. |
| Manual coding | Press letter keys A-Z repeatedly to reveal the code blocks. |
| Manual design | Click the pencil repeatedly to finish the drawing. |
| Manual sound | Drag every track's slider back and forth until all track progress is complete. |
| Manual debugging | Click each red error line. Clearing a line is an attempt to remove a bug; removal is probabilistic. |
| Build the game | Click the Build button and wait for the progress animation. |
| Sleep | Return to the room and click the bed. Wait for the fade and wake-up sequence. |
| Clean rubbish | Click rubbish that appears in the room. |
| Return or open Settings | Use the on-screen close/back buttons or **Esc**. Esc closes Settings, returns from a feature screen or the computer, or opens Settings from the room. Some tutorial/modal panels must be closed with their own button first. |

Click inside the Game view if keyboard input is going to the Unity Editor instead of the game.

## Playing a project

1. Open the computer and enter a project name in **Project Setup**. Leaving it blank uses **Untitled Project**.
2. Choose an available **Theme** and **Genre**, then click **Start Development**. Both choices are required.
3. Complete the stages in this order:
   **Coding -> Design -> Sound -> Debugging -> Build -> Results**.
4. Choose Manual or AI separately for each of the first four stages. You can mix the two methods in one project.
5. Manual activities use the controls above. AI completes the activity automatically after a short delay and consumes a credit when selected. If you run out of credits, use Manual or buy more in the Shop.
6. At Build, start the build animation. Results show the completed project's outcome and rewards.
7. Use the result screen's finish/exit button to return to the room. Open the computer again to start another game.

Closing the computer does not deliberately start a new project; reopening it returns to the current development stage. Use the result screen to finish the current project before starting the next one.

## Features and how to use them

### Project conditions and studio resources

The HUD shows the clock, money, fandom and developer title. During development, the project conditions panel also shows quality, workload, technical debt, bugs and development time.

Work method choices change these values. Manual work generally gives more skill and quality with greater workload. AI reduces the manual work required but tends to introduce more bugs and technical debt. Results depend on quality, workload, remaining bugs and development skill, with money and fandom rewards based on the final score. Results vary between projects.

### Themes, genres and unlock notifications

Locked choices show a lock icon. Clicking one opens its requirement message without selecting it. Close the message to continue choosing.

| Choice | Unlock condition |
| --- | --- |
| Fantasy theme | Available at the start. |
| RPG genre | Available at the start. |
| Sci-Fi theme (shown as SciFI) | Complete at least two games. |
| Horror theme | Purchase at least two different room upgrades. |
| Action genre | Complete one full sleep sequence. |
| Simulation genre | Reach Development Skill 41 or above. |
| **Surreal** hidden theme and **Metroidvania** hidden genre | Meet all four progression conditions above: two completed games, two different upgrades, completed sleep and Development Skill 41 or above. |

The hidden options are absent from the dropdowns until all requirements are met. Once available, they can be selected for a new project and stay available when Project Setup is reopened during the same session. Unlock messages are queued so each rule is announced once during that progression session.

The Simulation requirement message mentions the Developer title; the implemented unlock check is **Development Skill >= 41**. The full title promotion system has additional requirements, described below.

### Developer titles and the To-Do list

Click the title/progress display at the top to expand the Developer Status panel. Switch between **DEV.** and **TO-DO** to inspect progress and requirements; minimise the panel to return to the small HUD display.

Titles progress through Beginner Developer, Junior Developer, Developer, Senior Developer and Expert Developer. Promotion considers development skill, completed games, completed manual stages and total money earned. Follow the To-Do list to see the remaining goals.

### Dashboard and project history

Use the yellow dashboard/report icon in the room, between the chart and shopping-cart icons, to open **Dashboard**. Completed games appear in the project history. Select a project to inspect its name, theme, genre, quality, sales/reward information and other results. The dashboard also summarises studio progress. A new studio has no real completed-project history yet.

### Game reviews

From the dashboard, select a completed project and choose **View Review**. The review screen shows a rating, verdict, category feedback and what players liked or disliked. Return with its back button or Esc. If Reviews is opened without a selected completed project, its code can display sample data; complete and select a real project to review your own results.

### Market trends

Use the bar-chart icon at the upper left of the room to open **Market Trends**. Inspect theme directions, genre demand and remaining durations before planning your next game. Trends change with in-game time. The trend display currently covers the three regular themes and genres; Surreal and Metroidvania are not included in those trend tables.

### Shop and room upgrades

Use the shopping-cart icon to open the **Shop**. Switch between its upgrade and AI pages, or type into the search field to filter product cards. Clear the search to show the cards again.

- PC and lighting upgrades cost $800 each.
- Window and decoration upgrades cost $300 each.
- Purchased room upgrades change the room's appearance and their buttons show **PURCHASED**.
- A purchase requires enough money. Buy two different room upgrades to meet the Horror/hidden-content progression condition.
- The old Clean Up purchase card has been removed. Clean rubbish by clicking it in the room; the AI credit pack card occupies the former shop slot.

### AI credits and subscription

The Shop offers credit packs and an AI subscription. A credit pack gives 10 credits. Its price increases with purchases: $50, $100, $200, then $300 for subsequent packs. The displayed price is the current pack price.

The subscription costs $100 and gives 25 credits immediately, with renewal every seven in-game days. Renewal charges another $100 and gives another 25 credits. If you cannot afford renewal, the subscription pauses. Watch the credit and subscription displays and keep enough money for living costs as well as AI use.

### Day and night, sleep and rubbish

The clock advances during gameplay and drives day/night presentation and scheduled costs. Clicking the bed starts a seven-hour sleep sequence and removes up to 20 workload. Let the whole fade/wake-up sequence finish to record the sleep unlock condition.

Rubbish appears at room spawn points over time. Spawning rubbish reduces project quality, while clicking the cleanable rubbish removes it and can recover quality. This replaces the removed Shop cleaning card.

### Living costs

Rent is $300, first due on day 14 and then every 14 in-game days. When the rent popup appears, use its payment button. If there is not enough money when rent is checked, the rent Game Over ending appears. Keep an eye on the clock when buying upgrades or AI credits.

### Settings and saving

Open **Settings** from the start menu or the room. Adjust the BGM, SFX and keyboard volume sliders. Use **Save** to store a snapshot and **Load** to restore the last snapshot. Loading replaces your current live progress.

The snapshot includes money, fandom, skill, technical debt, AI credits/purchases/subscription state, clock, current project and stage, individual activity progress, completed-project history, room upgrades, developer-title counters and volume settings. Saving uses a local JSON snapshot in Unity `PlayerPrefs` under `StudioSaveSnapshot_v1`.

Starting a new Play session starts a fresh studio without deleting that saved snapshot. Use **Load** explicitly to continue it. Editor and standalone-player saves can be stored separately. Save from the main room's Settings, where the required managers are available.

The unlock progression's sleep flag and notification history are not included in the snapshot. Do not rely on hidden-option eligibility being preserved across a restart/load; it is guaranteed only for the same progression session. A restored selected theme/genre is separate from rebuilding its unlock eligibility.

### Endings and Game Over

The game includes these outcomes:

- **Fandom ending** when fandom reaches 100.
- **Workload ending** when workload reaches 100.
- **Technical Debt ending** when technical debt reaches 100.
- **AI ending** after completing three projects with all four development methods set to AI.
- **Rent Game Over** when the studio cannot afford the rent check.

Use **Restart** on an ending screen to restart the current game scene. The `EndingController.MainMenu` handler is a placeholder; use Restart rather than depending on that button to return to the lobby.

### Tutorials, animation and audio

The opening tutorial introduces the computer and development flow. Context tutorials cover the other systems, including day/night, titles, Dashboard, Reviews, Market Trends, Shop and AI costs. Use the tutorial's next, skip or close controls as shown. The game includes animated stage progress, room/title presentation, background music and interaction/development sound effects. Adjust their levels in Settings.

## How the implementation fits together

This section describes the code behind the features above. Paths are relative to the repository root.

### Starting the game and moving between screens

`Assets/Minjae/Scripts/Development process/LobbyController.cs` controls the opening menu. `PlayGame()` starts the camera/button transition, enables computer interaction and shows the opening tutorial. `ComputerInteractionIn.cs`, in `Assets/Minjae/Scripts/Interaction`, stores the room camera position and moves between Room, ComputerZoomedIn and ComputerUIOpen. This is why opening development uses two clicks rather than immediately replacing the room screen.

`Assets/Katie/ReviewScripts/FeatureSceneNavigation.cs` opens Dashboard, Market Trends, Shop and Reviews with additive scene loading. The room remains loaded while its renderers, canvases, cameras, colliders and relevant input components are suspended. Returning unloads the feature scene and restores the room components. This keeps the room state available and avoids using two active room/feature EventSystems at once. The scene names are assigned in the integrated scene's Inspector; they override the script's default strings.

`Assets/Minjae/Scripts/EscapeInputRouter.cs` gives Esc its context-dependent behaviour. It checks open Settings, feature navigation, blocking panels and the computer state before deciding what to close. The room's `InteractionManager.cs` also checks whether UI or a modal is blocking world clicks before raycasting to an `IInteractable`, such as the computer or bed.

### Project choices and development stages

`Assets/Minjae/Scripts/Development process/ProjectData.cs` holds the current game's name, theme, genre, chosen work methods and final result. `ProjectDataManager.cs` owns that data and the archived project list. Project names are trimmed, and whitespace-only input becomes Untitled Project. When a game is archived, its data is copied so that starting or editing the next game does not change the previous record.

`ProjectSetupUI.cs` connects the name input and theme/genre dropdowns to the current data. `ProjectSetupDropdown.cs` checks each option's access rule. Its generated entries use `LockedOptionToggle.cs`, which checks the lock before calling the normal Toggle click or submit behaviour. A locked entry opens its requirement instead of changing selection. `DevelopmentFlowManager.cs` checks the incoming option index again, and `StartDevelopment()` refuses to proceed without two valid, unlocked choices.

`DevelopmentFlowManager.cs` owns the development stage and selects the matching UI panel. It starts each activity after a work method is chosen. The activity reports completion back through `CompleteCoding()`, `CompleteDesign()`, `CompleteSound()` or `CompleteDebugging()`, which advances the flow. Choosing a method starts the activity; it does not immediately finish the stage.

The activity scripts in the same folder handle the player input and animation:

| Script | Behaviour |
| --- | --- |
| `CodingStageController.cs` | Reveals code blocks after letter-key presses, or reveals them automatically at intervals in AI mode. |
| `DesignStageController.cs` | Advances drawing frames when the pencil is clicked, or advances them automatically in AI mode. |
| `SoundStageController.cs` | Accumulates movement from each slider and fills its track; AI completes tracks over time. |
| `DebuggingStageController.cs` | Resolves four clickable red lines, with different Manual/AI bug-removal probabilities; AI attempts lines automatically. |
| `BuildStageController.cs` | Animates build progress and sprite frames, then calls the result stage. |

Each activity supplies its own serialised progress to the save system. This allows a saved coding block, drawing frame, track position, debugging state or build percentage to be restored instead of only restoring the stage name.

### Applying consequences and calculating results

The scripts under `Assets/Minjae/ProjectResources` separate studio resources from project conditions. `ResourceManager.cs` owns money, fandom, development skill, technical debt, credits and subscription state. It survives scene changes. `ProjectResourceController.cs` owns the current project's conditions and records the stage results that have already been applied. Its `HasStageResult()` check prevents applying the same stage reward twice.

`ProjectResultsCalculator.cs` creates a `StageResult` using ranges appropriate to the stage and work method. `ProjectResourceController.ApplyStageResult()` updates quality, workload, bugs and development time, then updates skill and technical debt through `ResourceManager`. Coding, Design and Sound process these consequences when their method is chosen. Debugging applies its stage consequences when it completes. Its clickable lines already attempt bug removal, so the flow sets the later percentage removal to zero to avoid removing bugs a second time.

`FinalResultsCalculator.cs` uses the following calculation:

```text
final score = clamp(quality - workload * 0.20 - bugs * 3 + development skill * 0.20, 0, 100)
```

It then chooses money and fandom rewards from score bands and applies them to the studio. `DevelopmentFlowManager.GotoResult()` calls this calculation, archives the project and updates title counters. `ProjectCompletionBridge.cs` connects the result/exit button to the dashboard's history format, with a check against saving the same result object twice.

### Unlock rules and hidden content

`Assets/Minjae/Scripts/ProjectUnlockProgression.cs` evaluates completed-project count, distinct purchased upgrades, completed sleep and skill. It stores achieved rules in a `HashSet` and places new unlocks into a notification queue. Once achieved, a rule remains achieved for that progression instance. `UnlockNotificationUI.cs` takes queued notifications and displays them in order, using unscaled time for its display duration.

`Assets/Minjae/Assets/HiddenContent/Runtime/HiddenContentUnlockRules.cs` contains the separate `CanUnlock()` method. It returns the AND of the four requirement flags. `ProjectSetupUI` uses the resulting HiddenContent rule to add Surreal and Metroidvania to the existing dropdowns at runtime. They are represented in `GameTheme.cs` and `GameGenre.cs` and are stored in the same project data as the regular options.

### Clock, rest, titles and purchases

`Assets/Jeremy/Scripts/GameTimeManager.cs` advances the in-game clock and raises minute, hour and day events. These events are used by systems such as Market Trends, AI renewals and living costs. `Assets/Katie/ReviewScripts/SleepController.cs` temporarily disables the normal clock while it advances seven hours itself, reduces workload and runs the fade. It calls `RecordSleep()` only after the sleep sequence completes. `BedInteraction.cs` provides the room click connection.

`Assets/Jeremy/Scripts/PlayerTitleController.cs` checks skill, games developed, manual stages and money earned against the requirements for the next title. `TitleUIToggle.cs` and `Assets/Jeremy/DeveloperStatusTabs.cs` manage the expanded/minimised display and its status/to-do tabs. `Assets/Minjae/Scripts/CharacterTitleAnimation.cs` switches the character's animation controller when skill reaches 41; this is a skill check and is separate from the full title promotion requirements.

`Assets/Jeremy/Scripts/StoreUIController.cs` checks money and purchase-button state before making an upgrade purchase. `Assets/Minjae/RoomUpgradeManager.cs` stores the purchased flags and refreshes the associated room objects. `AISubscriptionManager.cs` handles credit packs and listens for day changes to renew the subscription. Its current pack price comes from the purchase count in `ResourceManager`, and the subscription payment day is also stored there. `WeeklyLivingCostsController.cs` handles the rent schedule, payment popup and insufficient-money ending.

`Assets/Chenxuan/scripts/RubbishSpawner.cs` selects an unoccupied spawn point and creates its matching prefab, up to the configured maximum. Spawning an item subtracts quality. `RubbishClean.cs` checks the item's collider on a left-click, adds quality, plays the cleaning sound and removes the item. The separate `Rubbish.cs` component provides a simpler removal-only click handler, so the prefab's assigned components determine which cleaning effects are used.

### History, reviews and market information

`Assets/Katie/ReviewScripts/ProjectHistoryManager.cs` keeps completed-project display records and the selected dashboard project between scenes. `HistoryChanged` and `SelectedProjectChanged` allow the UI to refresh. `DashboardUI.cs` displays rows and aggregate progress, and `ReviewReportManager.cs` creates category feedback from the selected project for `ReviewReportUI.cs`.

The current `CompletedProjectRecord` constructor copies the project identity, final score and rewards, but sets some detailed fields to defaults: quality is initially the final score, and workload, debt, bugs, development time and market bonus default to zero. The displayed completion week is based on record order rather than the actual clock. Those dashboard/review details should therefore not be read as a full copy of every original project condition. The archived `ProjectData.finalResult` still contains the detailed final result.

`MarketTrendManager.cs` generates a direction for each regular theme and a demand percentage for each regular genre, with durations measured in in-game minutes. It refreshes expired trends and raises UI events. The current `FinalResultsCalculator` does not read these trends when calculating the release score, so this version displays market information without applying a market bonus to the score.

### Saving, tutorials, sound and endings

`Assets/Chenxuan/scripts/SaveLoadManager.cs` collects data from the managers into `Assets/Katie/ReviewScripts/StudioSaveData.cs` and serialises it with `JsonUtility`. Before loading, it checks the version, time values, stage and activity snapshots. It captures a backup of the live studio and attempts to restore that backup if applying the save throws an exception. It restores underlying data before refreshing the UI and avoids replaying purchase/reward methods during restoration.

`Assets/Minjae/DevelopmentTutorialPopup.cs` handles the introduction and development-stage messages. `Assets/Sofia/MainGameTutorialManager.cs` handles the other context tutorials and tracks which have been shown. Audio components in `Assets/Chenxuan/scripts` provide button, typing, drawing, mixing, build, store, cleaning and ending sounds. `SceneBGm.cs` changes the BGM when relevant feature scenes load or unload. `SettingMenu.cs` controls BGM/SFX volume, `KeyboardSound.cs` controls typing volume, and `StoreVolumeSync.cs` applies the stored SFX level in the Shop.

Ending conditions are checked where the relevant value changes: fandom and technical debt in `ResourceManager`, workload in `ProjectConditions`, fully AI-developed project count in `ProjectDataManager`, and missed rent in `WeeklyLivingCostsController`. They pass an `EndingData` asset to `Assets/Sofia/EndingController.cs`, which fills the ending title, description, statistics and background rather than maintaining a separate scene for each outcome.

## Build a standalone player

1. Open the project in Unity 6000.3.8f1 and wait for compilation to finish.
2. Open **File > Build Profiles** and select the target platform. Install that platform's Unity build support if it is missing.
3. Check the active profile's **Scene List**. The integrated game scene must be the first enabled scene (build index 0):

   ```text
   Assets/Minjae/Minjae's Scene.unity
   Assets/Jeremy/StoreScene.unity
   Assets/Katie/GameReviewsScene.unity
   Assets/Katie/Dashboard.unity
   Assets/Katie/MarketTrend Scene.unity
   ```

   Keep all feature scenes enabled because navigation loads them at runtime. `Assets/Jeremy/TutorialScene.unity` is an additional standalone scene already listed in the project; the integrated game tutorials are driven from the room scene.

4. **Check the order before building:** the repository's current shared scene list starts with the mostly empty `Assets/Scenes/MainScene.unity`. Disable it or move the integrated game scene above it for the playable build. A profile can override the shared list, so check the active profile too.
5. Choose **Build** or **Build and Run** and select an output folder outside `Assets`, such as `Builds/Windows`.
6. For Windows, launch the generated `.exe`. When sharing the build, include its companion `_Data` folder and the other files Unity generates, not only the executable.

## Tests and manual checks

Open **Window > General > Test Runner**, select **EditMode**, and run the tests in `Assets/Minjae/Assets/Tests/NewTestScript.cs`. They check the hidden-content eligibility rule for all conditions met, sleep incomplete, and no conditions met. These are unit tests of the rule, not a complete gameplay or UI test suite.

For a manual check, start from the integrated room and try:

1. Start the game, view/skip tutorials, name a project, and select Fantasy + RPG.
2. Click a locked option and confirm that its requirement appears without changing the saved choice.
3. Complete a game through all five activity/build screens; try the Manual controls and AI mode across projects.
4. Inspect Results, Dashboard history and a selected project's Review. Open Market Trends and return to the room.
5. Sleep until waking, reopen Project Setup and check Action. Purchase two different upgrades and check Horror. Complete two games and reach skill 41 to check the remaining and hidden options.
6. Check Shop search, credit pack pricing and subscription displays.
7. Save from Settings, change progress, then Load and compare the restored values. Check volume settings as well.
8. Check the ending triggers and rent popup in a separate test session.

Allow tutorials to close before interacting with covered controls. A dropdown's outside-click blocker can consume the first click on another panel; close the dropdown first if a tutorial close button appears unresponsive.

## Repository layout

| Folder | Contents |
| --- | --- |
| `Assets/Minjae` | Integrated room scene, project setup, development stages, resources, progression and hidden-content tests. |
| `Assets/Katie` | Sleep, project history, Dashboard, Reviews, Market Trends and shared feature navigation. |
| `Assets/Jeremy` | Shop, time, developer titles, AI credits/subscription and living costs. |
| `Assets/Sofia` | Context tutorials, ending presentation/data and related assets. |
| `Assets/Chenxuan` | Audio/settings, save/load, rubbish and related assets. |
| `Packages` | Unity package manifest and lock file. |
| `ProjectSettings` | Unity version, input, rendering, player and scene settings. |
| `Docs` | Earlier development/setup notes. The current code and this README take priority where those notes describe older behaviour. |

Keep Unity `.meta` files with their assets. Generated folders such as `Library`, `Temp` and `Logs` are not needed to open the source project on another machine.
