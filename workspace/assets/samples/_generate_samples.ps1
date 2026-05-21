# Generates three realistic floor covering sample PNGs into the current directory.
# 01: oak hardwood planks
# 02: ceramic tile checkerboard with grout
# 03: herringbone berber carpet weave

Add-Type -AssemblyName System.Drawing

$outDir = $PSScriptRoot
$W = 512
$H = 512

function New-Bitmap {
    param([int]$Width, [int]$Height)
    $bmp = New-Object System.Drawing.Bitmap($Width, $Height, [System.Drawing.Imaging.PixelFormat]::Format24bppRgb)
    return $bmp
}

function Save-Png {
    param($Bitmap, [string]$Path)
    $Bitmap.Save($Path, [System.Drawing.Imaging.ImageFormat]::Png)
    $Bitmap.Dispose()
    Write-Host "Wrote $Path"
}

function Clamp([int]$v) { if ($v -lt 0) { 0 } elseif ($v -gt 255) { 255 } else { $v } }

$rand = [System.Random]::new(20260519)

# ---------- 01: Oak hardwood planks ----------
$bmp1 = New-Bitmap -Width $W -Height $H
$plankH = 64
$baseR = 165; $baseG = 116; $baseB = 70
for ($y = 0; $y -lt $H; $y++) {
    $plankIdx = [Math]::Floor($y / $plankH)
    $offset = ($plankIdx * 53) % 80  # horizontal stagger feel via tone variation
    for ($x = 0; $x -lt $W; $x++) {
        # base wood tone with horizontal grain streaks
        $grain = [Math]::Sin(($x * 0.05) + ($plankIdx * 1.7)) * 12
        $streak = [Math]::Sin(($x * 0.6) + ($y * 0.08) + $plankIdx) * 6
        $noise = $rand.Next(-8, 9)
        $r = Clamp([int]($baseR + $grain + $streak + $noise - ($offset * 0.3)))
        $g = Clamp([int]($baseG + $grain * 0.7 + $streak * 0.6 + $noise - ($offset * 0.25)))
        $b = Clamp([int]($baseB + $grain * 0.4 + $streak * 0.3 + $noise - ($offset * 0.2)))
        # plank seam: dark line every $plankH rows
        if (($y % $plankH) -eq 0 -or ($y % $plankH) -eq ($plankH - 1)) {
            $r = [int]($r * 0.35); $g = [int]($g * 0.30); $b = [int]($b * 0.25)
        }
        $bmp1.SetPixel($x, $y, [System.Drawing.Color]::FromArgb($r, $g, $b))
    }
}
Save-Png -Bitmap $bmp1 -Path (Join-Path $outDir '01-floorcovering-sample.png')

# ---------- 02: Ceramic tile checkerboard with grout ----------
$bmp2 = New-Bitmap -Width $W -Height $H
$tile = 128
$grout = 6
$colorA = @{ R = 232; G = 226; B = 213 } # cream
$colorB = @{ R = 60;  G = 78;  B = 96 }  # slate blue
$groutCol = @{ R = 80; G = 80; B = 78 }
for ($y = 0; $y -lt $H; $y++) {
    for ($x = 0; $x -lt $W; $x++) {
        $tx = [Math]::Floor($x / $tile)
        $ty = [Math]::Floor($y / $tile)
        $isGrout = (($x % $tile) -lt $grout) -or (($y % $tile) -lt $grout)
        if ($isGrout) {
            $n = $rand.Next(-6, 7)
            $r = Clamp($groutCol.R + $n); $g = Clamp($groutCol.G + $n); $b = Clamp($groutCol.B + $n)
        }
        else {
            if ((($tx + $ty) % 2) -eq 0) { $c = $colorA } else { $c = $colorB }
            # subtle marbling
            $m = [Math]::Sin(($x * 0.07) + ($y * 0.05)) * 10
            $n = $rand.Next(-5, 6)
            $r = Clamp([int]($c.R + $m + $n))
            $g = Clamp([int]($c.G + $m + $n))
            $b = Clamp([int]($c.B + $m + $n))
        }
        $bmp2.SetPixel($x, $y, [System.Drawing.Color]::FromArgb($r, $g, $b))
    }
}
Save-Png -Bitmap $bmp2 -Path (Join-Path $outDir '02-floorcovering-sample.png')

# ---------- 03: Herringbone berber carpet ----------
$bmp3 = New-Bitmap -Width $W -Height $H
$base = @{ R = 198; G = 184; B = 156 } # warm beige
$dark = @{ R = 120; G = 96; B = 70 }   # cocoa
$light = @{ R = 230; G = 220; B = 196 } # cream
$blockW = 64; $blockH = 16
for ($y = 0; $y -lt $H; $y++) {
    $row = [Math]::Floor($y / $blockH)
    for ($x = 0; $x -lt $W; $x++) {
        $col = [Math]::Floor($x / $blockW)
        # herringbone diagonal alternation
        $diag = ($row + $col) % 2
        if ($diag -eq 0) {
            $localX = $x % $blockW
            $localY = $y % $blockH
            $stripe = ([Math]::Floor(($localX + $localY * 2) / 4)) % 3
        }
        else {
            $localX = $x % $blockW
            $localY = $y % $blockH
            $stripe = ([Math]::Floor(($localX - $localY * 2 + $blockW) / 4)) % 3
        }
        switch ($stripe) {
            0 { $c = $base }
            1 { $c = $dark }
            default { $c = $light }
        }
        # weave fiber noise
        $n = $rand.Next(-14, 15)
        $weave = [Math]::Sin(($x + $y) * 1.3) * 6
        $r = Clamp([int]($c.R + $n + $weave))
        $g = Clamp([int]($c.G + $n + $weave))
        $b = Clamp([int]($c.B + $n + $weave))
        $bmp3.SetPixel($x, $y, [System.Drawing.Color]::FromArgb($r, $g, $b))
    }
}
Save-Png -Bitmap $bmp3 -Path (Join-Path $outDir '03-floorcovering-sample.png')

Write-Host "Done."
