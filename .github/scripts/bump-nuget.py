#!/usr/bin/env python3
"""
Simplified package bumping for Codebelt service updates (Option B).

Only updates packages published by the triggering source repo.
Does NOT update Microsoft.Extensions.*, BenchmarkDotNet, or other third-party packages.
Does NOT parse TFM conditions - only bumps Codebelt/Cuemon/Savvyio packages to the triggering version.

Usage:
    TRIGGER_SOURCE=cuemon TRIGGER_VERSION=10.3.0 python3 bump-nuget.py

Behavior:
- If TRIGGER_SOURCE is "cuemon" and TRIGGER_VERSION is "10.3.0":
  - Cuemon.Core: 10.2.1 → 10.3.0
  - Cuemon.Extensions.IO: 10.2.1 → 10.3.0
  - Microsoft.Extensions.Hosting: 9.0.13 → UNCHANGED (not a Codebelt package)
  - BenchmarkDotNet: 0.15.8 → UNCHANGED (not a Codebelt package)
"""

import re
import os
import sys
from typing import Dict, List

TRIGGER_SOURCE = os.environ.get("TRIGGER_SOURCE", "")
TRIGGER_VERSION = os.environ.get("TRIGGER_VERSION", "")

# Map of source repos to their package ID prefixes
SOURCE_PACKAGE_MAP: Dict[str, List[str]] = {
    "cuemon": ["Cuemon."],
    "xunit": ["Codebelt.Extensions.Xunit"],
    "benchmarkdotnet": ["Codebelt.Extensions.BenchmarkDotNet"],
    "bootstrapper": ["Codebelt.Bootstrapper"],
    "newtonsoft-json": [
        "Codebelt.Extensions.Newtonsoft.Json",
        "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft",
    ],
    "aws-signature-v4": ["Codebelt.Extensions.AspNetCore.Authentication.AwsSignature"],
    "unitify": ["Codebelt.Unitify"],
    "yamldotnet": [
        "Codebelt.Extensions.YamlDotNet",
        "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml",
    ],
    "globalization": ["Codebelt.Extensions.Globalization"],
    "asp-versioning": ["Codebelt.Extensions.Asp.Versioning"],
    "swashbuckle-aspnetcore": ["Codebelt.Extensions.Swashbuckle"],
    "savvyio": ["Savvyio."],
    "shared-kernel": [],
}


def is_triggered_package(package_name: str) -> bool:
    """Check if package is published by the triggering source repo."""
    if not TRIGGER_SOURCE:
        return False
    prefixes = SOURCE_PACKAGE_MAP.get(TRIGGER_SOURCE, [])
    return any(package_name.startswith(prefix) for prefix in prefixes)


def main():
    if not TRIGGER_SOURCE or not TRIGGER_VERSION:
        print(
            "Error: TRIGGER_SOURCE and TRIGGER_VERSION environment variables required"
        )
        print(f"  TRIGGER_SOURCE={TRIGGER_SOURCE}")
        print(f"  TRIGGER_VERSION={TRIGGER_VERSION}")
        sys.exit(1)

    # Strip 'v' prefix if present in version
    target_version = TRIGGER_VERSION.lstrip("v")

    print(f"Trigger: {TRIGGER_SOURCE} @ {target_version}")
    print(f"Only updating packages from: {TRIGGER_SOURCE}")
    print()

    try:
        with open("Directory.Packages.props", "r") as f:
            content = f.read()
    except FileNotFoundError:
        print("Error: Directory.Packages.props not found")
        sys.exit(1)

    changes = []
    skipped_third_party = []

    def replace_version(m: re.Match) -> str:
        pkg = m.group(1)
        current = m.group(2)

        if not is_triggered_package(pkg):
            skipped_third_party.append(f"  {pkg} (skipped - not from {TRIGGER_SOURCE})")
            return m.group(0)

        if target_version != current:
            changes.append(f"  {pkg}: {current} → {target_version}")
            return m.group(0).replace(
                f'Version="{current}"', f'Version="{target_version}"'
            )

        return m.group(0)

    # Match PackageVersion elements (handles multiline)
    pattern = re.compile(
        r"<PackageVersion\b"
        r'(?=[^>]*\bInclude="([^"]+)")'
        r'(?=[^>]*\bVersion="([^"]+)")'
        r"[^>]*>",
        re.DOTALL,
    )
    new_content = pattern.sub(replace_version, content)

    # Show results
    if changes:
        print(f"Updated {len(changes)} package(s) from {TRIGGER_SOURCE}:")
        print("\n".join(changes))
    else:
        print(f"No packages from {TRIGGER_SOURCE} needed updating.")

    if skipped_third_party:
        print()
        print(f"Skipped {len(skipped_third_party)} third-party package(s):")
        print("\n".join(skipped_third_party[:5]))  # Show first 5
        if len(skipped_third_party) > 5:
            print(f"  ... and {len(skipped_third_party) - 5} more")

    with open("Directory.Packages.props", "w") as f:
        f.write(new_content)

    return 0 if changes else 0  # Return 0 even if no changes (not an error)


if __name__ == "__main__":
    sys.exit(main())
