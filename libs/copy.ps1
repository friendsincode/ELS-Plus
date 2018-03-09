param(
    [parameter(Mandatory = $true)] [string] $projectName,
    [parameter(Mandatory = $true)] [string] $assemblyname,
    [parameter()] [string]$src = $pwd

)
$pdb2mdb = "$PSScriptRoot\..\utils\pdb2mdb\pdb2mdb.exe"
$pdb2mdbFullPath = [System.IO.Path]::GetFullPath($pdb2mdb)

$FxServerbinary = [System.IO.Path]::Combine($env:FXSERVERBIN, "fxserver.exe")
if (-not(Test-Path -Path $FxServerbinary)) {
    Write-Host -Object "FXSERVERBIN eviroment varible not specified
    skipping copying to server folder" -ForegroundColor Yellow
    #exit
}

$FxServerData = [System.IO.Path]::Combine($env:FXSERVERDATA)
if (!(Test-Path -Path $FxServerData)) {
    Write-Host -Object "FXSERVERDATA eviroment varible not specified
    or we could not find the resources directory
    skipping copying to server folder" -ForegroundColor Yellow
    exit
}
$newpath = ([System.IO.Path]::Combine($env:FXSERVERDATA, $projectName))
if (-not (Test-Path -Path ([System.IO.Path]::Combine($env:FXSERVERDATA, $projectName)))) {
    New-Item -ItemType Directory -Path $newpath
    Write-Host -Object "Creating resource folder $newpath"
}
if (-not(Test-Path -Path ([System.IO.Path]::Combine($src, "bin")))) {
    #$strcommand = "libz inject-dll -a `"$newpath/$assemblyname.dll`" -i *.dll --move --overwrite"
    #Write-Host -Object "$strcommand -e $assemblyname.dll"
    #iex "$strcommand -e $assemblyname.dll"
    
    $dest = $newpath
    "Converting pdbs to mdbs"
    $pdbs = Get-ChildItem "$PWD" -Recurse -Include @('*.dll')
    foreach ($item in $pdbs) {
        pushd $([System.IO.Path]::GetDirectoryName($item))
        $(Invoke-Expression -Command "& '$pdb2mdbFullPath' `"$item`"" ) 2>&1 | Out-Null 
        popd
    }
    "copying visual studio project output to $dest"
    Copy-Item "*" -Recurse -Exclude @('.\*.xml', '*.pdb') -Destination $dest -Force
}
elseif (Test-Path -Path ([System.IO.Path]::Combine($src, "bin"))) {
    $tmppath = ([System.IO.Path]::Combine($src, "bin"))
    #$tmppath +='\*'
    $tmppath
    copy "$tmppath" .\test-path  -Recurse -Include '*'
}
elseif (Test-Path -Path $src) {
    
}