param (
    [Parameter (Mandatory=$true)][string]$address = "127.0.0.1",
    [Parameter(Mandatory=$true)][int]$port = 30120,
    [String]$password = "lovely",
    [Parameter (Mandatory=$true)] [string]$projectName,
    [Parameter (Mandatory=$true)] [System.IO.DirectoryInfo] $serverRootFolder,
    [Parameter (Mandatory=$true)] [string] $sourceDir

)
function getResourceFiles ($kj) {
    $lfiles = @()
    for($k=0;$k -lt $kj.count;$k++){
        [System.Text.RegularExpressions.MatchCollection] $h = [regex]::Matches($kj[$k],'object_entry\(''(.*)''\)')
        foreach($j in $h ){
            for($i=0;$i -lt $j.Groups.Count;$i++){
                if($i -ne 0){
                    [System.IO.FileInfo] $tfile = $j.Groups.Item($i).Value
                    $lfiles+=$tfile
                }
            }
        }
    }
    $lfiles
    return $lfiles
}
function getServerScripts ($kj) {

    $lfiles = @()
    for($k=0;$k -lt $kj.count;$k++){
        [System.Text.RegularExpressions.MatchCollection] $h = [regex]::Matches($kj[$k],'(?m)server_script\s''(.*)''')
        foreach($j in $h ){
            for($i=0;$i -lt $j.Groups.Count;$i++){
                if($i -ne 0){
                    [System.IO.FileInfo] $tfile = $j.Groups.Item($i).Value
                    $lfiles+=$tfile
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
                [System.IO.FileInfo] $tfile = $j.Groups.Item($i).Value
                    $lfiles+=$tfile
                }
            }
        }
    }
    return $lfiles
}
$jj=[System.Diagnostics.FileVersionInfo]::GetVersionInfo($projectName).ProductName
$sourceDir = New-Object -TypeName System.IO.DirectoryInfo -ArgumentList "$sourceDir"
$__resourceLua = [System.IO.Path]::Combine($y.FullName,'__resource.lua')
$targetDir = New-Object -TypeName System.IO.DirectoryInfo -ArgumentList ([System.IO.Path]::Combine($serverRootFolder.FullName,"resources","$jj\"))

$files+=@(getClientScripts(Get-Content $__resourceLua))
$files+=@(getServerScripts(Get-Content $__resourceLua))
$files+=@(getResourceFiles(Get-Content $__resourceLua))
$files+=@('__resource.lua')
if($targetDir.Exists.Equals($false)){
    $targetDir.Create()
}
foreach ( $file in $files){
    ([System.IO.FileInfo]$file).Directory
    $oldpath=[System.io.Path]::GetFullPath([System.IO.Path]::Combine($sourceDir.FullName,$file))
    $newpath=[System.io.Path]::GetFullPath([System.IO.Path]::Combine($targetDir.FullName,$file))
    ([System.IO.FileInfo]$newpath).Directory
    if(([System.IO.FileInfo]$newpath).Directory.Exists -eq $false){
        ([System.IO.FileInfo]$newpath).Directory.Create()
    }
    [System.IO.File]::Copy($oldpath,$newpath,$true)
}
