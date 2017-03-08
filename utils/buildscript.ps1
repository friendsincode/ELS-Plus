param (
    [Parameter (Mandatory=$true)][string]$address = "127.0.0.1",
    [Parameter(Mandatory=$true)][int]$port = 30120,
    [String]$password = "lovely",
    [Parameter (Mandatory=$true)] [string]$projectName,
    [Parameter (Mandatory=$true)] [System.IO.DirectoryInfo] $serverRootFolder,
    [Parameter (Mandatory=$true)] [string] $sourceDir,
    [String] $files

)
$y = New-Object -TypeName System.IO.DirectoryInfo -ArgumentList "$sourceDir"
$h = New-Object -TypeName System.IO.DirectoryInfo -ArgumentList ([System.IO.Path]::Combine($serverRootFolder.FullName,"resources","$projectName\"))
$h.FullName
 
if($h.Exists.Equals($false)){
    $h.Create()
}
foreach ($file in $files.Split(',')){
    [System.IO.File]::Copy([System.IO.Path]::Combine($y.FullName,$file),[System.IO.Path]::Combine($h.FullName,$file),$true)
}