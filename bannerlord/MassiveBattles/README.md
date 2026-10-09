# Massive Battles (Bannerlord mod)

Bigger battles than the 1000-troop slider allows, built for Realm of Thrones.

Targets Bannerlord **v1.5.5** (+ War Sails), compiled against the v1.5.4 reference assemblies.
Status: **compiles, not yet tested in game.**

## What it does (v0.1)
- **Battle size unlock**: separate field / siege / sally-out / naval sizes (default 1400 / 900 / 900 / game setting).
- **Horse budget**: every horse is an engine agent, and the engine stops at ~2048 agents.
  Once the budget (default 500 horses) is used, further cavalry spawn on foot. The player and lords keep their horses.
  Battle size is auto-capped to `agent limit - horse budget - safety margin` (about 1448 by default), so troops plus horses can't hit the limit.
- **Agent-limit guard**: if live agents still get close to the limit, reinforcements pause until there is room.
- **Corpse limits**: caps corpses (default 150) and fades them faster (30s) to keep long battles fast.
- All settings live in **Mod Options → Massive Battles** (MCM). Most apply from the next battle.

## Install
1. Copy `Modules/MassiveBattles` from the release zip into
   `...\steamapps\common\Mount & Blade II Bannerlord\Modules\`.
2. In the launcher, enable **Massive Battles** and place it **after ROT-Dragon** (last in the list).
3. Required: Harmony, ButterLib, UIExtenderEx, Mod Configuration Menu v5 (all already in a normal RoT setup).

## Build
Needs the .NET SDK (6+). `dotnet build src -c Release` writes the ready-to-copy module to `dist/Modules/MassiveBattles`.

## Testing checklist
When a battle starts, a yellow line shows the effective battle size. When it ends, another shows peak agents and how many cavalry spawned on foot. Please report:
1. Did the main menu show "[Massive Battles] loaded", or a "patches failed" message?
2. A field battle with big armies: the battle size it printed, your fps in the big clash, and peak agents.
3. A siege: same numbers.
4. Any crash: send `C:\ProgramData\Mount and Blade II Bannerlord\logs\rgl_log_*.txt` (newest) and the crash report folder.

## Not in v0.1 (needs in-game profiling first)
- AI throttling for distant units
- Arrow and dropped-weapon cleanup
- Smarter reinforcement waves
