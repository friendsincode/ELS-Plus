param([string]$g)
    
$webhook = "$env:webhook"

$Body = @{
    "content"    = $g
    #"avatar_url" = "https://i.imgur.com/bqYLP7g.png"
    # "avatar_url" = "$env:webhook_img"
        
}

$params = @{
    Headers = @{'accept' = 'application/json'}
    Body    = $Body | convertto-json
    Method  = 'Post'
    URI     = $webhook 
}

Invoke-RestMethod @params

