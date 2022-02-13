param(
[parameter(Mandatory=$true)] [string] $dir
)

function SetVersion ($file, $version) {

    "Changing version in $file to $version"
    $fileObject = get-item $file
    #$fileObject.Set_IsReadOnly($False)
 
    $sr = new-object System.IO.StreamReader( $file, [System.Text.Encoding]::GetEncoding("utf-8") )
    $content = $sr.ReadToEnd()
    $sr.Close()
 
    $content = [Regex]::Replace($content, "(\d+)\.(\d+)\.(\d+)[\.(\d+)]*", $version);
 
    $sw = new-object System.IO.StreamWriter( $file, $false, [System.Text.Encoding]::GetEncoding("utf-8") )
    $sw.Write( $content )
    $sw.Close()
    #$fileObject.Set_IsReadOnly($True)
}

function GetVersion ($dir) {   
    #$info_files = Get-ChildItem $dir -Recurse -Include "AssemblyInfo.cs" | where { $_ -match 'ELS' }
    $info_files = "$dir\src\Properties\AssemblyInfo.cs", "$dir\server\ELS-Server\Properties\AssemblyInfo.cs"
    Write-Host "Getting version in" $info_files[0]
    #$fileObject = get-item $info_files[0]
    #$fileObject.Set_IsReadOnly($False)
 
    $sr = new-object System.IO.StreamReader( $info_files[0], [System.Text.Encoding]::GetEncoding("utf-8") )
    $content = $sr.ReadToEnd()
    $sr.Close()
 
    $content = [Regex]::Match($content, "(\d+)\.(\d+)\.(\d+)\.(\d+)*");
    #Write-Host  $content.Groups
    $new = [int]$content.Groups[4].Value + 1
    if ($new -eq 10000) {
        $new = 0
        $major = [int]$content.Groups[3].Value + 1
        $content = [regex]::Replace($content, "(\d+)\.(\d+)\.(\d+)\.(\d+)*", '$1.$2.' + $major + '.' + $new)
        return $content
    }
    $content = [regex]::Replace($content, "(\d+)\.(\d+)\.(\d+)\.(\d+)*", '$1.$2.$3.' + $new)
    
    return $content
}

function setVersionInDir($dir, $version) {
    
    if ($version -eq "") {
        Write-Host "version not found"
        exit 1
    }
    
    # Set the Assembly version
    #$info_files = Get-ChildItem $dir -Recurse -Include "AssemblyInfo.cs" | where { $_ -match 'ELS' }
     $info_files = "$dir\src\Properties\AssemblyInfo.cs", "$dir\server\ELS-Server\Properties\AssemblyInfo.cs", "$dir\src\fxmanifest.lua"
    foreach ($file in $info_files) {
        SetVersion $file $version
    }
}
 
# First get tag from Git
#$dir = "./"
Write-Host $dir
$version = GetVersion $dir

setVersionInDir $dir $version