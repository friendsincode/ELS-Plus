param(
    # Build Debug
    [bool]
    $BuildDebug
)
pushd src
$vswherepath=$($PSScriptRoot + "\vswhere.exe")
$installpath = $(&$vswherepath -property installationPath)
&"$installpath/MSBuild/15.0/Bin/msbuild.exe" /p:outDir=`"$pwd\\tmp\\`" /m /p:Configuration=Release /p:PostBuildEvent= /t:Build ELSPlus.sln
popd

if(Test-Path -ErrorAction SilentlyContinue -Path "server/ELS-Server/tmp"){
    cp server/ELS-Server/tmp/* src/tmp
}
pushd src/tmp
$pdb2mdb="$PSScriptRoot\..\utils\pdb2mdb\pdb2mdb.exe"

"Converting pdbs to mdbs"
$pdbs = Get-ChildItem "$PWD" -Recurse -Include @('*.dll')
foreach ($item in $pdbs) {
    $(Invoke-Expression -Command "$pdb2mdb $item" ) 2>&1 | Out-Null 
}
rm -Recurse -Force *.pdb
popd