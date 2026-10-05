[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'

function Assert-CommandSucceeded([string] $Command) {
    if ($LASTEXITCODE -ne 0) { throw "$Command failed with exit code $LASTEXITCODE." }
}

Push-Location -LiteralPath $PSScriptRoot
try {
    $docfxRoot = $PSScriptRoot
    $sourceRoot = [System.IO.Path]::GetFullPath((Join-Path $docfxRoot '..\src'))
    $docfxConfig = Get-Content -LiteralPath (Join-Path $docfxRoot 'docfx.json') -Raw | ConvertFrom-Json
    $version = minver -i -t v -v w
    Assert-CommandSucceeded 'minver'

    $metadataProjectPatterns = @(
        foreach ($metadata in $docfxConfig.metadata) {
            foreach ($source in $metadata.src) {
                $metadataSourceRoot = if ($source.src) {
                    [System.IO.Path]::GetFullPath((Join-Path $docfxRoot $source.src))
                } else {
                    $docfxRoot
                }

                foreach ($file in $source.files) {
                    (Join-Path $metadataSourceRoot $file).Replace('/', [System.IO.Path]::DirectorySeparatorChar)
                }
            }
        }
    )
    $sourceProjects = @(Get-ChildItem -LiteralPath $sourceRoot -Recurse -File -Filter '*.csproj')
    $metadataProjects = @(
        $sourceProjects |
            Where-Object {
                $projectPath = $_.FullName
                foreach ($pattern in $metadataProjectPatterns) {
                    if ($projectPath -like $pattern) {
                        return $true
                    }
                }

                return $false
            }
    )
    $sourceProjectsHaveRestoreAssets = $sourceProjects.Count -gt 0
    foreach ($project in $sourceProjects) {
        $restoreAssetsPath = Join-Path $project.DirectoryName 'obj\project.assets.json'
        if (-not (Test-Path -LiteralPath $restoreAssetsPath -PathType Leaf)) {
            $sourceProjectsHaveRestoreAssets = $false
            break
        }
    }

    $useNoRestore = $metadataProjects.Count -gt 0 -and $sourceProjectsHaveRestoreAssets
    $restoreInputNames = @('Directory.Build.props', 'Directory.Build.targets', 'Directory.Packages.props', 'NuGet.Config', 'nuget.config', 'global.json')

    foreach ($project in $sourceProjects) {
        $restoreAssetsPath = Join-Path $project.DirectoryName 'obj\project.assets.json'
        if (-not (Test-Path -LiteralPath $restoreAssetsPath -PathType Leaf)) {
            $useNoRestore = $false
            break
        }

        $restoreAssetsLastWriteTime = (Get-Item -LiteralPath $restoreAssetsPath).LastWriteTimeUtc
        $restoreInputPaths = [System.Collections.Generic.List[string]]::new()
        $restoreInputPaths.Add($project.FullName)

        $inputDirectory = $project.DirectoryName
        while ($inputDirectory) {
            foreach ($name in $restoreInputNames) {
                $restoreInputPath = Join-Path $inputDirectory $name
                if (Test-Path -LiteralPath $restoreInputPath -PathType Leaf) {
                    $restoreInputPaths.Add($restoreInputPath)
                }
            }

            $parentDirectory = Split-Path -Parent $inputDirectory
            if (-not $parentDirectory -or $parentDirectory -eq $inputDirectory) {
                break
            }

            $inputDirectory = $parentDirectory
        }

        $lockFilePath = Join-Path $project.DirectoryName 'packages.lock.json'
        if (Test-Path -LiteralPath $lockFilePath -PathType Leaf) {
            $restoreInputPaths.Add($lockFilePath)
        }

        if ($env:APPDATA) {
            $userNuGetConfigPath = Join-Path $env:APPDATA 'NuGet\NuGet.Config'
            if (Test-Path -LiteralPath $userNuGetConfigPath -PathType Leaf) {
                $restoreInputPaths.Add($userNuGetConfigPath)
            }
        }

        foreach ($restoreInputPath in $restoreInputPaths | Sort-Object -Unique) {
            if ((Get-Item -LiteralPath $restoreInputPath).LastWriteTimeUtc -gt $restoreAssetsLastWriteTime) {
                $useNoRestore = $false
                break
            }
        }

        if (-not $useNoRestore) {
            break
        }
    }

    if (-not $useNoRestore) {
        # Restore the source graph once so DocFX does not restore every project separately.
        $restoreSolution = Join-Path ([System.IO.Path]::GetTempPath()) ("savvyio-docfx-{0}.slnx" -f [Guid]::NewGuid())
        try {
            $projectsXml = foreach ($project in $sourceProjects) {
                '  <Project Path="{0}" />' -f [Security.SecurityElement]::Escape($project.FullName)
            }
            @('<Solution>') + $projectsXml + @('</Solution>') | Set-Content -LiteralPath $restoreSolution -Encoding utf8
            dotnet restore $restoreSolution --verbosity quiet
            Assert-CommandSucceeded 'dotnet restore'
        } finally {
            if (Test-Path -LiteralPath $restoreSolution) { Remove-Item -LiteralPath $restoreSolution }
        }
    }

    # Keep all metadata groups in one process; DocFX carries resolver state across groups.
    docfx metadata docfx.json --noRestore
    Assert-CommandSucceeded 'docfx metadata'

    docker buildx build -t savvyio-docfx:$version --platform linux/arm64,linux/amd64 --load -f Dockerfile.docfx .
    Assert-CommandSucceeded 'docker buildx build'

    Get-ChildItem -Recurse -Path api -Include *.yml, .manifest | Remove-Item
} finally {
    Pop-Location
}
