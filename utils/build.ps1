pushd src
$installpath=$(utils/vswhere.exe -property installationPath)
&"$(../utils/vswhere.exe -property installationPath)/MSBuild/15.0/Bin/msbuild.exe" /p:outDir=tmp/ /p:Configuration=Release /p:PostBuildEvent= /t:Build ELSPlus.sln
popd

cp server/ELS-Server/tmp/* src/tmp

pushd src/tmp
$pdb2mdb="$PSScriptRoot\..\utils\pdb2mdb\pdb2mdb.exe"

"Converting pdbs to mdbs"
$pdbs = Get-ChildItem "$PWD" -Recurse -Include @('*.dll')
foreach ($item in $pdbs) {
    $(Invoke-Expression -Command "$pdb2mdb $item" ) 2>&1 | Out-Null 
}
rm -Recurse -Force *.pdb
popd