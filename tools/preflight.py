"""Preflight: lay every design sheet over the others and list unfilled cells,
unverified hooks and references between sheets that don't resolve.
Exit code 0 only when the sheets are clean enough to build."""
import json, sys
from pathlib import Path

DESIGN = Path(__file__).resolve().parent.parent / "design"

# column -> sheet whose ids it must reference
REFS = {
    ("mercs", "weapon"): "hooks", ("mercs", "outfit"): "hooks",
    ("rivals", "weapon"): "hooks", ("rivals", "outfit"): "hooks",
    ("rivals", "club"): "clubs", ("clubs", "roster"): "rivals",
    ("clubs", "tactic"): "tactics", ("seasons", "clubs"): "clubs",
    ("seasons", "unlocks"): "systems|tactics|clubs",
}

def load():
    return {p.stem: json.loads(p.read_text()) for p in sorted(DESIGN.glob("*.json"))}

def main():
    sheets = load()
    ids = {name: {r.get("id") for r in s["rows"]} for name, s in sheets.items()}
    problems = []
    for name, sheet in sheets.items():
        cols = sheet["columns"]
        for i, row in enumerate(sheet["rows"]):
            key = row.get("id", row.get("season", i))
            for col in cols:
                if col not in row:
                    problems.append(f"{name}.{key}.{col}: missing")
                    continue
                val = row[col]
                if val == "" or val is None:
                    problems.append(f"{name}.{key}.{col}: empty")
                if col == "verified" and val is not True:
                    problems.append(f"{name}.{key}: not verified against Fallout4.esm")
                targets = REFS.get((name, col))
                if targets and val not in ("", None):
                    known = set().union(*(ids.get(t, set()) for t in targets.split("|")))
                    for ref in (val if isinstance(val, list) else [val]):
                        if ref not in known:
                            problems.append(f"{name}.{key}.{col}: '{ref}' not found in {targets}")
            for extra in set(row) - set(cols):
                problems.append(f"{name}.{key}: column '{extra}' not declared")
    # every system param marker must point at a hook
    for row in sheets.get("systems", {}).get("rows", []):
        for k, v in row.get("params", {}).items():
            if k.endswith("Marker") and v not in ids["hooks"]:
                problems.append(f"systems.{row['id']}.params.{k}: '{v}' not found in hooks")
    total = sum(len(s["rows"]) * len(s["columns"]) for s in sheets.values())
    print(f"{len(sheets)} sheets, {total} cells checked, {len(problems)} open")
    for p in problems:
        print("  -", p)
    return 1 if problems else 0

if __name__ == "__main__":
    sys.exit(main())
