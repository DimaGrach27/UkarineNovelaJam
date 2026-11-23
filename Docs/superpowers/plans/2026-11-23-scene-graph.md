# Scene Graph Implementation Plan

> **For agentic workers:** Use superpowers:executing-plans to implement this plan task-by-task.

**Goal:** Finish the existing narrative editor and diagnose direct self references.
**Architecture:** Scene assets own transitions; NarrativeContainer owns layout only. Editor operations preserve each option's metadata, register Undo and rebuild views from current assets.
**Tech Stack:** Unity 6000.6.4f1, C#, GraphView, SerializedObject, AssetDatabase.
**Spec:** ../specs/2026-11-23-scene-graph-design.md

## Global Constraints
Allow multi-scene cycles; reject direct self references, including duplicate visual nodes. Do not modify runtime or story assets. Retain legacy layouts, return nodes and groups. Never derive game transitions from stale NodeLinks.

## Review Focus
- A disconnected middle option retains its metadata and index.
- Deleting a visual node does not clear incoming game transitions.
- Dynamic special branches are inspected without running game logic.
- Opening legacy layouts never dirties scenes.
- Empty layouts and terminal scenes survive save/reload.

## Task 1: Transition operations and validation
- [x] Add editor regression checks with temporary ScriptableObjects: A→A rejected; A→B→A allowed; reconnect/disconnect preserves label, text and condition references; deleting option zero retains option one's metadata; null/end/special branches validated.
- [x] Run checks against existing behavior and record failures.
- [x] Create SceneGraphData.cs: FindScenes(), SetTarget(scene,index,target), AddOption(scene), RemoveOption(scene,index), GetSpecialTransitions(scene), Validate(scenes). Use Undo and dirty marking for mutations only.
- [x] Replace SceneValidator window with results UI and asset selection/focus; run checks.

## Task 2: Graph editing and layout persistence
- [x] Extend checks for legacy layouts, disconnected options, duplicate return nodes, read-only loading, save/reload and Undo.
- [x] Rewrite SceneGraphView's unfinished port handling around actual option indices; preserve public entry points required by NodeSearchWindow. Add focus/search, Inspector, informational special edges, and refresh after Undo/Inspector changes.
- [x] Replace GraphSaveUtility with layout-only capture/load via AssetDatabase. Preserve existing node IDs/groups and entry marker; append missing targets as return nodes. No writes to scenes during load/save-layout.
- [x] Update SceneGraph toolbar: layout selection, load, all scenes, save/save as, search, validate; protect pending layout changes before switching.
- [x] Compile and run editor checks. Visual inspection attempted; native window capture unavailable.

## Task 3: Verification and handoff
- [x] Run complete editor regression suite and available compile checks.
- [x] Request fresh code review; resolve actionable findings.
- [x] Record exact validation and limitations; provide menu paths and usage.

## Execution ledger
- User approved design and explicitly requested code. Execute inline without another approval round.
- Working in current Unity checkout so changes are usable in the running editor; preserve all pre-existing modifications. No worktree requested.
- Shared interface: graph and validator consume SceneGraphData; GraphSaveUtility captures view layout, never scene transitions.

- Verification: original implementation failed read-only loading/disconnected-option checks; initial implementation passed 11 checks in Unity batch mode. Return routing, special reference placement and side-effect-only specials then failed dedicated regressions. Final Unity run passed 15 checks (Logs/Editor.log and Logs/scene-graph-check.result); final csc compile passed with the pre-existing ExposedProperties serialization warning.
- Review: independent reviewer identified return hint persistence and incremental overlap; both fixed and regression-tested. Final scoped review reported no significant remaining issues.
- Ruling: retain visual NodeLinks as target-node hints validated against live scene targets; never derive game state from them. Read inline node coordinates before Unity's layout pass. Skip Titre special targets because Check() always returns false.
- Temporary test runner removed. Product test command remains ReflectionOfAmber/Tests/Scene graph checks. No merge, push or changes to user story data were performed by editor implementation.
