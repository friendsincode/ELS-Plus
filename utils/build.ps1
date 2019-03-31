param(
    # Build Debug
    [bool]
    $BuildDebug
)
Push-Location src
$vswherepath = $($PSScriptRoot + "\vswhere.exe")
$installpath = $(&$vswherepath -property installationPath)
if ($BuildDebug) {
    $buildpath = "$pwd\\tmp-debug\\"
}
else {
    $buildpath = "$pwd\\tmp-release\\"
}
if ($BuildDebug) {
    &"$installpath/MSBuild/15.0/Bin/msbuild.exe" /p:outDir=`"$buildpath`" /m /p:Configuration=Debug /p:PostBuildEvent= /t:Build ELSPlus.sln
}
else {
    &"$installpath/MSBuild/15.0/Bin/msbuild.exe" /p:outDir=`"$buildpath`" /m /p:Configuration=Release /p:PostBuildEvent= /t:Build ELSPlus.sln
}
Pop-Location

if (Test-Path -ErrorAction SilentlyContinue -Path "server/ELS-Server/tmp") {
    Copy-Item server/ELS-Server/tmp/* $buildpath
}
Push-Location $buildpath
$pdb2mdb = "$PSScriptRoot\..\utils\pdb2mdb\pdb2mdb.exe"

"Converting pdbs to mdbs"
$pdbs = Get-ChildItem "$PWD" -Recurse -Include @('*.dll')
foreach ($item in $pdbs) {
    $(Invoke-Expression -Command "$pdb2mdb $item" ) 2>&1 | Out-Null 
}
Remove-Item -Recurse -Force *.pdb
Pop-Location