# Handoff: continue Kenshi Realms on the Windows PC

Agreed with the creator (do not re-ask):
- Mashup: single-player **Kenshi** mod (host game) bringing **Mount & Blade II: Bannerlord**'s features
  into Kenshi, kept Kenshi-themed. Bannerlord is an inspiration only; players need just Kenshi.
  Kenshi has no multiplayer, so the mod is solo.
- Kingdoms: Holy Nation, United Cities, Shek Kingdom + **The Free Realm** (Tinfist/Anti-Slavers),
  **The Western Hive Throne**, **The Deadcat Crown** (refugees retaking their city from the Cannibals).
- Each kingdom: ruler, clans, named lords, recruitable forces, wars and alliances.
- Ladder: **nobody → mercenary → vassal**. The vassal oath grants a **fief** (built with Kenshi's outpost system).
- **Capturing a lord does NOT flip their town.** The creator explicitly removed this.
- Starts: **Escaped Prisoner**, **A Nobody of each kingdom**, **The Last Contract** (Ashen Hounds survivor
  at the Hub picks an employer). Backstories are in `design/starts.json`.
- **Live Bannerlord politics and fief income from the first release** (creator picked this):
  influence, kingdom votes (war, peace, fief grants), AI lords voting by honor/valor/greed, defection,
  daily fief income. These run in a KenshiLib plugin on **RE_Kenshi**.

Melty findings (game_info kenshi, search_mashups):
- Melty installs no loader for Kenshi. No mashups exist yet for Kenshi or Bannerlord, so there's nothing to remix.
- RE_Kenshi (github.com/BFrizzleFoShizzle/RE_Kenshi, **GPLv3**) loads as an Ogre plugin (`dllStartPlugin`)
  through the line `Plugin=RE_Kenshi` in `{game}/Plugins_x64.cfg`. It loads a mod's own DLLs from
  `mods/<Mod>/RE_Kenshi.json` (`"Plugins"` loads for active mods, `"PreloadPlugins"` for all mods).
- Draft recipe (mode `installed`): `mods/` → `{game}/mods`; `RE_Kenshi.dll` and `Plugins_x64.cfg` → `{game}`;
  launch `game`. **one_click_check = yes.** Finding: "unverified-game-integration", meaning play it once through Melty.
- KenshiLib is GPLv3, so the plugin is released GPLv3 and credits BFrizzleFoShizzle / KenshiReclaimer.

Plugin groundwork (done in the cloud session):
- Current KenshiLib is github.com/BFrizzleFoShizzle/KenshiLib (KenshiReclaimer/KenshiLib has moved).
- KenshiLib_Examples (GPLv3) already shows each piece we need: main-loop hook (CharacterHighlight),
  custom FCS dialogue conditions and actions (Dialogue, Dialogue_FCS), and saving/loading plugin state
  (WorldStates). Start the plugin from those.
- There is no death event: lords killed or captured are found by polling `isDead` / `isChainedMode` from the tick.
- The examples' install steps say "enable the mod via Kenshi's Mods tab", so open question 2 below is real.

Open questions to settle on a real install:
1. The shipped `Plugins_x64.cfg` must be the player's vanilla file plus RE_Kenshi's line. Diff it against
   this PC's copy, and check Steam vs GOG versions.
2. The mod must be active on first Play. Kenshi's active-mod list is stored in a file in the game folder
   (check `data/mods.cfg` or `__mods.list` on this install). Find a one-click way that doesn't wipe the
   player's other mods, or use `PreloadPlugins` so the DLL loads anyway.
3. Pin the Kenshi version (`version` in the recipe) that RE_Kenshi supports.

Next steps:
1. Find Kenshi's install folder (Steam library or GOG). Read `gamedata.base` with OpenConstructionSet (or FCS)
   and fill the 54 `vanilla-ref` stringIds in `design/hooks.json`. Run `python tools/preflight.py` until it
   is clean (currently 54 open). The 6 `kenshilib` functions are already confirmed (Sept 2026 KenshiLib).
2. Generate `KenshiRealms.mod` from the sheets (OpenConstructionSet), and write the plugin from `design/systems.json`
   (VS2010 x64 toolset, Boost 1.60, KenshiLib_Examples_deps). Never commit Kenshi's own files.
3. Test in game: each start, the ladder, recruiting, a vote, fief income. Capture a real screenshot.
4. Melty publish steps: inspect_package → validate_recipe → one_click_check → create_mod → upload →
   submit_release → screenshot → Test in Melty app → publish on approval. Credits: RE_Kenshi and KenshiLib
   (GPLv3). Listing title, tagline, license and remix choice are **not chosen yet**, so ask the creator.

Melty access: paste a fresh **Publish prompt** from Melty into the new session (keep the token out of this repo).
