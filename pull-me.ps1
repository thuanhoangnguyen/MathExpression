$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
Set-Location $scriptPath

if(Test-Path ".git" -pathType container)
    {
        Write-Host "Pulling from $scriptPath"
        &git pull
    }
	
	Read-Host "Enter to close ..."