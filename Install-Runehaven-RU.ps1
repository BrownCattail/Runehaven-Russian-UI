[CmdletBinding()]
param(
    [Parameter(Mandatory = $false)]
    [string]$GamePath = 'E:\SteamLibrary\steamapps\common\Runehaven'
)

$ErrorActionPreference = 'Stop'

$dataPath = Join-Path $GamePath 'Runehaven_Data'
$streamingAssets = Join-Path $dataPath 'StreamingAssets'
$nodesPath = Join-Path $streamingAssets 'nodes'
$modPath = Join-Path $streamingAssets 'mods\runehaven_ru_content'

if (-not (Test-Path -LiteralPath (Join-Path $GamePath 'Runehaven.exe'))) {
    throw "Runehaven.exe not found: $GamePath"
}
if (-not (Test-Path -LiteralPath $nodesPath)) {
    throw "StreamingAssets\\nodes not found: $nodesPath"
}

$translationFile = Join-Path $PSScriptRoot 'translations.json'
$translations = Get-Content -LiteralPath $translationFile -Raw -Encoding UTF8 | ConvertFrom-Json

New-Item -ItemType Directory -Force -Path $modPath | Out-Null

$manifest = @{
    id = 'runehaven_ru_content'
    title = 'Runehaven — Русские тексты'
    description = 'Russian translations for tutorials, signs, notes, and end messages. Generated from the installed game version.'
    version = '0.1.12'
    node_path = 'nodes'
    zone_path = 'zones'
    world_path = 'worlds'
    debug = $false
} | ConvertTo-Json
Set-Content -LiteralPath (Join-Path $modPath 'mod.json') -Value $manifest -Encoding utf8NoBOM

$uiPlugin = Join-Path $PSScriptRoot 'BepInEx\plugins\RunehavenRussianUi.dll'
$pluginDirectory = Join-Path $GamePath 'BepInEx\plugins'
if ((Test-Path -LiteralPath $uiPlugin) -and (Test-Path -LiteralPath $pluginDirectory)) {
    Copy-Item -LiteralPath $uiPlugin -Destination (Join-Path $pluginDirectory 'RunehavenRussianUi.dll') -Force
    Write-Host 'Installed BepInEx UI translation plugin.'
}
elseif (Test-Path -LiteralPath $uiPlugin) {
    Write-Warning 'BepInEx 6 IL2CPP is not installed. Content text will work, but the UI translation plugin was skipped.'
}

# Zones and worlds are registered by ID. Copying them into the mod makes their
# existing node paths resolve against this mod's translated node collection.
foreach ($folder in 'zones', 'worlds') {
    $source = Join-Path $streamingAssets $folder
    $target = Join-Path $modPath $folder
    New-Item -ItemType Directory -Force -Path $target | Out-Null
    Get-ChildItem -LiteralPath $source -File -Filter '*.json' | ForEach-Object {
        Copy-Item -LiteralPath $_.FullName -Destination (Join-Path $target $_.Name) -Force
    }
}

$translatedFiles = 0
Get-ChildItem -LiteralPath $nodesPath -Recurse -File -Filter '*.node' | ForEach-Object {
    $contents = (Get-Content -LiteralPath $_.FullName -Raw) -replace "`r`n", "`n"
    $updated = $contents
    foreach ($entry in $translations) {
        # Level-editor files are inconsistent about a newline immediately
        # before the closing quote. Retain the file's own line ending instead
        # of treating it as part of the translatable sentence.
        $from = ([string]$entry.from).TrimEnd("`r", "`n")
        $to = ([string]$entry.to).TrimEnd("`r", "`n")
        $updated = $updated.Replace($from, $to)
    }

    if ($updated -eq $contents) {
        return
    }

    $relativePath = $_.FullName.Substring($nodesPath.Length).TrimStart('\')
    $target = Join-Path (Join-Path $modPath 'nodes') $relativePath
    New-Item -ItemType Directory -Force -Path (Split-Path -Parent $target) | Out-Null
    Set-Content -LiteralPath $target -Value $updated -Encoding utf8NoBOM -NoNewline
    $script:translatedFiles++
}

if ($translatedFiles -eq 0) {
    throw 'No known English source text was found. The game may have been updated; no mod was installed.'
}

Write-Host "Installed Runehaven Russian content mod: $modPath"
Write-Host "Translated node files: $translatedFiles"
Write-Host 'Restart Runehaven before testing the mod.'
