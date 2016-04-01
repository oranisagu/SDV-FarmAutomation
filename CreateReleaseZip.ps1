param (
	[string]$DirectoryToZip,
	[string]$ZipName,
	[string]$ZipTarget
)

function ZipFiles( $zipfilename, $sourcedir )
{
   Add-Type -Assembly System.IO.Compression.FileSystem
   Write-Host "Zipfile: " $zipfilename
   Write-Host "Directory: "$sourcedir
   $compressionLevel = [System.IO.Compression.CompressionLevel]::Optimal
   [System.IO.Compression.ZipFile]::CreateFromDirectory($sourcedir,
        $zipfilename, $compressionLevel, $false)
}
$zippath = -join($ZipTarget, $ZipName, ".zip")
if (Test-Path $zippath)
{
	rm $zippath
}
if (!(Test-Path $ZipTarget))
{
	mkdir $ZipTarget
}

ZipFiles -zipfilename $zippath -sourcedir $DirectoryToZip






