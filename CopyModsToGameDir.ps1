param (
[string]$GameFolder,
[string]$ModName,
[string]$BuildOutput
)

$modfolder = -join($GameFolder, "\Mods\", $ModName)

Write-Host $modfolder

if ((Test-Path $modfolder))
{
	rm -r $modfolder
}
mkdir $modfolder
$files = -join($BuildOutput, "\*")
copy $files $modfolder
