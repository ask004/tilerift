# TileRift

Hybrid-casual mobile puzzle game concept (Tile Sort / Block Puzzle + Daily Challenge).

## Scope (MVP - 7 days)
- Core tile sort gameplay loop
- 60 levels from JSON
- Daily Challenge + streak
- Rewarded + Interstitial ads
- IAP: No Ads, Coin Pack

## Project Files
- `PLAN.md`: 7-day execution task board

## Next Steps
1. Create Unity project (2022 LTS).
2. Implement core board + drag/drop + rule engine.
3. Integrate ads and IAP.
4. Ship Android soft launch.

## Unity Scene Setup
1. Open project in Unity 2022 LTS.
2. Run `TileRift/Create MVP Scene` from the Unity menu.
3. Open `Assets/Scenes/Main.unity`.
4. Press Play and use `GameSessionController.TryMove(...)` via hooked UI cell events.
