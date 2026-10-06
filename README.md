# Free Agents of the Commonwealth (working title)

A single-player Fallout 4 mod inspired by **Eslabong** (shirowita): run a mercenary club
out of the Combat Zone.

- Hire mercs from a rotating market (signing fee + weekly wages in caps)
- Pick a lineup and a tactic, then watch league matches from the stands
- Mercs gain XP and level up, get injured and miss matches, and ask for raises
- Multiple seasons; each unlocks something new (Cup and poaching, a fourth tactic and 4-a-side, a champions club)

Plain plugin + Papyrus scripts in `Data/` — no F4SE needed. Eslabong is an inspiration only; players need just Fallout 4.

## Layout
- `design/` — the design sheets (source of truth). Each row becomes one record/struct.
- `tools/preflight.py` — checks every cell and cross-sheet reference before a build.
- `tools/caprica-linux.patch` — patch to build the MIT Caprica Papyrus compiler
  (github.com/nikitalita/Caprica, branch `os-independent`) on Linux.

Status: design in progress, not built, untested.
