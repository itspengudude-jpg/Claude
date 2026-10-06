# Handoff: continue the Eslabong Overhaul on the Windows PC

Read `MODLOG.md` (agreed decisions, do not re-ask) and the sheets in `design/` first.

Next steps on the PC:
1. Find the Eslabong install (Steam library, `steamapps/common/Eslabong`). Read only; never copy game files into this repo.
2. Set up universal-modder (github.com/rehan-remade/universal-modder, `bin/um`). Run `um scan "Eslabong"` to get the Godot version and whether scripts are encrypted.
3. Recover the project with GDRE Tools (github.com/GDRETools/gdsdecomp) into a folder outside the repo (e.g. `%USERPROFILE%\eslabong-decomp`). Back up the saves (`um backup create`) before the first modded launch.
4. Fill the empty cells in `design/` from the recovered project. Run `python tools/preflight.py` until it says CLEAN.
5. Vertical slice: one class (Iron Scavenger) loaded through `override.cfg` + mod pck, verified in the running game. Then widen to the full pack.
6. Package as `eslabong-wasteland-<version>.zip` (`override.cfg` + pck) with install/uninstall steps. Not publishing to Melty.
