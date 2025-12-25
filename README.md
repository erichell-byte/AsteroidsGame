# AsteroidsGame

Unity 2D Asteroids-style game with Zenject DI, UniRx event flow, Addressables-based asset loading, and Firebase-backed remote configuration.

## Controls

- WASD: movement
- Shift: laser shot
- Space: main shot

## Architecture overview

### Dependency injection
- Zenject is the primary composition tool.
- Project-wide bindings live in `Assets/Project/Scripts/Installers/ProjectInstaller.cs`.
- Scene-level bindings live in `Assets/Project/Scripts/Installers/GameSceneInstaller.cs` (SceneContext in `Assets/Project/Scenes/Game.unity`).

### Game lifecycle
- `GameCycle` owns the state machine (Off, Playing, Pause, Finished) and notifies listeners.
- Gameplay systems implement `IGameStartListener`, `IGamePauseListener`, `IGameResumeListener`, `IGameFinishListener`.

Key files:
- `Assets/Project/Scripts/Systems/GameCycle.cs`
- `Assets/Project/Scripts/Systems/IGameListener.cs`

### Event bus (UniRx)
- `IGameEvents` is a lightweight reactive event hub.
- `GameEventsController` wires spaceship state and ads to the game lifecycle.

Key files:
- `Assets/Project/Scripts/Systems/GameEvents.cs`
- `Assets/Project/Scripts/Systems/GameEventsController.cs`

### Enemy system
- `EnemiesManager` subscribes to spawn timers and prepares enemies for gameplay.
- `EnemiesFactory` pulls from pools, initializes enemy configs, and positions spawns.
- Spawn points come from `IEnemySpawnPointProvider` (camera bounds).

Key files:
- `Assets/Project/Scripts/Enemies/EnemiesManager.cs`
- `Assets/Project/Scripts/Enemies/EnemiesFactory.cs`
- `Assets/Project/Scripts/Enemies/Spawn/CameraBoundsSpawnPointProvider.cs`

### Timers and combat
- `TimersController` owns enemy spawn cadence and weapon timers.
- `AttackComponent` coordinates `MainWeapon` and `LaserWeapon`.

Key files:
- `Assets/Project/Scripts/Utils/TimersController.cs`
- `Assets/Project/Scripts/Components/AttackComponent.cs`

### Assets and pooling
- Addressables are used for dynamic instantiation and preload.
- Generic pools wrap Addressables instantiation and reuse.

Key files:
- `Assets/Project/Scripts/Assets/AddressablesAssetLoader.cs`
- `Assets/Project/Scripts/Assets/AddressablesPreloader.cs`
- `Assets/Project/Scripts/Pools/AbstractPool.cs`

### Configuration
- Local config is a ScriptableObject `GameConfiguration`.
- Remote config is provided by Firebase (`RemoteConfig`) under the key `GameConfiguration`.

Key files:
- `Assets/Project/Scripts/Config/GameConfigurationSO.cs`
- `Assets/Project/Scripts/Config/FirebaseConfigProvider.cs`

### Effects, audio, analytics
- Mediators subscribe to `IGameEvents` and play effects/audio/analytics.

Key files:
- `Assets/Project/Scripts/Effects/EffectsMediator.cs`
- `Assets/Project/Scripts/Sound/AudioMediator.cs`
- `Assets/Project/Scripts/Analytics/AnalyticsMediator.cs`

## Scene structure

- `Assets/Project/Scenes/Bootstrap.unity` is the likely entry point.
- `Assets/Project/Scenes/Menu.unity` hosts the menu UI.
- `Assets/Project/Scenes/Game.unity` contains gameplay and Zenject `SceneContext`.
