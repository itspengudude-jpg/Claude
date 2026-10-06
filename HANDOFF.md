# Handoff: continue on the Windows PC

Agreed with the creator (do not re-ask):
- Mashup: single-player **Fallout 4** mod, inspired by **Eslabong** (shirowita; Melty slug `custom-eslabong`, credit as related game / inspiration only — players don't need it).
- Concept: you run a mercenary club whose home is the **Combat Zone**. First minute: join a league there and play the first match right away.
- Matches: player **manages from the stands** — picks lineup + tactic, watches squads fight in the cage.
- Scope v1: one season **plus progression** (XP/levels, injuries, wage rises) **and multiple seasons** that unlock new features (see `design/seasons.json`).
- No multiplayer (Fallout 4 can't without F4SE, which Melty does not install).

Constraints from Melty (`game_info fallout-4`):
- No loader Melty installs for Fallout 4 → plain files only (`.esp`/`.esl` + `.pex` in `{game}/Data`). Do **not** depend on F4SE.
- Draft recipe (`mode: installed`, mapping `Data/` → `{game}/Data`, launch `game`) already passed `one_click_check` = yes.
- Open question: confirm the plugin gets enabled in `%LOCALAPPDATA%\Fallout4\plugins.txt` during Melty's **Test**; if not, find a one-click way.
- Melty: no existing listing yet (`list_my_mods` empty). Nothing similar on Melty (only FalloutCraft).

Next steps:
1. Fill the 47 open cells in `design/hooks.json` from `Fallout4.esm` (Mutagen or xEdit): Combat Zone cell + cage/stands positions, Cait's quest, weapons, outfits, caps. Run `python tools/preflight.py` until clean.
2. Generate the plugin from the sheets (Mutagen) and the Papyrus scripts; compile with the Creation Kit's `Papyrus Compiler\PapyrusCompiler.exe` (or Caprica).
3. Test in game, capture a real screenshot of a league match, then follow the Melty publish steps (inspect_package → validate_recipe → one_click_check → create_mod → upload → submit_release → screenshot → Test in Melty app → publish on approval).

Melty access: paste a fresh **Publish prompt** from Melty into the new session (keep the token out of this repo).
