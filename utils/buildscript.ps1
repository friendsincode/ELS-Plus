 param (
    [Parameter (Mandatory=$true)][string]$address = "127.0.0.1",
    [Parameter(Mandatory=$true)][int]$port = 30120,
    [string]$password = "lovely",
    [Parameter (Mandatory=$true)] [string]$projectName = "",
    [System.IO.DirectoryInfo] $serverRootFolder = [System.IO.DirectoryInfo]("C:\cfx-server"),
    [Parameter (Mandatory=$true)][string] $sourceDir,
    [String] $files

 )
 $lfiles = $files.Split(',')
 "Source Dir"
 $y = New-Object -TypeName System.IO.DirectoryInfo -ArgumentList "$sourceDir"
 $h = New-Object -TypeName System.IO.DirectoryInfo -ArgumentList ([System.IO.Path]::Combine($serverRootFolder.FullName,"resources","$projectName\"))
 $h.FullName
 
 if($h.Exists.Equals($false)){
        $h.Create()
 }
 foreach ($file in $lfiles){
    "[System.IO.Path]::Combine($y.FullName,$file)\"
    [System.IO.File]::Copy([System.IO.Path]::Combine($y.FullName,$file),[System.IO.Path]::Combine($h.FullName,$file),$true)
 }