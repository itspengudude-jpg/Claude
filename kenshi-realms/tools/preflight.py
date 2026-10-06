"""Preflight: lay every design sheet over the others and list unfilled cells,
unverified hooks and references between sheets that don't resolve.
Exit code 0 only when the sheets are clean enough to build."""
import json, sys
from pathlib import Path

DESIGN = Path(__file__).resolve().parent.parent / "design"

# (sheet, column) -> sheet(s) whose ids it must reference ("|" = any of them)
REFS = {
    ("kingdoms", "faction"): "hooks", ("kingdoms", "capital"): "hooks", ("kingdoms", "ruler"): "lords",
    ("clans", "kingdom"): "kingdoms", ("clans", "leader"): "lords", ("clans", "seat"): "hooks",
    ("lords", "clan"): "clans", ("lords", "kingdom"): "kingdoms", ("lords", "party"): "troops",
    ("troops", "kingdom"): "kingdoms", ("troops", "weapon"): "hooks", ("troops", "armour"): "hooks",
    ("troops", "recruitAt"): "hooks", ("troops", "minRank"): "ladder",
    ("starts", "location"): "hooks", ("starts", "rank"): "ladder", ("starts", "squad"): "hooks",
    ("fiefs", "kingdom"): "kingdoms", ("fiefs", "site"): "hooks", ("fiefs", "garrison"): "troops",
    ("diplomacy", "a"): "kingdoms", ("diplomacy", "b"): "kingdoms|hooks",
}
# values that are allowed instead of a reference
SPECIAL = {("lords", "baseNpc"): {"new"}, ("starts", "kingdom"): {"any"},
           ("ladder", "dialogue"): {"none"}, ("fiefs", "holderAtStart"): {"open"}}
OPTIONAL_REFS = {("lords", "baseNpc"): "hooks", ("starts", "kingdom"): "kingdoms",
                 ("ladder", "dialogue"): "hooks", ("fiefs", "holderAtStart"): "clans"}

def load():
    return {p.stem: json.loads(p.read_text()) for p in sorted(DESIGN.glob("*.json"))}

def main():
    sheets = load()
    ids = {name: {r.get("id") for r in s["rows"]} for name, s in sheets.items()}
    problems = []
    for name, sheet in sheets.items():
        cols = sheet["columns"]
        seen = set()
        for i, row in enumerate(sheet["rows"]):
            key = row.get("id", i)
            if key in seen:
                problems.append(f"{name}.{key}: duplicate id")
            seen.add(key)
            for col in cols:
                if col not in row:
                    problems.append(f"{name}.{key}.{col}: missing")
                    continue
                val = row[col]
                if val == "" or val is None:
                    if not (name == "hooks" and col == "stringId"):  # reported below as unverified
                        problems.append(f"{name}.{key}.{col}: empty")
                if col == "verified" and val is not True:
                    problems.append(f"{name}.{key}: not verified ({row.get('kind')} {row.get('name')})")
                targets = REFS.get((name, col)) or OPTIONAL_REFS.get((name, col))
                if targets and val not in ("", None) and not (isinstance(val, str) and val in SPECIAL.get((name, col), set())):
                    known = set().union(*(ids.get(t, set()) for t in targets.split("|")))
                    for ref in (val if isinstance(val, list) else [val]):
                        if ref not in known:
                            problems.append(f"{name}.{key}.{col}: '{ref}' not found in {targets}")
            for extra in set(row) - set(cols):
                problems.append(f"{name}.{key}: column '{extra}' not declared")
    # every systems param named 'hook' must point at a hook
    for row in sheets.get("systems", {}).get("rows", []):
        for k, v in row.get("params", {}).items():
            if k in ("hook", "rankCheck") and v not in ids["hooks"]:
                problems.append(f"systems.{row['id']}.params.{k}: '{v}' not found in hooks")
    # a lord must belong to the kingdom of their clan; a ruler must lead the kingdom's first clan
    clans = {c["id"]: c for c in sheets["clans"]["rows"]}
    for l in sheets["lords"]["rows"]:
        c = clans.get(l["clan"])
        if c and c["kingdom"] != l["kingdom"]:
            problems.append(f"lords.{l['id']}: kingdom '{l['kingdom']}' differs from clan's '{c['kingdom']}'")
    # every pair of kingdoms has a diplomacy row
    ks = [k["id"] for k in sheets["kingdoms"]["rows"]]
    pairs = {frozenset((d["a"], d["b"])) for d in sheets["diplomacy"]["rows"]}
    for i, a in enumerate(ks):
        for b in ks[i + 1:]:
            if frozenset((a, b)) not in pairs:
                problems.append(f"diplomacy: no row for {a} / {b}")
    total = sum(len(s["rows"]) * len(s["columns"]) for s in sheets.values())
    print(f"{len(sheets)} sheets, {total} cells checked, {len(problems)} open")
    for p in problems:
        print("  -", p)
    return 1 if problems else 0

if __name__ == "__main__":
    sys.exit(main())
