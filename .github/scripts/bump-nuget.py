#!/usr/bin/env python3
"""
Package bumping for Codebelt service updates.

Updates packages published by the triggering source repo to the specified version.
Additionally fetches the latest stable version from NuGet for all other Codebelt-related
packages and updates them as well.
Does NOT update Microsoft.Extensions.*, BenchmarkDotNet, or other third-party packages.

Usage:
    TRIGGER_SOURCE=cuemon TRIGGER_VERSION=10.3.0 python3 bump-nuget.py

Behavior:
- If TRIGGER_SOURCE is "cuemon" and TRIGGER_VERSION is "10.3.0":
  - Cuemon.Core: 10.2.1 → 10.3.0  (triggered source, set to given version)
  - Cuemon.Extensions.IO: 10.2.1 → 10.3.0  (triggered source, set to given version)
  - Codebelt.Extensions.BenchmarkDotNet.*: 1.2.3 → <latest from NuGet>  (other Codebelt)
  - Microsoft.Extensions.Hosting: 9.0.13 → UNCHANGED (not a Codebelt package)
  - BenchmarkDotNet: 0.15.8 → UNCHANGED (not a Codebelt package)
"""

import json
import re
import os
import sys
import urllib.request
from typing import Dict, List, Optional

TRIGGER_SOURCE = os.environ.get("TRIGGER_SOURCE", "")
TRIGGER_VERSION = os.environ.get("TRIGGER_VERSION", "")

# Map of source repos to their package ID prefixes
SOURCE_PACKAGE_MAP: Dict[str, List[str]] = {
    "cuemon": ["Cuemon."],
    "xunit": ["Codebelt.Extensions.Xunit"],
    "benchmarkdotnet": ["Codebelt.Extensions.BenchmarkDotNet"],
    "bootstrapper": ["Codebelt.Bootstrapper"],
    "carter": ["Codebelt.Extensions.Carter"],
    "newtonsoft-json": [
        "Codebelt.Extensions.Newtonsoft.Json",
        "Codebelt.Extensions.AspNetCore.Newtonsoft.Json",
        "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft",
    ],
    "aws-signature-v4": ["Codebelt.Extensions.AspNetCore.Authentication.AwsSignature"],
    "unitify": ["Codebelt.Unitify"],
    "yamldotnet": [
        "Codebelt.Extensions.YamlDotNet",
        "Codebelt.Extensions.AspNetCore.Text.Yaml",
        "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml",
    ],
    "globalization": ["Codebelt.Extensions.Globalization"],
    "asp-versioning": ["Codebelt.Extensions.Asp.Versioning"],
    "swashbuckle-aspnetcore": ["Codebelt.Extensions.Swashbuckle"],
    "savvyio": ["Savvyio."],
    "shared-kernel": ["Codebelt.SharedKernel"],
}


def is_triggered_package(package_name: str) -> bool:
    """Check if package is published by the triggering source repo."""
    if not TRIGGER_SOURCE:
        return False
    prefixes = SOURCE_PACKAGE_MAP.get(TRIGGER_SOURCE, [])
    return any(package_name.startswith(prefix) for prefix in prefixes)


def is_codebelt_package(package_name: str) -> bool:
    """Check if package belongs to any Codebelt repo (regardless of trigger source)."""
    for repo_prefixes in SOURCE_PACKAGE_MAP.values():
        if any(package_name.startswith(prefix) for prefix in repo_prefixes if prefix):
            return True
    return False


_nuget_version_cache: Dict[str, Optional[str]] = {}


def get_latest_nuget_version(package_name: str) -> Optional[str]:
    """Fetch the latest stable version of a package from NuGet."""
    if package_name in _nuget_version_cache:
        return _nuget_version_cache[package_name]

    url = f"https://api.nuget.org/v3-flatcontainer/{package_name.lower()}/index.json"
    try:
        with urllib.request.urlopen(url, timeout=15) as response:
            data = json.loads(response.read())
        versions = data.get("versions", [])
        # Stable versions have no hyphen (no pre-release suffix)
        stable = [v for v in versions if "-" not in v]
        result = stable[-1] if stable else (versions[-1] if versions else None)
    except Exception as exc:
        print(f"  Warning: Could not fetch latest version for {package_name}: {exc}")
        result = None

    _nuget_version_cache[package_name] = result
    return result


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
    print(f"Triggered packages set to {target_version}; other Codebelt packages fetched from NuGet.")
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

        if is_triggered_package(pkg):
            if target_version != current:
                changes.append(f"  {pkg}: {current} → {target_version}")
                return m.group(0).replace(
                    f'Version="{current}"', f'Version="{target_version}"'
                )
            return m.group(0)

        if is_codebelt_package(pkg):
            latest = get_latest_nuget_version(pkg)
            if latest and latest != current:
                changes.append(f"  {pkg}: {current} → {latest} (latest from NuGet)")
                return m.group(0).replace(
                    f'Version="{current}"', f'Version="{latest}"'
                )
            return m.group(0)

        skipped_third_party.append(f"  {pkg} (skipped - not a Codebelt package)")
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
        print(f"Updated {len(changes)} package(s):")
        print("\n".join(changes))
    else:
        print("No Codebelt packages needed updating.")

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
