param(
    # Build Debug
    [bool]
    $BuildDebug
)
Get-ChildItem
Get-ChildItem src
if ($BuildDebug) {
    $name = "$($env:CI_PIPELINE_ID)-elsplus_Debug.zip"
    Get-ChildItem src/tmp-debug
    mkdir elsplus
    Copy-Item -Recurse src/tmp-debug/* elsplus
}
else {
    $name = "$($env:CI_PIPELINE_ID)-elsplus.zip"
    Get-ChildItem src/tmp-release
    mkdir elsplus
    Copy-Item -Recurse src/tmp-release/* elsplus
}
Compress-Archive elsplus $name
aws.exe configure set aws_access_key_id $env:aws_key
aws.exe configure set aws_secret_access_key $env:aws_secret
aws.exe --endpoint-url $env:aws_host s3 cp $name s3://els-plus/$($env:CI_COMMIT_REF_NAME)/
if ($?) {
    & $PSScriptRoot/notify.ps1 -g "Upload success.
    Get it at https://cdn01.friendsincode.com/els-plus/$($env:CI_COMMIT_REF_NAME)/$($name)"
}
else {
    "Upload failure."
}

