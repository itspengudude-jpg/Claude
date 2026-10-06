# Eslabong Overhaul: Wasteland Pack (working title)

A single-player content overhaul for **Eslabong** (Franz, Godot, Steam Early Access). It adds original
"homage" packs inspired by big media franchises, with no franchise names, art or sound. The first is the
**Wasteland pack** (post-apocalyptic):

- **6 classes:** Iron Scavenger, Rad Brawler, Pip-Gunner, Ghoul Medic, Super-Brute, Laser Ranger, each with its own ability
- **2 arenas:** Rustbelt Scrapyard (radiation storms, leaking barrels) and Vault 77 Atrium (security turret, cog door shortcut)
- **The Wasteland league:** 4 rival clubs, a Wasteland-only market, Caps economy, storm season

Later packs: Northlands, Heroes, Vigilantes, Galactic.

Players need Eslabong. The mod ships as a Godot content pack plus `override.cfg` next to the game, and
never changes the game's own files. Art is drawn in code or comes from free CC0/CC-BY libraries
(credited in `design/art.json`).

This is completely separate from the Fallout 4 project at the repo root.

## Layout
- `design/`: the design sheets, the source of truth. Each row becomes one resource/script.
- `tools/preflight.py`: checks every cell, every verification flag and every cross-sheet reference before a build.
- `MODLOG.md`: working journal (paths, findings, next steps).

Status: design sheets done; game not read yet; not built, untested.
