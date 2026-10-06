#!/usr/bin/env python3
"""Preflight for the design sheets: every cell filled, every row verified, every cross-sheet id resolves.
Exit code 0 only when the sheets are clean enough to build."""
import json, re, sys
from pathlib import Path

DESIGN = Path(__file__).resolve().parent.parent / "design"
ID_RE = re.compile(r"^(cls|ab|art|fx|prop|hz|arena|club|rv|hook|rule)_[a-z0-9_]+$")

sheets = {p.stem: json.loads(p.read_text()) for p in sorted(DESIGN.glob("*.json"))}
ids = {}
for name, s in sheets.items():
    for row in s["rows"]:
        if row["id"] in ids:
            print(f"DUP   {name}:{row['id']} also in {ids[row['id']]}")
        ids[row["id"]] = name

empty, unverified, broken = [], [], []

def refs(v):
    if isinstance(v, str):
        if ID_RE.match(v):
            yield v
    elif isinstance(v, list):
        for x in v:
            yield from refs(x)

for name, s in sheets.items():
    for row in s["rows"]:
        for col in s["columns"]:
            if col not in row:
                empty.append(f"{name}:{row['id']}.{col} (missing)")
                continue
            v = row[col]
            if v == "" or v is None:
                empty.append(f"{name}:{row['id']}.{col}")
            if col != "id":
                for r in refs(v):
                    if r not in ids:
                        broken.append(f"{name}:{row['id']}.{col} -> {r}")
        if row.get("verified") is False:
            unverified.append(f"{name}:{row['id']}")

# two-way consistency: a class's abilities name it as owner; a club's roster rows name the club
for row in sheets["classes"]["rows"]:
    for ab in row["abilities"]:
        a = next((x for x in sheets["abilities"]["rows"] if x["id"] == ab), None)
        if a and a["owner"] != row["id"]:
            broken.append(f"abilities:{ab}.owner is {a['owner']}, expected {row['id']}")
for row in sheets["clubs"]["rows"]:
    for rv in row["roster"]:
        r = next((x for x in sheets["rivals"]["rows"] if x["id"] == rv), None)
        if r and r["club"] != row["id"]:
            broken.append(f"rivals:{rv}.club is {r['club']}, expected {row['id']}")

for title, items in (("UNFILLED CELLS", empty), ("UNVERIFIED ROWS", unverified), ("BROKEN REFERENCES", broken)):
    print(f"\n{title}: {len(items)}")
    for i in items:
        print("  " + i)

clean = not (empty or unverified or broken)
print("\nPREFLIGHT:", "CLEAN" if clean else "NOT CLEAN - do not build")
sys.exit(0 if clean else 1)
