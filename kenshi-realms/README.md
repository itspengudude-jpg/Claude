# Kenshi Realms (working title)

A single-player **Kenshi** mod that brings **Mount & Blade II: Bannerlord**'s feudal game into Kenshi's world,
kept Kenshi in look and lore. Bannerlord is an inspiration only; players need just Kenshi.

- **Six kingdoms:** the Holy Nation, United Cities and Shek Kingdom, plus three new ones:
  The Free Realm (Tinfist's Anti-Slavers), The Western Hive Throne and The Deadcat Crown.
- Each kingdom has a ruler, two clans and named lords with their own war parties.
- **Climb from nobody to mercenary to vassal** with any kingdom: standing and renown unlock a mercenary
  contract, then a vassal oath that grants a fief you build up with Kenshi's outpost system.
- **Recruitable forces:** three troop tiers per kingdom, hired in its towns as your rank rises.
- **Live politics:** clans earn influence, kingdoms vote on war, peace and fief grants, AI lords vote
  by their honour, valour and greed, and unhappy lords can defect.
- **Fief income** is paid every in-game day. Capturing a lord does **not** flip their town (by design).
- **Starts:** Escaped Prisoner (break out of Rebirth), A Nobody of each kingdom (six starts), and
  The Last Contract (an Ashen Hounds survivor picking an employer in the Hub).

## How it is built
- `design/`: the design sheets (source of truth). Each row becomes one record or struct.
- `tools/preflight.py`: checks every cell and cross-sheet reference before a build.
- Data (kingdoms, lords, troops, starts, dialogue) goes into `KenshiRealms.mod` (Kenshi's own mod format).
- Live systems (`layer: plugin` in `design/systems.json`) go into `KenshiRealms.dll`, a KenshiLib plugin
  loaded by **RE_Kenshi** (BFrizzleFoShizzle, GPLv3), which is bundled with credit. The plugin is GPLv3 too.

## Release layout (Melty recipe, one_click_check: yes on paper)
| In the zip | Installs to |
|---|---|
| `mods/KenshiRealms/` (`.mod`, `RE_Kenshi.json`, `KenshiRealms.dll`, licenses) | `{game}/mods/KenshiRealms/` |
| `RE_Kenshi.dll`, `Plugins_x64.cfg` | `{game}/` |

Open before release: the shipped `Plugins_x64.cfg` must match the player's vanilla file plus RE_Kenshi's line.
The mod must also be active in Kenshi's mod list on first Play. Both need testing on a real install.

Status: design in progress, not built, untested.
