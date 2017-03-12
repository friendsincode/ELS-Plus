param (
    [Parameter (Mandatory=$true)][string]$address = "127.0.0.1",
    [Parameter(Mandatory=$true)][int]$port = 30120,
    [String]$password = "lovely",
    [Parameter (Mandatory=$true)] [string]$projectName,
    [Parameter (Mandatory=$true)] [System.IO.DirectoryInfo] $serverRootFolder,
    [Parameter (Mandatory=$true)] [string] $sourceDir
    
)
function getServerScripts ($kj) {
    
$lfiles = @()
for($k=0;$k -lt $kj.count;$k++){
[System.Text.RegularExpressions.MatchCollection] $h = [regex]::Matches($kj[$k],'(?m)server_script\s''(.*)''')
	foreach($j in $h ){
    for($i=0;$i -lt $j.Groups.Count;$i++){
        if($i -ne 0){
            $lfiles+=$j.Groups.Item($i).Value
        }
    }
}
}
return $lfiles
}
function getClientScripts ($kj) {
    
$lfiles = @()
for($k=0;$k -lt $kj.count;$k++){
[System.Text.RegularExpressions.MatchCollection] $h = [regex]::Matches($kj[$k],'(?m)client_script\s''(.*)''')
    foreach($j in $h ){
    for($i=0;$i -lt $j.Groups.Count;$i++){
        if($i -ne 0){
            $lfiles+=$j.Groups.Item($i).Value
        }
    }
}
}
return $lfiles
}
$jj=[System.Diagnostics.FileVersionInfo]::GetVersionInfo($projectName).ProductName
$y = New-Object -TypeName System.IO.DirectoryInfo -ArgumentList "$sourceDir"
$__resourceLua = [System.IO.Path]::Combine($y.FullName,'__resource.lua')
$h = New-Object -TypeName System.IO.DirectoryInfo -ArgumentList ([System.IO.Path]::Combine($serverRootFolder.FullName,"resources","$jj\"))

$files+=@(getClientScripts(Get-Content $__resourceLua))
$files+=@(getServerScripts(Get-Content $__resourceLua))
$files+=@('__resource.lua')
if($h.Exists.Equals($false)){
    $h.Create()
}
foreach ($file in $files){
$file
    [System.IO.File]::Copy([System.IO.Path]::Combine($y.FullName,$file),[System.IO.Path]::Combine($h.FullName,$file),$true)
}
