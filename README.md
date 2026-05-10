# Roulette Game — Case Study

A 3D roulette prototype built in Unity. The player selects a chip value, places bets on a standard European roulette table, optionally chooses a deterministic landing number for testing, then spins the wheel and receives a payout based on standard roulette payout rules.

---

## Demo

> _Replace the placeholders below with your own media._

**Video:** `<add YouTube / Loom / Drive link here>`

**Screenshots:**

`<add screenshot here — e.g. ./docs/gameplay.png>`

---

## Requirements

- Unity **2022.3 LTS** or newer
- TextMeshPro (auto-imported by Unity on first scene open)
- Open `Assets/Scenes/Gameplay_Scene.unity` and press Play

The starting bankroll is `1000` (configured in `GameInitializer.cs`).

---

## Controls and Gameplay Instructions

### Placing a Bet

1. **Select a chip** by clicking one of the chip buttons in the UI: `1`, `5`, `10`, `25`, `100`, or `500`. The selected chip value will be used for every subsequent placement.
2. **Click a bet tile** on the table to place the selected chip on it. Multiple chips can be stacked on the same tile, and bets can be spread across many tiles in a single round. The current total bet is shown in the UI.
3. **Clear all bets** with the "Clear" button to remove every chip from the table and refund the staked amount.

You cannot place a bet that would exceed your current money. The `CanAddModeBet()` check in `BetManager` prevents over-betting.

### Supported Bet Types and Payouts

Defined in `BetRules.cs`:

| Bet Type   | Payout |
|------------|:------:|
| Straight   | 35 : 1 |
| Split      | 17 : 1 |
| Street     | 11 : 1 |
| Corner     |  8 : 1 |
| Six Line   |  5 : 1 |
| Dozens     |  2 : 1 |
| Columns    |  2 : 1 |
| Red / Black|  1 : 1 |
| Even / Odd |  1 : 1 |
| High / Low |  1 : 1 |

Each `BetTile` knows which numbers it covers (`_numbers[]`) and which bet type it represents. After the spin, the winning number is checked against every active tile and payouts are summed.

### Spinning the Wheel

- **Spin button** — starts the ball animation. The wheel rotates idly throughout the session; the spin animates the ball to land on the target number.
- After the ball stops, a 2-step arc tween jumps the ball to the `_resultPoint` Transform, where it parents itself for 2 seconds before resetting.
- Net profit (`totalReturn − totalStaked`) is added to the player's money, and all bets are cleared automatically for the next round.

### Selecting a Deterministic Outcome (Debug)

For testing and demoing predictable wins:

- A **TMP_InputField** (`_debugNumberInput`) is wired to `RouletteViewMono.SetDebugSelectedNumber`.
- Type any valid roulette number (`0`–`36`, only numbers actually present on the wheel) and press Enter.
- The next spin will land on that number.
- Invalid input is rejected with a console warning; the previously selected number remains active.

Random landing is also implemented (`GetRandomNumberOfRouletteWheel`) and can be swapped in by replacing the call in `RollRouletteBall`.

### UI Interactions Summary

| Action                       | How                                            |
|------------------------------|------------------------------------------------|
| Select chip value            | Click one of the chip buttons                  |
| Place chip on a tile         | Left-click the bet tile                        |
| Clear all bets               | Click the "Clear" button                       |
| Pin the next outcome (debug) | Type a number into the debug input + Enter     |
| Start the round              | Click the "Spin" button                        |

There are no keyboard shortcuts at the moment — all interaction is mouse-driven. Bet tiles are picked via a physics raycast against the `_clickableLayer` (see `InputHandler.cs`), so each tile must have a collider on the correct layer.

---

## Design Patterns

The project uses the following design patterns. Each entry includes the concrete file(s) where the pattern lives.

### Service Locator
`Assets/_RouletteGame/Scripts/Utilities/Service Locator/`

A static `ServiceLocator` exposes `RegisterService<T>`, `GetService<T>`, `TryGetService<T>`, and `UnregisterService<T>`. The `ServiceManager` MonoBehaviour holds inspector-assigned references (`BetManager`, `AudioManager`) annotated with `[RegisterService]` and uses reflection in `Awake` to register them all at once. Consumers like `RouletteViewMono` and `BetManager` resolve dependencies via `ServiceLocator.GetService<T>()` instead of holding hard references to scene objects. `ServiceManager` runs at `[DefaultExecutionOrder(-100)]` so registration happens before any consumer's `Awake`.

### Singleton
`Assets/_RouletteGame/Scripts/Utilities/PoolSystem.cs`, `Assets/_RouletteGame/Scripts/Utilities/Tween/Tweener.cs`

Two scene-wide systems use the singleton pattern:
- `PoolSystem.Instance` — single object pool registry, instantiated by Unity in the scene.
- `Tweener.Instance` — a self-creating singleton; if no instance exists when the first tween is registered, it spawns its own `[Tweener]` GameObject and marks it `DontDestroyOnLoad`.

### Object Pool
`PoolSystem.cs`, `PoolableObject.cs`

A pre-warmed pool of GameObjects keyed by string tag (`Tags.POOL_TAG_CHIP_1`, `POOL_TAG_ROULETTE_NUMBER`, etc.). `SpawnGameObject(tag)` dequeues and activates an object; `ReturnToPool(tag, obj)` puts it back. Used heavily for chips placed on bet tiles and for roulette number slots, avoiding per-frame `Instantiate`/`Destroy` overhead.

### Observer / Event Bus
`Assets/_RouletteGame/Scripts/Utilities/EventManager.cs`, `Events/OnBetChanged.cs`, `Utilities/GameStaticEvents.cs`

Two complementary event systems:
- **Strongly-typed event bus** — `EventManager.Subscribe<T>` / `Publish<T>` decouples publishers from subscribers via `IEvent` payloads (e.g. `OnBetChanged`). The `UiManager` listens for `OnBetChanged` and refreshes the bet text without `BetManager` knowing the UI exists.
- **Static C# Action** — `GameStaticEvents.OnPlayerClickBet` lets `InputHandler` broadcast clicks on bet tiles to whichever system is interested (currently `BetManager.PlaceChipOnBet`).

### Strategy / Lookup-Table
`Assets/_RouletteGame/Scripts/Roulette/BetRules.cs`

Payout calculation uses a `Dictionary<BetType, int>` keyed by bet type instead of a `switch` cascade. Adding a new bet type requires one dictionary entry, and `GetPayout` / `GetNetProfit` operate uniformly on every type. This is a lightweight Strategy: each bet type "selects" its multiplier at runtime.

### Component-based MVC
- **Model** — `PlayerStats` (static state), `BetTile.totalBetAmount` (per-tile state).
- **View** — `RouletteViewMono` (wheel/ball visuals), `UiManager` (HUD).
- **Controller** — `BetManager` (bet placement and validation), `InputHandler` (raycasts and dispatches user input).

The split is not strict MVC, but the responsibilities are separated along similar lines: state holders, visual presenters, and orchestrators.

### Fluent Builder (Tween API)
`Assets/_RouletteGame/Scripts/Utilities/Tween/`

`TweenExtensions` provide chainable extension methods (`DoMove`, `DoLocalMove`, `DoRotateBy`, …) that return a `Tween` instance. The instance itself exposes a fluent API: `.OnComplete(...)`, `.SetLoops(...)`, `.Kill()`. This mirrors DOTween's surface and lets call sites read like a sentence:

```csharp
_ball.DoMove(apex, duration, Ease.Linear)
     .OnComplete(() => _ball.DoMove(end, duration, Ease.Linear)
                            .OnComplete(...));
```

### ScriptableObject Configuration
`Assets/_RouletteGame/Scripts/Scriptables/RouletteConfigSo.cs`

Tunable parameters (rotation duration, initial spin count) live as `ScriptableObject` assets so designers can tweak them without touching code or breaking prefab serialization.

### Attribute-Driven Auto-Registration
`ServiceManager.cs`, `ServiceAttribute.cs`

`RegisterServiceAttribute` is a marker attribute; `ServiceManager.AutoRegisterAllServices` reflects over its private fields, finds the marked ones, and registers each in the `ServiceLocator` generically. Adding a new service is a one-liner: declare the field, mark it `[RegisterService]`, drag the reference in the inspector.

---

## Project Structure

```
Assets/_RouletteGame/
├── Scripts/
│   ├── Events/                  # IEvent payloads (OnBetChanged, ...)
│   ├── Interfaces/              # IClickable, IEvent
│   ├── Managers/                # AudioManager, GameInitializer, Sound
│   ├── Roulette/                # BetManager, BetTile, BetRules, RouletteViewMono
│   ├── Scriptables/             # RouletteConfigSo
│   ├── UI/                      # UiManager
│   └── Utilities/
│       ├── Service Locator/     # ServiceLocator, ServiceManager, attribute
│       ├── Tween/               # custom tween system (Tweener, Tween, extensions)
│       ├── EventManager.cs      # typed event bus
│       ├── GameStaticEvents.cs  # global C# Action events
│       ├── PoolSystem.cs        # object pool
│       ├── InputHandler.cs      # mouse raycast → bet tile dispatcher
│       ├── Enums.cs, Tags.cs, Ease.cs
│       └── ...
└── Scenes/Gameplay_Scene.unity
```

---

## Known Issues and Future Improvements

### Known Issues

- **Player money UI does not refresh after a payout.** `UiManager.UpdateMoneyText` is only called once in `Start` and there is no `OnMoneyChanged` event. The new bankroll is correct in `PlayerStats.PlayerMoney` but the on-screen text stays stale until the next scene reload.
- **Spin button is not locked during a spin.** Clicking it again mid-spin starts a second ball animation on top of the first.
- **`AudioManager.PlaySound` cuts off the previous play of the same source.** Rapid chip placement causes a stuttering "tick" because `Play()` restarts the AudioSource. `PlayOneShot(clip)` would let multiple chip sounds overlap cleanly.
- **`AudioManager.PlaySound` fails silently on a missing name.** If a `Sound.name` in the inspector doesn't exactly match the requested string (case-sensitive), the method early-returns without a log. A `Debug.LogWarning` for the not-found case would make misconfigurations visible.
- **`BetManager.CanAddModeBet()` uses `>` instead of `>=`.** The player can never bet their last unit of money, even though there is no real reason to forbid it.
- **`InputHandler` has a hard-coded `_chipYOffset` TODO.** The intended behavior — stacking the offset per-chip on the same tile — is implemented in `BetManager.PlaceChipOnBet` but the duplicate field in `InputHandler` is unused dead code.

### Future Improvements

- **Win/lose feedback.** Highlight the winning number on the wheel and on covered bet tiles, animate gained chips back to the player or losing chips off the table.
- **Round history.** Show the last N landed numbers so the player can track patterns.
- **Money-changed event.** Add `OnMoneyChanged` to the event bus and have `UiManager` subscribe so the bankroll text always reflects state.
- **Disable spin button during animation.** Re-enable it only after `ResetBall` finishes.
- **Replace debug input with a proper "spin" mode toggle.** Hide the debug field in production builds and use the random outcome generator there.
- **Audio polish.** Add wheel-roll loop, ball drop / chip clatter on the bet table, and win stinger. Switch to `PlayOneShot` for SFX overlap.
- **Mobile/touch support.** Currently `Input.GetMouseButtonDown` only — wire up touch input or migrate to the new Input System.
- **Save/load bankroll.** Persist `PlayerStats.PlayerMoney` between sessions via `PlayerPrefs` or a save file.
- **Localization.** Bet labels and UI text are hard-coded in English in the scene.

---

## Credits

- UI SFX: _Game Music Stingers and UI SFX Pack 2_ (asset pack)
- Props/visuals: _Layer Lab_ asset pack
- All gameplay code is original to this case study.