# Generates deterministic PNG samples using the same five swatch colors as the default manufacturing channels.

Add-Type -AssemblyName System.Drawing

$outDir = $PSScriptRoot
$W = 512
$H = 512
$patternW = 480
$patternH = 320

$swatches = @(
    [System.Drawing.Color]::FromArgb(0x2C, 0x6F, 0x91),
    [System.Drawing.Color]::FromArgb(0xB9, 0x57, 0x3F),
    [System.Drawing.Color]::FromArgb(0xD2, 0xA1, 0x3D),
    [System.Drawing.Color]::FromArgb(0x7B, 0x8F, 0x45),
    [System.Drawing.Color]::FromArgb(0x3E, 0x4F, 0x63)
)

function New-Bitmap {
    param([int]$Width, [int]$Height)
    New-Object System.Drawing.Bitmap($Width, $Height, [System.Drawing.Imaging.PixelFormat]::Format24bppRgb)
}

function Save-Png {
    param($Bitmap, [string]$Path)
    $Bitmap.Save($Path, [System.Drawing.Imaging.ImageFormat]::Png)
    $Bitmap.Dispose()
    Write-Host "Wrote $Path"
}

function Fill-Rect {
    param($Graphics, [System.Drawing.Color]$Color, [int]$X, [int]$Y, [int]$Width, [int]$Height)
    $brush = [System.Drawing.SolidBrush]::new($Color)
    $Graphics.FillRectangle($brush, $X, $Y, $Width, $Height)
    $brush.Dispose()
}

function New-PlankSample {
    $bitmap = New-Bitmap -Width $W -Height $H
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    $graphics.Clear($swatches[4])
    for ($row = 0; $row -lt 8; $row++) {
        $y = $row * 64
        for ($col = -1; $col -lt 5; $col++) {
            $x = ($col * 128) + (($row % 2) * 64)
            Fill-Rect $graphics $swatches[($row + $col + 5) % $swatches.Count] $x $y 126 62
        }
        Fill-Rect $graphics $swatches[4] 0 $y $W 2
    }
    $graphics.Dispose()
    Save-Png -Bitmap $bitmap -Path (Join-Path $outDir '01-floorcovering-sample.png')
}

function New-TileSample {
    $bitmap = New-Bitmap -Width $W -Height $H
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    for ($row = 0; $row -lt 4; $row++) {
        for ($col = 0; $col -lt 4; $col++) {
            Fill-Rect $graphics $swatches[($row * 2 + $col) % $swatches.Count] ($col * 128) ($row * 128) 122 122
            Fill-Rect $graphics $swatches[4] (($col * 128) + 122) ($row * 128) 6 128
            Fill-Rect $graphics $swatches[4] ($col * 128) (($row * 128) + 122) 128 6
        }
    }
    $graphics.Dispose()
    Save-Png -Bitmap $bitmap -Path (Join-Path $outDir '02-floorcovering-sample.png')
}

function New-HerringboneSample {
    $bitmap = New-Bitmap -Width $W -Height $H
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    for ($row = 0; $row -lt 32; $row++) {
        for ($col = 0; $col -lt 8; $col++) {
            $colorIndex = if ((($row + $col) % 2) -eq 0) { ($col + 1) % $swatches.Count } else { ($row + 2) % $swatches.Count }
            Fill-Rect $graphics $swatches[$colorIndex] ($col * 64) ($row * 16) 64 16
        }
    }
    $graphics.Dispose()
    Save-Png -Bitmap $bitmap -Path (Join-Path $outDir '03-floorcovering-sample.png')
}

function New-GenericSample {
    $bitmap = New-Bitmap -Width 16 -Height 16
    for ($y = 0; $y -lt 16; $y++) {
        for ($x = 0; $x -lt 16; $x++) {
            $idx = ([Math]::Floor($x / 4) + [Math]::Floor($y / 4)) % $swatches.Count
            $bitmap.SetPixel($x, $y, $swatches[$idx])
        }
    }
    Save-Png -Bitmap $bitmap -Path (Join-Path $outDir 'generic-floorcovering-sample.png')
}

function New-ChevronSample {
    $bitmap = New-Bitmap -Width $patternW -Height $patternH
    for ($y = 0; $y -lt $patternH; $y++) {
        for ($x = 0; $x -lt $patternW; $x++) {
            $band = [Math]::Floor((($x % 120) + (($y % 80) * 2)) / 24) % $swatches.Count
            $bitmap.SetPixel($x, $y, $swatches[$band])
        }
    }
    Save-Png -Bitmap $bitmap -Path (Join-Path $outDir 'sample-pattern-chevron.png')
}

function New-ChannelGridSample {
    $bitmap = New-Bitmap -Width $patternW -Height $patternH
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    for ($row = 0; $row -lt 4; $row++) {
        for ($col = 0; $col -lt 6; $col++) {
            Fill-Rect $graphics $swatches[($row * 2 + $col) % $swatches.Count] ($col * 80) ($row * 80) 80 80
        }
    }
    $graphics.Dispose()
    Save-Png -Bitmap $bitmap -Path (Join-Path $outDir 'sample-pattern-channel-grid.png')
}

function New-RegistrationStripeSample {
    $bitmap = New-Bitmap -Width $patternW -Height $patternH
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    for ($i = 0; $i -lt $swatches.Count; $i++) {
        Fill-Rect $graphics $swatches[$i] 0 ($i * 64) $patternW 64
    }
    foreach ($x in @(48, 138, 228, 318, 408)) {
        Fill-Rect $graphics $swatches[4] $x 0 18 $patternH
    }
    $graphics.Dispose()
    Save-Png -Bitmap $bitmap -Path (Join-Path $outDir 'sample-pattern-registration-stripes.png')
}

New-PlankSample
New-TileSample
New-HerringboneSample
New-GenericSample
New-ChevronSample
New-ChannelGridSample
New-RegistrationStripeSample

Write-Host "Done."