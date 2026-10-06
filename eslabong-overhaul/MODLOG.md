# MODLOG: Eslabong Overhaul

## Agreed with the creator (do not re-ask)
- Complete overhaul built from original homage packs (no real franchise names/art). First release = **full Wasteland pack**: 6 classes, 2 arenas, Wasteland league with rival clubs and rules.
- **Single player only**: designed, tested and listed as single player; no co-op work.
- Art: **free only**, drawn in code over the game's base fighter sprites + CC0/CC-BY props (credit authors). No fal.ai spend.
- **Not publishing** (creator, 2026-10-06): local install only. Deliver a zip with `override.cfg` + the mod `.pck` to unzip into the Eslabong folder. No Melty listing/upload unless the creator asks again.
- Separate from the Fallout 4 "Eslabong-inspired" project in the repo root.

## Facts
- Eslabong: Godot engine, solo dev Franz (itch: franz10), Steam app 4560660, Early Access since 2026-08-24. No anti-cheat / accounts seen.
- Melty: `search_games` gives `custom-eslabong`, "not in the catalog; Melty can't install into it". `game_info` has no entry. `search_mashups`: none.
- Toolkit: universal-modder cloned at ~/tools/universal-modder (cloud session). Playbook: skills/mod-any-game/references/engines/godot.md.

## Route (planned, unverified)
1. Godot Mod Loader if the game ships it, else
2. `override.cfg` next to the exe adding an autoload that calls `ProjectSettings.load_resource_pack()` on the mod pck, then extends game scripts / registers content. OPEN: can an override.cfg autoload point at a script outside the main pack (absolute path / user://)? Test on the real build.
- Read the game with GDRE Tools (gdsdecomp), matching Godot editor version for exporting the mod pck.

## Next
1. Get the game files (the .pck, or the exe if the pck is embedded), then identify the Godot version and whether scripts are encrypted.
2. Recover the project outside the repo; fill hooks.json / baseClass / mechanic / art baseSprite cells.
3. (Only matters if publishing is wanted later) BLOCKER (2026-10-06): one_click_check with primary game `custom-eslabong` answers "Choose an existing host game from search_games as primary. Inspiration tags do not count as a required game." So nothing can be submitted until Melty adds Eslabong to its catalog. No MCP tool requests a game; the creator has to ask Melty. Build/test can continue meanwhile.
