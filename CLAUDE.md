# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

Unity game project (early prototype). Engine: **Unity 6000.5.3f1**, Universal Render Pipeline (URP 17.5), new Input System (`com.unity.inputsystem` 1.19). The only gameplay scene is `Assets/Scenes/Test.unity`. Current focus is basic player movement (branch `v1/basic-movement`).

## Language / tooling constraints

- Scripts compile with **C# 9** (Unity's default for this version). C# 10+ features are **not** available — notably **primary constructors** and **records with `init`** will fail to compile even if the IDE (Rider/VS) suggests them. Write classic constructors by hand.
- There are no `.asmdef` files, so all scripts compile into a single `Assembly-CSharp`.

## Building, running, and testing

There is no CLI build/test setup — everything runs through the Unity Editor:

- **Open/run:** Open the project in Unity 6000.5.3f1 and press Play on `Assets/Scenes/Test.unity`.
- **Tests:** The Unity Test Framework is installed but no tests exist yet. Run via **Window > General > Test Runner**.
- **Compile check:** Happens automatically on focusing the Editor, or headlessly with `Unity.exe -batchmode -quit -projectPath "D:/UnityGames/My project"`.
- The `.csproj`/`.sln` files are Editor-generated — do not hand-edit; they regenerate.

## Code architecture

All gameplay code lives under `Assets/Scripts/`. The player system is built on a **composition-based lifecycle-dispatch pattern** — this is the core design and spans several files.

### The lifecycle-action system (`Assets/Scripts/Utils/`)

Behaviors are plain C# objects (not `MonoBehaviour`s) that opt into Unity's update phases by implementing marker interfaces:

- `ILifecycleAction` — empty base marker.
- `IUpdateAction` → `UpdateTick()`  (dispatched from `Update`)
- `IFixedUpdateAction` → `FixedTick()`  (dispatched from `FixedUpdate`)
- `IAfterUpdateAction` → `AfterUpdateTick()`  (dispatched from `LateUpdate`)

`LifecycleActionHandler` (abstract `MonoBehaviour`, in `LifecycleActionsHandler.cs`) is the engine. A subclass implements `AllActions()` to build and return the list of behavior objects. In `Awake`, the handler sorts each action into per-phase lists by runtime `is` checks, then forwards `Update`/`FixedUpdate`/`LateUpdate` to the matching lists. **Consequence:** an action that implements none of the three phase interfaces is silently never ticked.

### RBManiplulatorAction and the player behaviors (`Assets/Scripts/`, `Assets/Scripts/Player/`)

- `RBManiplulatorAction` (note the spelling; in `RBManiplulator.cs`) — abstract base for anything that pushes a `Rigidbody`. Implements `IFixedUpdateAction`, holds a `protected readonly Rigidbody rb` injected via constructor, and leaves `FixedTick()` abstract.
- `Move` / `Jump` extend `RBManiplulatorAction` **and** additionally implement `IUpdateAction`. The convention here: **read input in `UpdateTick()`, apply physics in `FixedTick()`** (cache the input in a field between the two). Follow this split when adding input-driven behaviors.
- `Gravity` extends `RBManiplulatorAction` with only a `FixedTick()`.

### Player composition (`Assets/Scripts/Player/`)

`PlayerController : LifecycleActionHandler` (requires a `Rigidbody`, disables built-in gravity) wires everything in `AllActions()`: it creates `InputSystem_Actions`, `AddComponent<GroundState>()`, constructs `move`/`jump`/`gravity` with `new` (passing the rigidbody, serialized tuning fields, input actions, and camera), and returns them. `OnEnable`/`OnDisable` enable/disable the input actions.

Ground detection is component-based and separate from the action objects: `GroundCheck` (`MonoBehaviour`) raycasts down each frame; `GroundState` (`MonoBehaviour`) aggregates all child `GroundCheck`s and exposes `IsGrounded()`. `Jump` consults the injected `GroundState`.

### Why the `new` construction is valid here (unlike before)

`RBManiplulatorAction`, `Move`, `Jump`, `Gravity` are **plain C# classes** — they do not derive from `MonoBehaviour`. That is precisely why constructor injection and `new` are legal. Do **not** reintroduce `MonoBehaviour` to this family; it would break the `new` construction. State that must live as a Unity component (like `GroundState`/`GroundCheck`) stays a separate `MonoBehaviour`.

### Generated / stub code

- `Assets/Scripts/Utils/InputSystem_Actions.cs` is generated from `Assets/InputSystem_Actions.inputactions`. **Do not edit by hand** — regenerate from the `.inputactions` asset.
- `GameManager` (`Assets/Scripts/GameState/`) is currently an empty stub.
- `Assets/TutorialInfo/` is leftover Unity template content — not part of the game, safe to ignore or delete.

## Conventions to preserve

- Existing identifiers carry consistent misspellings (`RBManiplulator`, `jumpStrenght`, `ray_lenght`). Match the existing spelling when referencing them rather than "fixing" one call site and breaking compilation.
- Tuning values (speeds, forces, heights, ray length, layer masks) are `[SerializeField]` fields set in the Inspector, not constants in code.
