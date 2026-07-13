# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

Unity game project (early prototype). Engine: **Unity 6000.5.3f1**, Universal Render Pipeline (URP 17.5), new Input System (`com.unity.inputsystem` 1.19). The only gameplay scene is `Assets/Scenes/Test.unity`.

## Building, running, and testing

There is no CLI build/test setup — everything runs through the Unity Editor:

- **Open/run:** Open the project in Unity 6000.5.3f1 and press Play on `Assets/Scenes/Test.unity`.
- **Tests:** The Unity Test Framework (`com.unity.test-framework`) is installed but no tests exist yet. Run tests via **Window > General > Test Runner** in the Editor (there are no `.asmdef` files, so all scripts compile into `Assembly-CSharp`).
- **Compile check:** Compilation happens automatically on focusing the Editor, or headlessly with `Unity.exe -batchmode -quit -projectPath "D:/UnityGames/My project"`.
- The `.csproj` and `.sln` files are Editor-generated — do not hand-edit them; they regenerate.

## Code architecture

All gameplay code lives under `Assets/Scripts/`.

- **Player system (`Assets/Scripts/Player/`)** — intended composition-based design. `PlayerController` (a `MonoBehaviour` requiring a `Rigidbody`) owns the player and instantiates behavior objects (`Jump`, `Move`, `Gravity`, `GroundCheck`) via `Awake`. A comment notes `PlayerController` "is controlled by GameStateManager" — that manager does not exist yet.
- **`RBManiplulator`** (note the spelling; base class in `Assets/Scripts/`) — base for the player behaviors. It holds a `readonly Rigidbody rb` injected through its constructor. `Jump`, `Move`, and `Gravity` extend it.
- **`Utils/Floor.cs`** — procedural floor generator. `dupe = false` scales a single tile to `length × width`; `dupe = true` instantiates a grid of tiles. Tile source is the serialized `floorTile` GameObject.
- **`Utils/InputSystem_Actions.cs`** — generated from `Assets/InputSystem_Actions.inputactions`. **Do not edit by hand**; regenerate from the `.inputactions` asset in the Editor.

### Important architectural caveat

The `RBManiplulator` family (`RBManiplulator`, `Jump`, `Move`, `Gravity`) derives from `MonoBehaviour` **but also defines constructors and is instantiated with `new`** (e.g. `jump = new Jump(rigidbody)` in `PlayerController`). This is invalid Unity usage — `MonoBehaviour`s cannot be constructed with `new`; they must be added as components. `Gravity.cs` additionally uses invalid C# (`base(rigidbody);` as a statement) and will not compile. When touching the player system, expect to resolve this design: either make these plain C# classes (drop `MonoBehaviour`) so constructor injection works, or make them real components created via `AddComponent`/`GetComponent`.

## Third-party / template code

`Assets/TutorialInfo/` is leftover Unity template content (the `Readme` asset + its editor drawer). Not part of the game — safe to ignore or delete.
