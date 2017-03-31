param (
    [Parameter (Mandatory=$true)][string]$address = "127.0.0.1",
    [Parameter(Mandatory=$true)][int]$port = 30120,
    [String]$password = "lovely",
    [Parameter (Mandatory=$true)] [string]$projectName,
    [Parameter (Mandatory=$true)] [System.IO.DirectoryInfo] $serverRootFolder,
    [Parameter (Mandatory=$true)] [string] $sourceDir

)

$jj=[System.Diagnostics.FileVersionInfo]::GetVersionInfo($projectName).ProductName
$sourceDir = New-Object -TypeName System.IO.DirectoryInfo -ArgumentList "$sourceDir"
$__resourceLua = [System.IO.Path]::Combine($y.FullName,'__resource.lua')
$targetDir = New-Object -TypeName System.IO.DirectoryInfo -ArgumentList ([System.IO.Path]::Combine($serverRootFolder.FullName,"resources","$jj\"))

if($targetDir.Exists.Equals($false)){
    $targetDir.Create()
}
#for each file or folder in the source directory copy it to the destination directory and keep their full path
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
