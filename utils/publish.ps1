$name="$($env:CI_PIPELINE_ID)-elsplus.zip"
Compress-Archive elsplus $name
aws.exe configure set aws_access_key_id $env:aws_key
aws.exe configure set aws_secret_access_key $env:aws_secret
aws.exe --endpoint-url $env:aws_host s3 cp $name s3://els-plus/ 2>&1 | Out-Null 
if($?){
    "Upload success."
}else {
    "Upload failure."
}

