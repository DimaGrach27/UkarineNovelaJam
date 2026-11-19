# Demo analytics

Events are dispatched to the existing Unity Analytics and GameAnalytics integrations when
`ANALYTIC_ENABLED` initializes `AnalyticService`. No new provider or credentials are required.
The tracker survives scene changes. In builds without that define, tracking calls are no-ops.

## New event schema

All new events have these fields (register the event schemas in the Unity Analytics dashboard
before publishing): `BuildVersion` (string), `Language` (string), `Platform` (string),
`IsEditor` (bool), `IsDevelopmentBuild` (bool), `SessionID` (string), `RunNumber` (integer).
Existing event schemas have not changed.

| Event | Additional fields | Meaning |
| --- | --- | --- |
| DemoSessionStarted | — | One application session after analytics startup |
| NewGameStarted | — | Confirmed new game; increments local RunNumber |
| ContinueGame | — | Continue clicked in main menu |
| SaveLoaded | — | A selected save was loaded |
| GameplayEntered | SceneID string, PartIndex integer | Gameplay initialized at a new or restored position |
| ChoiceShown | SceneID string, OptionCount integer, Camera bool | A filtered choice list was displayed |
| ChoiceOptionShown | SceneID string, OptionID string, Camera bool | One available option in that display |
| ChoiceSelected | SceneID string, OptionID string, DecisionSeconds float, Camera bool | Accepted choice, with time since that list was shown |
| CameraUsed | — | Photo action invoked; scene is included in GA path |
| SceneTime | SceneID string, ActiveSeconds float | Foreground gameplay seconds in the preceding scene segment before transitioning |
| ProgressCheckpoint | SceneID string, PartIndex integer, ActiveSeconds float, Reason string | Every 30 active seconds, focus loss, or normal quit |
| StoryStatusChanged | StatusID string, StatusValue bool, SceneID string | A story/evidence status actually changed |
| NotebookOpened | — | Notebook opened |
| NotebookTabOpened | — | Notebook tab selected; tab is included in GA path |
| PauseMenuOpened | — | Pause menu opened |
| SettingsOpened | — | Settings opened; source is included in GA path |
| SettingUsed | Setting string, SettingValue string | Settings snapshot at application startup |
| SettingChanged | Setting string, SettingValue string | Changed setting, after one second without another change |
| DemoEndScreenShown | — | End screen reached its interactive state |

For events where the category is in the GA path, Unity also receives `EventPath` (string).

Settings keys: `language`, `dialog_keyboard_mouse`, `text_speed`, `music_volume`,
`sound_volume`, `brightness`. Numeric settings use rounded UI-scale buckets (saved value × 10),
not raw floating-point values. Both settings screens are covered by notifications from the six SaveService setting setters,
after SaveJson succeeds. There is no periodic settings polling. Opening or reinitializing a control with the same value does not count
as a change. Returning a slider to its last reported bucket before debounce expires emits nothing.
Pending changes are flushed on focus loss and normal quit. Startup writes are represented
by the initial snapshot; subsequent writes of the same value do not queue changes.

GameAnalytics uses bounded design-event paths and puts timing in the numeric event value;
structured fields are passed as custom fields. Session IDs and timings never become path segments.
Use the provider's available custom-field reporting/export capabilities for segmentation.

## Interpretation

- Keep the existing StartScene, EndScene, ChooseComplete, KillerChosen, EndDemoGame and
  InputTypeUsed reports. ChoiceSelected adds stable destination-scene identifiers and timing.
- OptionID is the destination scene key, matching the game's current answer/localization model.
  Keep scene keys stable across releases. Two answers with the same destination share an OptionID.
- ChoiceShown and ChoiceOptionShown count displays, including a refreshed list after using the
  camera. Use unique players or session/run grouping, not raw impressions, for player percentages.
- RunNumber is local to the installation and survives ordinary save resets. It is not a global
  player ID or a historical save identifier. Loading an old save does not restore its original
  run number. Pre-update installations may begin at run 0. Use provider user identity plus run
  number and SaveLoaded events when analyzing replays.
- ActiveSeconds is session-cumulative foreground gameplay time; the explicit pause menu is
  excluded. Notebook reading, thinking, and foreground inactivity are included. This is not an
  idle-detection metric. SceneTime covers a segment after resume, not necessarily a full scene.
- The last checkpoint is evidence of last observed progress, not proof of abandonment or crash.
  A forced termination can lose up to one checkpoint interval plus provider-buffered events.
- Exclude editor/development builds and split reports by BuildVersion and Language.
- No wishlist/link event was added: the inspected end screen has no corresponding link button.
  Add tracking to the actual button handler if one is introduced; a click is not a wishlist add.

## Validation and release check

The game assembly was compiled with Unity 6000.6.4f1's Roslyn compiler and existing Unity
response-file references, both with and without ANALYTIC_ENABLED. Both pass; the existing
StartDataCollection deprecation warning remains. Compilation output was isolated in /tmp.

Before shipping, register the Unity custom-event schemas and verify delivery in the provider
 dashboards from a development build:

1. Start a session: six SettingUsed events and one DemoSessionStarted.
2. Open/close each settings screen without changes: SettingsOpened, no SettingChanged.
3. Drag each slider, wait a second: one final bucket event; toggle input and change language.
4. Display a choice, use the camera, choose an answer: displayed options match the UI,
   selected destination matches the transition, timing is numeric.
5. Continue/load a save, play 30 seconds, pause, unfocus and resume: check checkpoint position
   and pause-excluded timing. Reach the final screen and check DemoEndScreenShown.

Live provider delivery and an interactive Unity playthrough were not performed by the code check.
