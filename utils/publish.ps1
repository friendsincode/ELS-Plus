param(
    # Build Debug
    [bool]
    $BuildDebug
)
if ($BuildDebug) {
    $name = "$($env:CI_PIPELINE_ID)-elsplus.zip"
}
else {
    $name = "$($env:CI_PIPELINE_ID)-elsplus_Debug.zip"
}
Compress-Archive elsplus $name
aws.exe configure set aws_access_key_id $env:aws_key
aws.exe configure set aws_secret_access_key $env:aws_secret
aws.exe --endpoint-url $env:aws_host s3 cp $name s3://els-plus/$($env:CI_COMMIT_REF_NAME)/
if ($?) {
    notify.ps1 -g "Upload success.
    Get it at `"https://cdn01.friendsincode.com/els-plus/$($env:CI_COMMIT_REF_NAME)/$($env:CI_PIPELINE_ID)-elsplus.zip`""
}
else {
    "Upload failure."
}

